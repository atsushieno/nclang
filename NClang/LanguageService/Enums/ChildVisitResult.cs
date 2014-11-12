using System;

namespace NClang
{
    /// <summary>
    /// Describes how the traversal of the children of a particular
    /// cursor should proceed after visiting a particular child cursor.
    ///
    /// A value of this enumeration type should be returned by each
    /// <c>CXCursorVisitor</c> to indicate how clang_visitChildren() proceed.
    /// </summary>
    public enum ChildVisitResult
    {
        /// <summary>
        /// Terminates the cursor traversal.
        /// </summary>
        Break,

        /// <summary>
        /// Continues the cursor traversal with the next sibling of
        /// the cursor just visited, without visiting its children.
        /// </summary>
        Continue,

        /// <summary>
        /// Recursively traverse the children of this cursor, using
        /// the same visitor and client data.
        /// </summary>
        Recurse
    }
}
