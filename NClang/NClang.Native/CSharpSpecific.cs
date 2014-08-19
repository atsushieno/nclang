using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace NClang.Natives
{
	static partial class LibClang
	{
        public const string LibraryName = "libclang";
        public const System.Runtime.InteropServices.CallingConvention LibraryCallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl;

		public static ClangFile Wrap (this IntPtr cxFile)
		{
			return cxFile == IntPtr.Zero ? null : new ClangFile (cxFile);
		}

		internal static CXUnsavedFile [] ToNative (this IEnumerable<ClangUnsavedFile> source)
		{
			return source != null && source.Any () ? source.Select (s => new CXUnsavedFile (s.FileName, s.Contents)).ToArray () : new CXUnsavedFile [0];
		}
	}
}

