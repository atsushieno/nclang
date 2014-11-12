using System;

namespace NClang
{
	/// <summary>
	/// The level of the diagnostic, after it has been through mapping.
	/// </summary>
	public enum DiagnosticSeverity
	{
		Ignored = 0,
		Note = 1,
		Warning = 2,
		Error = 3,
		Fatal = 4
	}
}
