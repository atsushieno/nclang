using System;
using NUnit.Framework;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangDiagnosticTest
	{

		[Test]
		public void SimpleSemanticIssue ()
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
	}
}

