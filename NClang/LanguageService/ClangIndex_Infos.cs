using System;
using System.Runtime.InteropServices;
using NClang.Natives;
using System.Collections.Generic;
using System.Linq;

namespace NClang
{
	public partial class ClangIndex
	{
		public abstract class ClangIndexInfo
		{
			readonly IntPtr address;

			protected ClangIndexInfo (IntPtr address)
			{
				this.address = address;
			}

			public IntPtr Address {
				get { return address; }
			}
		}

		public class DeclarationInfo : ClangIndexInfo
		{
			internal DeclarationInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxDeclInfo? info;

			CXIdxDeclInfo Info {
				get { return (CXIdxDeclInfo) (info ?? (info = (CXIdxDeclInfo)Marshal.PtrToStructure (Address, typeof(CXIdxDeclInfo)))); }
			}

			public EntityInfo EntityInfo {
				get { return new EntityInfo (Info.EntityInfo); }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.Cursor); }
			}

			public Location Location {
				get { return new Location (Info.Loc); }
			}

			public ClangIndex.ContainerInfo SemanticContainer {
				get { return new ClangIndex.ContainerInfo (Info.SemanticContainer); }
			}

			public ClangIndex.ContainerInfo LexicalContainer {
				get { return new ClangIndex.ContainerInfo (Info.LexicalContainer); }
			}

			public bool IsRedeclaration {
				get { return Info.IsRedeclaration != 0; }
			}

			public bool IsDefinition {
				get { return Info.IsDefinition != 0; }
			}

			public bool IsContainer {
				get { return Info.IsContainer != 0; }
			}

			public ClangIndex.ContainerInfo DeclarationAsContainer {
				get { return Info.DeclAsContainer != IntPtr.Zero ? new ClangIndex.ContainerInfo (Info.DeclAsContainer) : null; }
			}

			public bool IsImplicit {
				get { return Info.IsImplicit != 0; }
			}

			public IEnumerable<AttributeInfo> Attributes {
				get { return Enumerable.Range (0, (int) Info.NumAttributes).Select (i => new AttributeInfo (Info.Attributes + Marshal.SizeOf (typeof(IntPtr)) * i)); }
			}

			public IndexDeclInfoFlags Flags {
				get { return Info.Flags; }
			}

			public ObjCContainerDeclarationInfo ObjCContainerDeclaration {
				get { return new ObjCContainerDeclarationInfo (LibClang.clang_index_getObjCContainerDeclInfo (Address)); }
			}

			public ObjCInterfaceDeclarationInfo ObjCInterfaceDeclaration {
				get { return new ObjCInterfaceDeclarationInfo (LibClang.clang_index_getObjCInterfaceDeclInfo (Address)); }
			}

			public ObjCCategoryDeclarationInfo ObjCCategoryDeclaration {
				get { return new ObjCCategoryDeclarationInfo (LibClang.clang_index_getObjCCategoryDeclInfo (Address)); }
			}

			public ObjCProtocolReferenceListDeclarationInfo ObjCProtocolReferenceListDeclaration {
				get { return new ObjCProtocolReferenceListDeclarationInfo (LibClang.clang_index_getObjCProtocolRefListInfo (Address)); }
			}

			public ObjCPropertyDeclarationInfo ObjCPropertyDeclaration {
				get { return new ObjCPropertyDeclarationInfo (LibClang.clang_index_getObjCPropertyDeclInfo (Address)); }
			}

			public CxxClassDeclarationInfo CxxClassDeclaration {
				get { return new CxxClassDeclarationInfo (LibClang.clang_index_getCXXClassDeclInfo (Address)); }
			}
		}

		public class ContainerInfo : ClangIndexInfo
		{
			CXIdxContainerInfo? info;

			public ContainerInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxContainerInfo Info {
				get { return (CXIdxContainerInfo)(info ?? (info = (CXIdxContainerInfo)Marshal.PtrToStructure (Address, typeof(CXIdxContainerInfo)))); }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.Cursor); }
			}

			public IntPtr ClientContainer {
				get { return LibClang.clang_index_getClientContainer (Address); }
				set { LibClang.clang_index_setClientContainer (Address, value); }
			}
		}

		public class AttributeInfo : ClangIndexInfo
		{
			CXIdxAttrInfo? info;

			internal AttributeInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxAttrInfo Info {
				get { return (CXIdxAttrInfo)(info ?? (info = (CXIdxAttrInfo)Marshal.PtrToStructure (Address, typeof(CXIdxAttrInfo)))); }
			}

			public IndexAttributeKind Kind {
				get { return Info.Kind; }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.Cursor); }
			}

			public Location Location {
				get { return new Location (Info.Loc); }
			}

			public IBOutletCollectionAttributeInfo ObjCOutletCollectionAttribute {
				get { return new IBOutletCollectionAttributeInfo (LibClang.clang_index_getIBOutletCollectionAttrInfo (Address)); }
			}
		}

		public class EntityInfo : ClangIndexInfo
		{
			CXIdxEntityInfo? info;

			internal EntityInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxEntityInfo Info {
				get { return (CXIdxEntityInfo)(info ?? (info = (CXIdxEntityInfo)Marshal.PtrToStructure (Address, typeof(CXIdxEntityInfo)))); }
			}

			public IndexEntityKind Kind {
				get { return Info.Kind; }
			}

			public IndexEntityCxxTemplateKind CxxTemplateKind {
				get { return Info.CxxTemplateKind; }
			}

			public IndexEntityLanguage EntityLanguage {
				get { return Info.Lang; }
			}

			public string Name {
				get { return Info.Name; }
			}

			public string USR {
				get { return Info.USR; }
			}

			public int AttributeCount {
				get { return (int) Info.NumAttributes; }
			}

			public IntPtr Attributes {
				get { return Info.Attributes; }
			}

			public AttributeInfo GetAttribute (int index)
			{
				return new AttributeInfo (Info.Attributes + Marshal.SizeOf (typeof(IntPtr)) * index);
			}

			public IntPtr ClientEntity {
				get { return LibClang.clang_index_getClientEntity (Address); }
				set { LibClang.clang_index_setClientEntity (Address, value); }
			}
		}

		public class EntityReferenceInfo : ClangIndexInfo
		{
			CXIdxEntityRefInfo? info;

			internal EntityReferenceInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxEntityRefInfo Info {
				get { return (CXIdxEntityRefInfo) (info ?? (info = (CXIdxEntityRefInfo)Marshal.PtrToStructure (Address, typeof(CXIdxEntityRefInfo)))); }
			}

			public IndexEntityRefKind Kind {
				get { return Info.Kind; }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.Cursor); }
			}

			public Location Location {
				get { return new Location (Info.Loc); }
			}

			public EntityInfo Parent {
				get { return new EntityInfo (Info.ParentEntity); }
			}

			public ClangIndex.ContainerInfo Container {
				get { return new ClangIndex.ContainerInfo (Info.Container); }
			}
		}

		public class IBOutletCollectionAttributeInfo : ClangIndexInfo
		{
			CXIdxIBOutletCollectionAttrInfo? info;

			internal IBOutletCollectionAttributeInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxIBOutletCollectionAttrInfo Info {
				get { return (CXIdxIBOutletCollectionAttrInfo)(info ?? (info = (CXIdxIBOutletCollectionAttrInfo)Marshal.PtrToStructure (Address, typeof(CXIdxIBOutletCollectionAttrInfo)))); }
			}

			public AttributeInfo AttrInfo {
				get { return new AttributeInfo (Info.AttrInfo); }
			}

			public EntityInfo ObjCClass {
				get { return new EntityInfo (Info.ObjcClass); }
			}

			public ClangCursor ClassCursor {
				get { return new ClangCursor (Info.ClassCursor); }
			}

			public Location ClassLocation {
				get { return new Location (Info.ClassLoc); }
			}
		}

		public class ImportedAstFileInfo : ClangIndexInfo
		{
			CXIdxImportedASTFileInfo? info;

			internal ImportedAstFileInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxImportedASTFileInfo Info {
				get { return (CXIdxImportedASTFileInfo)(info ?? (info = (CXIdxImportedASTFileInfo)Marshal.PtrToStructure (Address, typeof(CXIdxImportedASTFileInfo)))); }
			}

			public ClangFile File {
				get { return new ClangFile (Info.File); }
			}
			public ClangModule Module {
				get { return new ClangModule (Info.Module); }
			}
			public Location Location {
				get { return new Location (Info.Loc); }
			}
			public bool IsImplicit {
				get { return Info.IsImplicit != 0; }
			}
		}

		public class ClientFile : ClangIndexInfo
		{
			public ClientFile (IntPtr address)
				: base (address)
			{
			}
		}

		public class ClientAstFile : ClangIndexInfo
		{
			public ClientAstFile (IntPtr address)
				: base (address)
			{
			}
		}

		public class Location 
		{
			CXIdxLoc source;

			internal Location (CXIdxLoc source)
			{
				this.source = source;
			}

			internal CXIdxLoc ToNative ()
			{
				return source;
			}

			public ClangSourceLocation.IndexFileLocation FileLocation {
				get {
					IntPtr idx, f;
					uint l, c, o;
					LibClang.clang_indexLoc_getFileLocation (source, out idx, out f, out l, out c, out o);
					return new ClangSourceLocation.IndexFileLocation (idx, new ClangFile (f), (int) l, (int) c, (int) o);
				}
			}

			public ClangSourceLocation SourceLocation {
				get { return new ClangSourceLocation (LibClang.clang_indexLoc_getCXSourceLocation (source)); }
			}
		}

		public class IncludedFileInfo : ClangObject
		{
			internal IncludedFileInfo (IntPtr handle) // CXIdxIncludedFileInfo*
				: base (handle)
			{
			}
		}

		public class ObjCContainerDeclarationInfo : ClangIndexInfo
		{
			CXIdxObjCContainerDeclInfo? info;

			public ObjCContainerDeclarationInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxObjCContainerDeclInfo Info {
				get { return (CXIdxObjCContainerDeclInfo) (info ?? (info = (CXIdxObjCContainerDeclInfo)Marshal.PtrToStructure (Address, typeof(CXIdxObjCContainerDeclInfo)))); }
			}

			public DeclarationInfo Declaration {
				get { return new DeclarationInfo (Info.DeclInfo); }
			}

			public IndexObjCContainerKind Kind {
				get { return Info.Kind; }
			}
		}

		public class ObjCInterfaceDeclarationInfo : ClangIndexInfo
		{
			CXIdxObjCInterfaceDeclInfo? info;

			public ObjCInterfaceDeclarationInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxObjCInterfaceDeclInfo Info {
				get { return (CXIdxObjCInterfaceDeclInfo) (info ?? (info = (CXIdxObjCInterfaceDeclInfo)Marshal.PtrToStructure (Address, typeof(CXIdxObjCInterfaceDeclInfo)))); }
			}

			public ObjCContainerDeclarationInfo Container {
				get { return new ObjCContainerDeclarationInfo (Info.ContainerInfo); }
			}

			public BaseClassInfo Super {
				get { return Info.SuperInfo != IntPtr.Zero ? new BaseClassInfo (Info.SuperInfo) : null; }
			}

			public ObjCProtocolReferenceListDeclarationInfo Protocols {
				get { return new ObjCProtocolReferenceListDeclarationInfo (Info.Protocols); }
			}
		}

		public class BaseClassInfo : ClangIndexInfo
		{
			CXIdxBaseClassInfo? info;

			public BaseClassInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxBaseClassInfo Info {
				get { return (CXIdxBaseClassInfo) (info ?? (info = (CXIdxBaseClassInfo)Marshal.PtrToStructure (Address, typeof(CXIdxBaseClassInfo)))); }
			}

			public EntityInfo EntityInfo {
				get { return Info.Base != IntPtr.Zero ? new EntityInfo (Info.Base) : null; }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.Cursor); }
			}

			public Location Location {
				get { return new Location (Info.Loc); }
			}
		}

		public class ObjCCategoryDeclarationInfo : ClangIndexInfo
		{
			CXIdxObjCCategoryDeclInfo? info;

			public ObjCCategoryDeclarationInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxObjCCategoryDeclInfo Info {
				get { return (CXIdxObjCCategoryDeclInfo) (info ?? (info = (CXIdxObjCCategoryDeclInfo)Marshal.PtrToStructure (Address, typeof(CXIdxObjCCategoryDeclInfo)))); }
			}

			public ObjCContainerDeclarationInfo Container {
				get { return new ObjCContainerDeclarationInfo (Info.ContainerInfo); }
			}

			public EntityInfo Class {
				get { return new EntityInfo (Info.ObjcClass); }
			}

			public ClangCursor ClassCursor {
				get { return new ClangCursor (Info.ClassCursor); }
			}

			public Location ClassLocation {
				get { return new Location (Info.ClassLoc); }
			}

			public ObjCProtocolReferenceListDeclarationInfo Protocols {
				get { return new ObjCProtocolReferenceListDeclarationInfo (Info.Protocols); }
			}
		}

		public class ObjCProtocolReferenceInfo : ClangIndexInfo
		{
			CXIdxObjCProtocolRefInfo? info;

			public ObjCProtocolReferenceInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxObjCProtocolRefInfo Info {
				get { return (CXIdxObjCProtocolRefInfo) (info ?? (info = (CXIdxObjCProtocolRefInfo)Marshal.PtrToStructure (Address, typeof(CXIdxObjCProtocolRefInfo)))); }
			}

			public EntityInfo EntityInfo {
				get { return new EntityInfo (Info.Protocol); }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.Cursor); }
			}

			public Location Location {
				get { return new Location (Info.Loc); }
			}
		}

		public class ObjCProtocolReferenceListDeclarationInfo : ClangIndexInfo
		{
			CXIdxObjCProtocolRefListInfo? info;

			public ObjCProtocolReferenceListDeclarationInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxObjCProtocolRefListInfo Info {
				get { return (CXIdxObjCProtocolRefListInfo) (info ?? (info = (CXIdxObjCProtocolRefListInfo)Marshal.PtrToStructure (Address, typeof(CXIdxObjCProtocolRefListInfo)))); }
			}

			public int Count {
				get { return (int) Info.NumProtocols; }
			}

			public IEnumerable<ObjCProtocolReferenceInfo> Items {
				get { return Enumerable.Range (0, Count).Select (i => Get (i)); }
			}

			public ObjCProtocolReferenceInfo Get (int index)
			{
				return new ObjCProtocolReferenceInfo (Info.Protocols + (index * Marshal.SizeOf (typeof(CXIdxObjCProtocolRefInfo))));
			}
		}

		public class ObjCPropertyDeclarationInfo : ClangIndexInfo
		{
			CXIdxObjCPropertyDeclInfo? info;

			public ObjCPropertyDeclarationInfo (IntPtr address)
				: base (address)
			{
			}

			CXIdxObjCPropertyDeclInfo Info {
				get { return (CXIdxObjCPropertyDeclInfo) (info ?? (info = (CXIdxObjCPropertyDeclInfo)Marshal.PtrToStructure (Address, typeof(CXIdxObjCPropertyDeclInfo)))); }
			}

			public EntityInfo Getter {
				get { return Info.Getter != IntPtr.Zero ? new EntityInfo (Info.Getter) : null; }
			}

			public EntityInfo Setter {
				get { return Info.Setter != IntPtr.Zero ? new EntityInfo (Info.Setter) : null; }
			}
		}

		public class CxxClassDeclarationInfo : ClangIndexInfo
		{
			public CxxClassDeclarationInfo (IntPtr address)
				: base (address)
			{
			}
		}
	}
}
