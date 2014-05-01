using System;
using System.Runtime.InteropServices;

using CXString = NClang.ClangString;

namespace NClang.Natives
{
	// done
	static partial class LibClang
	{
		[DllImport (LibraryName)]
		 internal static extern CommentKind 	clang_Comment_getKind (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_Comment_getNumChildren (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXComment 	clang_Comment_getChild (CXComment Comment, [MarshalAs (UnmanagedType.SysUInt)] uint ChildIdx);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_Comment_isWhitespace (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_InlineContentComment_hasTrailingNewline (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_TextComment_getText (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_InlineCommandComment_getCommandName (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CommentInlineCommandRenderKind 	clang_InlineCommandComment_getRenderKind (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_InlineCommandComment_getNumArgs (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_InlineCommandComment_getArgText (CXComment Comment, [MarshalAs (UnmanagedType.SysUInt)] uint ArgIdx);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_HTMLTagComment_getTagName (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_HTMLStartTagComment_isSelfClosing (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_HTMLStartTag_getNumAttrs (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_HTMLStartTag_getAttrName (CXComment Comment, [MarshalAs (UnmanagedType.SysUInt)] uint AttrIdx);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_HTMLStartTag_getAttrValue (CXComment Comment, [MarshalAs (UnmanagedType.SysUInt)] uint AttrIdx);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_BlockCommandComment_getCommandName (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_BlockCommandComment_getNumArgs (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_BlockCommandComment_getArgText (CXComment Comment, [MarshalAs (UnmanagedType.SysUInt)] uint ArgIdx);

		[DllImport (LibraryName)]
		 internal static extern CXComment 	clang_BlockCommandComment_getParagraph (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_ParamCommandComment_getParamName (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_ParamCommandComment_isParamIndexValid (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_ParamCommandComment_getParamIndex (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_ParamCommandComment_isDirectionExplicit (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CommentParamPassDirection 	clang_ParamCommandComment_getDirection (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_TParamCommandComment_getParamName (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_TParamCommandComment_isParamPositionValid (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_TParamCommandComment_getDepth (CXComment Comment);

		[return:MarshalAs (UnmanagedType.SysUInt)] 
		[DllImport (LibraryName)]
		 internal static extern uint 	clang_TParamCommandComment_getIndex (CXComment Comment, [MarshalAs (UnmanagedType.SysUInt)] uint Depth);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_VerbatimBlockLineComment_getText (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_VerbatimLineComment_getText (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_HTMLTagComment_getAsString (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_FullComment_getAsHTML (CXComment Comment);

		[DllImport (LibraryName)]
		 internal static extern CXString 	clang_FullComment_getAsXML (CXComment Comment);
	}
}

