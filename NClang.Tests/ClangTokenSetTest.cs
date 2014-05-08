using System;
using NUnit.Framework;
using System.Linq;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangTokenSetTest
	{
		class Result
		{
			public TokenKind Kind;
			public string Spelling;
			public int Line;
			public int Start;
			public int End;

			public Result (TokenKind kind, string spelling, int line, int start, int end)
			{
				this.Kind = kind;
				this.Spelling = spelling;
				this.Line = line;
				this.Start = start;
				this.End = end;
			}
		}

		[Test]
		public void Tokenize ()
		{
			Result [] results = new Result [] {
				new Result (TokenKind.Keyword, "int", 1, 1, 4),
				new Result (TokenKind.Identifier, "bar", 1, 5, 8),
				new Result (TokenKind.Punctuation, "(", 1, 9, 10),
				new Result (TokenKind.Punctuation, ")", 1, 10, 11),
				new Result (TokenKind.Punctuation, "{", 1, 12, 13),
				new Result (TokenKind.Keyword, "return", 1, 14, 20),
				new Result (TokenKind.Literal, "3", 1, 21, 22),
				new Result (TokenKind.Punctuation, ";", 1, 22, 23),
				new Result (TokenKind.Punctuation, "}", 1, 24, 25),
				new Result (TokenKind.Keyword, "int", 1, 26, 29),
				new Result (TokenKind.Identifier, "foo", 1, 30, 33),
				new Result (TokenKind.Punctuation, "(", 1, 34, 35),
				new Result (TokenKind.Punctuation, ")", 1, 35, 36),
				new Result (TokenKind.Punctuation, "{", 1, 37, 38),
				new Result (TokenKind.Keyword, "return", 1, 39, 45),
				new Result (TokenKind.Literal, "5", 1, 46, 47),
				new Result (TokenKind.Punctuation, ";", 1, 47, 48),
				new Result (TokenKind.Punctuation, "}", 1, 49, 50),
				new Result (TokenKind.Keyword, "int", 1, 51, 54),
				new Result (TokenKind.Identifier, "main", 1, 55, 59),
				new Result (TokenKind.Punctuation, "(", 1, 60, 61),
				new Result (TokenKind.Punctuation, ")", 1, 61, 62),
				new Result (TokenKind.Punctuation, "{", 1, 63, 64),
				new Result (TokenKind.Keyword, "return", 1, 65, 71),
				new Result (TokenKind.Identifier, "foo", 1, 72, 75),
				new Result (TokenKind.Punctuation, "(", 1, 76, 77),
				new Result (TokenKind.Punctuation, ")", 1, 77, 78),
				new Result (TokenKind.Punctuation, "*", 1, 79, 80),
				new Result (TokenKind.Identifier, "bar", 1, 81, 84),
				new Result (TokenKind.Punctuation, "(", 1, 85, 86),
				new Result (TokenKind.Punctuation, ")", 1, 86, 87),
				new Result (TokenKind.Punctuation, ";", 1, 87, 88),
				new Result (TokenKind.Punctuation, "}", 1, 89, 90),
				};
			string filename = "TranslationUnitTest.DefaultProperties.c";
			string code = "int bar () { return 3; } int foo () { return 5; } int main () { return foo () * bar (); }";
			ClangTestHelpers.WithTranslationUnit ((idx, tu) => {
				var ts = tu.Tokenize (tu.GetCursor ().CursorExtent);
				var tokens = ts.Tokens.ToArray ();
				/*
				foreach (var t in ts.Tokens) {
					Console.Error.WriteLine ("--------");
					foreach (var pi in t.GetType ().GetProperties ())
						Console.Error.WriteLine ("  {0}: {1}", pi, pi.GetValue (t, null));
				}
				*/
				for (int i = 0; i < Math.Min (tokens.Length, results.Length); i++) {
					var r = results [i];
					var tok = tokens [i];
					Assert.AreEqual (r.Kind, tok.Kind, "Kind." + i);
					Assert.AreEqual (r.Spelling, tok.Spelling, "Spelling." + i);
					Assert.AreEqual (r.Line, tok.Location.FileLocation.Line, "Line." + i);
					Assert.AreEqual (r.Start, tok.Location.FileLocation.Column, "Start." + i);
					Assert.AreEqual (r.End, tok.Extent.End.FileLocation.Column, "End." + i);
				}
				Assert.AreEqual (results.Length, tokens.Length, "result count");
			}, filename, code);
		}
	}
}

