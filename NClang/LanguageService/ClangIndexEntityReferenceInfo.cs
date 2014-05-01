using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
	public class ClangIndexEntityReferenceInfo
	{
		readonly CXIdxEntityRefInfo source;
		readonly IntPtr address;

		internal ClangIndexEntityReferenceInfo (IntPtr address)
		{
			this.address = address;
			source = Marshal.PtrToStructure<CXIdxEntityRefInfo> (address);
		}

		public IntPtr Address {
			get { return address; }
		}

		public IndexEntityRefKind Kind {
			get { return source.Kind; }
		}

		public ClangCursor Cursor {
			get { return new ClangCursor (source.Cursor); }
		}

		public ClangIndexLocation Location {
			get { return new ClangIndexLocation (source.Loc); }
		}

		public ClangIndexEntityInfo Parent {
			get { return new ClangIndexEntityInfo (source.ParentEntity); }
		}

		public ClangIndexContainerInfo Container {
			get { return new ClangIndexContainerInfo (source.Container); }
		}
	}
}
