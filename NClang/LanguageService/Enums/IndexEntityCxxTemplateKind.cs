using System;

namespace NClang
{
	public enum IndexEntityCxxTemplateKind
	{
		NonTemplate = 0,
		Template = 1,
		TemplatePartialSpecialization = 2,
		TemplateSpecialization = 3
	}
}
