using System;

namespace NClang
{
	/// <summary>
	/// 
	/// </summary>
	public enum IndexOptionFlags
	{
		/// <summary>
		/// Used to indicate that no special indexing options are needed.
		/// </summary>
		None = 0x0,

		/// <summary>
		/// Used to indicate that IndexerCallbacks#indexEntityReference should
		/// be invoked for only one reference of an entity per source file that does
		/// not also include a declaration/definition of the entity.
		/// </summary>
		SuppressRedundantRefs = 0x1,

		/// <summary>
		/// Function-local symbols should be indexed. If this is not set
		/// function-local symbols will be ignored.
		/// </summary>
		IndexFunctionLocalSymbols = 0x2,

		/// <summary>
		/// Implicit function/class template instantiations should be indexed.
		/// If this is not set, implicit instantiations will be ignored.
		/// </summary>
		IndexImplicitTemplateInstantiations = 0x4,

		/// <summary>
		/// Suppress all compiler warnings when parsing for indexing.
		/// </summary>
		SuppressWarnings = 0x8,

		/// <summary>
		/// Skip a function/method body that was already parsed during an
		/// indexing session assosiated with a <c>CXIndexAction</c> object.
		/// Bodies in system headers are always skipped.
		/// </summary>
		SkipParsedBodiesInSession = 0x10
	}
}
