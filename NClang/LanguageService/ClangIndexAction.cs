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

		public ClangTranslationUnit IndexSourceFile (IntPtr clientData, ClangIndexerCallbacks [] indexCallbacks, IndexOptFlags options, string sourceFileName, string [] commandLineArgs, ClangUnsavedFile [] unsavedFiles, TranslationUnitFlags translationUnitOptions)
		{
			if (indexCallbacks == null)
				throw new ArgumentNullException ("indexCallbacks");

			var cbs = indexCallbacks.Select (ic => ic.ToNative ()).ToArray ();
			IntPtr tu;
			var uf = unsavedFiles.Select (u => new CXUnsavedFile (u.FileName, u.Contents)).ToArray ();
			var ret = LibClang.clang_indexSourceFile (Handle, clientData, cbs, (uint) cbs.Length, options, sourceFileName, commandLineArgs, commandLineArgs.Length, uf, (uint) unsavedFiles.Length, out tu, translationUnitOptions);
			if (ret != 0)
				throw new ClangServiceException ("Faied to index source file");
			return new ClangTranslationUnit (tu);
		}

		public void IndexTranslationUnit (IntPtr clientData, ClangIndexerCallbacks [] indexCallbacks, IndexOptFlags options, ClangTranslationUnit translationUnit)
		{
			if (indexCallbacks == null)
				throw new ArgumentNullException ("indexCallbacks");
			if (translationUnit == null)
				throw new ArgumentNullException ("translationUnit");

			var cbs = indexCallbacks.Select (ic => ic.ToNative ()).ToArray ();
			var ret = LibClang.clang_indexTranslationUnit (Handle, clientData, cbs, (uint) cbs.Length, options, translationUnit.Handle);
			if (ret != 0)
				throw new ClangServiceException ("Faied to index translation unit: " + translationUnit.TranslationUnitSpelling);
		}
	}

}
