using System;
using NUnit.Framework;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangSourceLocationTest
	{
		[Test]
		public void TranslationUnitGetLocation ()
		{
			string filename = "TranslationUnitTest.GetLocation.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var file = tu.GetFile (filename);
				var loc = tu.GetLocation (file, 5, 4);
				Assert.AreEqual (5, loc.ExpansionLocation.Line, "ExpansionLocation.Line");
				Assert.AreEqual (4, loc.ExpansionLocation.Column, "ExpansionLocation.Column");
				Assert.AreEqual (39, loc.ExpansionLocation.Offset, "ExpansionLocation.Offset");
				Assert.AreEqual (5, loc.FileLocation.Line, "FileLocation.Line");
				Assert.AreEqual (4, loc.FileLocation.Column, "FileLocation.Column");
				Assert.AreEqual (39, loc.FileLocation.Offset, "FileLocation.Offset");
				Assert.AreEqual (5, loc.InstantiationLocation.Line, "InstantiationLocation.Line");
				Assert.AreEqual (4, loc.InstantiationLocation.Column, "InstantiationLocation.Column");
				Assert.AreEqual (39, loc.InstantiationLocation.Offset, "InstantiationLocation.Offset");
				Assert.AreEqual (true, loc.IsFromMainFile, "IsFromMainFile");
				Assert.AreEqual (false, loc.IsInSystemHeader, "IsInSystemHeader");
				Assert.AreEqual (5, loc.PresumedLocation.Line, "PresumedLocation.Line");
				Assert.AreEqual (4, loc.PresumedLocation.Column, "PresumedLocation.Column");
				Assert.AreEqual (5, loc.SpellingLocation.Line, "SpellingLocation.Line");
				Assert.AreEqual (4, loc.SpellingLocation.Column, "SpellingLocation.Column");
				Assert.AreEqual (39, loc.SpellingLocation.Offset, "SpellingLocation.Offset");
			}, filename);
		}

		[Test]
		public void TranslationUnitGetLocationForOffset ()
		{
			string filename = "TranslationUnitTest.GetLocation.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var file = tu.GetFile (filename);
				var loc = tu.GetLocationForOffset (file, 20);
				Assert.AreEqual (3, loc.ExpansionLocation.Line, "ExpansionLocation.Line");
				Assert.AreEqual (1, loc.ExpansionLocation.Column, "ExpansionLocation.Column");
				Assert.AreEqual (20, loc.ExpansionLocation.Offset, "ExpansionLocation.Offset");
				Assert.AreEqual (3, loc.FileLocation.Line, "FileLocation.Line");
				Assert.AreEqual (1, loc.FileLocation.Column, "FileLocation.Column");
				Assert.AreEqual (20, loc.FileLocation.Offset, "FileLocation.Offset");
				Assert.AreEqual (3, loc.InstantiationLocation.Line, "InstantiationLocation.Line");
				Assert.AreEqual (1, loc.InstantiationLocation.Column, "InstantiationLocation.Column");
				Assert.AreEqual (20, loc.InstantiationLocation.Offset, "InstantiationLocation.Offset");
				Assert.AreEqual (true, loc.IsFromMainFile, "IsFromMainFile");
				Assert.AreEqual (false, loc.IsInSystemHeader, "IsInSystemHeader");
				Assert.AreEqual (3, loc.PresumedLocation.Line, "PresumedLocation.Line");
				Assert.AreEqual (1, loc.PresumedLocation.Column, "PresumedLocation.Column");
				Assert.AreEqual (3, loc.SpellingLocation.Line, "SpellingLocation.Line");
				Assert.AreEqual (1, loc.SpellingLocation.Column, "SpellingLocation.Column");
				Assert.AreEqual (20, loc.SpellingLocation.Offset, "SpellingLocation.Offset");
			}, filename);
		}

		[Test]
		public void TranslationUnitGetLocationBeyondRange ()
		{
			string filename = "TranslationUnitTest.GetLocation.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var file = tu.GetFile (filename);
				var loc = tu.GetLocation (file, 100, 100);
				// anything beyond the actual range is shrinked to the max location...
				Assert.AreEqual (6, loc.ExpansionLocation.Line, "ExpansionLocation.Line");
				Assert.AreEqual (2, loc.ExpansionLocation.Column, "ExpansionLocation.Column");
				Assert.AreEqual (63, loc.ExpansionLocation.Offset, "ExpansionLocation.Offset");

				// For GetLocationForOffset(), I tried some, but cannot predict the results...
			}, filename);
		}
	}
}

