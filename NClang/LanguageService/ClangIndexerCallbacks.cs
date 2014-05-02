using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{

	public class ClangIndexerCallbacks
	{
		public event Func<IntPtr,int> AbortQuery;
		public event Action<IntPtr,ClangDiagnosticSet> Diagnostic;
		public event Func<IntPtr,ClangFile,ClangIndexClientFile> EnteredMainFile;
		public event Func<IntPtr,ClangIndexedFileInfo,ClangIndexClientFile> PreprocessIncludedFile;
		public event Func<IntPtr,ClangIndexImportedAstFileInfo,ClangIndexClientAstFile> ImportedAstFile;
		public event Func<IntPtr,ClangIndexContainerInfo> StartedTranslationUnit;
		public event Action<IntPtr,ClangIndexDeclarationInfo> IndexDeclaration;
		public event Action<IntPtr,ClangIndexEntityReferenceInfo> IndexEntityReference;

		internal IndexerCallbacks ToNative ()
		{
			var ret = new IndexerCallbacks ();
			//if (AbortQuery != null)
			ret.AbortQuery = (clientData, reserved) => AbortQuery (clientData);
			//if (Diagnostic != null)
			ret.Diagnostic = (clientData, ds, reserved) => Diagnostic (clientData, new ClangDiagnosticSet (ds));
			//if (EnteredMainFile != null)
			ret.EnteredMainFile = (clientData, f, reserved) => EnteredMainFile (clientData, new ClangFile (f)).Handle;
			//if (PreprocessIncludedFile != null)
			ret.PpIncludedFile = (IntPtr clientData, ref CXIdxIncludedFileInfo includedFile) => PreprocessIncludedFile (clientData, new ClangIndexedFileInfo (includedFile)).Handle;
			//if (ImportedAstFile != null)
			ret.ImportedASTFile = (IntPtr clientData, ref CXIdxImportedASTFileInfo importedAstFile) => ImportedAstFile (clientData, new ClangIndexImportedAstFileInfo (importedAstFile)).Handle;
			//if (StartedTranslationUnit != null)
			ret.StartedTranslationUnit = (clientData, reserved) => StartedTranslationUnit (clientData).Address;
			//if (IndexDeclaration != null)
			ret.IndexDeclaration = (IntPtr clientData, IntPtr declInfo) => IndexDeclaration (clientData, new ClangIndexDeclarationInfo (declInfo));
			//if (IndexEntityReference != null)
			ret.IndexEntityReference = (IntPtr clientData, IntPtr entRefInfo) => IndexEntityReference (clientData, new ClangIndexEntityReferenceInfo (entRefInfo));

			return ret;
		}
	}
}
