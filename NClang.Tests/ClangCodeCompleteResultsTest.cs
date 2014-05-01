using System;
using NClang;
using NUnit.Framework;
using System.IO;

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
	}
}

