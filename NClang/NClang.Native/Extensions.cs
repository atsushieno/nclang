using System;
using NClang.Natives;

namespace NClang
{
	static class Extensions
	{
		public static ClangType ToManaged (this CXType type)
		{
			return type.Kind != TypeKind.Invalid ? new ClangType (type) : null;
		}
	}
}

