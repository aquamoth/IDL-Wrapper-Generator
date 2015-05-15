//<#@ include file="Helpers.tt.cs" #>
//<#+
#if NOT_IN_T4
namespace IDL_Wrapper
{
	using System;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Collections.Generic;
	using System.IO;
#endif


public class BuildIdlBaseTemplates
{
	readonly IdlHelper helper;
	readonly string templateFile;

	public BuildIdlBaseTemplates(string templateFile) 
		: this(templateFile, new IdlHelper())
	{
	}

	public BuildIdlBaseTemplates(string templateFile, IdlHelper helper)
	{
		this.templateFile = templateFile;
		this.helper = helper;
	}

	public void processAll(string relativeRootPath, string relativeOutputPath)
	{
		var rootPath = helper.resolvePath(templateFile, relativeRootPath);
		var outputPath = helper.resolvePath(templateFile, relativeOutputPath);

		var allIdlFiles = Directory.GetFiles(rootPath, "*.idl", SearchOption.AllDirectories);
		WriteLine("Found {0} idl files in solution directory.", allIdlFiles.Count());

		helper.registerIdlEnumsAsUnreferencedTypes(allIdlFiles);

		foreach (var path in helper.filesToGenerate(templateFile, allIdlFiles, idlFile => outputFileFor(idlFile, outputPath)))
		{
			WriteLine("Creating {0}", path.Value);
			using (var writer = new StreamWriter(path.Value, false))
			{
				processIdlFile(path.Key, writer);
			}
		}
	}

	void processIdlFile(string idlFile, StreamWriter writer)
	{
		try
		{
			var idl = helper.idlContentWithoutComments(idlFile);
			helper.writeFileHeader(templateFile, writer);

			writeIncludeForIdlOutputProxy(idlFile, writer);
			helper.writeIncludesForRequiredProxies(idl, writer);
			writer.WriteLine("");

			//writeInterfaceForwarders(idl, writer);
			helper.forEachInterfaceIn(idl, (className, hresultMethods) => writeBaseTemplateFor(className, hresultMethods, writer));
		}
		catch(Exception ex)
		{
			throw new ApplicationException("Failed to parse IDL " + idlFile, ex);
		}
	}

	internal void writeIncludeForIdlOutputProxy(string idlFile, TextWriter writer)
	{
		writer.WriteLine("#include \"{0}Proxy.h\"", Path.GetFileNameWithoutExtension(idlFile));
	}

	void writeBaseTemplateFor(string className, string hresultMethods, StreamWriter writer)
	{
		WriteLine("    " + className);
		writer.WriteLine("template<class T>");
		writer.WriteLine("class {0}Base : public T", className);
		writer.WriteLine("{");
		writer.WriteLine("public:");

		var hresultMethodsArray = helper.splitHRESULTmethods(hresultMethods).ToArray();
		forMethods(hresultMethodsArray, (methodName, attributes, parameters) => {
			writeIdlMethodForFunction(methodName, attributes, parameters, writer);
		});

		writer.WriteLine("");
		writer.WriteLine("protected:");

		forMethods(hresultMethodsArray, (methodName, attributes, parameters) => {
			writeProtectedMethodForFunction(methodName, attributes, parameters, writer);
		});

		writer.WriteLine("};");
		writer.WriteLine("");
	}

	internal void writeTryCatch(TextWriter writer, Action action)
	{
		writer.WriteLine("        try");
		writer.WriteLine("        {");
		action();
		writer.WriteLine("        }");
		writer.WriteLine("        mx_catch;");
	}

	internal void writeIdlMethodForFunction(string methodName, string attributes, IEnumerable<Parameter> parameters, TextWriter writer)
	{
		var returnParameter = parameters.SingleOrDefault(p => p.IsA("retval"));
		var inputParameters = parameters.Where(p => !p.IsA("retval"));

		if (attributes == "propget")
			methodName = "get_" + methodName;
		var protectedName = protectedMethodName(methodName);


		writer.WriteLine("    virtual HRESULT STDMETHODCALLTYPE {0}({1})", methodName, callStringOf(parameters));
		writer.WriteLine("    {");

		writePointerChecksFor(parameters, writer);

		writeTryCatch(writer, () =>
		{
			writeInternalVariableDeclarations(writer, inputParameters);

			var internalCallString = internalCallStringFor(inputParameters);
			if (returnParameter == null)
			{
				writer.WriteLine("            {0}({1});", protectedName, internalCallString);
			}
			else
			{
				var returnName = string.Concat(returnParameter.Name, "Value");
				var returnType = helper.returnTypeForWrapper(returnParameter);
				writer.WriteLine("            {0} {3} = {1}({2});", returnType, protectedName, internalCallString, returnName);

				writeCopyInternalVariableToPublic(returnName, returnParameter, writer);
			}

			writeCopyBackFromInternalVariables(writer, inputParameters);

			writer.WriteLine("            return S_OK;");
		});
	
		writer.WriteLine("    }");
	}

	private string internalCallStringFor(IEnumerable<Parameter> parameters)
	{
		//Convert all parameters to proper call parameters to the underlying function
		if (parameters == null)
			return "";

		var parameterStrings = parameters.Select(p =>
		{
			var parameterType = helper.classNameOf(p.Type);
			if (p.IsA("out"))
			{
				if (parameterType == "BSTR")
				{
					return string.Concat(p.Name, "Value");
				}
				else if (parameterType == "VARIANT_BOOL")
				{
					return string.Concat(p.Name, "Value");
				}
				else if (helper.typeShouldBeProxied(parameterType))
				{
					return string.Concat(p.Name, "Proxy");
				}
				else if (helper.unreferencedTypes.Contains(parameterType))
				{
					return string.Concat(p.Name, "Value");
				}
				else
				{
					return p.Name;
				}
			}
			else //[in] is default //if(p.isa("in"))
			{
				if (parameterType == "VARIANT_BOOL")
					return string.Concat(p.Name, "Value");
				else if (helper.typeShouldBeProxied(parameterType))
				{
					return string.Concat(p.Name, "Proxy");
				}
				else 
					return p.Name;
			}
		});
		var result = string.Join(", ", parameterStrings.ToArray());
		return result;
	}

	private void writePointerChecksFor(IEnumerable<Parameter> parameters, TextWriter writer)
	{
		foreach (var parameter in parameters.Where(p => helper.isPointer(p.Type)))
		{
			writer.WriteLine("        CHECKPOINTER_R({0});", parameter.Name);
		}
	}

	private void writeInternalVariableDeclarations(TextWriter writer, IEnumerable<Parameter> parameters)
	{
		foreach (var parameter in parameters)
		{
			if (parameter.IsA("out"))
			{
				if (helper.typeShouldBeProxied(parameter.Type))
				{
					writer.WriteLine("            {0}Proxy {1}Proxy;", helper.classNameOf(parameter.Type), parameter.Name);
				}
				else if(helper.classNameOf(parameter.Type) == "BSTR")
				{
					writer.WriteLine("            std::wstring {0}Value;", parameter.Name);
				}
				else if (helper.isPointer(parameter.Type))
				{
					writer.WriteLine("            {0} {1}Value;", helper.returnTypeForWrapper(parameter), parameter.Name);
				}
			}
			else
			{
				if (helper.classNameOf(parameter.Type) == "VARIANT_BOOL")
				{
					writer.WriteLine("            bool {0}Value = VARIANT_FALSE != {0};", parameter.Name);
				}
				else if (helper.classNameOf(parameter.Type) == "BSTR")
				{
					//Not needed
				//	writer.WriteLine("            std::wstring {0}Value = {0};");
				}
				else if (helper.typeShouldBeProxied(parameter.Type))
				{
					writer.WriteLine("            {0}Proxy {1}Proxy({1});", helper.classNameOf(parameter.Type), parameter.Name);
				}
				else if (helper.isPointer(parameter.Type))
				{
					writer.WriteLine("            {0} {1}Value;", helper.returnTypeForWrapper(parameter), parameter.Name);
				}
			}
		}
	}

	private void writeCopyBackFromInternalVariables(TextWriter writer, IEnumerable<Parameter> inputParameters)
	{
		foreach (var parameter in inputParameters.Where(p => p.IsA("out")))
		{
			if (helper.typeShouldBeProxied(parameter.Type))
			{
				writeCopyInternalVariableToPublic(string.Concat(parameter.Name, "Proxy"), parameter, writer);
			}
			else if (helper.isPointer(parameter.Type))
			{
				writeCopyInternalVariableToPublic(string.Concat(parameter.Name, "Value"), parameter, writer);
			}
		}
	}

	private void writeCopyInternalVariableToPublic(string internalName, Parameter parameter, TextWriter writer)
	{
#warning Not under test
		if (helper.typeShouldBeProxied(parameter.Type))
		{
			writer.WriteLine("            CHECKHR_R({1}.ref().CopyTo({0}));", parameter.Name, internalName);
		}
		else
		{
			writer.WriteLine("            CHECKHR_R(Export({1}, {0}));", parameter.Name, internalName);
		}
	}

	internal string protectedParameterString(IEnumerable<Parameter> parameters)
	{
#warning Not under tests
		var protectedParameters = parameters.Select(p =>
		{
			if (helper.classNameOf(p.Type) == "BSTR")
			{
				if (p.IsA("out"))
					return string.Format("CMxBstr::out({0})", p.Name);
				else

					return "";
			}
			else if (helper.typeShouldBeProxied(p.Type))
				return string.Concat(p.Name, "Proxy");
			else if (helper.isPointer(p.Type))
				return string.Concat(p.Name, "Value");
			else
				return p.Name;
		}).ToArray();
		return string.Join(", ", protectedParameters);
	}

	internal string callStringOf(IEnumerable<Parameter> parameters)
	{
#warning not under tests!
		//Convert all parameters to proper call parameters to the underlying function
		if (parameters == null)
			return "";

		var parameterStrings = parameters.Select(p =>
		{
			return string.Format("/*[{3}]*/{0} {1}{2}", p.Type, p.Name, p.Suffix, string.Join(",", p.Attributes));
		});
		var result = string.Join(", ", parameterStrings.ToArray());
		return result;
	}

	internal void writeProtectedMethodForFunction(string methodName, string attributes, IEnumerable<Parameter> parameters, TextWriter writer)
	{
		var returnParameter = parameters.SingleOrDefault(p => p.IsA("retval"));

		if (attributes == "propget")
			methodName = "get_" + methodName;

		var returnType = helper.returnTypeForWrapper(returnParameter);
		var paramArray = helper.parameterStringOf(parameters);
		var signature = string.Format("    virtual {1} {0}({2}) = 0;", protectedMethodName(methodName), returnType, paramArray);
		writer.WriteLine(signature);
		WriteLine("    " + signature);
	}











	internal string protectedMethodName(string methodName)
	{
		var firstChar = methodName.Substring(0,1);
		if (firstChar.ToUpper() == firstChar)
			return firstChar.ToLower() + methodName.Substring(1);
		else
			return "_" + methodName;
	}


	internal void forMethods(IEnumerable<string> hresultMethodsArray, Action<string, string, IEnumerable<Parameter>> action)
	{
		foreach (var className in hresultMethodsArray)
		{
			try
			{
				forMethod(className, action);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Failed to write method for " + className, ex);
			}
		}
	}

	internal void forMethod(string method, Action<string, string, IEnumerable<Parameter>> action)
	{
		var optionalSeparatorPattern = @"(?:\s*,\s*)?";
		var attributesPattern = @"\[(?:(\w+)" + optionalSeparatorPattern + @")+\]";
		var methodNamePattern = @"(\w+)";
		var parametersPattern = @"\((.*?)\)";
		var pattern = @"\s*(?:" + attributesPattern + @")?\s*HRESULT\s+" + methodNamePattern + @"\s*" + parametersPattern + @"\s*;";

		var regex = new Regex(pattern, helper.CommonRegexOptions);
		var match = regex.Match(method);
		if (!match.Success)
			throw new ApplicationException("Failed to match: " + method.Trim());
		
		var attributes = match.Groups[1].Value;
		var methodName = match.Groups[2].Value;
		var parametersString = match.Groups[3].Value;

		try 
		{
			var parameters = helper.splitParameters(parametersString);
			action(methodName, attributes, parameters);
		}
		catch(Exception ex)
		{
			throw new ApplicationException("Failed to write method: " + methodName, ex);
		}
	}

	internal string outputFileFor(string idlFile, string outputPath)
	{
		var idlDir = Path.GetDirectoryName(idlFile);
		var filename = Path.GetFileNameWithoutExtension(idlFile);
		var outputFilename = filename + "Base.h";
		//var outputPath = Path.Combine(idlDir, "Output");
		Directory.CreateDirectory(outputPath);
		return Path.Combine(outputPath, outputFilename);
	}

	void WriteLine(string format, params object[] args)
	{
		System.Diagnostics.Trace.TraceInformation(format, args);
	}
}


#if NOT_IN_T4
} //end the namespace
#endif
//#>
