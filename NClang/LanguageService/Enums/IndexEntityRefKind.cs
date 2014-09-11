using System;

namespace NClang
{
    /// <summary>
    /// Data for <c>IndexerCallbacks#indexEntityReference</c>.
    /// </summary>
	public enum IndexEntityRefKind
	{
        /// <summary>
        /// The entity is referenced directly in user's code.
        /// </summary>
		Direct = 1,

        /// <summary>
        /// An implicit reference, e.g. a reference of an ObjC method via the
        /// dot syntax.
        /// </summary>
		Implicit = 2
	}
}
