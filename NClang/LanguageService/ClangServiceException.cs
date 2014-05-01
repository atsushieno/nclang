using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using NClang.Natives;

namespace NClang
{
	
	public class ClangServiceException : Exception
	{
		public ClangServiceException ()
		{
		}

		public ClangServiceException (string message) : base (message)
		{
		}

		public ClangServiceException (SerializationInfo info, StreamingContext context) : base (info, context)
		{
		}

		public ClangServiceException (string message, Exception innerException) : base (message, innerException)
		{
		}
	}
}
