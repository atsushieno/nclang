using System;
using System.Runtime.InteropServices;

namespace NClang
{
	[StructLayout (LayoutKind.Sequential)]
	public struct ClangString
	{
		internal readonly IntPtr Data;
		[MarshalAs (UnmanagedType.SysUInt)]
		internal uint PrivateFlags;
	}
	
}
