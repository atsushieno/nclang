using System;
using System.Runtime.CompilerServices;
using NClang.Natives;

using LibClang = NClang.Natives.Natives;

namespace NClang
{
	public struct ClangSourceRange
	{
		public static ClangSourceRange GetRange (ClangSourceLocation begin, ClangSourceLocation end)
		{
			return new ClangSourceRange (LibClang.clang_getRange (begin.Source, end.Source));
		}
		
		readonly CXSourceRange source;

		internal ClangSourceRange (CXSourceRange source)
		{
			this.source = source;
		}

		public ClangSourceRange (uint begin, uint end)
		{
			this.source = new CXSourceRange { begin_int_data = begin, end_int_data = end};
		}

		internal CXSourceRange Source {
			get { return source; }
		}

		public override bool Equals (object obj)
		{
			return obj is ClangSourceRange && Equals ((ClangSourceRange) obj);
		}

		public bool Equals (ClangSourceRange other)
		{
			return LibClang.clang_equalRanges (source, other.source) != 0;
		}

		public static bool operator == (ClangSourceRange o1, ClangSourceRange o2)
		{
			return o1.Equals (o2);
		}

		public static bool operator != (ClangSourceRange o1, ClangSourceRange o2)
		{
			return !o1.Equals (o2);
		}

		public override int GetHashCode ()
		{
			return (int) ((source.begin_int_data << 16) + source.end_int_data);
		}

		public bool IsNull {
			get { return LibClang.clang_Range_isNull (source) != 0; }
		}

		public ClangSourceLocation Start {
			get { return new ClangSourceLocation (LibClang.clang_getRangeStart (source)); }
		}

		public ClangSourceLocation End {
			get { return new ClangSourceLocation (LibClang.clang_getRangeEnd (source)); }
		}

		public override string ToString ()
		{
			return IsNull ? "[Null]" : string.Format ("[{0} - {1}]", Start, End);
		}
	}	
}
