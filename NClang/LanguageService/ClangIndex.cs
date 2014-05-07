using System;
using System.Linq;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
	
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
		
		public GlobalOptionFlags GlobalOptions {
			get { return LibClang.clang_CXIndex_getGlobalOptions (Handle); }
			set { LibClang.clang_CXIndex_setGlobalOptions (Handle, value); }
		}
		
		// TranslationUnitManipulation
		
		public ClangTranslationUnit CreateTranslationUnitFromSourceFile (string sourceFilename, string [] clangCommandLineAgs, ClangUnsavedFile [] unsavedFiles)
		{
			var cx = unsavedFiles.Select (o => new CXUnsavedFile (o.FileName, o.Contents)).ToArray ();
			return new ClangTranslationUnit (LibClang.clang_createTranslationUnitFromSourceFile (Handle, sourceFilename, clangCommandLineAgs.Length, clangCommandLineAgs, (uint) unsavedFiles.Length, cx));
		}
		
		public ClangTranslationUnit CreateTranslationUnit (string astFilename)
		{
			return new ClangTranslationUnit (LibClang.clang_createTranslationUnit (Handle, astFilename));
		}
		
		public ClangTranslationUnit ParseTranslationUnit (string sourceFilename, string [] commandLineArgs, ClangUnsavedFile [] unsavedFiles, TranslationUnitFlags options)
		{
			var files = unsavedFiles.Select (u => new CXUnsavedFile (u.FileName, u.Contents)).ToArray ();
			return new ClangTranslationUnit (LibClang.clang_parseTranslationUnit (Handle, sourceFilename, commandLineArgs, commandLineArgs.Length, files, (uint) files.Length, options));
		}

		// HighLevelApi
		public ClangIndexAction CreateIndexAction ()
		{
			return new ClangIndexAction (LibClang.clang_IndexAction_create (Handle));
		}
	}
}
