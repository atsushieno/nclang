using System;

namespace NClang
{
	/// <summary>
	/// Flags that can be passed to <c>clang_codeCompleteAt()</c> to
	/// modify its behavior.
	///
	/// The enumerators in this enumeration can be bitwise-OR'd together to
	/// provide multiple options to <c>clang_codeCompleteAt()</c>.
	/// </summary>
	[Flags]
	public enum CodeCompleteFlags
	{
		None = 0,

		/// <summary>
		/// Whether to include macros within the set of code
		/// completions returned.
		/// </summary>
		IncludeMacros = 0x01,

		/// <summary>
		/// Whether to include code patterns for language constructs
		/// within the set of code completions, e.g., for loops.
		/// </summary>
		IncludeCodePatterns = 0x02,

		/// <summary>
		/// Whether to include brief documentation within the set of code
		/// completions returned.
		/// </summary>
		IncludeBriefComments = 0x04
	}
}
