using System;

namespace NClang
{
    /// <summary>
    /// Describes the type of the comment AST node (<c>CXComment</c>).  A comment
    /// node can be considered block content (e. g., paragraph), inline content
    /// (plain text) or neither (the root AST node).
    /// </summary>
    public enum CommentKind
    {
        /// <summary>
        /// Null comment.  No AST node is constructed at the requested location
        /// because there is no text or a syntax error.
        /// </summary>
        Null = 0,

        /// <summary>
        /// Plain text. Inline content.
        /// </summary>
        Text = 1,

        /// <summary>
        /// A command with word-like arguments that is considered inline content.
        ///
        /// For example: \\c command.
        /// </summary>
        InlineCommand = 2,

        /// <summary>
        /// HTML start tag with attributes (name-value pairs).  Considered
        /// inline content.
        ///
        /// For example:
        /// <example>
        /// <br> <br /> <a href="http://example.org/">
        /// </example>
        /// </summary>
        HTMLStartTag = 3,

        /// <summary>
        /// HTML end tag.  Considered inline content.
        ///
        /// For example:
        /// <example>
        /// </a>
        /// </example>
        /// </summary>
        HTMLEndTag = 4,

        /// <summary>
        /// A paragraph, contains inline comment.  The paragraph itself is
        /// block content.
        /// </summary>
        Paragraph = 5,

        /// <summary>
        /// A command that has zero or more word-like arguments (number of
        /// word-like arguments depends on command name) and a paragraph as an
        /// argument.  Block command is block content.
        ///
        /// Paragraph argument is also a child of the block command.
        ///
        /// For example: \\brief has 0 word-like arguments and a paragraph argument.
        ///
        /// AST nodes of special kinds that parser knows about (e. g., \\param
        /// command) have their own node kinds.
        /// </summary>
        BlockCommand = 6,

        /// <summary>
        /// A \\param or \\arg command that describes the function parameter
        /// (name, passing direction, description).
        ///
        /// For example: \\param [in] ParamName description.
        /// </summary>
        ParamCommand = 7,

        /// <summary>
        /// A \\tparam command that describes a template parameter (name and
        /// description).
        ///
        /// For example: \\tparam T description.
        /// </summary>
        TParamCommand = 8,

        /// <summary>
        /// A verbatim block command (e. g., preformatted code).  Verbatim
        /// block has an opening and a closing command and contains multiple lines of
        /// text (<c>CXComment_VerbatimBlockLine</c> child nodes).
        ///
        /// For example:
        /// <code>
        /// aaa
        /// </code>
        /// </summary>
        VerbatimBlockCommand = 9,

        /// <summary>
        /// A line of text that is contained within a
        /// CXComment_VerbatimBlockCommand node.
        /// </summary>
        VerbatimBlockLine = 10,

        /// <summary>
        /// A verbatim line command.  Verbatim line has an opening command,
        /// a single line of text (up to the newline after the opening command) and
        /// has no closing command.
        /// </summary>
        VerbatimLine = 11,

        /// <summary>
        /// A full comment attached to a declaration, contains block content.
        /// </summary>
        FullComment = 12
    }
}
