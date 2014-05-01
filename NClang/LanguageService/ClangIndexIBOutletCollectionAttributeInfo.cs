using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{

	public class ClangIndexIBOutletCollectionAttributeInfo
	{
		readonly CXIdxIBOutletCollectionAttrInfo source;
		readonly IntPtr address;

		internal ClangIndexIBOutletCollectionAttributeInfo (IntPtr address)
		{
			this.address = address;
			source = Marshal.PtrToStructure<CXIdxIBOutletCollectionAttrInfo> (address);
		}

		public ClangIndexAttributeInfo AttrInfo {
			get { return new ClangIndexAttributeInfo (source.AttrInfo); }
		}

		public ClangIndexEntityInfo ObjCClass {
			get { return new ClangIndexEntityInfo (source.ObjcClass); }
		}

		public ClangCursor ClassCursor {
			get { return new ClangCursor (source.ClassCursor); }
		}

		public ClangIndexLocation ClassLocation {
			get { return new ClangIndexLocation (source.ClassLoc); }
		}
	}

}
