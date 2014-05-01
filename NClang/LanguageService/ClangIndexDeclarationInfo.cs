using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{

	public class ClangIndexDeclarationInfo
	{
		public class ClangIndexObjCContainerDeclarationInfo
		{
			readonly IntPtr source;

			public ClangIndexObjCContainerDeclarationInfo (IntPtr source)
			{
				this.source = source;
			}
		}

		public class ClangIndexObjCInterfaceDeclarationInfo
		{
			readonly IntPtr source;

			public ClangIndexObjCInterfaceDeclarationInfo (IntPtr source)
			{
				this.source = source;
			}
		}

		public class ClangIndexObjCCategoryDeclarationInfo
		{
			readonly IntPtr source;

			public ClangIndexObjCCategoryDeclarationInfo (IntPtr source)
			{
				this.source = source;
			}
		}

		public class ClangIndexObjCProtocolReferenceListDeclarationInfo
		{
			readonly IntPtr source;

			public ClangIndexObjCProtocolReferenceListDeclarationInfo (IntPtr source)
			{
				this.source = source;
			}
		}

		public class ClangIndexObjCPropertyDeclarationInfo
		{
			readonly IntPtr source;

			public ClangIndexObjCPropertyDeclarationInfo (IntPtr source)
			{
				this.source = source;
			}
		}

		public class ClangIndexCxxClassDeclarationInfo
		{
			readonly IntPtr source;

			public ClangIndexCxxClassDeclarationInfo (IntPtr source)
			{
				this.source = source;
			}
		}

		readonly IntPtr source;

		internal ClangIndexDeclarationInfo (IntPtr source)
		{
			this.source = source;
		}

		public ClangIndexObjCContainerDeclarationInfo ObjCContainerDeclaration {
			get { return new ClangIndexObjCContainerDeclarationInfo (LibClang.clang_index_getObjCContainerDeclInfo (source)); }
		}

		public ClangIndexObjCInterfaceDeclarationInfo ObjCInterfaceDeclaration {
			get { return new ClangIndexObjCInterfaceDeclarationInfo (LibClang.clang_index_getObjCInterfaceDeclInfo (source)); }
		}

		public ClangIndexObjCCategoryDeclarationInfo ObjCCategoryDeclaration {
			get { return new ClangIndexObjCCategoryDeclarationInfo (LibClang.clang_index_getObjCCategoryDeclInfo (source)); }
		}

		public ClangIndexObjCProtocolReferenceListDeclarationInfo ObjCProtocolReferenceListDeclaration {
			get { return new ClangIndexObjCProtocolReferenceListDeclarationInfo (LibClang.clang_index_getObjCProtocolRefListInfo (source)); }
		}

		public ClangIndexObjCPropertyDeclarationInfo ObjCPropertyDeclaration {
			get { return new ClangIndexObjCPropertyDeclarationInfo (LibClang.clang_index_getObjCPropertyDeclInfo (source)); }
		}

		public ClangIndexCxxClassDeclarationInfo CxxClassDeclaration {
			get { return new ClangIndexCxxClassDeclarationInfo (LibClang.clang_index_getCXXClassDeclInfo (source)); }
		}
	}
}
