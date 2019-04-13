using System;
using System.Linq;
using System.Runtime.InteropServices;
using NClang.Natives;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
    /// <summary>
    /// An "index" that consists of a set of translation units that would
    /// typically be linked together into an executable or library.
    /// </summary>
	public partial class ClangIndex : ClangObject, IDisposable
	{
		// TopLevel
		
		internal ClangIndex (IntPtr handle)
			: base (handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
		}
		
		public void Dispose ()
		{
			LibClang.clang_disposeIndex (Handle);
		}
		
        /// <summary>
        /// Gets or sets general options associated with a <see cref="ClangIndex"/>.
        /// </summary>
		public GlobalOptionFlags GlobalOptions {
			get { return (GlobalOptionFlags) LibClang.clang_CXIndex_getGlobalOptions (Handle); }
			set { LibClang.clang_CXIndex_setGlobalOptions (Handle, (uint) value); }
		}
		
		// TranslationUnitManipulation

        /// <summary>
        /// Return the CXTranslationUnit for a given source file and the provided
        /// command line arguments one would pass to the compiler.
        /// </summary>
        /// <param name="sourceFilename">
        /// The name of the source file to load, or NULL if the
        /// source file is included in <paramref name="clangCommandLineAgs"/>.
        /// </param>
        /// <param name="clangCommandLineAgs">
        /// The command-line arguments that would be
        /// passed to the clang executable if it were being invoked out-of-process.
        /// These command-line options will be parsed and will affect how the translation
        /// unit is parsed. Note that the following options are ignored: '-c',
        /// '-emit-ast', '-fsyntax-only' (which is the default), and '-o \&lt;output file&gt;'.
        /// </param>
        /// <param name="unsavedFiles">
        /// The files that have not yet been saved to disk
        /// but may be required for code completion, including the contents of
        /// those files.  The contents and name of these files (as specified by
        /// CXUnsavedFile) are copied when necessary, so the client only needs to
        /// guarantee their validity until the call to this function returns.
        /// </param>
        /// <remarks>
        /// Note: The <paramref name="sourceFilename"/> argument is optional.  If the caller provides a
        /// NULL pointer, the name of the source file is expected to reside in the
        /// specified command line arguments.
        ///
        /// Note: When encountered in 'clang_command_line_args', the following options
        /// are ignored:
        ///
        ///   '-c'
        ///   '-emit-ast'
        ///   '-fsyntax-only'
        ///   '-o \&lt;output file&gt;'  (both '-o' and '\&lt;output file&gt;' are ignored)
        /// </remarks>
		public ClangTranslationUnit CreateTranslationUnitFromSourceFile (string sourceFilename, string [] clangCommandLineArgs, ClangUnsavedFile [] unsavedFiles)
		{
			var unsavedFilesNative = unsavedFiles.Select (o => new CXUnsavedFile () { Filename = o.FileName, Contents = o.Contents}).ToArray ().ToHGlobalNativeArray ();

			var cl = new NativeArrayHolder (clangCommandLineArgs.ToHGlobalAllocatedArray ());
			
			var ret = new ClangTranslationUnit (LibClang.clang_createTranslationUnitFromSourceFile (Handle, sourceFilename, clangCommandLineArgs.Length, cl.NativeArray, (uint) unsavedFiles.Length, unsavedFilesNative));
			ret.AddToFreeList (cl);
			ret.AddToFreeList (unsavedFilesNative);
			return ret;
		}

        /// <summary>
        /// Create a translation unit from an AST file (-emit-ast).
        /// </summary>
		public ClangTranslationUnit CreateTranslationUnit (string astFilename)
		{
			return new ClangTranslationUnit (LibClang.clang_createTranslationUnit (Handle, astFilename));
		}

        /// <summary>
        /// Parse the given source file and the translation unit corresponding
        /// to that file.
        /// </summary>
        /// <remarks>
        /// This routine is the main entry point for the Clang C API, providing the
        /// ability to parse a source file into a translation unit that can then be
        /// queried by other functions in the API. This routine accepts a set of
        /// command-line arguments so that the compilation can be configured in the same
        /// way that the compiler is configured on the command line.
        /// </remarks>
        /// <param name="sourceFilename">
        /// The name of the source file to load, or NULL if the
        /// source file is included in \p command_line_args.
        /// </param>
        /// <param name="commandLineArgs">
        /// The command-line arguments that would be
        /// passed to the clang executable if it were being invoked out-of-process.
        /// These command-line options will be parsed and will affect how the translation
        /// unit is parsed. Note that the following options are ignored: '-c', 
        /// '-emit-ast', '-fsyntax-only' (which is the default), and '-o \&lt;output file&gt;'.
        /// </param>
        /// <param name="unsavedFiles">
        /// the files that have not yet been saved to disk
        /// but may be required for parsing, including the contents of
        /// those files.  The contents and name of these files (as specified by
        /// CXUnsavedFile) are copied when necessary, so the client only needs to
        /// guarantee their validity until the call to this function returns.
        /// </param>
        /// <param name="options">
        /// A bitmask of options that affects how the translation unit
        /// is managed but not its compilation. This should be a bitwise OR of the
        /// <see cref="TranslationUnitFlags"/> flags.
        /// </param>
        /// <returns>
        /// A new translation unit describing the parsed code and containing
        /// any diagnostics produced by the compiler. If there is a failure from which
        /// the compiler cannot recover, returns <c>null</c>.
        /// </returns>
		public ClangTranslationUnit ParseTranslationUnit (string sourceFilename, string [] commandLineArgs, ClangUnsavedFile [] unsavedFiles, TranslationUnitFlags options)
		{
			var files = (unsavedFiles ?? new ClangUnsavedFile [0]).Select (u => new CXUnsavedFile () { Filename = u.FileName, Contents = u.Contents}).ToArray ().ToHGlobalNativeArray ();

			var cl = new NativeArrayHolder (commandLineArgs.ToHGlobalAllocatedArray ());
			
			var ret = new ClangTranslationUnit (LibClang.clang_parseTranslationUnit (Handle, sourceFilename, cl.NativeArray, (commandLineArgs ?? new string [0]).Length, files, (uint) unsavedFiles.Length, (uint) options));
			ret.AddToFreeList (cl);
			ret.AddToFreeList (files);
			return ret;
		}

        /// <summary>
        /// Parse the given source file and the translation unit corresponding
        /// to that file.
        /// </summary>
        /// <remarks>
        /// This routine is the main entry point for the Clang C API, providing the
        /// ability to parse a source file into a translation unit that can then be
        /// queried by other functions in the API. This routine accepts a set of
        /// command-line arguments so that the compilation can be configured in the same
        /// way that the compiler is configured on the command line.
        /// </remarks>
        /// <param name="sourceFilename">
        /// The name of the source file to load, or NULL if the
        /// source file is included in \p command_line_args.
        /// </param>
        /// <param name="commandLineArgs">
        /// The command-line arguments that would be
        /// passed to the clang executable if it were being invoked out-of-process.
        /// These command-line options will be parsed and will affect how the translation
        /// unit is parsed. Note that the following options are ignored: '-c', 
        /// '-emit-ast', '-fsyntax-only' (which is the default), and '-o \&lt;output file&gt;'.
        /// </param>
        /// <param name="unsavedFiles">
        /// the files that have not yet been saved to disk
        /// but may be required for parsing, including the contents of
        /// those files.  The contents and name of these files (as specified by
        /// CXUnsavedFile) are copied when necessary, so the client only needs to
        /// guarantee their validity until the call to this function returns.
        /// </param>
        /// <param name="options">
        /// A bitmask of options that affects how the translation unit
        /// is managed but not its compilation. This should be a bitwise OR of the
        /// <see cref="TranslationUnitFlags"/> flags.
        /// </param>
        /// <param name="translationUnit">
        /// A new translation unit describing the parsed code and containing
        /// any diagnostics produced by the compiler. If there is a failure from which
        /// the compiler cannot recover, returns <c>null</c>.
        /// </param>
        /// <returns>An <seealso cref="ErrorCode"/>.</returns>
        public ErrorCode ParseTranslationUnit(string sourceFilename, string [] commandLineArgs, ClangUnsavedFile [] unsavedFiles, TranslationUnitFlags options, out ClangTranslationUnit translationUnit)
        {
            var files = (unsavedFiles ?? new ClangUnsavedFile [0]).Select(u => new CXUnsavedFile() { Filename = u.FileName, Contents = u.Contents}).ToArray().ToHGlobalNativeArray ();
	    var cl = new NativeArrayHolder(commandLineArgs.ToHGlobalAllocatedArray ());
            IntPtr tuptr = IntPtr.Zero;
            var error = (ErrorCode) LibClang.clang_parseTranslationUnit2(Handle, sourceFilename, cl.NativeArray, commandLineArgs.Length, files, (uint) unsavedFiles?.Length, (uint) options, tuptr);
	    translationUnit = error == ErrorCode.Success ? new ClangTranslationUnit (Marshal.ReadIntPtr (tuptr)) : null;
	    translationUnit?.AddToFreeList (cl);
            return error;
        }

		// HighLevelApi
        /// <summary>
        /// An indexing action/session, to be applied to one or multiple
        /// translation units.
        /// </summary>
		public ClangIndexAction CreateIndexAction ()
		{
			return new ClangIndexAction (LibClang.clang_IndexAction_create (Handle));
		}
	}
}
