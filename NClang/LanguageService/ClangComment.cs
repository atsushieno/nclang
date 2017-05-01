using System;
using NClang.Natives;
using System.Linq;
using System.Runtime.InteropServices;

using SystemLongLong = System.Int64;
using SystemULongLong = System.UInt64;

using CXString = NClang.ClangString;

namespace NClang
{
	public class ClangComment
	{
		CXComment source;

		internal ClangComment (CXComment source)
		{
			this.source = source;
		}

		// CommentASTIntrospection

		public CommentKind Kind {
			get { return LibClang.clang_Comment_getKind (source); }
		}

		public int ChildCount {
			get { return (int) LibClang.clang_Comment_getNumChildren (source); }
		}

		public ClangComment GetChild (int index)
		{
			return new ClangComment (LibClang.clang_Comment_getChild (source, (uint) index));
		}

		public bool IsWhitespace {
			get { return LibClang.clang_Comment_isWhitespace (source) != 0; }
		}

		public bool InlineContentCommentHasTrailingNewLine {
			get { return LibClang.clang_InlineContentComment_hasTrailingNewline (source) != 0; }
		}

		public string TextCommentText {
			get { return LibClang.clang_TextComment_getText (source).Unwrap (); }
		}

		public string InlineCommandName {
			get { return LibClang.clang_InlineCommandComment_getCommandName (source).Unwrap (); }
		}

		public CommentInlineCommandRenderKind InlineCommandRenderKind {
			get { return LibClang.clang_InlineCommandComment_getRenderKind (source); }
		}

		public int InlineCommandArgumentCunt {
			get { return (int) LibClang.clang_InlineCommandComment_getNumArgs (source); }
		}

		public string GetInlineCommandArgument (int index)
		{
			return LibClang.clang_InlineCommandComment_getArgText (source, (uint) index).Unwrap ();
		}

		public string HtmlTagCommentTagName {
			get { return LibClang.clang_HTMLTagComment_getTagName (source).Unwrap (); }
		}

		public bool HtmlStartTagIsSelfClosing {
			get { return LibClang.clang_HTMLStartTagComment_isSelfClosing (source) != 0; }
		}

		public int HtmlStartTagAttributeCount {
			get { return (int) LibClang.clang_HTMLStartTag_getNumAttrs (source); }
		}

		public string GetHtmlStartTagAttributeName (int index)
		{
			return LibClang.clang_HTMLStartTag_getAttrName (source, (uint) index).Unwrap ();
		}

		public string GetHtmlStartTagAttributeValue (int index)
		{
			return LibClang.clang_HTMLStartTag_getAttrValue (source, (uint) index).Unwrap ();
		}

		public string BlockCommandName {
			get { return LibClang.clang_BlockCommandComment_getCommandName (source).Unwrap (); }
		}

		public int BlockCommandArgumentCount {
			get { return (int) LibClang.clang_BlockCommandComment_getNumArgs (source); }
		}

		public string GetBlockCommandArgument (int index)
		{
			return LibClang.clang_BlockCommandComment_getArgText (source, (uint) index).Unwrap ();
		}

		public ClangComment BlockCommandParagrath {
			get { return new ClangComment (LibClang.clang_BlockCommandComment_getParagraph (source)); }
		}

		public string ParameterCommandParameterName {
			get { return LibClang.clang_ParamCommandComment_getParamName (source).Unwrap (); }
		}

		public bool ParameterCommandIsIndexValid {
			get { return LibClang.clang_ParamCommandComment_isParamIndexValid (source) != 0; }
		}

		public int ParameterCommandParameterIndex {
			get { return (int) LibClang.clang_ParamCommandComment_getParamIndex (source); }
		}

		public bool ParameterCommandIsDirectionExplicit {
			get { return LibClang.clang_ParamCommandComment_isDirectionExplicit (source) != 0; }
		}

		public CommentParamPassDirection ParameterCommandDirection {
			get { return LibClang.clang_ParamCommandComment_getDirection (source); }
		}

		public string TypeParameterCommandParameterName {
			get { return LibClang.clang_TParamCommandComment_getParamName (source).Unwrap (); }
		}

		public bool TypeParameterCommandIsPositionValid {
			get { return LibClang.clang_TParamCommandComment_isParamPositionValid (source) != 0; }
		}

		public int ParameterCommandDepth {
			get { return (int) LibClang.clang_TParamCommandComment_getDepth (source); }
		}

		public int ParameterCommandGetIndex (int depth)
		{
			return (int) LibClang.clang_TParamCommandComment_getIndex (source, (uint) depth);
		}

		public string VerbatimBlockLineCommentText {
			get { return LibClang.clang_VerbatimBlockLineComment_getText (source).Unwrap (); }
		}

		public string VerbatimLineCommentText {
			get { return LibClang.clang_VerbatimLineComment_getText (source).Unwrap (); }
		}

		public string HtmlTagAsString {
			get { return LibClang.clang_HTMLTagComment_getAsString (source).Unwrap (); }
		}

		public string FullCommentAsHtml {
			get { return LibClang.clang_FullComment_getAsHTML (source).Unwrap (); }
		}

		public string FullCommentAsXml {
			get { return LibClang.clang_FullComment_getAsXML (source).Unwrap (); }
		}
	}

}
