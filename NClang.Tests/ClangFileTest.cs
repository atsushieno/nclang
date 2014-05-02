using System;
using NUnit.Framework;
using System.IO;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangFileTest
	{

		Tuple<ClangIndex,ClangTranslationUnit> CreateTranslationUnit (string filename, string content = null)
		{
			content = content ?? @"#include <stdio.h> void main () { printf (""hello world""); }";
			File.WriteAllText (filename, content);
			var idx = ClangService.CreateIndex ();
			return new Tuple<ClangIndex, ClangTranslationUnit> (idx, idx.CreateTranslationUnitFromSourceFile (filename, new string [0], new ClangUnsavedFile [0]));
		}

		[Test]
		public void UniqueId ()
		{
			string filename = "TranslationUnitTest.ResourceUsage.c";
			var t = CreateTranslationUnit (filename);
			var idx = t.Item1;
			var tu = t.Item2;
			try {
				var f = tu.GetFile (filename);
				Assert.AreEqual (filename, f.FileName, "FileName");
				Assert.IsFalse (tu.IsMultipleIncludeGuarded (f), "IsMultipleIncludeGuarded");
				var u1 = f.FileUniqueId;
				var u2 = f.FileUniqueId;
				Assert.IsFalse (u1 == default (ClangFileUniqueId), "FileUniqueId is not null");
				Assert.IsTrue (u1.Equals (u2), "FileUniqueId.Equals(): first: {0} second: {1}", u1, u2);
				Assert.IsTrue (u1 == u2, "FileUniqueId ==: first: {0} second: {1}", u1, u2);
			} finally {
				File.Delete (filename);
				tu.Dispose ();
				idx.Dispose ();
			}
		}
	}
}

