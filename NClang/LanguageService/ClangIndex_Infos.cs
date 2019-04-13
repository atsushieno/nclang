using System;
using System.Runtime.InteropServices;
using NClang.Natives;
using System.Collections.Generic;
using System.Linq;

using LibClang = NClang.Natives.Natives;

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
				get { return new EntityInfo (Info.entityInfo); }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.cursor); }
			}

			public Location Location {
				get { return new Location (Info.loc); }
			}

			public ClangIndex.ContainerInfo SemanticContainer {
				get { return new ClangIndex.ContainerInfo (Info.semanticContainer); }
			}

			public ClangIndex.ContainerInfo LexicalContainer {
				get { return new ClangIndex.ContainerInfo (Info.lexicalContainer); }
			}

			public bool IsRedeclaration {
				get { return Info.isRedeclaration != 0; }
			}

			public bool IsDefinition {
				get { return Info.isDefinition != 0; }
			}

			public bool IsContainer {
				get { return Info.isContainer != 0; }
			}

			public ClangIndex.ContainerInfo DeclarationAsContainer {
				get { return (IntPtr) Info.declAsContainer != IntPtr.Zero ? new ClangIndex.ContainerInfo (Info.declAsContainer) : null; }
			}

			public bool IsImplicit {
				get { return Info.isImplicit != 0; }
			}

			public IEnumerable<AttributeInfo> Attributes {
				get { return Enumerable.Range (0, (int) Info.numAttributes).Select (i => new AttributeInfo ((IntPtr) Info.attributes + Marshal.SizeOf (typeof(IntPtr)) * i)); }
			}

			public IndexDeclInfoFlags Flags {
				get { return (IndexDeclInfoFlags) Info.flags; }
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
				get { return new ClangCursor (Info.cursor); }
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
				get { return (IndexAttributeKind) Info.kind; }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.cursor); }
			}

			public Location Location {
				get { return new Location (Info.loc); }
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
				get { return (IndexEntityKind) Info.kind; }
			}

			public IndexEntityCxxTemplateKind TemplateKind {
				get { return (IndexEntityCxxTemplateKind) Info.templateKind; }
			}

			public IndexEntityLanguage EntityLanguage {
				get { return (IndexEntityLanguage) Info.lang; }
			}

			public string Name {
				get { return Info.name; }
			}

			public string USR {
				get { return Info.USR; }
			}

			public int AttributeCount {
				get { return (int) Info.numAttributes; }
			}

			public IntPtr Attributes {
				get { return Info.attributes; }
			}

			public AttributeInfo GetAttribute (int index)
			{
				return new AttributeInfo ((IntPtr) Info.attributes + Marshal.SizeOf (typeof(IntPtr)) * index);
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
				get { return (IndexEntityRefKind) Info.kind; }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.cursor); }
			}

			public Location Location {
				get { return new Location (Info.loc); }
			}

			public EntityInfo Parent {
				get { return new EntityInfo (Info.parentEntity); }
			}

			public ClangIndex.ContainerInfo Container {
				get { return new ClangIndex.ContainerInfo (Info.container); }
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
				get { return new AttributeInfo (Info.attrInfo); }
			}

			public EntityInfo ObjCClass {
				get { return new EntityInfo (Info.objcClass); }
			}

			public ClangCursor ClassCursor {
				get { return new ClangCursor (Info.classCursor); }
			}

			public Location ClassLocation {
				get { return new Location (Info.classLoc); }
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
				get { return new ClangFile (Info.file); }
			}
			public ClangModule Module {
				get { return new ClangModule (Info.module); }
			}
			public Location Location {
				get { return new Location (Info.loc); }
			}
			public bool IsImplicit {
				get { return Info.isImplicit != 0; }
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
					IntPtr idx = IntPtr.Zero, f = IntPtr.Zero;
					IntPtr l = IntPtr.Zero, c = IntPtr.Zero, o = IntPtr.Zero;
					LibClang.clang_indexLoc_getFileLocation (source, idx, f, l, c, o);
					return new ClangSourceLocation.IndexFileLocation (Marshal.ReadIntPtr (idx), new ClangFile (Marshal.ReadIntPtr (f)), Marshal.ReadInt32 (l), Marshal.ReadInt32 (c), Marshal.ReadInt32 (o));
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
				get { return new DeclarationInfo (Info.declInfo); }
			}

			public IndexObjCContainerKind Kind {
				get { return (IndexObjCContainerKind) Info.kind; }
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
				get { return new ObjCContainerDeclarationInfo (Info.containerInfo); }
			}

			public BaseClassInfo Super {
				get { return (IntPtr) Info.superInfo != IntPtr.Zero ? new BaseClassInfo (Info.superInfo) : null; }
			}

			public ObjCProtocolReferenceListDeclarationInfo Protocols {
				get { return new ObjCProtocolReferenceListDeclarationInfo (Info.protocols); }
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
				get { return (IntPtr) Info.@base != IntPtr.Zero ? new EntityInfo (Info.@base) : null; }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.cursor); }
			}

			public Location Location {
				get { return new Location (Info.loc); }
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
				get { return new ObjCContainerDeclarationInfo (Info.containerInfo); }
			}

			public EntityInfo Class {
				get { return new EntityInfo (Info.objcClass); }
			}

			public ClangCursor ClassCursor {
				get { return new ClangCursor (Info.classCursor); }
			}

			public Location ClassLocation {
				get { return new Location (Info.classLoc); }
			}

			public ObjCProtocolReferenceListDeclarationInfo Protocols {
				get { return new ObjCProtocolReferenceListDeclarationInfo (Info.protocols); }
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
				get { return new EntityInfo (Info.protocol); }
			}

			public ClangCursor Cursor {
				get { return new ClangCursor (Info.cursor); }
			}

			public Location Location {
				get { return new Location (Info.loc); }
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
				get { return (int) Info.numProtocols; }
			}

			public IEnumerable<ObjCProtocolReferenceInfo> Items {
				get { return Enumerable.Range (0, Count).Select (i => Get (i)); }
			}

			public ObjCProtocolReferenceInfo Get (int index)
			{
				return new ObjCProtocolReferenceInfo ((IntPtr) Info.protocols + (index * Marshal.SizeOf (typeof(CXIdxObjCProtocolRefInfo))));
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
				get { return (IntPtr) Info.getter != IntPtr.Zero ? new EntityInfo (Info.getter) : null; }
			}

			public EntityInfo Setter {
				get { return (IntPtr) Info.setter != IntPtr.Zero ? new EntityInfo (Info.setter) : null; }
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
