using System;

namespace NClang
{
    /// <summary>
    /// The kind of C++0x ref-qualifier associated with a function type,
    /// which determines whether a member function's "this" object can be an
    /// lvalue, rvalue, or neither.
    /// </summary>
    public enum RefQualifierKind
    {
        /// <summary>
        /// No ref-qualifier was provided.
        /// </summary>
        None = 0,

        /// <summary>
        /// An lvalue ref-qualifier was provided (<c>&</c>).
        /// </summary>
        LValue,

        /// <summary>
        /// An rvalue ref-qualifier was provided (<c>&&</c>).
        /// </summary>
        RValue
    }
}
