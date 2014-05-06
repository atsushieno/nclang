using System;
using NUnit.Framework;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangTypeTest
	{
		[Test]
		public void NullCursorType ()
		{
			var c = ClangCursor.GetNullCursor ();
			Assert.AreEqual (-1, c.ArgumentCount, "ArgumentCount");
			Assert.AreEqual (AvailabilityKind.Available, c.AvailabilityKind, "AvailabilityKind");

			var t = c.CursorType;

			Assert.AreEqual (-1, t.AlignOf, "AlignOf");
			Assert.AreEqual (-1, t.ArgumentTypeCount, "ArgumentTypeCount");
			var tt = t.ArrayElementType;
			Assert.IsNotNull (tt, "ArrayElementType");
			Assert.AreEqual (-1, t.ArraySize, "ArraySize");
			tt = t.CanonicalType;
			Assert.IsNotNull (tt, "CanonicalType");
			tt = t.ClassType;
			Assert.IsNotNull (tt, "ClassType");
			Assert.AreEqual (-1, t.ElementCount, "ElementCount");
			tt = t.ElementType;
			Assert.IsNotNull (tt, "ElementType");
			Assert.AreEqual (CallingConvention.Invalid, t.FunctionTypeCallingConvention, "FunctionTypeCallingConvention");
			Assert.AreEqual (false, t.IsConstQualifiedType, "IsConstQualifiedType");
			Assert.AreEqual (false, t.IsFunctionTypeVariadic, "IsFunctionTypeVariadic");
			Assert.AreEqual (false, t.IsPODType, "IsPODType");
			Assert.AreEqual (false, t.IsRestrictQualifiedType, "IsRestrictQualifiedType");
			Assert.AreEqual (false, t.IsVolatileQualifiedType, "IsVolatileQualifiedType");
			Assert.AreEqual (TypeKind.Invalid, t.Kind, "Kind");
			tt = t.PointeeType;
			Assert.IsNotNull (tt, "PointeeType");
			Assert.AreEqual (RefQualifierKind.None, t.RefQualifier, "RefQualifier");
			tt = t.ResultType;
			Assert.IsNotNull (tt, "ResultType");
			Assert.AreEqual (-1, t.SizeOf, "SizeOf");
			Assert.AreEqual (string.Empty, t.Spelling, "Spelling");
			// not in libclang 3.5
			// Assert.AreEqual (-1, t.TemplateArgumentCount, "TemplateArgumentCount");
			var cc = t.TypeDeclaration;
			Assert.IsNotNull (cc, "TypeDeclaration");
		}
	}
}

