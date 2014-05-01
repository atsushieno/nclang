using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	public static partial class LibClang
	{
		[DllImport (LibraryName)]
		[return:MarshalAs (UnmanagedType.LPStr)]
		 internal static extern string clang_getCString (CXString @string);

		// FIXME: determine how/when to call it.
		[DllImport (LibraryName)]
		 internal static extern void clang_disposeString (CXString @string);

		internal static string Unwrap (this CXString s)
		{
			return clang_getCString (s);
		}
	}
}

