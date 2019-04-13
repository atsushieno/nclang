#if MANUAL

using System;
using System.Runtime.InteropServices;

using CXFile = System.IntPtr;
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*
using CXClientData = System.IntPtr; // void*
using CXEvalResult = System.IntPtr; // void*

using CXString = NClang.ClangString;

using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

namespace NClang.Natives
{
    [UnmanagedFunctionPointer(LibClang.LibraryCallingConvention)]
	delegate void CXInclusionVisitor (CXFile included_file, IntPtr inclusion_stack, uint include_len, CXClientData client_data); // CXSourceLocation*

	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getClangVersion ();

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_toggleCrashRecovery (uint isEnabled);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_getInclusions (CXTranslationUnit tu, CXInclusionVisitor visitor, CXClientData client_data);


		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern CXEvalResult clang_Cursor_Evaluate (CXCursor C);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern EvalResultKind clang_EvalResult_getKind (CXEvalResult E);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern int clang_EvalResult_getAsInt (CXEvalResult E);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern SystemLongLong clang_EvalResult_getAsLongLong (CXEvalResult E);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern uint clang_EvalResult_isUnsignedInt (CXEvalResult E);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern SystemULongLong clang_EvalResult_getAsUnsigned (CXEvalResult E);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern double clang_EvalResult_getAsDouble (CXEvalResult E);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern string clang_EvalResult_getAsStr (CXEvalResult E);
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_EvalResult_dispose (CXEvalResult E);
	}
}

#endif
