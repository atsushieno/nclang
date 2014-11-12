using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CommentKind 	clang_Comment_getKind (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_Comment_getNumChildren (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXComment 	clang_Comment_getChild (CXComment Comment, uint ChildIdx);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_Comment_isWhitespace (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_InlineContentComment_hasTrailingNewline (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_TextComment_getText (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_InlineCommandComment_getCommandName (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CommentInlineCommandRenderKind 	clang_InlineCommandComment_getRenderKind (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_InlineCommandComment_getNumArgs (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_InlineCommandComment_getArgText (CXComment Comment, uint ArgIdx);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_HTMLTagComment_getTagName (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_HTMLStartTagComment_isSelfClosing (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_HTMLStartTag_getNumAttrs (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_HTMLStartTag_getAttrName (CXComment Comment, uint AttrIdx);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_HTMLStartTag_getAttrValue (CXComment Comment, uint AttrIdx);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_BlockCommandComment_getCommandName (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_BlockCommandComment_getNumArgs (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_BlockCommandComment_getArgText (CXComment Comment, uint ArgIdx);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXComment 	clang_BlockCommandComment_getParagraph (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_ParamCommandComment_getParamName (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_ParamCommandComment_isParamIndexValid (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_ParamCommandComment_getParamIndex (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_ParamCommandComment_isDirectionExplicit (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CommentParamPassDirection 	clang_ParamCommandComment_getDirection (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_TParamCommandComment_getParamName (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_TParamCommandComment_isParamPositionValid (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_TParamCommandComment_getDepth (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern uint 	clang_TParamCommandComment_getIndex (CXComment Comment, uint Depth);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_VerbatimBlockLineComment_getText (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_VerbatimLineComment_getText (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_HTMLTagComment_getAsString (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_FullComment_getAsHTML (CXComment Comment);

		[DllImport (LibraryName, CallingConvention = LibraryCallingConvention)]
		 internal static extern CXString 	clang_FullComment_getAsXML (CXComment Comment);
	}
}

