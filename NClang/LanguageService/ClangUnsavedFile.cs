using System;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
	public class ClangUnsavedFile
	{
		public ClangUnsavedFile (string filename, string contents)
		{
			if (filename == null)
				throw new ArgumentNullException ("filename");
			if (contents == null)
				throw new ArgumentNullException ("contents");
			this.FileName = filename;
			this.Contents = contents;
		}

		public readonly string FileName;
		public readonly string Contents;
	}
}
