using System;

namespace NClang
{
	/// <summary>
	/// 'Qualifiers' written next to the return and parameter types in
	/// ObjC method declarations.
	/// </summary>
	[Flags]
	public enum ObjCDeclarationQualifierFlags
	{
		None = 0x0,
		In = 0x1,
		Inout = 0x2,
		Out = 0x4,
		Bycopy = 0x8,
		Byref = 0x10,
		Oneway = 0x20
	}
}
