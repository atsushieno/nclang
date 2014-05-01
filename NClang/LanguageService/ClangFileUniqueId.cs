using System;
using NClang.Natives;
using System.Linq;

using CXFile = System.IntPtr;
using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

namespace NClang
{
	public struct ClangFileUniqueId
	{
		SystemULongLong v1, v2, v3;

		internal ClangFileUniqueId (CXFileUniqueID id)
		{
			v1 = id.Data1;
			v2 = id.Data2;
			v3 = id.Data3;
		}

		public override bool Equals (object obj)
		{
			return obj is ClangFileUniqueId ? Equals ((ClangFileUniqueId) obj) : false;
		}

		public bool Equals (ClangFileUniqueId other)
		{
			return this == other;
		}

		public static bool operator == (ClangFileUniqueId o1, ClangFileUniqueId o2)
		{
			return o1.v1 == o2.v1 && o1.v2 == o2.v2 && o1.v3 == o2.v3;
		}

		public static bool operator != (ClangFileUniqueId o1, ClangFileUniqueId o2)
		{
			return o1.v1 != o2.v1 || o1.v2 != o2.v2 || o1.v3 != o2.v3;
		}

		public override int GetHashCode ()
		{
			return (int) (v1.GetHashCode () + v2.GetHashCode () + v3.GetHashCode ());
		}

		public override string ToString ()
		{
			return string.Format ("{0:X08}-{1:X08}-{2:X08}", v1, v2, v3);
		}
	}
}
