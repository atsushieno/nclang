using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;
using CXStringSet = NClang.ClangStringSet;

namespace NClang.Natives
{
	public static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern IntPtr clang_getCString (CXString @string);

		// FIXME: determine how/when to call it.
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern void clang_disposeString (CXString @string);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		internal static extern void clang_disposeStringSet (CXStringSet set);

		internal static string Unwrap (this CXString s)
		{
			// Normal marshalling causes double free crash at mono runtime. So I manually process marshaling here.
			var p = clang_getCString (s);
			if (p == IntPtr.Zero)
				return null;
			int x = 0;
			unsafe {
				byte* ptr = (byte*) p;
				while (ptr [x] != 0)
					x++;
				var e = System.Text.Encoding.Default;
				var l = e.GetCharCount (ptr, x);
                if (l == 0)
                {
                    return string.Empty;
                }
				char* buf = stackalloc char [l];
				e.GetChars (ptr, x, buf, l);
				return new string (buf, 0, l);
			}
		}
	}
}

