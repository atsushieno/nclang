using System;

namespace NClang
{
    /// <summary>
    /// 
    /// </summary>
    public enum FindResult
    {
        /// <summary>
        /// Function returned successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// One of the parameters was invalid for the function.
        /// </summary>
        Invalid = 1,

        /// <summary>
        /// The function was terminated by a callback (e.g. it returned
        /// CXVisit_Break)
        /// </summary>
        VisitBreak = 2
    }
}
