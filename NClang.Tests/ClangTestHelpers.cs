using System;
using System.IO;

namespace NClang.Tests
{
	public static class ClangTestHelpers
	{
		public static void WithTranslationUnit (Action<ClangIndex,ClangTranslationUnit> test, string testFullName, string content = null)
		{
			string filename = testFullName;
			content = content ?? @"
#include <stdio.h>

void main () {
	printf (""hello world""); 
}
";
			File.WriteAllText (filename, content);
			var idx = ClangService.CreateIndex ();
			var tu = idx.CreateTranslationUnitFromSourceFile (filename, new string [0], new ClangUnsavedFile [0]);
			try {
				test (idx, tu);
			} finally {
				File.Delete (filename);
				tu.Dispose ();
				idx.Dispose ();
			}
		}
	}
}

