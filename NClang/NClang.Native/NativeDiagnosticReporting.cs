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
		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_getNumDiagnosticsInSet (CXDiagnosticSet diags);

		[DllImport (LibraryName)]
		 internal static extern CXDiagnostic clang_getDiagnosticInSet (CXDiagnosticSet diags, [MarshalAs (UnmanagedType.SysUInt)] uint index);

		[DllImport (LibraryName)]
		 internal static extern CXDiagnosticSet clang_loadDiagnostics (string file, out LoadDiagError error, out CXString errorString);

		[DllImport (LibraryName)]
		 internal static extern void clang_disposeDiagnosticSet (CXDiagnosticSet diags);

		[DllImport (LibraryName)]
		 internal static extern CXDiagnosticSet clang_getChildDiagnostics (CXDiagnostic d);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_getNumDiagnostics (CXTranslationUnit unit);

		[DllImport (LibraryName)]
		 internal static extern CXDiagnostic clang_getDiagnostic (CXTranslationUnit unit, [MarshalAs (UnmanagedType.SysUInt)] uint index);

		[DllImport (LibraryName)]
		 internal static extern CXDiagnosticSet clang_getDiagnosticSetFromTU (CXTranslationUnit unit);

		[DllImport (LibraryName)]
		 internal static extern void clang_disposeDiagnostic (CXDiagnostic diagnostic);

		[DllImport (LibraryName)]
		internal static extern CXString clang_formatDiagnostic (CXDiagnostic diagnostic, [MarshalAs (UnmanagedType.SysUInt)] DiagnosticDisplayOptions options);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		internal static extern DiagnosticDisplayOptions clang_defaultDiagnosticDisplayOptions ();

		[DllImport (LibraryName)]
		 internal static extern DiagnosticSeverity clang_getDiagnosticSeverity (CXDiagnostic _);

		[DllImport (LibraryName)]
		 internal static extern CXSourceLocation clang_getDiagnosticLocation (CXDiagnostic _);

		[DllImport (LibraryName)]
		 internal static extern CXString clang_getDiagnosticSpelling (CXDiagnostic _);

		[DllImport (LibraryName)]
		internal static extern CXString clang_getDiagnosticOption (CXDiagnostic diag, ref IntPtr disable);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_getDiagnosticCategory (CXDiagnostic _);

		[DllImport (LibraryName)]
		 internal static extern CXString clang_getDiagnosticCategoryText (CXDiagnostic _);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_getDiagnosticNumRanges (CXDiagnostic _);

		[DllImport (LibraryName)]
		 internal static extern CXSourceRange clang_getDiagnosticRange (CXDiagnostic diagnostic, [MarshalAs (UnmanagedType.SysUInt)] uint range);

		[return:MarshalAs (UnmanagedType.SysUInt)]
		[DllImport (LibraryName)]
		 internal static extern uint clang_getDiagnosticNumFixIts (CXDiagnostic diagnostic);

		[DllImport (LibraryName)]
		 internal static extern CXString clang_getDiagnosticFixIt (CXDiagnostic diagnostic, [MarshalAs (UnmanagedType.SysUInt)] uint dixIt, out CXSourceRange ReplacementRange);
	}
}
