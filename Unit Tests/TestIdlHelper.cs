using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDL_Wrapper;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace MxCSUtilUnitTests
{
	[TestClass]
	public class TestIdlHelper
	{
		const string TEMPLATE_FILENAME = "T4 unit tests";
		readonly IdlHelper helper;

		public TestIdlHelper()
		{
			helper = new IdlHelper();

		}

		[TestMethod]
		public void IdlHelper_Can_create_IdlHelper()
		{
		}

		[TestMethod]
		public void IdlHelper_Writes_file_header()
		{
			var actual = capture((writer) =>
			{
				helper.writeFileHeader(TEMPLATE_FILENAME, writer);
			});

			StringAssert.Contains(actual, "Auto-generated code by " + TEMPLATE_FILENAME, "Header includes name of T4 template");
			StringAssert.Contains(actual, "#pragma once", "Proxy starts with #pragma once");
		}

		[TestMethod]
		public void IdlHelper_Includes_required_proxies()
		{
			var actual = capture((writer) =>
			{
				string idl = File.ReadAllText("UnitTests.idl");
				helper.writeIncludesForRequiredProxies(idl, writer);
			});
			Assert.AreEqual("#include \"MxFrameworkInterfacesProxy.h\"\r\n", actual);
		}

		[TestMethod]
		public void IdlHelper_Identifies_idl_interfaces()
		{
			var idl = File.ReadAllText("UnitTests.idl");
			var expectedInterfaces = new HashSet<string>(new[] { 
				"IMxCollection", "IMxQueue", "IMxFrameTracker", "IMxModuleId", "IMxLogValue", 
				"IMxLogErrorValue", "IMxLogEventValue", "IMxLogTraceValue", "IMxLog", "IMxSystemPaths" 
			});
			var expectedCount = expectedInterfaces.Count;
			var actualCount = 0;

			helper.forEachInterfaceIn(idl, (a, b) =>
			{
				expectedInterfaces.Remove(a);
				actualCount++;
			});

			Assert.AreEqual(expectedCount, actualCount, "Expected count of interfaces");
			Assert.AreEqual(0, expectedInterfaces.Count, "Expected interfaces not found");
		}

		[TestMethod]
		public void IdlHelper_Splits_methods()
		{
			var rows = new StringBuilder();
			rows.AppendLine("	[propget]	HRESULT Name										( [out,ref,retval] BSTR* pVal );");
			rows.AppendLine("				HRESULT At									");
			rows.AppendLine("( [in] long idx, [out,ref,retval] IMxGrabLinesOrder** ppVal );HRESULT Stop										( );");

			var result = helper.splitHRESULTmethods(rows.ToString()).ToArray();

			Assert.AreEqual(3, result.Count(), "Found 3 methods");
			Assert.AreEqual("[propget]	HRESULT Name										( [out,ref,retval] BSTR* pVal );", result[0].Trim(), "First method");
			Assert.AreEqual("HRESULT At									\r\n( [in] long idx, [out,ref,retval] IMxGrabLinesOrder** ppVal );", result[1].Trim(), "Second method");
			Assert.AreEqual("HRESULT Stop										( );", result[2].Trim(), "Third method");
		}

		[TestMethod]
		public void IdlHelper_Parses_method()
		{
			var actual = helper.parseMethod("	[propget]	HRESULT At	\r\n( [out,ref,retval] IMxGrabLinesOrder** ppVal );");
			Assert.AreEqual("propget", actual.Attributes);
			Assert.AreEqual("At", actual.Name);
			Assert.AreEqual("[out,ref,retval] IMxGrabLinesOrder** ppVal", actual.Parameters.Trim());
		}

		[TestMethod]
		public void IdlHelper_Splits_no_parameter()
		{
			var result = helper.splitParameters("   ").ToArray();
			Assert.AreEqual(0, result.Count());
		}

		[TestMethod]
		public void IdlHelper_Splits_one_parameter()
		{
			var result = helper.splitParameters(" [out,ref,retval] BSTR* pVal ").ToArray();
			Assert.AreEqual(1, result.Count());
			Assert.AreEqual("out|ref|retval", string.Join("|", result[0].Attributes));
			Assert.AreEqual("BSTR*", result[0].Type);
			Assert.AreEqual("pVal", result[0].Name);
			Assert.AreEqual("", result[0].Suffix);
		}

		[TestMethod]
		public void IdlHelper_Splits_multiple_parameters()
		{
			var result = helper.splitParameters(" [in] long idx[], [in] int, [out,ref,retval] IMxGrabLinesOrder ** ppVal ").ToArray();
			Assert.AreEqual(3, result.Count());

			Assert.AreEqual("in", string.Join("|", result[0].Attributes), "First parameter attributes");
			Assert.AreEqual("long", result[0].Type, "First parameter type");
			Assert.AreEqual("idx", result[0].Name, "First parameter name");
			Assert.AreEqual("[]", result[0].Suffix, "First parameter suffix");

			Assert.AreEqual("in", string.Join("|", result[1].Attributes), "Second parameter attributes");
			Assert.AreEqual("int", result[1].Type, "Second parameter type");
			Assert.AreEqual("p2", result[1].Name, "Second parameter name");
			Assert.AreEqual("", result[1].Suffix, "Second parameter suffix");

			Assert.AreEqual("out|ref|retval", string.Join("|", result[2].Attributes), "Third parameter attributes");
			Assert.AreEqual("IMxGrabLinesOrder **", result[2].Type, "Third parameter type");
			Assert.AreEqual("ppVal", result[2].Name, "Third parameter name");
			Assert.AreEqual("", result[2].Suffix, "Third parameter suffix");
		}

		[TestMethod]
		public void IdlHelper_Registers_enums_as_unreferenced_types()
		{
			var allIdlFiles = new string[] { "UnitTests.idl" };
			var expectedTypeCount = helper.unreferencedTypes.Count() + 4;

			helper.registerIdlEnumsAsUnreferencedTypes(allIdlFiles);

			var actualTypeCount = helper.unreferencedTypes.Count();
			Assert.AreEqual(expectedTypeCount, actualTypeCount, "Registered enums as unreferenced types");
			Assert.AreEqual("ESeverity", helper.unreferencedTypes[expectedTypeCount - 4]);
			Assert.AreEqual("ELogType", helper.unreferencedTypes[expectedTypeCount - 3]);
			Assert.AreEqual("ETraceLevel", helper.unreferencedTypes[expectedTypeCount - 2]);
			Assert.AreEqual("EMxFrameWorkErrorCode", helper.unreferencedTypes[expectedTypeCount - 1]);
		}

		[TestMethod]
		public void IdlHelper_Identifies_all_pointer_types()
		{
			Assert.IsFalse(helper.isPointer("long"), "long is not a pointer");
			Assert.IsTrue(helper.isPointer("long*"), "long* is a pointer");
			Assert.IsTrue(helper.isPointer(" long *  "), "long * is a pointer");
			Assert.IsTrue(helper.isPointer("IMx_Is_a_strange_type **"), "IMx_Is_a_strange_type ** is a pointer");
			Assert.IsTrue(helper.isPointer("BSTR"), "BSTR is actually a pointer too");
		}

		[TestMethod]
		public void IdlHelper_Identifies_all_proxied_types()
		{
			Assert.IsFalse(helper.typeShouldBeProxied("long"), "long is not proxied");
			Assert.IsFalse(helper.typeShouldBeProxied("long*"), "long* is not proxied");
			Assert.IsTrue(helper.typeShouldBeProxied("IMxTest"), "IMxTest should be proxied");
			Assert.IsTrue(helper.typeShouldBeProxied("MxTest"), "MxTest should be proxied");
			Assert.IsTrue(helper.typeShouldBeProxied("IMxTest *"), "IMxTest * should be proxied");
		}

		[TestMethod]
		public void IdlHelper_Strips_a_type_to_its_actual_className()
		{
			Assert.AreEqual("long", helper.classNameOf("long"));
			Assert.AreEqual("long", helper.classNameOf(" long *  "));
			Assert.AreEqual("IMxTest", helper.classNameOf("IMxTest **"));
		}

		[TestMethod]
		public void IdlHelper_Determines_return_type_for_wrapper()
		{
			Assert.AreEqual("void", helper.returnTypeForWrapper(null));
			Assert.AreEqual("bool", helper.returnTypeForWrapper(new Parameter { Type = "VARIANT_BOOL" }));
			Assert.AreEqual("std::wstring", helper.returnTypeForWrapper(new Parameter { Type = "BSTR" }));
			Assert.AreEqual("long", helper.returnTypeForWrapper(new Parameter { Type = "long" }));
			Assert.AreEqual("long", helper.returnTypeForWrapper(new Parameter { Type = "long*" }));
			Assert.AreEqual("long *", helper.returnTypeForWrapper(new Parameter { Type = "long **" }));
			Assert.AreEqual("IMxTestProxy", helper.returnTypeForWrapper(new Parameter { Type = "IMxTest*" }));
			Assert.AreEqual("Ref<IStream>", helper.returnTypeForWrapper(new Parameter { Type = "IStream" }));
		}

		[TestMethod]
		public void IdlHelper_Generates_parameter_strings()
		{
			Assert.AreEqual("", helper.parameterStringOf(null));
			AssertSplitParameters("", "");
			AssertSplitParameters("", "[out,ref,retval] long abc");				//retval out-parameter is stripped

			//out-parameters
			AssertSplitParameters("/*[out]*/IMxModuleIdProxy& pSource", " [out] IMxModuleId** pSource ");	//Proxied out-parameters
			AssertSplitParameters("/*[out]*/long& pVal", "[out] long pVal");	//intrinsic out-parameters
			AssertSplitParameters("/*[out]*/std::wstring& p1", "[out] BSTR");
			AssertSplitParameters("/*[out]*/bool& p1", "[out] VARIANT_BOOL");
			AssertSplitParameters("/*[out]*/Gnurgla* pVal", "[out] Gnurgla* pVal");	//all other out-parameters

			//in-parameters
			AssertSplitParameters("/*[in]*/SAFEARRAY* pVal", "[in] SAFEARRAY(byte) pVal");
			AssertSplitParameters("/*[in]*/const std::wstring& p1", "[in] BSTR  ");
			AssertSplitParameters("/*[in]*/const IMxModuleIdProxy& p1", "[in] IMxModuleId *");	//Proxied in-parameters
			AssertSplitParameters("/*[in]*/long pVal", "[in] long pVal");		//all other in-parameters

			//Multiple (unnamed) parameter types on one line
			//...also make sure a retval-param doesn't cause extra commas at any position
			AssertSplitParameters("/*[in]*/const std::wstring& p1, /*[out]*/std::wstring& p2", "[in] BSTR,[out] BSTR,[out,ref,retval] long abc");
			AssertSplitParameters("/*[in]*/const std::wstring& p1, /*[out]*/std::wstring& p3", "[in] BSTR,[out,ref,retval] long abc,[out] BSTR");
			AssertSplitParameters("/*[in]*/const std::wstring& p2, /*[out]*/std::wstring& p3", "[out,ref,retval] long abc,[in] BSTR,[out] BSTR");
		}

		[TestMethod]
		public void IdlHelper_Reads_idl_file_without_comments()
		{
			var idlFile = "UnitTests.idl";
			var content = File.ReadAllText(idlFile);
			Assert.IsTrue(content.Contains("interface IMxCollection : IUnknown"), "Expected interface IMxCollection");
			Assert.IsTrue(content.Contains("This is a singleline comment"), "Expected singleline comment");
			Assert.IsTrue(content.Contains("This is a multiline comment"), "Expected multiline comment");

			content = helper.idlContentWithoutComments(idlFile);

			Assert.IsTrue(content.Contains("interface IMxCollection : IUnknown"), "Expected interface IMxCollection");
			Assert.IsFalse(content.Contains("This is a singleline comment"), "Didn't expect singleline comment");
			Assert.IsFalse(content.Contains("This is a multiline comment"), "Didn't expect multiline comment");
		}

		[TestMethod]
		public void IdlHelper_Only_rebuilds_idls_thats_changed()
		{
			var inputFiles = new[] { "UnitTests.idl" };
			var expectedOutputFile = "ProxyUnitTests.idl";
			
			File.Delete(expectedOutputFile);
			var outputFiles = helper.filesToGenerate(TEMPLATE_FILENAME, inputFiles, filename => "Proxy" + filename).ToArray();
			Assert.AreEqual(1, outputFiles.Count(), "Expected to generate one file");
			Assert.AreEqual(inputFiles[0], outputFiles[0].Key);
			Assert.AreEqual(expectedOutputFile, outputFiles[0].Value);

			File.WriteAllText(expectedOutputFile, "");
			outputFiles = helper.filesToGenerate(TEMPLATE_FILENAME, inputFiles, filename => "Proxy" + filename).ToArray();
			Assert.AreEqual(0, outputFiles.Count(), "Expected to generate no files");

			File.Delete(expectedOutputFile);
		}

		[TestMethod]
		public void IdlHelper_Resolves_absolute_paths()
		{
			var expected = @"c:\abc\ghi\";

			var actual = helper.resolvePath(@"c:\abc\def\testtemplate.tt", @"..\ghi\");
	
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IdlHelper_Resolves_relative_paths()
		{
			var tempDir = Environment.ExpandEnvironmentVariables("%TEMP%");
			var expected = Path.GetFullPath(Path.Combine(tempDir, ".."));
			var originalCurrentDirectory = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory(tempDir);

			var actual = helper.resolvePath(TEMPLATE_FILENAME, "..");
	
			Directory.SetCurrentDirectory(originalCurrentDirectory);
			Assert.AreEqual(expected, actual);
		}

	
		//public string[] unreferencedTypes
		//public RegexOptions CommonRegexOptions
		//public bool isNullOrWhitespace

		//Parameter.ToString()?!





		#region Assertion helpers

		string capture(Action<TextWriter> action)
		{
			var sb = new StringBuilder();
			using (var writer = new StringWriter(sb))
			{
				action(writer);
			}
			return sb.ToString();
		}

		void AssertSplitParameters(string expected, string parameters)
		{
			var array = helper.splitParameters(parameters);
			var actual = helper.parameterStringOf(array);
			Assert.AreEqual(expected, actual);
		}

		#endregion Assertion helpers
	}
}
