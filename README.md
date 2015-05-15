# IDL-Wrapper-Generator
IDL-Wrapper-Generator is a Visual Studio T4 code-generator that creates proxies and base-templates around user-defined IDLs.

##Why use the generator instead of using the raw IDL-classes directly?
As a technology, COM isn't really object oriented. It supports the C-style code format rather than that of C++. As such, to consume a COM object, you need to send in-, out- and return values as parameters to each call, and always check the _HRESULT_ for errors. The _HRESULT_ is the only way a COM class can throw an exception back to the client.

COM also has its own types of boolean and string; _VARIANT_BOOL_ and _BSTR_. This wrapper automatically converts them to **bool** and **std::wstring**.

The generated proxy objects fixes the plumbing of the calls, converts parameters to proper COM types, throws when a COM _HRESULT_ "exception" is encountered and returns the _retval_ of the IDL.

###It's not that you can't do this yourself
It's that the business logic in your own code gets so much more readable then you get all this COM-pumbing out of the way!

##Project components
The IDL-Wrapper-Generator packages its two main T4 templates, **BuildIdlProxies.tt** and **BuildIdlBaseTemplates.tt**, together with some helper files and wraps it all up in a C#-project.

To test-drive the T4 templates and properly unit test them, they are split into a "view" (which is the acual *.tt*-file) and a *code-behind* (which is called *.tt.cs*). The unit tests are included in the folder called *Unit Tests*.

Included in the project is also a *Demo* folder in which a demo IDL file is included. You ware free to change this file or add adjacent IDL-files to see the templates at work. As long as the original configuration file isn't changed, the resulting proxies and base templates can be seen in the *Output* folder.

The templates are automatically run every time the project is rebuilt.

##How to use the templates with your production code
Start by downloading this project and add it to your existing solution.
- Edit the file *BuildIdl.tt*;
  -   Change the SourcePath to a path from which the generator shall search for IDL-files to wrap.
  -   Change the OutputPath to the destination where the generated c++-files shall be written.
-   Build the project to generate the wrappers.
-   Make a reference to the file *Ref.h*, which is found in the *Demo*-folder.
-   Replace your current references to your IDLs generated outputs with those of the proxies or base templates. You only need to reference one of them.
