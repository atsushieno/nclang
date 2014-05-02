using System;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangTranslationUnitTest
	{
		[Test]
		public void DefaultEditingTranslationUnitOptions ()
		{
			Assert.AreEqual (TranslationUnitFlags.PrecompiledPreamble | TranslationUnitFlags.CacheCompletionResults, ClangTranslationUnit.DefaultEditingTranslationUnitOptions, "#1");
		}

		// TranslationUnitManipulation
		[Test]
		public void DefaultProperties ()
		{
			string filename = "TranslationUnitTest.DefaultProperties.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				Assert.AreEqual (SaveTranslationUnitFlags.None, tu.DefaultSaveOptions, "DefaultSaveOptions");
				Assert.AreEqual (ReparseTranslationUnitFlags.None, tu.DefaultReparseOptions, "DefaultReparseOptions");
				Assert.AreEqual (filename, tu.TranslationUnitSpelling, "TranslationUnitSpelling");
				Assert.AreEqual (1, tu.DiagnosticCount, "DiagnosticCount");
			}, filename);
		}

		[Test]
		public void DiagnosticSet ()
		{
			string filename = "TranslationUnitTest.DiagnosticSet.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				Action<ClangDiagnostic,string> test = (d, label) => {
					Assert.IsNotNull (d, label + "Diagnostic");
					Assert.AreEqual (2, d.Category, label + "D.Category");
					Assert.AreEqual ("Semantic Issue", d.CategoryText, label + "D.CategoryText");
					Assert.IsNotNull (d.ChildDiagnostics, label + "D.ChildDiagnostics");
					Assert.AreEqual (1, d.ChildDiagnostics.Count, label + "D.ChildDiagnostics.Count");
					//Assert.AreEqual ("", d.ChildDiagnostics.Items.First ().CategoryText, label + "D.ChildDiagnostics.Items.First ().CategoryText");
					Assert.AreEqual (0, d.FixItCount, label + "D.FixItCount");
					Assert.AreEqual (true, d.Location.IsFromMainFile, label + "D.Location.IsFromMainFile");
					Assert.AreEqual (false, d.Location.IsInSystemHeader, label + "D.Location.IsInSystemHeader");
					Assert.AreEqual ("-Wmain-return-type", d.Options.Enable, label + "D.Options.Enable");
					Assert.AreEqual ("-Wno-main-return-type", d.Options.Disable, label + "D.Options.Disable");
				};
				test (tu.GetDiagnostic (0), "From TranslationUnit:");
				using (var dset = tu.DiagnosticSet) {
					Assert.AreEqual (1, dset.Count, "Set.Count");
					test (dset.Get (0), "From Set:");
				}
			}, filename);
		}

		[Test]
		public void ResourceUsage ()
		{
			string filename = "TranslationUnitTest.ResourceUsage.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				using (var ru = tu.GetResourceUsage ()) {
					Assert.AreEqual (12, ru.Count, "ResourceUsage.Count");
					for (int i = 0; i < ru.Count; i++)
						ru.GetEntry (i);
				}
			}, filename);
		}

		[Test]
		public void GetFile ()
		{
			string filename = "TranslationUnitTest.ResourceUsage.c";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				tu.GetFile (filename);
			}, filename);
		}
	}
}

