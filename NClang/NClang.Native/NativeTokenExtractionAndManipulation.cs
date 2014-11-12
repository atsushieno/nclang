using System;
using System.Runtime.InteropServices;

using CXFile = System.IntPtr;
using CXTranslationUnit = System.IntPtr; // CXTranslationUnitImpl*

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	[StructLayout (LayoutKind.Sequential)]
	struct CXToken
	{
		public readonly uint int_data1;
		public readonly uint int_data2;
		public readonly uint int_data3;
		public readonly uint int_data4;
		public readonly IntPtr ptr_data;
	}

	// done
	public static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern TokenKind 	clang_getTokenKind (CXToken _);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_getTokenSpelling (CXTranslationUnit _, CXToken __);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceLocation 	clang_getTokenLocation (CXTranslationUnit _, CXToken __);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXSourceRange 	clang_getTokenExtent (CXTranslationUnit _, CXToken __);
		// CXToken** Tokens
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void 	clang_tokenize (CXTranslationUnit TU, CXSourceRange Range, out IntPtr Tokens, out uint NumTokens);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void 	clang_annotateTokens (CXTranslationUnit TU, IntPtr Tokens, uint NumTokens, ref IntPtr Cursors);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void 	clang_disposeTokens (CXTranslationUnit TU, IntPtr Tokens, uint NumTokens);
	}
}
