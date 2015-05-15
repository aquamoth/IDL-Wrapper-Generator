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




	public class IdlHelper
	{
		
		//
		// WRITE methods
		//
	

		public void writeFileHeader(string templateFile, TextWriter writer)
		{
			writer.WriteLine("/*");
			writer.WriteLine(" * Auto-generated code by {0}", templateFile);
			writer.WriteLine(" * Created at {0:yyyy-MM-dd HH:mm:ss}.", DateTime.Now);
			writer.WriteLine(" */");
			writer.WriteLine("");
			writer.WriteLine("#pragma once");
			writer.WriteLine("");
		}

		internal void writeIncludesForRequiredProxies(string idl, TextWriter writer)
		{
#warning not under tests
			var pattern = @"import\s+""(Mx\w+).idl""\s*;";
			var regex = new Regex(pattern, CommonRegexOptions);
			var matches = regex.Matches(idl);
			foreach (Match match in matches)
			{
				writer.WriteLine("#include \"{0}Proxy.h\"", match.Groups[1].Value);
			}
		}




		//
		// IDL parsing logic
		//

		public IEnumerable<string> splitHRESULTmethods(string rows)
		{
			var regex = new Regex(@"(.*?);", CommonRegexOptions);
			var matches = regex.Matches(rows);
			foreach(Match match in matches)
			{
				yield return match.Groups[0].Value;
			}
		}

		public IdlMethod parseMethod(string method)
		{
			var optionalSeparatorPattern = @"(?:\s*,\s*)?";
			var attributesPattern = @"\[(?:(\w+)" + optionalSeparatorPattern + @")+\]";
			var methodNamePattern = @"(\w+)";
			var parametersPattern = @"\((.*?)\)";
			var pattern = @"\s*(?:" + attributesPattern + @")?\s*HRESULT\s+" + methodNamePattern + @"\s*" + parametersPattern + @"\s*;";

			var regex = new Regex(pattern, CommonRegexOptions);
			var match = regex.Match(method);
			if (!match.Success)
				throw new ApplicationException("Failed to match: " + method.Trim());

			var attributes = match.Groups[1].Value;
			var methodName = match.Groups[2].Value;
			var parametersString = match.Groups[3].Value;

			var idlMethod = new IdlMethod
			{
				Attributes = attributes,
				Name = methodName,
				Parameters = parametersString
			};
			return idlMethod;
		}

		public IEnumerable<Parameter> splitParameters(string parameters)
		{
			if (isNullOrWhitespace(parameters))
				yield break;

			var optionPattern = @"(?:([(\w\(\)-]+)\s*,?\s*)";
			var typePattern = @"([(\w\(\)]+\s*\*{0,2})";
			var namePattern = @"(?:(\w+)(\[\])?)?";
			var pattern = @"\s*(?:\[\s*"+ optionPattern + @"*\])?\s*"+ typePattern + @"\s*" + namePattern;
			var regex = new Regex(pattern, CommonRegexOptions);
			var matches = regex.Matches(parameters);
			if (matches.Count == 0)
				throw new ApplicationException("No matches for: " + parameters);

			var index = 0;
			foreach (Match match in matches)
			{
				index++;
				var dataType = match.Groups[2].Value;
				var parameterName = match.Groups[3].Value;
				var parameterSuffix = match.Groups[4].Value;
				if (isNullOrWhitespace(parameterName))
				{
					parameterName = string.Concat("p", index);
				}
				var attributes = match.Groups[1].Captures.OfType<Capture>().Select(c=>c.Value);
				yield return new Parameter
				{ 
					Type = dataType.Trim(), 
					Name = parameterName, 
					Suffix = parameterSuffix,
					Attributes = attributes.ToArray()
				};
			}
		}

		public void registerIdlEnumsAsUnreferencedTypes(IEnumerable<string> allIdlFiles)
		{
			var idlEnums = allIdlFiles.SelectMany(idlFile => 
				findEnumsInIdlFile(idlFile)
				).ToArray();
			_unreferencedTypes.AddRange(idlEnums);
		}

		IEnumerable<string> findEnumsInIdlFile(string idlFile)
		{
			var idl = idlContentWithoutComments(idlFile);

			var pattern = @"typedef.*?enum\s+\w+\s*\{.*?\}\s*(\w+)\s*;";
			var regex = new Regex(pattern, CommonRegexOptions);
			var matches = regex.Matches(idl);
			foreach (Match match in matches)
			{
				yield return match.Groups[1].Value;
			}
		}

		public void forEachInterfaceIn(string idl, Action<string, string> action)
		{
			var pattern = @"\[.*?\]\s+interface\s+(\w+)[\w\s:]*?\{(.*?)\}";
			var regex = new Regex(pattern, CommonRegexOptions);

			var matches = regex.Matches(idl);
			foreach (Match match in matches)
			{
				var className = match.Groups[1].Value;
				var hresultMethods = match.Groups[2].Value;
				action(className, hresultMethods);
			}
		}


		//
		// Support methods
		//

		public bool isPointer(string type)
		{
			type = type.Trim();
			return type.EndsWith("*") || type.Equals("BSTR");
		}

		public bool typeShouldBeProxied(string type)
		{
			return (type.StartsWith("IMx") || type.StartsWith("Mx"));
		}

		public string classNameOf(string type)
		{
			type = type.Trim();
			var index = type.IndexOf("*");
			if (index >= 0)
			{
				type = type.Substring(0,index);
			}
			return type.Trim();
		}

		public string parameterStringOf(IEnumerable<Parameter> parameters)
		{
			//Convert all non-[retval] parameters to proper parameters in the Proxy method definition
			if (parameters == null)
				return "";

			parameters = parameters.Where(p => !(p.IsA("out") && p.IsA("retval")));
			var parameterStrings = parameters.Select(p =>
			{
				var parameterType = p.Type.Trim();
				var parameterComment = p.Attributes.Any() ? string.Format("/*[{0}]*/", string.Join(",", p.Attributes)) : "";
				if (p.IsA("out"))
				{
					if (typeShouldBeProxied(parameterType))
					{
						return string.Format("{2}{0}Proxy& {1}", classNameOf(parameterType), p.Name, parameterComment);
					}
					else if (classNameOf(parameterType) == "BSTR")
					{
						return string.Format("{0}std::wstring& {1}", parameterComment, p.Name);
					}
					else if (unreferencedTypes.Contains(classNameOf(parameterType)))
					{
						return string.Format("{0}{2}& {1}", parameterComment, p.Name, classNameOf(parameterType));
					}
					else if (classNameOf(parameterType) == "VARIANT_BOOL")
					{
						return string.Format("{0}bool& {1}", parameterComment, p.Name);
					}
					else
					{
#warning non-proxied [out] parameters are treated as [in] parameter for now
						return string.Format("{2}{0} {1}", parameterType, p.Name, parameterComment);
					}
				}
				else
				{
					if (parameterType == "SAFEARRAY(byte)")
					{
						return string.Concat(parameterComment, "SAFEARRAY* ", p.Name);
					}
					else if (parameterType == "BSTR")
					{
						return string.Concat(parameterComment, "const std::wstring& ", p.Name);
					}
					else if (typeShouldBeProxied(parameterType))
					{
						return string.Format("{2}const {0}Proxy& {1}", classNameOf(parameterType), p.Name, parameterComment);
					}
					else
					{
						return string.Format("{2}{0} {1}{3}", parameterType, p.Name, parameterComment, p.Suffix);
					}
				}
			});
			var result = string.Join(", ", parameterStrings.ToArray());
			return result;
		}

		[Obsolete("Try to use classNameOf() instead")]
		public string returnTypeFor(string type)
		{
			type = type.Trim();
			if (type[type.Length - 1] == '*')
				type = type.Substring(0, type.Length - 1);
			return type.Trim();
		}

		public string returnTypeForWrapper(Parameter parameter)
		{
			if (parameter == null)
				return "void";

			var returnType = returnTypeFor(parameter.Type);
			var parameterClass = classNameOf(parameter.Type);
			if (returnType == "VARIANT_BOOL")
				return "bool";

			if (returnType == "BSTR")
				return "std::wstring";

			if (unreferencedTypes.Contains(parameterClass))
				return returnType;

			if (typeShouldBeProxied(parameter.Type))
				return parameterClass + "Proxy";

			return string.Format("Ref<{0}>", parameterClass);
		}

		public string idlContentWithoutComments(string idlFile)
		{
			var idl = File.ReadAllText(idlFile);
			idl = new Regex(@"//.*").Replace(idl, "");
			idl = new Regex(@"/\*.*?\*/", CommonRegexOptions).Replace(idl, "");
			return idl;
		}

		public IEnumerable<KeyValuePair<string, string>> filesToGenerate(string templateFile, IEnumerable<string> allFiles, Func<string, string> outputFileFunction)
		{
			var templateLastWriteTime = File.GetLastWriteTime(getFullPathOfTemplateFile(templateFile));
			foreach (var inputFile in allFiles)
			{
				var outputPath = outputFileFunction(inputFile);
				var outputLastWriteTime = File.GetLastWriteTime(outputPath);
				if (!File.Exists(outputPath) || outputLastWriteTime < File.GetLastWriteTime(inputFile) || outputLastWriteTime < templateLastWriteTime)
				{
					yield return new KeyValuePair<string,string>(inputFile, outputPath);
				}
				else
				{
					WriteLine("Skipping {0} because it is already up to date.", outputPath);
				}
			}
		}

		public string resolvePath(string templateFile, string relativePath)
		{
			var absolutePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(getFullPathOfTemplateFile(templateFile)), relativePath));
			WriteLine("Resolved \"{0}\" to \"{1}\".", relativePath, absolutePath);
			return absolutePath;
		}

		string getFullPathOfTemplateFile(string templateFile)
		{
			if (Path.IsPathRooted(templateFile))
			{
				return templateFile;
			}
			else
			{
				//Required when running in a C++ project, since TemplateFile doesn't contain the full path then
				WriteLine("Calculating path to template file.");
				return Path.Combine(Directory.GetCurrentDirectory(), templateFile);
			}
		}

		List<string> _unreferencedTypes = new List<string>(new[]{ "__int64", "__int32", "long", "int", "byte", "short", "double", "float", "BSTR", "REFIID", "GUID", "IID", "HRESULT" } );
		public string[] unreferencedTypes { get { return _unreferencedTypes.ToArray(); } }

		public RegexOptions CommonRegexOptions { get { return RegexOptions.Multiline | RegexOptions.Singleline; } }

		public bool isNullOrWhitespace(string value) { return value == null || value.TrimEnd() == ""; }


		void WriteLine(string format, params object[] args)
		{
			System.Diagnostics.Trace.TraceInformation(format, args);
		}
	}



	//
	// Help classes
	//


	public class IdlMethod
	{
		public string Attributes { get; set; }
		public string Name { get; set; }
		public string Parameters { get; set; }
	}

	public class Parameter
	{
		public string[] Attributes { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Suffix { get; set; }

		public override string ToString()
		{
			return string.Format("[{2}] {0}{3} {1}", Type, Name, Attributes == null ? "" : string.Join(",",Attributes), Suffix);
		}

		public bool IsA(string attribute)
		{
			return this.Attributes.Contains(attribute);
		}
	}



#if NOT_IN_T4
} //end the namespace
#endif
//#>
