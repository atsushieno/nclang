using System;

namespace NClang
{
	public enum TemplateArgumentKind
	{
		Null,
		Type,
		Declaration,
		NullPtr,
		Integral,
		Template,
		TemplateExpansion,
		Expression,
		Pack,
		Invalid
	}
}
