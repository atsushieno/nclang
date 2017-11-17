using System;
using System.Runtime.InteropServices;

namespace NClang
{
	[StructLayout (LayoutKind.Sequential)]
	public struct ClangString
	{
		internal readonly IntPtr Data;
		internal uint PrivateFlags;
	}

	[StructLayout (LayoutKind.Sequential)]
	public struct ClangStringSet
	{
		internal readonly IntPtr Strings;
		internal uint Count;
	}
}
