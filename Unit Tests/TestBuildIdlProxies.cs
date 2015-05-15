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
	public class TestBuildIdlProxies
	{
		const string TEMPLATE_FILENAME = "T4 unit tests";
		readonly IdlHelper helper;
		readonly BuildIdlProxy generator;

		public TestBuildIdlProxies()
		{
			helper = new IdlHelper();
			generator = new BuildIdlProxy(TEMPLATE_FILENAME, helper);
		}

#warning processAll() not tested
#warning processIdlFile() not tested

		[TestMethod]
		public void BuildIdlProxy_Includes_idl()
		{
			var expected = "#include \"unit_test_idl.h\"\r\n";

			var actual = capture((writer) =>
				generator.writeIncludeForIdlOutput("unit_test_idl.idl", writer)
			);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void BuildIdlProxy_Writes_interface_forwarders()
		{
			var expected = new StringBuilder();
			expected.AppendLine("class IMxSystemPathsProxy;");
			expected.AppendLine("class IMxLogProxy;");
			expected.AppendLine();

			string idl = File.ReadAllText("UnitTests.idl");
			var actual = capture((writer) =>
			{
				generator.writeInterfaceForwarders(idl, writer);
			});

			Assert.AreEqual(expected.ToString(), actual);
		}

		[TestMethod]
		public void BuildIdlProxy_Writes_method_skeleton()
		{
			var expected = new StringBuilder();
			expected.AppendLine("    IMxGrabLinesOrderProxy Test(/*[in]*/long idx[], /*[in]*/int p2) const;");

			var actual = capture((writer) =>
			{
				generator.writeMethodSkeletonFor("  HRESULT Test( [in] long idx[], [in] int, [out,ref,retval] IMxGrabLinesOrder** ppVal );  ", writer);
			});

			Assert.AreEqual(expected.ToString(), actual);
		}

		[TestMethod]
		public void BuildIdlProxy_Writes_method_definition()
		{
			var expected = new StringBuilder();
			expected.AppendLine("IMxGrabLinesOrderProxy TestClassProxy::Test(/*[in]*/long idx[], /*[in]*/int p2) const");
			expected.AppendLine("{");
			expected.AppendLine("    Ref<IMxGrabLinesOrder> ppVal;");
			expected.AppendLine("    CHECKHR_T( m_principal->Test( idx, p2, &ppVal ) );");
			expected.AppendLine("    return IMxGrabLinesOrderProxy(ppVal);");
			expected.AppendLine("}");
			expected.AppendLine();

			var actual = capture((writer) =>
			{
				var idlMethodDefinition = "  HRESULT Test( [in] long idx[], [in] int, [out,ref,retval] IMxGrabLinesOrder** ppVal );  ";
				generator.writeMethodDefinitionFor("TestClassProxy", idlMethodDefinition, writer);
			});

			Assert.AreEqual(expected.ToString(), actual);
		}

		[TestMethod]
		public void BuildIdlProxy_Writes_method_skeleton_returning_wstring()
		{
			var expected = new StringBuilder();
			expected.AppendLine("    std::wstring get_HWInterfaces() const;");

			var actual = capture((writer) =>
			{
				generator.writeMethodSkeletonFor("  HRESULT get_HWInterfaces( [out,ref,retval] BSTR returnValue );  ", writer);
			});

			Assert.AreEqual(expected.ToString(), actual);
		}

		[TestMethod]
		public void BuildIdlProxy_Writes_method_definition_returning_wstring()
		{
			var expected = new StringBuilder();
			expected.AppendLine("std::wstring TestClassProxy::get_HWInterfaces() const");
			expected.AppendLine("{");
			expected.AppendLine("    std::wstring returnValue;");
			expected.AppendLine("    CHECKHR_T( m_principal->get_HWInterfaces( CMxBstr::out(returnValue) ) );");
			expected.AppendLine("    return returnValue;");
			expected.AppendLine("}");
			expected.AppendLine();

			var actual = capture((writer) =>
			{
				var idlMethodDefinition = "  HRESULT get_HWInterfaces( [out,ref,retval] BSTR* returnValue );  ";
				generator.writeMethodDefinitionFor("TestClassProxy", idlMethodDefinition, writer);
			});

			Assert.AreEqual(expected.ToString(), actual);
		}

		[TestMethod]
		public void BuildIdlProxy_Writes_method_definition_returning_double_ptr()
		{
			var expected = new StringBuilder();
			expected.AppendLine("double* TestClassProxy::get_HWInterfaces() const");
			expected.AppendLine("{");
			expected.AppendLine("    double* returnValue;");
			expected.AppendLine("    CHECKHR_T( m_principal->get_HWInterfaces( &returnValue ) );");
			expected.AppendLine("    return returnValue;");
			expected.AppendLine("}");
			expected.AppendLine();

			var actual = capture((writer) =>
			{
				var idlMethodDefinition = "  HRESULT get_HWInterfaces( [out,ref,retval] double** returnValue );  ";
				generator.writeMethodDefinitionFor("TestClassProxy", idlMethodDefinition, writer);
			});

			Assert.AreEqual(expected.ToString(), actual);
		}

		[TestMethod]
		public void BuildIdlProxy_Generates_call_string()
		{
			Assert.AreEqual("", generator.callStringOf(null));
			Assert.AreEqual("", generator.callStringOf(new Parameter[0]));

			AssertCallString("CMxBstr::out(pVal)", "[out,ref,retval] BSTR pVal");
			AssertCallString("&p1", "[out] Gnurgla*");
			AssertCallString("CMxBoolSetter::out(p1)", "[out] VARIANT_BOOL");
			AssertCallString("&p1", "[retval,out] VARIANT_BOOL");
			AssertCallString("&pVal", "[out] long* pVal");
			AssertCallString("&pValRef", "[out] IMxLog* pVal");
			AssertCallString("&pVal", "[out,ref,retval] IMxLog* pVal");
			AssertCallString("&pVal", "[out] long** pVal");

//#warning Are **-pointer tests correct?
//			AssertCallString("&pVal", "[out] Gnurgla** pVal");
//			AssertCallString("&pVal", "[out,retval] Gnurgla** pVal");
		
			AssertCallString("CMxBstr::in(pVal)", "[in] BSTR pVal");
			AssertCallString("pVal.ref().ptr()", "[in] IMxLog* pVal");
			AssertCallString("pVal", "long pVal");
		}

		[TestMethod]
		public void BuildIdlProxy_Generates_output_filesnames()
		{
			var tempPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%TEMP%"));
			var outputPath = Path.Combine(tempPath, "Output");
			if (Directory.Exists(outputPath))
				Directory.Delete(outputPath);
			var expected = Path.Combine(outputPath, "TestIdlFileProxy.h");

			var actual = generator.outputFileFor(Path.Combine(tempPath, "TestIdlFile.idl"), outputPath);

			Assert.AreEqual(expected, actual);

			if (!Directory.Exists(outputPath))
				Assert.Fail("Expected output path to be created.");
		}



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

		void AssertCallString(string expected, string parameters)
		{
			var p = helper.splitParameters(parameters);
			var actual = generator.callStringOf(p);
			Assert.AreEqual(expected, actual, "Tested parameters: " + parameters);
		}

		#endregion Assertion helpers

	}
}
