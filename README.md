# IDL-Wrapper-Generator
IDL-Wrapper-Generator is a Visual Studio T4 code-generator that creates proxies and base-templates around user-defined IDLs.

##Why use the generator instead of using the raw IDL-classes directly?
As a technology, COM isn't really object oriented. It supports the C-style code format rather than that of C++. As such, to consume a COM object, you need to send in-, out- and return values as parameters to each call, and always check the _HRESULT_ for errors. The _HRESULT_ is the only way a COM class can throw an exception back to the client.

COM also has its own types of boolean and string; _VARIANT_BOOL_ and _BSTR_. This wrapper automatically converts them to **bool** and **std::wstring**.

The generated proxy objects fixes the plumbing of the calls, converts parameters to proper COM types, throws when a COM _HRESULT_ "exception" is encountered and returns the _retval_ of the IDL.

###It's not that you can't do this yourself
It's that the business logic in your own code gets so much more readable then you get all this COM-pumbing out of the way!

#How to use the templates
The IDL-Wrapper-Generator packages its two main T4 templates, **BuildIdlProxies.tt** and **BuildIdlBaseTemplates.tt**, together with some helper files and wraps it all up in a C#-project.
