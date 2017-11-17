using System;

namespace NClang
{
	public enum EvalResultKind
	{
		Int = 1,
		Float = 2,
		ObjCStrLiteral = 3,
		StrLiteral = 4,
		Str = 5,
		Other = 6,
		UnExposed = 0,
	}
}
