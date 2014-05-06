using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{

	public class ClangIndexedFileInfo : ClangObject
	{
		internal ClangIndexedFileInfo (IntPtr handle) // CXIdxIncludedFileInfo*
			: base (handle)
		{
		}
	}
}
