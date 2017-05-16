using System;
using System.Runtime.InteropServices;
using System.Text;
using NClang.Natives;

namespace NClang
{
	static class Extensions
	{
		public static ClangType ToManaged (this CXType type)
		{
			return type.Kind != TypeKind.Invalid ? new ClangType (type) : null;
		}

#if NET35
		public static IntPtr Add(this IntPtr p, int offset)
		{
			return IntPtr.Size == 4 ? new IntPtr(p.ToInt32() + offset) : new IntPtr(p.ToInt64() + offset);
		}
#else
		public static IntPtr Add(this IntPtr p, int offset)
		{
			return p + offset;
		}
#endif

#if NETSTANDARD1_4
        public static T ToStructure<T>(this IntPtr p)
            where T : struct
	    {
	        return Marshal.PtrToStructure<T>(p);
	    }

	    public static int SizeOf<T>()
            where T : struct
	    {
	        return Marshal.SizeOf<T>();
	    }

        public static Encoding DefaultEncoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
#else
        public static T ToStructure<T>(this IntPtr p)
            where T : struct
	    {
	        return (T)Marshal.PtrToStructure(p, typeof(T));
	    }

	    public static int SizeOf<T>()
            where T : struct
	    {
	        return Marshal.SizeOf(typeof(T));
	    }

        public static Encoding DefaultEncoding
        {
            get
            {
                return Encoding.Default;
            }
        }
#endif
    }
}
