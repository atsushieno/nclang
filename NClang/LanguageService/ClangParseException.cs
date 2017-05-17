using System;
using System.Linq;
using System.Runtime.InteropServices;
using NClang.Natives;

namespace NClang
{
	
	public class ClangParseException : Exception
	{
		public ClangParseException ()
		{
		}
		
		public ClangParseException (string message) : base (message)
		{
		}
		
		public ClangParseException (string message, string [] commandLineArgs, ClangUnsavedFile [] unsavedFiles) : base (message)
		{
			CommandLineArgs = (string []) commandLineArgs.Clone ();
			UnsavedFiles = (ClangUnsavedFile []) unsavedFiles.Clone ();
		}
		
		/*public ClangParseException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
			throw new NotImplementedException ();
		}*/
		
		public ClangParseException (string message, Exception innerException) : base (message, innerException)
		{
		}
		
		public string [] CommandLineArgs { get; private set; }
		public ClangUnsavedFile [] UnsavedFiles { get; private set; }
		
		
	}

}
