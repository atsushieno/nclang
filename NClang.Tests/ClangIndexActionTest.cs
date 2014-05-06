using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangIndexActionTest
	{
		[Test]
		public void IndexTranslationUnit ()
		{
			string filename = "ClangIndexActionTest.IndexTranslationUnit.c";
			string code = @"#include <stdio.h>
int bar () { return 3; } int foo () { return 5; } int main () { return foo () * bar (); }";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var a = idx.CreateIndexAction ();
				var cb = new ClangIndexerCallbacks ();
				int abortRequested = 0;
				cb.AbortQuery += (arg) => { abortRequested++; return 0; };

				int diagnosticCount = -1;
				cb.Diagnostic += (arg1, arg2) => diagnosticCount = arg2.Count;
				bool enteredMainFile = false;
				cb.EnteredMainFile += (arg1, arg2) => { enteredMainFile = true; return new ClangIndexClientFile (arg2.Handle); };

				// not practically tested here...
				cb.ImportedAstFile += (arg1, arg2) => { return new ClangIndexClientAstFile (arg2.Handle); };

				var refs = new List<string> ();
				cb.IndexEntityReference += (arg1, arg2) => refs.Add (string.Format ("({0}, {1})", arg2.Location.SourceLocation.ExpansionLocation.Line, arg2.Location.SourceLocation.ExpansionLocation.Column));

				bool startedTranslationUnit = false;
				cb.StartedTranslationUnit += (arg) => { startedTranslationUnit = true; return new ClangIndexContainerInfo (tu.Handle); };

				int includeCount = 0;
				cb.PreprocessIncludedFile += (arg1, arg2) => { includeCount++; return new ClangIndexClientFile (arg2.Handle); };

				a.IndexTranslationUnit (IntPtr.Zero, new ClangIndexerCallbacks[] {cb}, IndexOptionFlags.None, tu);

				Assert.IsTrue (startedTranslationUnit, "startedTranslationUnit");
				Assert.IsTrue (enteredMainFile, "enteredMainFile");
				Assert.IsTrue (abortRequested > 0, "abortRequested > 0");
				Assert.AreEqual (0, diagnosticCount, "diagnosticCount");
				Assert.IsTrue (refs.Contains ("(2, 72)"), "Entity Reference Indexed: (2, 72)");
				Assert.IsTrue (refs.Contains ("(2, 81)"), "Entity Reference Indexed: (2, 81)");
				Assert.IsTrue (includeCount > 0, "includeCount > 0");
			}, filename, code);
		}
	}
}

