#if MANUAL

using System;
using System.Runtime.InteropServices;

using CXDiagnostic = System.IntPtr; // void*
using CXDiagnosticSet = System.IntPtr; // void*

using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_getNumDiagnosticsInSet (CXDiagnosticSet diags);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXDiagnostic clang_getDiagnosticInSet (CXDiagnosticSet diags, uint index);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXDiagnosticSet clang_loadDiagnostics (string file, out LoadDiagError error, out CXString errorString);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_disposeDiagnosticSet (CXDiagnosticSet diags);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXDiagnosticSet clang_getChildDiagnostics (CXDiagnostic d);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_getNumDiagnostics (CXTranslationUnit unit);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXDiagnostic clang_getDiagnostic (CXTranslationUnit unit, uint index);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXDiagnosticSet clang_getDiagnosticSetFromTU (CXTranslationUnit unit);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_disposeDiagnostic (CXDiagnostic diagnostic);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXString clang_formatDiagnostic (CXDiagnostic diagnostic, DiagnosticDisplayOptions options);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern DiagnosticDisplayOptions clang_defaultDiagnosticDisplayOptions ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern DiagnosticSeverity clang_getDiagnosticSeverity (CXDiagnostic _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceLocation clang_getDiagnosticLocation (CXDiagnostic _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_getDiagnosticSpelling (CXDiagnostic _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_getDiagnosticOption (CXDiagnostic diag, out CXString disable);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_getDiagnosticCategory (CXDiagnostic _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_getDiagnosticCategoryName (int category);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_getDiagnosticCategoryText (CXDiagnostic _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_getDiagnosticNumRanges (CXDiagnostic _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRange clang_getDiagnosticRange (CXDiagnostic diagnostic, uint range);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint clang_getDiagnosticNumFixIts (CXDiagnostic diagnostic);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString clang_getDiagnosticFixIt (CXDiagnostic diagnostic, uint dixIt, out CXSourceRange ReplacementRange);
	}
}

#endif
