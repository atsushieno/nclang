using System;
using NClang;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangCodeCompleteResultsTest
	{
		[Test]
		public void DefaultCodeCompleteOptions ()
		{
			Assert.AreEqual (CodeCompleteFlags.IncludeMacros, ClangCodeCompleteResults.DefaultCodeCompleteOptions, "#1");
		}

		[Test]
		public void CodeCompleteAt ()
		{
			string filename = "ClangCodeCompleteResults.CodeCompleteAt.c";
			string code = "int bar () { return 3; } int foo () { return 5; } int main () { return foo () * bar (); }";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				// 38 is before "return" in foo
				using (var res = tu.CodeCompleteAt (filename, 1, 38, null, CodeCompleteFlags.None)) {
					Assert.AreEqual (32, res.ResultCount, "ResultCount");
					Assert.AreEqual ("", res.ContainerUSR, "ContainerUSR");
					Assert.AreEqual (CompletionContext.AnyType | CompletionContext.AnyValue | CompletionContext.ObjCInterface, res.Contexts, "Contexts");
					var valid = new List<string> ();
					foreach (var r in res.Results.Where (r => r.CursorKind != CursorKind.NotImplemented)) {
						valid.Add (string.Join (" ", r.CompletionString.Chunks.Select (c => c.Text)));
						/*
						int i = 0;
						Console.Error.WriteLine ("{0} {1} {2}", r.CursorKind, r.CompletionString.Priority, r.CompletionString.ChunkCount);
						foreach (var c in r.CompletionString.Chunks)
							Console.Error.WriteLine (" - {0} {1} [{2}]", i++, c.Kind, c.Text);
						*/
					}
					Assert.AreEqual (2, valid.Count, "valid.Count"); // bar and foo (but not main, as it is not declared yet)
					Assert.IsTrue (valid.Contains ("int foo ( )"), "valid contains foo");
					Assert.IsTrue (valid.Contains ("int bar ( )"), "valid contains bar");
					Assert.AreEqual (0, res.DiagnosticsCount, "DiagnosticsCount");
				}
			}, filename, code);
		}

		[Test]
		public void CodeCompleteAt2 ()
		{
			string filename = "ClangCodeCompleteResults.CodeCompleteAt2.c";
			string code = "int bar () { return 3; } int foo () { return 5; } int main () { return foo () * bar (); }";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				// 45 is after "return " in foo
				using (var res = tu.CodeCompleteAt (filename, 1, 45, null, CodeCompleteFlags.None)) {
					Assert.AreEqual (6, res.ResultCount, "ResultCount");
					Assert.AreEqual ("", res.ContainerUSR, "ContainerUSR");
					Assert.AreEqual (CompletionContext.AnyValue, res.Contexts, "Contexts");
					var valid = new List<string> ();
					foreach (var r in res.Results.Where (r => r.CursorKind != CursorKind.NotImplemented))
						valid.Add (string.Join (" ", r.CompletionString.Chunks.Select (c => c.Text)));
					Assert.AreEqual (2, valid.Count, "valid.Count"); // bar and foo (but not main, as it is not declared yet)
					Assert.IsTrue (valid.Contains ("int foo ( )"), "valid contains foo");
					Assert.IsTrue (valid.Contains ("int bar ( )"), "valid contains bar");
					Assert.AreEqual (0, res.DiagnosticsCount, "DiagnosticsCount");
				}
			}, filename, code);
		}
	}
}

