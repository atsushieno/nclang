using System;
using System.Runtime.InteropServices;
using NClang.Natives;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public class ClangObject
	{
		internal ClangObject (IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				throw new ArgumentNullException ("handle");
			this.Handle = handle;
		}
		
		public IntPtr Handle { get; set; }
	}


}
