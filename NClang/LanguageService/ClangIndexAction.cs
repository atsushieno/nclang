using System;
using System.Linq;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
	
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

		public void IndexTranslationUnit (IntPtr clientData, ClangIndexerCallbacks [] indexCallbacks, IndexOptionFlags options, ClangTranslationUnit translationUnit)
		{
			if (indexCallbacks == null)
				throw new ArgumentNullException ("indexCallbacks");
			if (translationUnit == null)
				throw new ArgumentNullException ("translationUnit");

			var cbs = indexCallbacks.Select (ic => ic.ToNative ()).ToArray ();
			var ret = LibClang.clang_indexTranslationUnit (Handle, clientData, cbs, (uint) (cbs.Length * Marshal.SizeOf<IndexerCallbacks> ()), options, translationUnit.Handle);
			if (ret != 0)
				throw new ClangServiceException (string.Format ("Faied to index translation unit: {0} Reason: {1}", translationUnit.TranslationUnitSpelling, ret));
		}
	}

}
