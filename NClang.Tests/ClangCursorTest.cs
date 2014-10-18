using System;
using NUnit.Framework;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangCursorTest
	{
		[Test]
		public void NullCursor ()
		{
			var c = ClangService.GetNullCursor ();
			Assert.AreEqual (-1, c.ArgumentCount, "ArgumentCount");
			Assert.AreEqual (AvailabilityKind.Available, c.AvailabilityKind, "AvailabilityKind");

			var t = c.CursorType;
			Assert.IsNull (t, "CursorType");

			Assert.AreEqual (CXXAccessSpecifier.Invalid, c.CxxAccessSpecifier, "CxxAccessSpecifier");
			Assert.AreEqual (string.Empty, c.DeclObjCTypeEncoding, "DeclObjCTypeEncoding");
			Assert.AreEqual (18446744073709551615m, c.EnumConstantDeclUnsignedValue, "EnumConstantDeclUnsignedValue");
			Assert.AreEqual (-9223372036854775808m, c.EnumConstantDeclValue, "EnumConstantDeclValue");

			t = c.EnumDeclIntegerType;
			Assert.IsNull (t, "EnumDeclIntegerType");

			Assert.AreEqual (-1, c.FieldDeclBitWidth, "FieldDeclBitWidth");
			Assert.AreEqual (null, c.IncludedFile, "IncludedFile");
			Assert.AreEqual (false, c.IsBitField, "IsBitField");
			Assert.AreEqual (false, c.IsVirtualBase, "IsVirtualBase");
			Assert.AreEqual (CursorKind.FirstInvalid, c.Kind, "Kind");
			Assert.AreEqual (LanguageKind.Invalid, c.Language, "Language");

			var cc = c.LexicalParent;
			Assert.IsNotNull (cc, "LexicalParent");

			Assert.AreEqual (LinkageKind.Invalid, c.Linkage, "Linkage");
			Assert.AreEqual (0, c.OverloadedDeclarationCount, "OverloadedDeclarationCount");
			t = c.ResultType;
			Assert.IsNull (t, "ResultType");

			cc = c.SemanticParent;
			Assert.IsNotNull (cc, "SemanticParent");

			Assert.IsNull (c.TranslationUnit, "TranslationUnit");
			t = c.TypeDefDeclUnderlyingType;
			Assert.IsNull (t, "TypeDefDeclUnderlyingType");
		}

		[Test]
		public void FromTranslationUnit ()
		{
			string filename = "ClangCursorTest.FromTranslationUnit.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var c = tu.GetCursor ();
				Assert.AreEqual (-1, c.ArgumentCount, "ArgumentCount");
				Assert.AreEqual (AvailabilityKind.Available, c.AvailabilityKind, "AvailabilityKind");

				var t = c.CursorType;
				Assert.IsNull (t, "CursorType");

				Assert.AreEqual (CXXAccessSpecifier.Invalid, c.CxxAccessSpecifier, "CxxAccessSpecifier");
				Assert.AreEqual (string.Empty, c.DeclObjCTypeEncoding, "DeclObjCTypeEncoding");
				Assert.AreEqual (18446744073709551615m, c.EnumConstantDeclUnsignedValue, "EnumConstantDeclUnsignedValue");
				Assert.AreEqual (-9223372036854775808m, c.EnumConstantDeclValue, "EnumConstantDeclValue");

				t = c.EnumDeclIntegerType;
				Assert.IsNull (t, "EnumDeclIntegerType");

				Assert.AreEqual (-1, c.FieldDeclBitWidth, "FieldDeclBitWidth");
				Assert.AreEqual (null, c.IncludedFile, "IncludedFile");
				Assert.AreEqual (false, c.IsBitField, "IsBitField");
				Assert.AreEqual (false, c.IsVirtualBase, "IsVirtualBase");
				Assert.AreEqual (CursorKind.TranslationUnit, c.Kind, "Kind");
				Assert.AreEqual (LanguageKind.Invalid, c.Language, "Language");

				var cc = c.LexicalParent;
				Assert.IsNotNull (cc, "LexicalParent");

				Assert.AreEqual (LinkageKind.Invalid, c.Linkage, "Linkage");
				Assert.AreEqual (0, c.OverloadedDeclarationCount, "OverloadedDeclarationCount");
				t = c.ResultType;
				Assert.IsNull (t, "ResultType");

				cc = c.SemanticParent;
				Assert.IsNotNull (cc, "SemanticParent");

				Assert.AreEqual (tu, c.TranslationUnit, "TranslationUnit");
				t = c.TypeDefDeclUnderlyingType;
				Assert.IsNull (t, "TypeDefDeclUnderlyingType");
			}, filename);
		}

		[Test]
		public void FromTranslationUnitParents ()
		{
			string filename = "ClangCursorTest.FromTranslationUnit.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var cursor = tu.GetCursor ();
				Assert.AreEqual (AvailabilityKind.Available, cursor.AvailabilityKind, "AvailabilityKind");
				Assert.AreEqual (CursorKind.TranslationUnit, cursor.Kind, "Kind");
				Assert.AreEqual (LanguageKind.Invalid, cursor.Language, "Language");
				Assert.AreEqual (LinkageKind.Invalid, cursor.Linkage, "LinkageKind");

				var lp = cursor.LexicalParent;
				Assert.IsNotNull (lp, "LexicalParent");
				Assert.AreEqual (AvailabilityKind.Available, lp.AvailabilityKind, "lp.AvailabilityKind");
				Assert.AreEqual (CursorKind.FirstInvalid, lp.Kind, "lp.Kind");
				Assert.AreEqual (LanguageKind.Invalid, lp.Language, "lp.Language");
				Assert.AreEqual (LinkageKind.Invalid, lp.Linkage, "lp.LinkageKind");

				var sp = cursor.SemanticParent;
				Assert.IsNotNull (sp, "SemanticParent");
				Assert.AreEqual (AvailabilityKind.Available, sp.AvailabilityKind, "sp.AvailabilityKind");
				Assert.AreEqual (CursorKind.FirstInvalid, sp.Kind, "sp.Kind");
				Assert.AreEqual (LanguageKind.Invalid, sp.Language, "sp.Language");
				Assert.AreEqual (LinkageKind.Invalid, sp.Linkage, "sp.LinkageKind");

				Assert.AreEqual (tu, cursor.TranslationUnit, "TranslationUnit");
				var file = cursor.IncludedFile;
				Assert.IsNull (file, "file");
			}, filename);
		}

		// based on http://eli.thegreenplace.net/2011/07/03/parsing-c-in-python-with-clang/ but results seem to be different...
		[Test]
		public void NodeTraversalWithVisitor ()
		{
			string source = @"
bool foo()
{
    return true;
}

void bar()
{
    foo();
    for (int i = 0; i < 10; ++i)
        foo();
}

int main()
{
    bar();
    if (foo())
        bar();
}";
			string filename = "ClangCursorTest.NodeTraversalWithVisitor.c";
			var file = new ClangUnsavedFile (filename, source);
			using (var idx = ClangService.CreateIndex ()) {
				using (var tu = idx.ParseTranslationUnit (filename, new string [0], new ClangUnsavedFile [] {file}, TranslationUnitFlags.None)) {
					Func<ClangCursor,ClangCursor,IntPtr,ChildVisitResult> func = (cursor, parent, clientData) => {
						if (cursor.Kind == CursorKind.CallExpression)
							Console.Error.WriteLine ("Found {0} [line:{1}, column:{2}]", cursor.DisplayName, cursor.Location.FileLocation.Line, cursor.Location.FileLocation.Column);
						else
							Console.Error.WriteLine ("different kind {0} ({1}, {2})", cursor.Kind, cursor.Location.FileLocation.Line, cursor.Location.FileLocation.Column);
						return ChildVisitResult.Recurse;
					};
					tu.GetCursor ().VisitChildren (func, IntPtr.Zero);
				}
			}
		}
	}
}

