using System;

namespace NClang
{
    /// <summary>
    /// The most appropriate rendering mode for an inline command, chosen on
    /// command semantics in Doxygen.
    /// </summary>
    public enum CommentInlineCommandRenderKind
    {
        /// <summary>
        /// Command argument should be rendered in a normal font.
        /// </summary>
        Normal,

        /// <summary>
        /// Command argument should be rendered in a bold font.
        /// </summary>
        Bold,

        /// <summary>
        /// Command argument should be rendered in a monospaced font.
        /// </summary>
        Monospaced,

        /// <summary>
        /// Command argument should be rendered emphasized (typically italic
        /// font).
        /// </summary>
        Emphasized
    }
}
