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
				get { return (CXIdxDeclInfo) (info ?? (info = Marshal.PtrToStructure<CXIdxDeclInfo> (Address))); }
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
				get { return Enumerable.Range (0, (int) Info.NumAttributes).Select (i => new AttributeInfo (Info.Attributes + Marshal.SizeOf<IntPtr> () * i)); }
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
				get { return (CXIdxContainerInfo)(info ?? (info = Marshal.PtrToStructure<CXIdxContainerInfo> (Address))); }
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
				get { return (CXIdxAttrInfo)(info ?? (info = Marshal.PtrToStructure<CXIdxAttrInfo> (Address))); }
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
				get { return (CXIdxEntityInfo)(info ?? (info = Marshal.PtrToStructure<CXIdxEntityInfo> (Address))); }
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
				return new AttributeInfo (Info.Attributes + Marshal.SizeOf<IntPtr> () * index);
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
				get { return (CXIdxEntityRefInfo) (info ?? (info = Marshal.PtrToStructure<CXIdxEntityRefInfo> (Address))); }
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
				get { return (CXIdxIBOutletCollectionAttrInfo)(info ?? (info = Marshal.PtrToStructure<CXIdxIBOutletCollectionAttrInfo> (Address))); }
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
				get { return (CXIdxImportedASTFileInfo)(info ?? (info = Marshal.PtrToStructure<CXIdxImportedASTFileInfo> (Address))); }
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
			public ObjCContainerDeclarationInfo (IntPtr address)
				: base (address)
			{
			}
		}

		public class ObjCInterfaceDeclarationInfo : ClangIndexInfo
		{
			public ObjCInterfaceDeclarationInfo (IntPtr address)
				: base (address)
			{
			}
		}

		public class ObjCCategoryDeclarationInfo : ClangIndexInfo
		{
			public ObjCCategoryDeclarationInfo (IntPtr address)
				: base (address)
			{
			}
		}

		public class ObjCProtocolReferenceListDeclarationInfo : ClangIndexInfo
		{
			public ObjCProtocolReferenceListDeclarationInfo (IntPtr address)
				: base (address)
			{
			}
		}

		public class ObjCPropertyDeclarationInfo : ClangIndexInfo
		{
			public ObjCPropertyDeclarationInfo (IntPtr address)
				: base (address)
			{
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