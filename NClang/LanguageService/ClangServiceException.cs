using System;
using System.Linq;
using System.Runtime.InteropServices;
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
#if !NETSTANDARD1_4
        public ClangServiceException (System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base (info, context)
		{
		}
#endif
        public ClangServiceException (string message, Exception innerException) : base (message, innerException)
		{
		}
	}
}
