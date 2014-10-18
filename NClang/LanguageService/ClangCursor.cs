using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

using CXString = NClang.ClangString;
using System.Collections.Generic;

namespace NClang
{	
	public class ClangCursor
	{
		// instance members

		readonly CXCursor source;

		internal ClangCursor (CXCursor source)
		{
			this.source = source;
		}

		internal CXCursor Source {
			get { return source; }
		}

		// CursorManipulation

		public static bool operator == (ClangCursor c1, ClangCursor c2)
		{
			return LibClang.clang_equalCursors (c1.source, c2.source) != 0;
		}

		public static bool operator != (ClangCursor c1, ClangCursor c2)
		{
			return !(c1 == c2);
		}

		public override bool Equals (object obj)
		{
			var cc = obj as ClangCursor;
			return (object) cc != null && this == cc;
		}

		public override int GetHashCode ()
		{
			return (int) LibClang.clang_hashCursor (source);
		}

		public ClangFile IncludedFile {
			get {
				var h = LibClang.clang_getIncludedFile (source);
				return h == IntPtr.Zero ? null : new ClangFile (h);
			}
		}

		public ClangCursor LexicalParent {
			get { return new ClangCursor (LibClang.clang_getCursorLexicalParent (source)); }
		}

		public ClangCursor SemanticParent {
			get { return new ClangCursor (LibClang.clang_getCursorSemanticParent (source)); }
		}

		public AvailabilityKind AvailabilityKind {
			get { return LibClang.clang_getCursorAvailability (source); }
		}

		public CursorKind Kind {
			get { return LibClang.clang_getCursorKind (source); }
		}

		public LanguageKind Language {
			get { return LibClang.clang_getCursorLanguage (source); }
		}

		public LinkageKind Linkage {
			get { return LibClang.clang_getCursorLinkage (source); }
		}

		public ClangTranslationUnit TranslationUnit {
			get {
				var ret = LibClang.clang_Cursor_getTranslationUnit (source);
				return ret == IntPtr.Zero ? null : new ClangTranslationUnit (ret);
			}
		}

		public ClangPlatformAvailability [] GetPlatformAvailability (out bool isAlwaysDeprecated, out string deprecatedMessage,
			out bool isAlwaysUnavailable, out string unavailableMessage)
		{
			int ad, au;
			CXString dm, um;
			IntPtr dummy = IntPtr.Zero;
			var n = LibClang.clang_getCursorPlatformAvailability (source, out ad, out dm, out au, out um, ref dummy, 0);
			var size = Marshal.SizeOf (typeof(CXPlatformAvailability));
			var ptr = Marshal.AllocHGlobal (size * n);
			LibClang.clang_getCursorPlatformAvailability (source, out ad, out dm, out au, out um, ref ptr, n);
			isAlwaysDeprecated = ad != 0;
			isAlwaysUnavailable = au != 0;
			deprecatedMessage = dm.Unwrap ();
			unavailableMessage = um.Unwrap ();
			var ret = new ClangPlatformAvailability [n];
			for (int i = 0; i < n; i++)
				ret [i] = new ClangPlatformAvailability (ptr + size * i);
			return ret;
		}

		public IEnumerable<ClangCursor> OverridenCursors {
			get {
				IntPtr ptr;
				uint n = 0;
				LibClang.clang_getOverriddenCursors (source, out ptr, ref n);
				var ptrs = new IntPtr [n];
				for (int i = 0; i < n; i++)
					LibClang.clang_getOverriddenCursors (source, out ptrs [i], ref n);
				return Enumerable.Range (0, (int) n).Select (i => new ClangCursor (Marshal.PtrToStructure<CXCursor> (ptrs [i])));
			}
		}

		// TypeInformationForCXCursor

		public ClangType CursorType {
			get { return LibClang.clang_getCursorType (source).ToManaged (); }
		}

		public ClangType TypeDefDeclUnderlyingType {
			get { return LibClang.clang_getTypedefDeclUnderlyingType (source).ToManaged (); }
		}

		public ClangType EnumDeclIntegerType {
			get { return LibClang.clang_getEnumDeclIntegerType (source).ToManaged (); }
		}

		public SystemLongLong EnumConstantDeclValue {
			get { return LibClang.clang_getEnumConstantDeclValue (source); }
		}

		public SystemULongLong EnumConstantDeclUnsignedValue {
			get { return LibClang.clang_getEnumConstantDeclUnsignedValue (source); }
		}

		public int FieldDeclBitWidth {
			get { return LibClang.clang_getFieldDeclBitWidth (source); }
		}

		public int ArgumentCount {
			get { return LibClang.clang_Cursor_getNumArguments (source); }
		}

		public ClangCursor GetArgument (int index)
		{
			return new ClangCursor (LibClang.clang_Cursor_getArgument (source, (uint) index));
		}

		public string DeclObjCTypeEncoding {
			get { return LibClang.clang_getDeclObjCTypeEncoding (source).Unwrap (); }
		}

		public ClangType ResultType {
			get { return LibClang.clang_getCursorResultType (source).ToManaged (); }
		}

		public bool IsBitField {
			get { return LibClang.clang_Cursor_isBitField (source) != 0; }
		}

		public bool IsVirtualBase {
			get { return LibClang.clang_isVirtualBase (source) != 0; }
		}

		public CXXAccessSpecifier CxxAccessSpecifier {
			get { return LibClang.clang_getCXXAccessSpecifier (source); }
		}

		public int OverloadedDeclarationCount {
			get { return (int) LibClang.clang_getNumOverloadedDecls (source); }
		}

		public ClangCursor GetOverloadedDeclaration (int index)
		{
			return new ClangCursor (LibClang.clang_getOverloadedDecl (source, (uint) index));
		}

		// CodeCompletion

		public ClangCompletionString CompletionString {
			get { return new ClangCompletionString (LibClang.clang_getCursorCompletionString (source)); }
		}

		// TraversingASTWithCursor

		public void VisitChildren (Func<ClangCursor,ClangCursor,IntPtr,ChildVisitResult> visitor, IntPtr clientData)
		{
			var ret = LibClang.clang_visitChildren (source, (cursor, parent, cd) => visitor (new ClangCursor (cursor), new ClangCursor (parent), cd), clientData);
			if (ret != 0)
				throw new ClangServiceException (string.Format ("Cursor children visitor failed. Error code: {0}", ret));
		}

		// CrossReferencingAST

		public string UnifiedSymbolResolution {
			get { return LibClang.clang_getCursorUSR (source).Unwrap (); }
		}

		public string Spelling {
			get { return LibClang.clang_getCursorSpelling (source).Unwrap (); }
		}

		public ClangSourceRange GetSpellingNameRange (int pieceIndex)
		{
			return new ClangSourceRange (LibClang.clang_Cursor_getSpellingNameRange (source, (uint) pieceIndex, 0)); // dummy
		}

		public string DisplayName {
			get { return LibClang.clang_getCursorDisplayName (source).Unwrap (); }
		}

		public ClangCursor Referenced {
			get { return new ClangCursor (LibClang.clang_getCursorReferenced (source)); }
		}

		public ClangCursor Definition {
			get { return new ClangCursor (LibClang.clang_getCursorDefinition (source)); }
		}

		public bool IsDefinition {
			get { return LibClang.clang_isCursorDefinition (source) != 0; }
		}

		public ClangCursor CanonicalCursor {
			get { return new ClangCursor (LibClang.clang_getCanonicalCursor (source)); }
		}

		public int ObjectSelectorIndex {
			get { return LibClang.clang_Cursor_getObjCSelectorIndex (source); }
		}

		public bool IsDynamicCall {
			get { return LibClang.clang_Cursor_isDynamicCall (source) != 0; }
		}

		public ClangType ReceiverType {
			get { return LibClang.clang_Cursor_getReceiverType (source).ToManaged (); }
		}

		public uint GetObjCPropertyAttributes ()
		{
			return LibClang.clang_Cursor_getObjCPropertyAttributes (source, 0); // dummy
		}

		public uint ObjCDeclQualifiers {
			get { return LibClang.clang_Cursor_getObjCDeclQualifiers (source); }
		}

		public bool IsObjCOptional {
			get { return LibClang.clang_Cursor_isObjCOptional (source) != 0; }
		}

		public bool IsVariadic {
			get { return LibClang.clang_Cursor_isVariadic (source) != 0; }
		}

		public ClangSourceRange CommentRange {
			get { return new ClangSourceRange (LibClang.clang_Cursor_getCommentRange (source)); }
		}

		public string RawCommentText {
			get { return LibClang.clang_Cursor_getRawCommentText (source).Unwrap (); }
		}

		public string BriefCommentText {
			get { return LibClang.clang_Cursor_getBriefCommentText (source).Unwrap (); }
		}

		public ClangComment ParsedComment {
			get { return new ClangComment (LibClang.clang_Cursor_getParsedComment (source)); }
		}

		// MappingBetweenCursorAndLocation

		public ClangSourceLocation Location {
			get { return new ClangSourceLocation (LibClang.clang_getCursorLocation (source)); }
		}

		public ClangSourceRange CursorExtent {
			get { return new ClangSourceRange (LibClang.clang_getCursorExtent (source)); }
		}

		// CppASTIntrospection

		public bool IsCxxPureVirtual {
			get { return LibClang.clang_CXXMethod_isPureVirtual (source) != 0; }
		}

		public bool IsCxxStatic {
			get { return LibClang.clang_CXXMethod_isStatic (source) != 0; }
		}

		public bool IsCxxVirtual {
			get { return LibClang.clang_CXXMethod_isVirtual (source) != 0; }
		}

		public bool IsCxxConst {
			get { return LibClang.clang_CXXMethod_isConst (source) != 0; }
		}

		public CursorKind TemplateCursorKind {
			get { return LibClang.clang_getTemplateCursorKind (source); }
		}

		public ClangCursor SpecializedCursorTemplate {
			get { return new ClangCursor (LibClang.clang_getSpecializedCursorTemplate (source)); }
		}

		public ClangSourceRange GetCursorReferenceNameRange (NameRefFlags nameFlags, int pieceIndex)
		{
			return new ClangSourceRange (LibClang.clang_getCursorReferenceNameRange (source, nameFlags, (uint) pieceIndex));
		}

		// InformationForAttributes
		public ClangType IBOutletCollectionType {
			get { return LibClang.clang_getIBOutletCollectionType (source).ToManaged (); }
		}

		// ModuleIntrospection

		public ClangModule Module {
			get { return new ClangModule (LibClang.clang_Cursor_getModule (source)); }
		}

		// HighLevelAPI

		public FindResult FindReferenceInFile (ClangFile file, Func<object,ClangCursor,ClangSourceRange,VisitorResult> visitor)
		{
			return LibClang.clang_findReferencesInFile (source, file.Handle, new CXCursorAndRangeVisitor ((ctx, cursor, range) => visitor (ctx, new ClangCursor (cursor), new ClangSourceRange (range))));
		}
	}
}
