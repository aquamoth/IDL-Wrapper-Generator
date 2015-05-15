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


public class BuildIdlProxy
{
	readonly IdlHelper helper;
	readonly string templateFile;

	public BuildIdlProxy(string templateFile)
		: this(templateFile, new IdlHelper())
	{
	}

	public BuildIdlProxy(string templateFile, IdlHelper helper)
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

	void processIdlFile(string idlFile, TextWriter writer)
	{
		try
		{
			var idl = helper.idlContentWithoutComments(idlFile);
			helper.writeFileHeader(templateFile, writer);

			writeIncludeForIdlOutput(idlFile, writer);
			helper.writeIncludesForRequiredProxies(idl, writer);
			writer.WriteLine("");

			writeInterfaceForwarders(idl, writer);
			helper.forEachInterfaceIn(idl, (className, hresultMethods) => writeProxySkeletonFor(className, hresultMethods, writer));
			helper.forEachInterfaceIn(idl, (className, hresultMethods) => writeProxyMethodDefinitionsFor(className, hresultMethods, writer));
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Failed to parse IDL " + idlFile, ex);
		}
	}

	internal void writeIncludeForIdlOutput(string idlFile, TextWriter writer)
	{
		writer.WriteLine("#include \"{0}.h\"", Path.GetFileNameWithoutExtension(idlFile));
	}

	internal void writeInterfaceForwarders(string idl, TextWriter writer)
	{
		var pattern = @"interface\s+(\w+)\s*;";
		var regex = new Regex(pattern, helper.CommonRegexOptions);

		var matches = regex.Matches(idl);
		foreach (Match match in matches)
		{
			var className = match.Groups[1].Value;
			writer.WriteLine("class {0}Proxy;", className);
		}
		writer.WriteLine("");
	}

	void writeProxySkeletonFor(string className, string hresultMethods, TextWriter writer)
	{
		var proxyClassName = className + "Proxy";

		writer.WriteLine("class " + proxyClassName);
		writer.WriteLine("{");
		writer.WriteLine("public:");
		writer.WriteLine("    {0}() {{ }}", proxyClassName);
		writer.WriteLine("    {0}(const Ref<{1}>& principal) : m_principal(principal) {{ }}", proxyClassName, className);
		writer.WriteLine("    Ref<{0}> ref() const {{ return m_principal; }}", className);
		writer.WriteLine("    bool operator<(const {0}& rhs) const {{ return m_principal.operator<(rhs.m_principal); }}", proxyClassName);
		writer.WriteLine("    bool operator>(const {0}& rhs) const {{ return m_principal.operator>(rhs.m_principal); }}", proxyClassName);
		writer.WriteLine("    bool operator!=(const {0}& rhs) const {{ return m_principal.operator!=(rhs.m_principal); }}", proxyClassName);
		writer.WriteLine("    bool operator==(const {0}& rhs) const {{ return m_principal.operator==(rhs.m_principal); }}", proxyClassName);
		writer.WriteLine("    {0}** operator&() {{ return m_principal.operator&(); }}", className);
		writer.WriteLine("    operator Ref<{0}>() const {{ return m_principal; }}", className);
		writer.WriteLine("    ISafePtr<{0}>* operator->() const {{ return m_principal.operator->(); }}", className);

		var hresultMethodsArray = helper.splitHRESULTmethods(hresultMethods).ToArray();
		foreach (var method in hresultMethodsArray)
		{
			try
			{
				writeMethodSkeletonFor(method, writer);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Failed to write method for " + className, ex);
			}
		}

		writer.WriteLine("");
		writer.WriteLine("private:");
		writer.WriteLine("    Ref<{0}> m_principal;", className);
		writer.WriteLine("};");
		writer.WriteLine("");
	}

	void writeProxyMethodDefinitionsFor(string className, string hresultMethods, TextWriter writer)
	{
		var proxyClassName = className + "Proxy";

		var hresultMethodsArray = helper.splitHRESULTmethods(hresultMethods).ToArray();
		foreach (var method in hresultMethodsArray)
		{
			try
			{
				writeMethodDefinitionFor(proxyClassName, method, writer);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Failed to write method for " + className, ex);
			}
		}
	}

	internal void writeMethodSkeletonFor(string method, TextWriter writer)
	{
		var idlMethod = helper.parseMethod(method);
		var parameters = helper.splitParameters(idlMethod.Parameters);
		writeMethodSkeletonAsFunction(idlMethod.Name, idlMethod.Attributes, parameters, writer);
	}
	void writeMethodSkeletonAsFunction(string methodName, string attributes, IEnumerable<Parameter> parameters, TextWriter writer)
	{
		var returnParameter = parameters.SingleOrDefault(p => p.IsA("retval"));
		var inputParameters = parameters.Where(p => !p.IsA("retval"));

		if (attributes == "propget")
			methodName = "get_" + methodName;

		string returnType = helper.returnTypeForWrapper(returnParameter);

		writer.WriteLine("    {0} {1}({2}) const;", returnType, methodName, helper.parameterStringOf(inputParameters));
	}

	internal void writeMethodDefinitionFor(string proxyClassName, string method, TextWriter writer)
	{
		var idlMethod = helper.parseMethod(method);
		var parameters = helper.splitParameters(idlMethod.Parameters);
		writeMethodDefinitionAsFunction(proxyClassName, idlMethod.Name, idlMethod.Attributes, parameters, writer);
	}
	void writeMethodDefinitionAsFunction(string proxyClassName, string methodName, string attributes, IEnumerable<Parameter> parameters, TextWriter writer)
	{
		var returnParameter = parameters.SingleOrDefault(p => p.IsA("retval"));
		var inputParameters = parameters.Where(p => !p.IsA("retval"));

		if (attributes == "propget")
			methodName = "get_" + methodName;

		string returnTypeProxy = helper.returnTypeForWrapper(returnParameter);
		string internalVariableType, returnExpression;

		if (returnParameter == null)
		{
			internalVariableType = null;
			returnExpression = "";
		}
		else
		{
			var returnType = helper.returnTypeFor(returnParameter.Type);
			var returnClassName = helper.classNameOf(returnParameter.Type);
			if (returnType == "VARIANT_BOOL")
			{
				internalVariableType = returnClassName;
				returnExpression = "VARIANT_FALSE != " + returnParameter.Name;
			}
			else if (returnClassName == "BSTR")
			{
				internalVariableType = "std::wstring";
				returnExpression = returnParameter.Name;
			}
			else if (helper.unreferencedTypes.Contains(returnClassName))
			{
				internalVariableType = returnType;
				returnExpression = returnParameter.Name;
			}
			else if (helper.typeShouldBeProxied(returnParameter.Type))
			{
				internalVariableType = string.Format("Ref<{0}>", returnClassName);
				returnExpression = string.Format("{0}({1})", returnTypeProxy, returnParameter.Name);
			}
			else
			{
				internalVariableType = returnTypeProxy;
				returnExpression = returnParameter.Name;
			}
		}

		writer.WriteLine("{0} {3}::{1}({2}) const", returnTypeProxy, methodName, helper.parameterStringOf(inputParameters), proxyClassName);
		writer.WriteLine("{");

		if (!string.IsNullOrEmpty(internalVariableType))
		{
			writer.WriteLine("    {0} {1};", internalVariableType, returnParameter.Name);
		}

		foreach (var param in inputParameters.Where(p => p.IsA("out") && helper.typeShouldBeProxied(p.Type)))
		{
			writer.WriteLine("    Ref<{0}> {1}Ref;", helper.classNameOf(param.Type), param.Name);
		}

		writer.WriteLine("    CHECKHR_T( m_principal->{0}( {1} ) );", methodName, callStringOf(parameters));

		foreach (var param in inputParameters.Where(p => p.IsA("out") && helper.typeShouldBeProxied(p.Type)))
		{
			writer.WriteLine("    {1} = {0}Proxy({1}Ref);", helper.classNameOf(param.Type), param.Name);
		}

		if (!string.IsNullOrEmpty(returnExpression))
		{
			writer.WriteLine("    return {0};", returnExpression);
		}

		writer.WriteLine("}");
		writer.WriteLine("");
	}

	internal string callStringOf(IEnumerable<Parameter> parameters)
	{
		//Convert all parameters to proper call parameters to the underlying function
		if (parameters == null)
			return "";

		var parameterStrings = parameters.Select(p =>
		{
			var parameterType = p.Type.Trim();
			var parameterClassName = helper.classNameOf(p.Type);
			if (p.IsA("out"))
			{
				if (parameterClassName == "BSTR")
				{
					return string.Format("CMxBstr::out({0})", p.Name);
				}
				else if (parameterClassName == "VARIANT_BOOL")
				{
					if (p.IsA("retval"))
						return "&" + p.Name;
					else
						return string.Format("CMxBoolSetter::out({0})", p.Name);
				}
				else if (helper.unreferencedTypes.Contains(parameterClassName))
				{
					return "&" + p.Name;
				}
				else if (helper.typeShouldBeProxied(parameterType))
				{
					if (p.IsA("retval"))
						return "&" + p.Name;
					else
						return "&" + p.Name + "Ref";
				}
				else if (parameterType.EndsWith("**"))
				{
					if (p.IsA("retval"))
						return "&" + p.Name;
					else
						return p.Name;
				}
				else
				{
					return "&" + p.Name;
				}
			}
			else //[in] is DEFAULT //if(p.IsA("in"))
			{
				if (parameterType == "BSTR")
					return string.Format("CMxBstr::in({0})", p.Name);
				else if (helper.typeShouldBeProxied(parameterType))
					return string.Format("{0}.ref().ptr()", p.Name);
				else
					return p.Name;
			}
		});
		var result = string.Join(", ", parameterStrings.ToArray());
		return result;
	}


	internal string outputFileFor(string idlFile, string outputPath)
	{
		var idlDir = Path.GetDirectoryName(idlFile);
		var filename = Path.GetFileNameWithoutExtension(idlFile);
		var outputFilename = filename + "Proxy.h";
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
