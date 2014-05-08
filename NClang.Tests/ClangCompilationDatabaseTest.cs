using System;
using NUnit.Framework;
using System.IO;

namespace NClang.Tests
{
	[TestFixture]
	public class ClangCompilationDatabaseTest
	{
		[Test]
		[ExpectedException (typeof (ClangServiceException))]
		public void CreateDatabaseFromDirectory ()
		{
			// cannot really create a database from nothing.
			var dir = Path.GetDirectoryName (new Uri (GetType ().Assembly.CodeBase).LocalPath);
			ClangService.CreateDatabaseFromDirectory (dir);
		}
	}
}

