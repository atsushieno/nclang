using System;
using System.Runtime.InteropServices;

namespace NClang.Natives
{
	static partial class LibClang
	{
		public const string LibraryName = "clang";

		public static ClangFile Wrap (this IntPtr cxFile)
		{
			return cxFile == IntPtr.Zero ? null : new ClangFile (cxFile);
		}
	}
}

