using System;
using System.Linq;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
    /// <summary>
    /// An indexing action/session, to be applied to one or multiple
    /// translation units.
    /// </summary>
	public class ClangIndexAction : ClangObject, IDisposable
	{
		public ClangIndexAction (IntPtr handle)
			: base (handle)
		{
		}

		public void Dispose ()
		{
			LibClang.clang_IndexAction_dispose (Handle);
		}

        /// <summary>
        /// Index the given source file and the translation unit corresponding
        /// to that file via callbacks implemented through <see cref="ClangIndexerCallbacks"/>.
        /// </summary>
        /// <param name="clientData">data supplied by the client, which will
        /// be passed to the invoked callbacks.</param>
        /// <param name="indexCallbacks">Pointer to indexing callbacks that the client
        /// implements.</param>
        /// <param name="options">A bitmask of options that affects how indexing is
        /// performed. This should be a bitwise OR of the <see cref="IndexOptionFlags"/> flags.</param>
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
        /// <param name="translationUnitOptions">
        /// A bitmask of options that affects how the translation unit
        /// is managed but not its compilation. This should be a bitwise OR of the
        /// <see cref="TranslationUnitFlags"/> flags.
        /// </param>
        /// <returns><see cref="ClangTranslationUnit"/> that can be reused after indexing is finished.</returns>
		public ClangTranslationUnit IndexSourceFile (IntPtr clientData, ClangIndexerCallbacks [] indexCallbacks, IndexOptionFlags options, string sourceFileName, string [] commandLineArgs, ClangUnsavedFile [] unsavedFiles, TranslationUnitFlags translationUnitOptions)
		{
			if (indexCallbacks == null)
				throw new ArgumentNullException ("indexCallbacks");

			var cbs = indexCallbacks.Select (ic => ic.ToNative ()).ToArray ();
			IntPtr tu;
			var uf = unsavedFiles.ToNative ();
			var ret = LibClang.clang_indexSourceFile (Handle, clientData, cbs, (uint) cbs.Length, options, sourceFileName, commandLineArgs, commandLineArgs.Length, uf, (uint) uf.Length, out tu, translationUnitOptions);
			if (ret != 0)
				throw new ClangServiceException ("Faied to index source file");
			return new ClangTranslationUnit (tu);
		}

        /// <summary>
        /// Index the given translation unit via callbacks implemented through <see cref="ClangIndexerCallbacks"/>.
        /// </summary>
        /// <remarks>
        /// The order of callback invocations is not guaranteed to be the same as
        /// when indexing a source file. The high level order will be:
        /// 
        ///   -Preprocessor callbacks invocations
        ///   -Declaration/reference callbacks invocations
        ///   -Diagnostic callback invocations
        /// </remarks>
        /// <param name="clientData">data supplied by the client, which will
        /// be passed to the invoked callbacks.</param>
        /// <param name="indexCallbacks">Pointer to indexing callbacks that the client
        /// implements.</param>
        /// <param name="options">A bitmask of options that affects how indexing is
        /// performed. This should be a bitwise OR of the <see cref="IndexOptionFlags"/> flags.</param>
        /// <param name="translationUnit"></param>
		public void IndexTranslationUnit (IntPtr clientData, ClangIndexerCallbacks [] indexCallbacks, IndexOptionFlags options, ClangTranslationUnit translationUnit)
		{
			if (indexCallbacks == null)
				throw new ArgumentNullException ("indexCallbacks");
			if (translationUnit == null)
				throw new ArgumentNullException ("translationUnit");

			var cbs = indexCallbacks.Select (ic => ic.ToNative ()).ToArray ();
			var ret = LibClang.clang_indexTranslationUnit (Handle, clientData, cbs, (uint) (cbs.Length * Marshal.SizeOf (typeof(IndexerCallbacks))), options, translationUnit.Handle);
			if (ret != 0)
				throw new ClangServiceException (string.Format ("Faied to index translation unit: {0} Reason: {1}", translationUnit.TranslationUnitSpelling, ret));
		}
	}

}
