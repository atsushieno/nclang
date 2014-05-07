using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{

	public class ClangIndexerCallbacks
	{
		public event Func<IntPtr,bool> AbortQuery;
		public event Action<IntPtr,ClangDiagnosticSet> Diagnostic;
		public event Func<IntPtr,ClangFile,ClangIndex.ClientFile> EnteredMainFile;
		public event Func<IntPtr,ClangIndex.IncludedFileInfo,ClangIndex.ClientFile> PreprocessIncludedFile;
		public event Func<IntPtr,ClangIndex.ImportedAstFileInfo,ClangIndex.ClientAstFile> ImportedAstFile;
		public event Func<IntPtr,ClangIndex.ContainerInfo> StartedTranslationUnit;
		public event Action<IntPtr,ClangIndex.DeclarationInfo> IndexDeclaration;
		public event Action<IntPtr,ClangIndex.EntityReferenceInfo> IndexEntityReference;

		internal IndexerCallbacks ToNative ()
		{
			var ret = new IndexerCallbacks ();
			if (AbortQuery != null)
				ret.AbortQuery = (clientData, reserved) => AbortQuery (clientData) ? 1 : 0;
			if (Diagnostic != null)
				ret.Diagnostic = (clientData, ds, reserved) => Diagnostic (clientData, new ClangDiagnosticSet (ds));
			if (EnteredMainFile != null)
				ret.EnteredMainFile = (clientData, f, reserved) => EnteredMainFile (clientData, new ClangFile (f)).Address;
			if (PreprocessIncludedFile != null)
				ret.PpIncludedFile = (IntPtr clientData, IntPtr includedFile) => PreprocessIncludedFile (clientData, new ClangIndex.IncludedFileInfo (includedFile)).Address;
			if (ImportedAstFile != null)
				ret.ImportedASTFile = (IntPtr clientData, IntPtr importedAstFile) => ImportedAstFile (clientData, new ClangIndex.ImportedAstFileInfo (importedAstFile)).Address;
			if (StartedTranslationUnit != null)
				ret.StartedTranslationUnit = (clientData, reserved) => StartedTranslationUnit (clientData).Address;
			if (IndexDeclaration != null)
				ret.IndexDeclaration = (IntPtr clientData, IntPtr declInfo) => IndexDeclaration (clientData, new ClangIndex.DeclarationInfo (declInfo));
			if (IndexEntityReference != null)
				ret.IndexEntityReference = (IntPtr clientData, IntPtr entRefInfo) => IndexEntityReference (clientData, new ClangIndex.EntityReferenceInfo (entRefInfo));

			return ret;
		}
	}
}
