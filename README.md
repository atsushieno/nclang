# What is NClang?

NClang is a C# binding to LibClang. LibClang is a language service for C, C++ and Objective-C. A language service is a service (mostly as a library) that provides functionality to support writing programs in certain languages, such as a database for user program and its included files, or code completion engine that works against unsaved sources (text strings).

LibClang is part of Clang project which is part of LLVM project. Clang itself is a C/C++/ObjC compiler frontend for LLVM, and LLVM is the compiler infrastructure for several language frontends and backend binary formats.

NClang so far targets only LibClang. Note that Clang project has more language service like tools and libraries such as LibTooling (written in C++).

NClang is a hand-written C# binding that aims to bind every LibClang C API using P/Invoke (DllImport). The public API is to offer somewhat wrapped library around the native P/Invoke API which is internal within NClang.dll. The primary namespace is `NClang`.

The native binding API is based on the documentation.
http://clang.llvm.org/doxygen/group__CINDEX.html

Clang API is documented per feature category, by the Clang dev. team. NClang API is designed to match its subject (as .NET is basically an object oriented infrastructure). There is most likely CXFooBar internal marshal class for each ClangFooBar public class. Every function that does not take Clang-specific type is collected into ClangService static class.

Some important classes are:

* ClangIndex: represents an API entrypoint for LibClang, mostly to create TranslationUnit or IndexAction. CXIndex holds a set of translation units.
* ClangTranslationUnit: represents an abstract parsed source. 
* ClangUnsavedFile represents a virtual "file" resources. It consists of a filename and its content string.
* ClangCursor: represents a "cursor" in a source document.
* ClangSourceLocation: represents a location in a source document, indicating file, line number and column number, and/or offset in a file.
* ClangSourceRange: represents a range between two source locations.
* ClangDiagnostic/-Set: give source analysis results as in error, warning and fix candidates.
* ClangCodeCompleteResults, ClangCompletionResult and ClangCompletionString: support code completion.
* ClangCompilationDatabase, ClangCompileCommands and ClangCompileCommand: seem to support "compilation database" which can be built from e.g. CMake output "compile_commands.json".
* ClangIndexAction and ClangIndexerCallbacks: used to construct code index database, using callbacks that run through a file (either via filename or a ClangTranslationUnit) to be parsed.
* ClangIndexInfo and all those derived classes: are passed as arguments in the callbacks represented as ClangIndexerCallbacks, to represent the node information to be indexed.
* ClangToken/-Set: are returned as results of tokenization from translation unit.
* ClangString: represents a string (CXString in native API), that wraps libclang's conceptual "string" that offers the actual string content.

Typically a ClangTranslationUnit is parsed from files (either filename string or ClangUnsavedFile objects) using ClangIndex.ParseTranslationUint() method.

The public API is designed to be close to the native ones, but sometimes that results in annoyance (e.g. ClangIndexerCallback expects IntPtr and some types that takes cryptic IntPtr thing).

# What we can (and cannot) do with NClang?

NClang is just a wrapper around LibClang, and LibClang is not an ultimate language service implementation. It has pretty much limited functionality. What it supports are:

* code completion, via ClangTranslationUnit.CodeCompleteAt()
* tokenization, via ClangTranslationUnit.Tokenize() that would help semantic syntax highlighting
* reference finder via ClangCursor.FindReferencesInFile()
* code issue finder, via ClangTranslationUnit.DiagnosticSet
* goto definition (indirectly), via ClangIndexAction.IndexSourceFile()
* member list toolbar, via ClangIndexAction.IndexSourceFile()

What LibClang does NOT support:

* comprehensive unsaved buffer support, probably. CXUnsavedFile only contains "contents" string without solid document structure.
* any refactoring operation that involves updating sources: (1)code formatting (clang::format) (2)renaming
* (more to list up, but you'd get the point)

Some features are supported in other parts of Clang such as LibTooling (clang::tooling namespace) but since there is no P/Invoke-able API, I cannot bind them right now. Probably CppSharp or Swig is required.
