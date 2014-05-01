using System;
using System.IO;
using NUnit.Framework;
using NClang;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangIndexTest
	{
		// TopLevel
		
		[Test]
		public void NewIndex ()
		{
			ClangService.CreateIndex ().Dispose ();
		}
		
		[Test]
		public void OptionFlags ()
		{
			using (var idx = ClangService.CreateIndex ()) {
				Assert.AreEqual (GlobalOptionFlags.None, idx.GlobalOptions, "default flags");
				idx.GlobalOptions = GlobalOptionFlags.ThreadBackgroundPriorityForAll;
				Assert.AreEqual (GlobalOptionFlags.ThreadBackgroundPriorityForAll, idx.GlobalOptions, "setter results");
			}
		}
		
		// TranslationUnitManipulation
		[Test]
		public void CreateTranslationUnitFromSourceFile ()
		{
			string file = "ClangIndexTest.CreateTranslationUnit.c";
			File.WriteAllText (file, @"#include <stdio.h> void main () { printf (""hello world""); }");
			try {
				using (var idx = ClangService.CreateIndex ()) {
					var tu = idx.CreateTranslationUnitFromSourceFile (file, new string [0], new ClangUnsavedFile [0]);
					tu.Dispose ();
				}
			} finally {
				File.Delete (file);
			}
		}
		
		[Test]
		public void ParseTranslationUnitFromSourceFile ()
		{
			string file = "ClangIndexTest.CreateTranslationUnit.c";
			File.WriteAllText (file, @"#include <stdio.h> void main () { printf (""hello world""); }");
			try {
				using (var idx = ClangService.CreateIndex ()) {
					var tu = idx.ParseTranslationUnit (file, new string [0], new ClangUnsavedFile [0], TranslationUnitFlags.None);
					tu.Dispose ();
				}
			} finally {
				File.Delete (file);
			}
		}
	}
}
