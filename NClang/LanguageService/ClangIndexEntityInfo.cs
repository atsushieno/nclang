using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
	public class ClangIndexEntityInfo
	{
		readonly CXIdxEntityInfo source;
		readonly IntPtr address;

		internal ClangIndexEntityInfo (IntPtr address)
		{
			this.address = address;
			source = Marshal.PtrToStructure<CXIdxEntityInfo> (address);
		}

		public IndexEntityKind Kind {
			get { return source.Kind; }
		}

		public IndexEntityCxxTemplateKind CxxTemplateKind {
			get { return source.CxxTemplateKind; }
		}

		public IndexEntityLanguage EntityLanguage {
			get { return source.Lang; }
		}

		public string Name {
			get { return source.Name; }
		}

		public string USR {
			get { return source.USR; }
		}

		public int AttributeCount {
			get { return (int) source.NumAttributes; }
		}

		public IntPtr Attributes {
			get { return source.Attributes; }
		}

		public ClangIndexAttributeInfo GetAttribute (int index)
		{
			return new ClangIndexAttributeInfo (source.Attributes + Marshal.SizeOf<IntPtr> () * index);
		}

		public IntPtr ClientEntity {
			get { return LibClang.clang_index_getClientEntity (address); }
			set { LibClang.clang_index_setClientEntity (address, value); }
		}
	}

}
