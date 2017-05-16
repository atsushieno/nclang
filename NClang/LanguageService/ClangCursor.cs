using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

using CXString = NClang.ClangString;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NClang
{
	/// <summary>
	/// A cursor representing some element in the abstract syntax tree for
	/// a translation unit.
	///
	/// The cursor abstraction unifies the different kinds of entities in a
	/// program--declaration, statements, expressions, references to declarations,
	/// etc.--under a single "cursor" abstraction with a common set of operations.
	/// Common operation for a cursor include: getting the physical location in
	/// a source file where the cursor points, getting the name associated with a
	/// cursor, and retrieving cursors for any child nodes of a particular cursor.
	///
	/// Cursors can be produced in two specific ways.
	/// clang_getTranslationUnitCursor() produces a cursor for a translation unit,
	/// from which one can use clang_visitChildren() to explore the rest of the
	/// translation unit. clang_getCursor() maps from a physical source location
	/// to the entity that resides at that location, allowing one to map from the
	/// source code into the AST.
	/// </summary>
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

		/// <summary>
		/// Determine whether two cursors are equivalent.
		/// </summary>
		public static bool operator == (ClangCursor c1, ClangCursor c2)
		{
			return LibClang.clang_equalCursors (c1.source, c2.source) != 0;
		}

		/// <summary>
		/// Determine whether two cursors are not equivalent.
		/// </summary>
		public static bool operator != (ClangCursor c1, ClangCursor c2)
		{
			return !(c1 == c2);
		}

		/// <summary>
		/// Determine whether two cursors are equivalent.
		/// </summary>
		public override bool Equals (object obj)
		{
			var cc = obj as ClangCursor;
			return (object) cc != null && this == cc;
		}

		/// <summary>
		/// Compute a hash value for the given cursor.
		/// </summary>
		public override int GetHashCode ()
		{
			return (int) LibClang.clang_hashCursor (source);
		}

		/// <summary>
		/// Retrieve the file that is included by the given inclusion directive
		/// cursor.
		/// </summary>
		public ClangFile IncludedFile {
			get {
				var h = LibClang.clang_getIncludedFile (source);
				return h == IntPtr.Zero ? null : new ClangFile (h);
			}
		}

		/// <summary>
		/// Determine the lexical parent of the given cursor.
		/// </summary>
		/// <remarks>
		/// The lexical parent of a cursor is the cursor in which the given cursor
		/// was actually written. For many declarations, the lexical and semantic parents
		/// are equivalent (the semantic parent is returned by <see cref="SemanticParent"/>).
		/// They diverge when declarations or definitions are provided out-of-line. For example:
		///
		/// <code>
		/// class C {
		///  void f();
		/// };
		///
		/// void C::f() { }
		/// </code>
		///
		/// In the out-of-line definition of <c>C::f</c>, the semantic parent is the 
		/// the class <c>C</c>, of which this function is a member. The lexical parent is
		/// the place where the declaration actually occurs in the source code; in this
		/// case, the definition occurs in the translation unit. In general, the 
		/// lexical parent for a given entity can change without affecting the semantics
		/// of the program, and the lexical parent of different declarations of the
		/// same entity may be different. Changing the semantic parent of a declaration,
		/// on the other hand, can have a major impact on semantics, and redeclarations
		/// of a particular entity should all have the same semantic context.
		///
		/// In the example above, both declarations of <c>C::f</c> have <c>C</c> as their
		/// semantic context, while the lexical context of the first <c>C::f</c> is <c>C</c>
		/// and the lexical context of the second <c>C::f</c> is the translation unit.
		///
		/// For declarations written in the global scope, the lexical parent is
		/// the translation unit.
		/// </remarks>
		public ClangCursor LexicalParent {
			get { return new ClangCursor (LibClang.clang_getCursorLexicalParent (source)); }
		}

		/// <summary>
		/// Determine the semantic parent of the given cursor.
		/// </summary>
		/// <remarks>
		/// The semantic parent of a cursor is the cursor that semantically contains
		/// the given cursor. For many declarations, the lexical and semantic parents
		/// are equivalent (the lexical parent is returned by <see cref="LexicalParent"/>).
		/// They diverge when declarations or definitions are provided out-of-line. For example:
		///
		/// <code>
		/// class C {
		///  void f();
		/// };
		///
		/// void C::f() { }
		/// </code>
		///
		/// In the out-of-line definition of <c>C::f</c>, the semantic parent is the 
		/// the class <c>C</c>, of which this function is a member. The lexical parent is
		/// the place where the declaration actually occurs in the source code; in this
		/// case, the definition occurs in the translation unit. In general, the 
		/// lexical parent for a given entity can change without affecting the semantics
		/// of the program, and the lexical parent of different declarations of the
		/// same entity may be different. Changing the semantic parent of a declaration,
		/// on the other hand, can have a major impact on semantics, and redeclarations
		/// of a particular entity should all have the same semantic context.
		///
		/// In the example above, both declarations of <c>C::f</c> have <c>C</c> as their
		/// semantic context, while the lexical context of the first <c>C::f</c> is <c>C</c>
		/// and the lexical context of the second <c>C::f</c> is the translation unit.
		///
		/// For global declarations, the semantic parent is the translation unit.
		/// </remarks>
		public ClangCursor SemanticParent {
			get { return new ClangCursor (LibClang.clang_getCursorSemanticParent (source)); }
		}

		/// <summary>
		/// Determine the availability of the entity that this cursor refers to,
		/// taking the current target platform into account.
		/// </summary>
		public AvailabilityKind AvailabilityKind {
			get { return LibClang.clang_getCursorAvailability (source); }
		}

		/// <summary>
		/// Retrieve the kind of the given cursor.
		/// </summary>
		public CursorKind Kind {
			get { return LibClang.clang_getCursorKind (source); }
		}

		/// <summary>
		/// Determine the "language" of the entity referred to by a given cursor.
		/// </summary>
		public LanguageKind Language {
			get { return LibClang.clang_getCursorLanguage (source); }
		}

		/// <summary>
		/// Determine the linkage of the entity referred to by a given cursor.
		/// </summary>
		public LinkageKind Linkage {
			get { return LibClang.clang_getCursorLinkage (source); }
		}

		/// <summary>
		/// Returns the translation unit that a cursor originated from.
		/// </summary>
		public ClangTranslationUnit TranslationUnit {
			get {
				var ret = LibClang.clang_Cursor_getTranslationUnit (source);
				return ret == IntPtr.Zero ? null : new ClangTranslationUnit (ret);
			}
		}

		/// <summary>
		/// Determine the availability of the entity that this cursor refers to
		/// on any platforms for which availability information is known.
		/// </summary>
		/// <param name="isAlwaysDeprecated">Will be set to indicate whether the
		/// entity is deprecated on all platforms.</param>
		/// <param name="deprecatedMessage">Will be set to the message text
		/// provided along with the unconditional deprecation of this entity.
		/// </param>
		/// <param name="isAlwaysUnavailable">Will be set to indicate whether the
		/// entity is unavailable on all platforms.
		/// </param>
		/// <param name="unavailableMessage">Will be set to the message text
		/// provided along with the unconditional unavailability of this entity.
		/// </param>
		/// <returns>An array of <see cref="ClangPlatformAvailability"/> instances
		/// that will be populated with platform availability information, up to either
		/// the number of platforms for which availability information is available
		/// or <c>availability_size</c> , whichever is smaller.
		/// </returns>
		public ClangPlatformAvailability [] GetPlatformAvailability (out bool isAlwaysDeprecated, out string deprecatedMessage,
		                                                             out bool isAlwaysUnavailable, out string unavailableMessage)
		{
			int ad, au;
			CXString dm, um;
			IntPtr dummy = IntPtr.Zero;
			var n = LibClang.clang_getCursorPlatformAvailability (source, out ad, out dm, out au, out um, ref dummy, 0);
			var size = Extensions.SizeOf<CXPlatformAvailability>();
			var ptr = Marshal.AllocHGlobal (size * n);
			LibClang.clang_getCursorPlatformAvailability (source, out ad, out dm, out au, out um, ref ptr, n);
			isAlwaysDeprecated = ad != 0;
			isAlwaysUnavailable = au != 0;
			deprecatedMessage = dm.Unwrap ();
			unavailableMessage = um.Unwrap ();
			var ret = new ClangPlatformAvailability [n];
			for (int i = 0; i < n; i++)
				ret [i] = new ClangPlatformAvailability (ptr.Add(size * i));
			return ret;
		}

		/// <summary>
		/// Gets a set of methods that are overridden by the given
		/// method.
		/// </summary>
		/// <remarks>
		/// In both Objective-C and C++, a method (aka virtual member function,
		/// in C++) can override a virtual method in a base class. For
		/// Objective-C, a method is said to override any method in the class's
		/// base class, its protocols, or its categories' protocols, that has the same
		/// selector and is of the same kind (class or instance).
		/// If no such method exists, the search continues to the class's superclass,
		/// its protocols, and its categories, and so on. A method from an Objective-C
		/// implementation is considered to override the same methods as its
		/// corresponding method in the interface.
		///
		/// For C++, a virtual member function overrides any virtual member
		/// function with the same signature that occurs in its base
		/// classes. With multiple inheritance, a virtual member function can
		/// override several virtual member functions coming from different
		/// base classes.
		///
		/// In all cases, this function determines the immediate overridden
		/// method, rather than all of the overridden methods. For example, if
		/// a method is originally declared in a class A, then overridden in B
		/// (which in inherits from A) and also in C (which inherited from B),
		/// then the only overridden method returned from this function when
		/// invoked on C's method will be B's method. The client may then
		/// invoke this function again, given the previously-found overridden
		/// methods, to map out the complete method-override set.
		/// </remarks>
		public IEnumerable<ClangCursor> OverridenCursors {
			get {
				IntPtr ptr;
				uint n = 0;
				LibClang.clang_getOverriddenCursors (source, out ptr, ref n);
				var ptrs = new IntPtr [n];
				for (int i = 0; i < n; i++)
					LibClang.clang_getOverriddenCursors (source, out ptrs [i], ref n);
				return Enumerable.Range (0, (int) n).Select (i => new ClangCursor (ptrs [i].ToStructure<CXCursor>()));
			}
		}

		// TypeInformationForCXCursor

		/// <summary>
		/// Retrieve the type of a <see cref="ClangCursor"/> (if any).
		/// </summary>
		public ClangType CursorType {
			get { return LibClang.clang_getCursorType (source).ToManaged (); }
		}

		/// <summary>
		/// Retrieve the underlying type of a typedef declaration.
		/// </summary>
		/// <remarks>
		/// If the cursor does not reference a typedef declaration, an invalid type is
		/// returned.
		/// </remarks>
		public ClangType TypeDefDeclUnderlyingType {
			get { return LibClang.clang_getTypedefDeclUnderlyingType (source).ToManaged (); }
		}

		/// <summary>
		/// Retrieve the integer type of an enum declaration.
		/// </summary>
		/// <remarks>
		/// If the cursor does not reference an enum declaration, an invalid type is
		/// returned.
		/// </remarks>
		public ClangType EnumDeclIntegerType {
			get { return LibClang.clang_getEnumDeclIntegerType (source).ToManaged (); }
		}

		/// <summary>
		/// Retrieve the integer value of an enum constant declaration as a <see cref="SystemLongLong"/>.
		/// </summary>
		/// <remarks>
		/// If the cursor does not reference an enum constant declaration, LLONG_MIN is returned.
		/// Since this is also potentially a valid constant value, the kind of the cursor
		/// must be verified before calling this function.
		/// </remarks>
		public SystemLongLong EnumConstantDeclValue {
			get { return LibClang.clang_getEnumConstantDeclValue (source); }
		}

		/// <summary>
		/// Retrieve the integer value of an enum constant declaration as an <see cref="SystemULongLong"/>.
		/// </summary>
		/// <remarks>
		/// If the cursor does not reference an enum constant declaration, ULLONG_MAX is returned.
		/// Since this is also potentially a valid constant value, the kind of the cursor
		/// must be verified before calling this function.
		/// </remarks>
		public SystemULongLong EnumConstantDeclUnsignedValue {
			get { return LibClang.clang_getEnumConstantDeclUnsignedValue (source); }
		}

		/// <summary>
		/// Retrieve the bit width of a bit field declaration as an integer.
		/// </summary>
		/// <remarks>
		/// If a cursor that is not a bit field declaration is passed in, -1 is returned.
		/// </remarks>
		public int FieldDeclBitWidth {
			get { return LibClang.clang_getFieldDeclBitWidth (source); }
		}

		/// <summary>
		/// Retrieve the number of non-variadic arguments associated with a given
		/// cursor.
		/// </summary>
		/// <remarks>
		/// The number of arguments can be determined for calls as well as for
		/// declarations of functions or methods. For other cursors -1 is returned.
		/// </remarks>
		public int ArgumentCount {
			get { return LibClang.clang_Cursor_getNumArguments (source); }
		}

		/// <summary>
		/// Retrieve the argument cursor of a function or method.
		/// </summary>
		/// <remarks>
		/// The argument cursor can be determined for calls as well as for declarations
		/// of functions or methods. For other cursors and for invalid indices, an
		/// invalid cursor is returned.
		/// </remarks>
		public ClangCursor GetArgument (int index)
		{
			return new ClangCursor (LibClang.clang_Cursor_getArgument (source, (uint) index));
		}

		/// <summary>
		/// Returns the Objective-C type encoding for the specified declaration.
		/// </summary>
		public string DeclObjCTypeEncoding {
			get { return LibClang.clang_getDeclObjCTypeEncoding (source).Unwrap (); }
		}

		/// <summary>
		/// Retrieve the result type associated with a given cursor.
		/// </summary>
		/// <remarks>
		/// This only returns a valid type if the cursor refers to a function or method.
		/// </remarks>
		public ClangType ResultType {
			get { return LibClang.clang_getCursorResultType (source).ToManaged (); }
		}

		/// <summary>
		/// Returns <c>true</c> if the cursor specifies a Record member that is a
		/// bitfield.
		/// </summary>
		public bool IsBitField {
			get { return LibClang.clang_Cursor_isBitField (source) != 0; }
		}

		/// <summary>
		/// Returns <c>true</c> if the base class specified by the cursor with kind
		/// CX_CXXBaseSpecifier is virtual.
		/// </summary>
		public bool IsVirtualBase {
			get { return LibClang.clang_isVirtualBase (source) != 0; }
		}

		/// <summary>
		/// Returns the access control level for the referenced object.
		/// </summary>
		/// <remarks>
		/// If the cursor refers to a C++ declaration, its access control level within its
		/// parent scope is returned. Otherwise, if the cursor refers to a base specifier or
		/// access specifier, the specifier itself is returned.
		/// </remarks>
		public CXXAccessSpecifier CxxAccessSpecifier {
			get { return LibClang.clang_getCXXAccessSpecifier (source); }
		}

		/// <summary>
		/// Determine the number of overloaded declarations referenced by a 
		/// <c>CXCursor_OverloadedDeclRef</c> cursor.
		/// </summary>
		/// <value>
		/// The number of overloaded declarations referenced by this cursor. If it
		/// is not a <c>CXCursor_OverloadedDeclRef</c> cursor, returns 0.
		/// </value>
		public int OverloadedDeclarationCount {
			get { return (int) LibClang.clang_getNumOverloadedDecls (source); }
		}

		/// <summary>
		/// Retrieve a cursor for one of the overloaded declarations referenced
		/// by a <c>CXCursor_OverloadedDeclRef</c> cursor.
		/// </summary>
		/// <param name="index">The zero-based index into the set of overloaded declarations in
		/// the cursor.</param>
		/// <returns>
		/// A cursor representing the declaration referenced by the given 
		/// cursor at the specified <paramref name="index"/>. If the cursor does not have an 
		/// associated set of overloaded declarations, or if the index is out of bounds,
		/// returns <c>null</c>.
		/// </returns>
		public ClangCursor GetOverloadedDeclaration (int index)
		{
			return new ClangCursor (LibClang.clang_getOverloadedDecl (source, (uint) index));
		}

		// CodeCompletion

		/// <summary>
		/// Retrieve a completion string for an arbitrary declaration or macro
		/// definition cursor.
		/// </summary>
		/// <value>
		/// A non-context-sensitive completion string for declaration and macro
		/// definition cursors, or <c>null</c> for other kinds of cursors.
		/// </value>
		public ClangCompletionString CompletionString {
			get { return new ClangCompletionString (LibClang.clang_getCursorCompletionString (source)); }
		}

		// TraversingASTWithCursor

		/// <summary>
		/// Visit the children of a particular cursor.
		/// </summary>
		/// <remarks>
		/// This function visits all the direct children of the given cursor,
		/// invoking the given <paramref name="visitor"/> function with the cursors of each
		/// visited child. The traversal may be recursive, if the visitor returns
		/// <c>ChildVisitResult.Recurse</c>. The traversal may also be ended prematurely, if
		/// the visitor returns <c>ChildVisitResult.Break</c>.
		/// </remarks>
		/// <param name="visitor">The visitor function that will be invoked for each child.</param>
		/// <param name="clientData">Data supplied by the client, which will
		/// be passed to the visitor each time it is invoked.</param>
		/// <returns>
		/// <c>true</c> if the traversal was terminated prematurely by the visitor returning
		/// <c>ChildVisitResult.Break</c>.
		/// </returns>
		public bool VisitChildren (Func<ClangCursor,ClangCursor,IntPtr,ChildVisitResult> visitor, IntPtr clientData)
		{
			var ret = LibClang.clang_visitChildren (source, (cursor, parent, cd) => visitor (new ClangCursor (cursor), new ClangCursor (parent), cd), clientData);
			return ret != 0;
		}

		/// <summary>
		/// Return an iterator for accessing the children of this cursor.
		/// </summary>
		/// <returns></returns>
		public ReadOnlyCollection<ClangCursor> GetChildren ()
		{
			List<ClangCursor> children = new List<ClangCursor> ();

			this.VisitChildren ((child, parent, data) => {
				children.Add (child);
				return ChildVisitResult.Continue;
			}, IntPtr.Zero);

			return new ReadOnlyCollection<ClangCursor> (children);
		}

		/// <summary>
		/// Return an iterator for accessing the children of this cursor that are
		/// of the specified <paramref name="kind"/>.
		/// </summary>
		/// <returns></returns>
		public ReadOnlyCollection<ClangCursor> GetChildrenOfKind (CursorKind kind)
		{
			List<ClangCursor> children = new List<ClangCursor> ();

			this.VisitChildren ((child, parent, data) => {
				if (child.Kind == kind) {
					children.Add (child);
				}
				return ChildVisitResult.Continue;
			}, IntPtr.Zero);

			return new ReadOnlyCollection<ClangCursor> (children);
		}

		// CrossReferencingAST

		/// <summary>
		/// Retrieve a Unified Symbol Resolution (USR) for the entity referenced
		/// by the given cursor.
		/// </summary>
		/// <remarks>
		/// A Unified Symbol Resolution (USR) is a string that identifies a particular
		/// entity (function, class, variable, etc.) within a program. USRs can be
		/// compared across translation units to determine, e.g., when references in
		/// one translation refer to an entity defined in another translation unit.
		/// </remarks>
		public string UnifiedSymbolResolution {
			get { return LibClang.clang_getCursorUSR (source).Unwrap (); }
		}

		/// <summary>
		/// Retrieve a name for the entity referenced by this cursor.
		/// </summary>
		public string Spelling {
			get { return LibClang.clang_getCursorSpelling (source).Unwrap (); }
		}

		/// <summary>
		/// Retrieve a range for a piece that forms the cursors spelling name.
		/// </summary>
		/// <remarks>
		/// Most of the times there is only one range for the complete spelling but for
		/// objc methods and objc message expressions, there are multiple pieces for each
		/// selector identifier.
		/// </remarks>
		/// <param name="pieceIndex">The index of the spelling name piece. If this is greater
		/// than the actual number of pieces, it will return a <c>null</c> (invalid) range.
		/// </param>
		/// <returns></returns>
		public ClangSourceRange GetSpellingNameRange (int pieceIndex)
		{
			return new ClangSourceRange (LibClang.clang_Cursor_getSpellingNameRange (source, (uint) pieceIndex, 0)); // dummy
		}

		/// <summary>
		/// Retrieve the display name for the entity referenced by this cursor.
		/// </summary>
		/// <remarks>
		/// The display name contains extra information that helps identify the cursor,
		/// such as the parameters of a function or template or the arguments of a 
		/// class template specialization.
		/// </remarks>
		public string DisplayName {
			get { return LibClang.clang_getCursorDisplayName (source).Unwrap (); }
		}

		/// <summary>
		/// For a cursor that is a reference, retrieve a cursor representing the
		/// entity that it references.
		/// </summary>
		/// <remarks>
		/// Reference cursors refer to other entities in the AST. For example, an
		/// Objective-C superclass reference cursor refers to an Objective-C class.
		/// This function produces the cursor for the Objective-C class from the
		/// cursor for the superclass reference. If the input cursor is a declaration or
		/// definition, it returns that declaration or definition unchanged.
		/// Otherwise, returns the <c>null</c> cursor.
		/// </remarks>
		public ClangCursor Referenced {
			get { return new ClangCursor (LibClang.clang_getCursorReferenced (source)); }
		}

		/// <summary>
		/// For a cursor that is either a reference to or a declaration
		/// of some entity, retrieve a cursor that describes the definition of
		/// that entity.
		/// </summary>
		/// <remarks>
		///  Some entities can be declared multiple times within a translation
		///  unit, but only one of those declarations can also be a
		///  definition. For example, given:
		///
		///  <code>
		///  int f(int, int);
		///  int g(int x, int y) { return f(x, y); }
		///  int f(int a, int b) { return a + b; }
		///  int f(int, int);
		///  </code>
		///
		///  there are three declarations of the function "f", but only the
		///  second one is a definition. The clang_getCursorDefinition()
		///  function will take any cursor pointing to a declaration of "f"
		///  (the first or fourth lines of the example) or a cursor referenced
		///  that uses "f" (the call to "f' inside "g") and will return a
		///  declaration cursor pointing to the definition (the second "f"
		///  declaration).
		///
		///  If given a cursor for which there is no corresponding definition,
		///  e.g., because there is no definition of that entity within this
		///  translation unit, returns a NULL cursor.
		/// </remarks>
		public ClangCursor Definition {
			get { return new ClangCursor (LibClang.clang_getCursorDefinition (source)); }
		}

		/// <summary>
		/// Determine whether the declaration pointed to by this cursor
		/// is also a definition of that entity.
		/// </summary>
		public bool IsDefinition {
			get { return LibClang.clang_isCursorDefinition (source) != 0; }
		}

		/// <summary>
		/// Retrieve the canonical cursor corresponding to the given cursor.
		/// </summary>
		/// <remarks>
		/// In the C family of languages, many kinds of entities can be declared several
		/// times within a single translation unit. For example, a structure type can
		/// be forward-declared (possibly multiple times) and later defined:
		///
		/// <code>
		/// struct X;
		/// struct X;
		/// struct X {
		///   int member;
		/// };
		/// </code>
		///
		/// The declarations and the definition of <c>X</c> are represented by three 
		/// different cursors, all of which are declarations of the same underlying 
		/// entity. One of these cursor is considered the "canonical" cursor, which
		/// is effectively the representative for the underlying entity. One can 
		/// determine if two cursors are declarations of the same underlying entity by
		/// comparing their canonical cursors.
		/// </remarks>
		/// <value>The canonical cursor for the entity referred to by the given cursor.</value>
		public ClangCursor CanonicalCursor {
			get { return new ClangCursor (LibClang.clang_getCanonicalCursor (source)); }
		}

		/// <summary>
		/// If the cursor points to a selector identifier in a objc method or
		/// message expression, this returns the selector index.
		/// </summary>
		/// <remarks>
		/// After getting a cursor, this can be called to
		/// determine if the location points to a selector identifier.
		/// </remarks>
		/// <value>
		/// The selector index if the cursor is an objc method or message
		/// expression and the cursor is pointing to a selector identifier, or -1
		/// otherwise.
		/// </value>
		public int ObjectSelectorIndex {
			get { return LibClang.clang_Cursor_getObjCSelectorIndex (source); }
		}

		/// <summary>
		/// Given a cursor pointing to a C++ method call or an ObjC message,
		/// returns non-zero if the method/message is "dynamic", meaning:
		/// 
		/// For a C++ method: the call is virtual.
		/// For an ObjC message: the receiver is an object instance, not 'super' or a
		/// specific class.
		/// 
		/// If the method/message is "static" or the cursor does not point to a
		/// method/message, it will return zero.
		/// </summary>
		public bool IsDynamicCall {
			get { return LibClang.clang_Cursor_isDynamicCall (source) != 0; }
		}

		/// <summary>
		/// Gets the <see cref="ClangType"/> of the receiver.
		/// </summary>
		public ClangType ReceiverType {
			get { return LibClang.clang_Cursor_getReceiverType (source).ToManaged (); }
		}

		/// <summary>
		/// Given a cursor that represents a property declaration, return the
		/// associated property attributes.
		/// </summary>
		public ObjCPropertyAttributeFlags ObjCPropertyAttributes {
			get {
				return (ObjCPropertyAttributeFlags) LibClang.clang_Cursor_getObjCPropertyAttributes (source, 0);
			}
		}

		/// <summary>
		/// Given a cursor that represents an ObjC method or parameter
		/// declaration, return the associated ObjC qualifiers for the return type or the
		/// parameter respectively.
		/// </summary>
		public ObjCDeclarationQualifierFlags ObjCDeclQualifiers {
			get { return (ObjCDeclarationQualifierFlags) LibClang.clang_Cursor_getObjCDeclQualifiers (source); }
		}

		/// <summary>
		/// Given a cursor that represents an ObjC method or property declaration,
		/// return <c>true</c> if the declaration was affected by "@optional".
		/// Returns <c>false</c> if the cursor is not such a declaration or it is "@required".
		/// </summary>
		public bool IsObjCOptional {
			get { return LibClang.clang_Cursor_isObjCOptional (source) != 0; }
		}

		/// <summary>
		/// Returns <c>true</c> if the given cursor is a variadic function or method.
		/// </summary>
		public bool IsVariadic {
			get { return LibClang.clang_Cursor_isVariadic (source) != 0; }
		}

		/// <summary>
		/// Given a cursor that represents a declaration, return the associated
		/// comment's source range. The range may include multiple consecutive comments
		/// with whitespace in between.
		/// </summary>
		public ClangSourceRange CommentRange {
			get { return new ClangSourceRange (LibClang.clang_Cursor_getCommentRange (source)); }
		}

		/// <summary>
		/// Given a cursor that represents a declaration, return the associated
		/// comment text, including comment markers.
		/// </summary>
		public string RawCommentText {
			get { return LibClang.clang_Cursor_getRawCommentText (source).Unwrap (); }
		}

		/// <summary>
		/// Given a cursor that represents a documentable entity (e.g.,
		/// declaration), return the associated \\brief paragraph; otherwise return the
		/// first paragraph.
		/// </summary>
		public string BriefCommentText {
			get { return LibClang.clang_Cursor_getBriefCommentText (source).Unwrap (); }
		}

		/// <summary>
		/// Given a cursor that represents a documentable entity (e.g.,
		/// declaration), return the associated parsed comment as a
		/// <c>CXComment_FullComment</c> AST node.
		/// </summary>
		public ClangComment ParsedComment {
			get { return new ClangComment (LibClang.clang_Cursor_getParsedComment (source)); }
		}

		// MappingBetweenCursorAndLocation

		/// <summary>
		/// Retrieve the physical location of the source constructor referenced
		/// by the given cursor.
		/// </summary>
		/// <remarks>
		/// The location of a declaration is typically the location of the name of that
		/// declaration, where the name of that declaration would occur if it is
		/// unnamed, or some keyword that introduces that particular declaration.
		/// The location of a reference is where that reference occurs within the
		/// source code.
		/// </remarks>
		public ClangSourceLocation Location {
			get { return new ClangSourceLocation (LibClang.clang_getCursorLocation (source)); }
		}

		/// <summary>
		/// Retrieve the physical extent of the source construct referenced by
		/// the given cursor.
		/// </summary>
		/// <remarks>
		/// The extent of a cursor starts with the file/line/column pointing at the
		/// first character within the source construct that the cursor refers to and
		/// ends with the last character withinin that source construct. For a
		/// declaration, the extent covers the declaration itself. For a reference,
		/// the extent covers the location of the reference (e.g., where the referenced
		/// entity was actually used).
		/// </remarks>
		public ClangSourceRange CursorExtent {
			get { return new ClangSourceRange (LibClang.clang_getCursorExtent (source)); }
		}

		// CppASTIntrospection

		/// <summary>
		/// Determine if a C++ member function or member function template is
		/// pure virtual.
		/// </summary>
		public bool IsCxxPureVirtual {
			get { return LibClang.clang_CXXMethod_isPureVirtual (source) != 0; }
		}

		/// <summary>
		/// Determine if a C++ member function or member function template is 
		/// declared 'static'.
		/// </summary>
		public bool IsCxxStatic {
			get { return LibClang.clang_CXXMethod_isStatic (source) != 0; }
		}

		/// <summary>
		/// Determine if a C++ member function or member function template is
		/// explicitly declared 'virtual' or if it overrides a virtual method from
		/// one of the base classes.
		/// </summary>
		public bool IsCxxVirtual {
			get { return LibClang.clang_CXXMethod_isVirtual (source) != 0; }
		}

		public bool IsCxxConst {
			get { return LibClang.clang_CXXMethod_isConst (source) != 0; }
		}

		/// <summary>
		/// Given a cursor that represents a template, determine
		/// the cursor kind of the specializations would be generated by instantiating
		/// the template.
		/// </summary>
		/// <remarks>
		/// This routine can be used to determine what flavor of function template,
		/// class template, or class template partial specialization is stored in the
		/// cursor. For example, it can describe whether a class template cursor is
		/// declared with "struct", "class" or "union".
		/// </remarks>
		/// <value>
		/// The cursor kind of the specializations that would be generated
		/// by instantiating the template \p C. If \p C is not a template, returns
		/// <c>CursorKind.NoDeclarationFound</c>.
		/// </value>
		public CursorKind TemplateCursorKind {
			get { return LibClang.clang_getTemplateCursorKind (source); }
		}

		/// <summary>
		/// Given a cursor that may represent a specialization or instantiation
		/// of a template, retrieve the cursor that represents the template that it
		/// specializes or from which it was instantiated.
		/// </summary>
		/// <remarks>
		/// This routine determines the template involved both for explicit 
		/// specializations of templates and for implicit instantiations of the template,
		/// both of which are referred to as "specializations". For a class template
		/// specialization (e.g., <c>std::vector&lt;bool&gt;</c>), this routine will return 
		/// either the primary template (<c>std::vector</c>) or, if the specialization was
		/// instantiated from a class template partial specialization, the class template
		/// partial specialization. For a class template partial specialization and a
		/// function template specialization (including instantiations), this
		/// this routine will return the specialized template.
		///
		/// For members of a class template (e.g., member functions, member classes, or
		/// static data members), returns the specialized or instantiated member. 
		/// Although not strictly "templates" in the C++ language, members of class
		/// templates have the same notions of specializations and instantiations that
		/// templates do, so this routine treats them similarly.
		/// </remarks>
		/// <value>
		/// If the given cursor is a specialization or instantiation of a 
		/// template or a member thereof, the template or member that it specializes or
		/// from which it was instantiated. Otherwise, returns a NULL cursor.
		/// </value>
		public ClangCursor SpecializedCursorTemplate {
			get { return new ClangCursor (LibClang.clang_getSpecializedCursorTemplate (source)); }
		}

		/// <summary>
		/// Given a cursor that references something else, return the source range
		/// covering that reference.
		/// </summary>
		/// <param name="nameFlags">A bitset with three independent flags.</param>
		/// <param name="pieceIndex">For contiguous names or when passing the flag
		/// <c>NameRefFlags.WantSinglePiece</c>, only one piece with index 0 is 
		/// available. When the <c>NameRefFlags.WantSinglePiece</c> flag is not passed for a
		/// non-contiguous names, this index can be used to retrieve the individual
		/// pieces of the name. See also <c>NameRefFlags.WantSinglePiece</c>.
		/// </param>
		/// <returns>
		/// The piece of the name pointed to by the given cursor. If there is no
		/// name, or if the PieceIndex is out-of-range, a null-cursor will be returned.
		/// </returns>
		public ClangSourceRange GetCursorReferenceNameRange (NameRefFlags nameFlags, int pieceIndex)
		{
			return new ClangSourceRange (LibClang.clang_getCursorReferenceNameRange (source, nameFlags, (uint) pieceIndex));
		}

		// InformationForAttributes

		/// <summary>
		/// For cursors representing an iboutletcollection attribute,
		/// this returns the collection element type.
		/// </summary>
		public ClangType IBOutletCollectionType {
			get { return LibClang.clang_getIBOutletCollectionType (source).ToManaged (); }
		}

		// ModuleIntrospection

		/// <summary>
		/// Given a CXCursor_ModuleImportDecl cursor, return the associated module.
		/// </summary>
		public ClangModule Module {
			get { return new ClangModule (LibClang.clang_Cursor_getModule (source)); }
		}

		// HighLevelAPI

		/// <summary>
		/// Find references of a declaration in a specific file.
		/// </summary>
		/// <param name="file">File to search for references.</param>
		/// <param name="visitor">callback that will receive pairs of ClangCursor/ClangSourceRange for
		/// each reference found.
		/// </param>
		/// <returns>One of the FindResult enumerators.</returns>
		public FindResult FindReferenceInFile (ClangFile file, Func<object,ClangCursor,ClangSourceRange,VisitorResult> visitor)
		{
			return LibClang.clang_findReferencesInFile (source, file.Handle, new CXCursorAndRangeVisitor ((ctx, cursor, range) => visitor (ctx, new ClangCursor (cursor), new ClangSourceRange (range))));
		}
	}
}
