using System;

namespace NClang
{
    /// <summary>
    /// Represents the C++ access control level to a base class for a
    /// cursor with kind <c>CX_CXXBaseSpecifier</c>.
    /// </summary>
    public enum CXXAccessSpecifier
    {
        Invalid,
        Public,
        Protected,
        Private
    }
}
