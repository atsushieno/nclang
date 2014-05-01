using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
	public class ClangIndexAttributeInfo
	{
		readonly CXIdxAttrInfo source;
		readonly IntPtr address;

		internal ClangIndexAttributeInfo (IntPtr address)
		{
			this.address = address;
			source = Marshal.PtrToStructure<CXIdxAttrInfo> (address);
		}

		public IndexAttributeKind Kind {
			get { return source.Kind; }
		}

		public ClangCursor Cursor {
			get { return new ClangCursor (source.Cursor); }
		}

		public ClangIndexLocation Location {
			get { return new ClangIndexLocation (source.Loc); }
		}

		public ClangIndexIBOutletCollectionAttributeInfo ObjCOutletCollectionAttribute {
			get { return new ClangIndexIBOutletCollectionAttributeInfo (LibClang.clang_index_getIBOutletCollectionAttrInfo (address)); }
		}
	}


}
