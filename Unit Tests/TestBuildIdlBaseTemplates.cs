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
	public class TestBuildIdlBaseTemplates
	{
		const string TEMPLATE_FILENAME = "T4 unit tests";
		readonly IdlHelper helper;
		readonly BuildIdlBaseTemplates generator;

		public TestBuildIdlBaseTemplates()
		{
			helper = new IdlHelper();
			generator = new BuildIdlBaseTemplates(TEMPLATE_FILENAME, helper);
		}

#warning processAll() not tested
#warning processIdlFile() not tested

		[TestMethod]
		public void BuildIdlBaseTemplates_Includes_idl_proxy()
		{
			var expected = "#include \"unit_test_idlProxy.h\"\r\n";

			var actual = capture((writer) =>
				generator.writeIncludeForIdlOutputProxy("unit_test_idl.idl", writer)
			);

			Assert.AreEqual(expected, actual);
		}

#warning writeBaseTemplateFor() not tested

		[TestMethod]
		public void BuildIdlBaseTemplates_Writes_public_method_definitions_no_params()
		{
			var expected = new StringBuilder();
			expected.AppendLine("    virtual HRESULT STDMETHODCALLTYPE Test()");
			expected.AppendLine("    {");
			expected.AppendLine("        try");
			expected.AppendLine("        {");
			expected.AppendLine("            test();");
			expected.AppendLine("            return S_OK;");
			expected.AppendLine("        }");
			expected.AppendLine("        mx_catch;");
			expected.AppendLine("    }");
			AssertIdlMethod(expected.ToString(), "   HRESULT Test();");
		}

		[TestMethod]
		public void BuildIdlBaseTemplates_Writes_public_method_definitions_propget()
		{
			var expected = new StringBuilder();
			expected.AppendLine("    virtual HRESULT STDMETHODCALLTYPE get_IntegrationTime(/*[out,ref,retval]*/double* pVal)");
			expected.AppendLine("    {");
			expected.AppendLine("        CHECKPOINTER_R(pVal);");
			expected.AppendLine("        try");
			expected.AppendLine("        {");
			expected.AppendLine("            double pValValue = _get_IntegrationTime();");
			expected.AppendLine("            CHECKHR_R(Export(pValValue, pVal));");
			expected.AppendLine("            return S_OK;");
			expected.AppendLine("        }");
			expected.AppendLine("        mx_catch;");
			expected.AppendLine("    }");
			AssertIdlMethod(expected.ToString(), "[propget]   HRESULT IntegrationTime([out,ref,retval] double* pVal);");
		}

		[TestMethod]
		public void BuildIdlBaseTemplates_Writes_public_method_definitions_input_params()
		{
			var expected = new StringBuilder();
			expected.AppendLine("    virtual HRESULT STDMETHODCALLTYPE ChannelConnection(/*[in]*/IMxDetectorChannel* pCh, /*[out,ref,retval]*/IMxLineValueFeedInput** ppVal, /*[out,ref]*/VARIANT_BOOL* outBool, /*[in]*/VARIANT_BOOL inBool, /*[in]*/BSTR inString, /*[out,ref]*/BSTR* outString, /*[in]*/double inDouble, /*[out,ref]*/double* outDouble)");
			expected.AppendLine("    {");
			expected.AppendLine("        CHECKPOINTER_R(pCh);");
			expected.AppendLine("        CHECKPOINTER_R(ppVal);");
			expected.AppendLine("        CHECKPOINTER_R(outBool);");
			expected.AppendLine("        CHECKPOINTER_R(inString);");
			expected.AppendLine("        CHECKPOINTER_R(outString);");
			expected.AppendLine("        CHECKPOINTER_R(outDouble);");
			expected.AppendLine("        try");
			expected.AppendLine("        {");
			expected.AppendLine("            IMxDetectorChannelProxy pChProxy(pCh);");
			expected.AppendLine("            bool outBoolValue;");
			expected.AppendLine("            bool inBoolValue = VARIANT_FALSE != inBool;");
			//expected.AppendLine("            std::wstring inStringValue = inString;");
			expected.AppendLine("            std::wstring outStringValue;");
			expected.AppendLine("            double outDoubleValue;");
			expected.AppendLine("            IMxLineValueFeedInputProxy ppValValue = channelConnection(pChProxy, outBoolValue, inBoolValue, inString, outStringValue, inDouble, outDoubleValue);");
			expected.AppendLine("            CHECKHR_R(ppValValue.ref().CopyTo(ppVal));");
			expected.AppendLine("            CHECKHR_R(Export(outBoolValue, outBool));");
			expected.AppendLine("            CHECKHR_R(Export(outStringValue, outString));");
			expected.AppendLine("            CHECKHR_R(Export(outDoubleValue, outDouble));");
			expected.AppendLine("            return S_OK;");
			expected.AppendLine("        }");
			expected.AppendLine("        mx_catch;");
			expected.AppendLine("    }");
			AssertIdlMethod(expected.ToString(), "    HRESULT ChannelConnection      ( [in] IMxDetectorChannel* pCh, [out,ref,retval] IMxLineValueFeedInput** ppVal, [out,ref] VARIANT_BOOL* outBool, [in] VARIANT_BOOL inBool, [in] BSTR inString, [out,ref] BSTR* outString, [in] double inDouble, [out,ref] double* outDouble );");
		}

		[TestMethod]
		public void BuildIdlBaseTemplates_Writes_protected_method_definitions()
		{
			AssertProtectedMethod("    virtual void testMethod() = 0;\r\n", "   HRESULT TestMethod();");
			AssertProtectedMethod("    virtual double _get_IntegrationTime() = 0;\r\n", "[propget]   HRESULT IntegrationTime([out,ref,retval] double* pVal);");
			AssertProtectedMethod("    virtual void testMethod(/*[in]*/int pVal) = 0;\r\n", " HRESULT TestMethod([in] int pVal);");
			AssertProtectedMethod("    virtual void testMethod(/*[in]*/const IMxLogProxy& pVal) = 0;\r\n", " HRESULT TestMethod([in] IMxLog* pVal);");
			AssertProtectedMethod("    virtual IMxLogProxy myLogger() = 0;\r\n", " HRESULT MyLogger([out,ref,retval] IMxLog* pVal);");
			AssertProtectedMethod("    virtual void testMethod(/*[out,ref]*/bool& pVal) = 0;\r\n", " HRESULT TestMethod([out,ref] VARIANT_BOOL* pVal);");
		}

		[TestMethod]
		public void BuildIdlBaseTemplates_Generates_protected_method_name()
		{
			Assert.AreEqual("writeCount", generator.protectedMethodName("WriteCount"), "Upper CamelCase supported");
			Assert.AreEqual("_writeCount", generator.protectedMethodName("writeCount"), "Lower camelCase supported");
		}

		[TestMethod]
		public void BuildIdlBaseTemplates_Enumerates_methods()
		{
			var methodSignatures = new[] 
			{
				"	[propget]	HRESULT WriteCount					( [out,ref,retval] long* pVal );",
				"	[propget]	HRESULT ReadCount					( [out,ref,retval] long* pVal );",
				"				HRESULT Close						();"
			};
			var actionCalled = 0;
			generator.forMethods(methodSignatures, (methodName, attributes, parameters) =>
				{
					actionCalled++;
				});

			Assert.AreEqual(3, actionCalled, "Enumerate three methods");
		}

		[TestMethod]
		public void BuildIdlBaseTemplates_Splits_method_into_parts()
		{
			var methodSignature = "	[propget]	HRESULT Begin	( [out,retval] long* pVal );";
			int actionCalled = 0;

			generator.forMethod(methodSignature, (methodName, attributes, parameters) =>
			{
				actionCalled++;
				Assert.AreEqual("Begin", methodName, "Method name");
				Assert.AreEqual("propget", attributes, "Attributes");
				var p = parameters.ToArray();
				Assert.AreEqual(1, p.Length, "Parameters");
				Assert.AreEqual("out|retval", string.Join("|", p[0].Attributes.ToArray()));
				Assert.AreEqual("long*", p[0].Type);
				Assert.AreEqual("pVal", p[0].Name);
				Assert.AreEqual("", p[0].Suffix);
			});

			Assert.AreEqual(1, actionCalled, "Action called exactly once per signature");
		}

		[TestMethod]
		public void BuildIdlBaseTemplates_Generates_output_filenames()
		{
			var tempPath = Path.GetFullPath(Environment.ExpandEnvironmentVariables("%TEMP%"));
			var outputPath = Path.Combine(tempPath, "Output");
			if (Directory.Exists(outputPath))
				Directory.Delete(outputPath);
			var expected = Path.Combine(outputPath, "TestIdlFileBase.h");

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

		void AssertIdlMethod(string expected, string idlMethod)
		{
			var actual = capture((writer) =>
			{
				var method = helper.parseMethod(idlMethod);
				var parameters = helper.splitParameters(method.Parameters);
				generator.writeIdlMethodForFunction(method.Name, method.Attributes, parameters, writer);
			});
			Assert.AreEqual(expected, actual);
		}

		void AssertProtectedMethod(string expected, string idlMethod)
		{
			var actual = capture((writer) =>
			{
				var method = helper.parseMethod(idlMethod);
				var parameters = helper.splitParameters(method.Parameters);
				generator.writeProtectedMethodForFunction(method.Name, method.Attributes, parameters, writer);
			});
			Assert.AreEqual(expected, actual);
		}

		#endregion Assertion helpers
	}
}
