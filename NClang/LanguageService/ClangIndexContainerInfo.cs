using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{

	public class ClangIndexContainerInfo
	{
		readonly CXIdxContainerInfo source;
		readonly IntPtr address;

		public ClangIndexContainerInfo (IntPtr address)
		{
			this.address = address;
			source = Marshal.PtrToStructure<CXIdxContainerInfo> (address);
		}

		public IntPtr Address {
			get { return address; }
		}

		public ClangCursor Cursor {
			get { return new ClangCursor (source.Cursor); }
		}

		public IntPtr ClientContainer {
			get { return LibClang.clang_index_getClientContainer (address); }
			set { LibClang.clang_index_setClientContainer (address, value); }
		}
	}
}
