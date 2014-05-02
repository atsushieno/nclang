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

