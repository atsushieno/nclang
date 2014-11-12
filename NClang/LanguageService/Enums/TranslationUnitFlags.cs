using System;

namespace NClang
{
	/// <summary>
	/// Flags that control the creation of translation units.
	/// </summary>
	[Flags]
	public enum TranslationUnitFlags
	{
		/// <summary>
		/// Used to indicate that no special translation-unit options are
		/// needed.
		/// </summary>
		None = 0x0,

		/// <summary>
		/// Used to indicate that the parser should construct a "detailed"
		/// preprocessing record, including all macro definitions and instantiations.
		/// </summary>
		DetailedPreprocessingRecord = 0x01,

		/// <summary>
		/// Used to indicate that the translation unit is incomplete.
		/// </summary>
		Incomplete = 0x02,

		/// <summary>
		/// Used to indicate that the translation unit should be built with an 
		/// implicit precompiled header for the preamble.
		/// </summary>
		PrecompiledPreamble = 0x04,

		/// <summary>
		/// Used to indicate that the translation unit should cache some
		/// code-completion results with each reparse of the source file.
		/// </summary>
		CacheCompletionResults = 0x08,

		/// <summary>
		/// Used to indicate that the translation unit will be serialized with
		/// clang_saveTranslationUnit.
		/// </summary>
		ForSerialization = 0x10,

		/// <summary>
		/// Enabled chained precompiled preambles in C++.
		/// </summary>
		[Obsolete]
		CXXChainedPCH = 0x20,

		/// <summary>
		/// Used to indicate that function/method bodies should be skipped while
		/// parsing.
		/// </summary>
		SkipFunctionBodies = 0x40,

		/// <summary>
		/// Used to indicate that brief documentation comments should be
		/// included into the set of code completions returned from this translation
		/// unit.
		/// </summary>
		IncludeBriefCommentsInCodeCompletion = 0x80
	}
}
