using System;

namespace NClang
{
	[Flags]
	public enum NameRefFlags
	{
		/// <summary>
		/// Include the nested-name-specifier, e.g. Foo:: in x.Foo::y, in the
		/// range.
		/// </summary>
		WantQualifier = 0x1,

		/// <summary>
		/// Include the explicit template arguments, e.g. \&lt;int&gt; in x.f&lt;int&gt;,
		/// in the range.
		/// </summary>
		WantTemplateArgs = 0x2,

		/// <summary>
		/// If the name is non-contiguous, return the full spanning range.
		///
		/// Non-contiguous names occur in Objective-C when a selector with two or more
		/// parameters is used, or in C++ when using an operator:
		/// <code>
		/// [object doSomething:here withValue:there]; // ObjC
		/// return some_vector[1]; // C++
		/// </code>
		/// </summary>
		WantSinglePiece = 0x4
	}
}
