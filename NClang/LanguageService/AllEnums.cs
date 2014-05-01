using System;

namespace NClang
{
	public enum ErrorCode
	{
		Success = 0,
		Failure = 1,
		Crashed = 2,
		InvalidArguments = 3,
		ASTReadError = 4

	}

	[Flags]
	public enum GlobalOptionFlags
	{
		None = 0x0,
		ThreadBackgroundPriorityForIndexing = 0x1,
		ThreadBackgroundPriorityForEditing = 0x2,
		ThreadBackgroundPriorityForAll

	}

	public enum AvailabilityKind
	{
		Available,
		Deprecated,
		NotAvailable,
		NotAccessible

	}

	public enum CursorKind
	{
		UnexposedDeclaration = 1,
		StructDeclaration = 2,
		UnionDeclaration = 3,
		ClassDeclaration = 4,
		EnumDeclaration = 5,
		FieldDeclaration = 6,
		EnumConstantDeclaration = 7,
		FunctionDeclaration = 8,
		VarDeclaration = 9,
		ParmDeclaration = 10,
		ObjCInterfaceDeclaration = 11,
		ObjCCategoryDeclaration = 12,
		ObjCProtocolDeclaration = 13,
		ObjCPropertyDeclaration = 14,
		ObjCIvarDeclaration = 15,
		ObjCInstanceMethodDeclaration = 16,
		ObjCClassMethodDeclaration = 17,
		ObjCImplementationDeclaration = 18,
		ObjCCategoryImplDeclaration = 19,
		TypedefDeclaration = 20,
		CXXMethod = 21,
		Namespace = 22,
		LinkageSpec = 23,
		Constructor = 24,
		Destructor = 25,
		ConversionFunction = 26,
		TemplateTypeParameter = 27,
		NonTypeTemplateParameter = 28,
		TemplateTemplateParameter = 29,
		FunctionTemplate = 30,
		ClassTemplate = 31,
		ClassTemplatePartialSpecialization = 32,
		NamespaceAlias = 33,
		UsingDirective = 34,
		UsingDeclaration = 35,
		TypeAliasDeclaration = 36,
		ObjCSynthesizeDeclaration = 37,
		ObjCDynamicDeclaration = 38,
		CXXAccessSpecifier = 39,
		
		FirstDeclaration = UnexposedDeclaration,
		LastDeclaration = CXXAccessSpecifier,
		
		FirstReference = 40,
		/* Declaration references */
		ObjCSuperClassReference = 40,
		ObjCProtocolReference = 41,
		ObjCClassReference = 42,
		TypeReference = 43,
		CXXBaseSpecifier = 44,
		TemplateReference = 45,
		NamespaceReference = 46,
		MemberReference = 47,
		LabelReference = 48,
		
		OverloadedDeclarationReference = 49,
		
		VariableReference = 50,
		
		LastReference = VariableReference,
		
		FirstInvalid = 70,
		InvalidFile = 70,
		NoDeclarationFound = 71,
		NotImplemented = 72,
		InvalidCode = 73,
		LastInvalid = InvalidCode,
		
		FirstExpression = 100,
		
		UnexposedExpression = 100,
		
		DeclarationReferenceExpression = 101,
		
		MemberReferenceExpression = 102,
		
		CallExpression = 103,
		
		ObjCMessageExpression = 104,
		
		BlockExpression = 105,
		
		IntegerLiteral = 106,
		
		FloatingLiteral = 107,
		
		ImaginaryLiteral = 108,
		
		StringLiteral = 109,
		
		CharacterLiteral = 110,
		
		ParenExpression = 111,
		
		UnaryOperator = 112,
		
		ArraySubscriptExpression = 113,
		
		BinaryOperator = 114,
		
		CompoundAssignOperator = 115,
		
		ConditionalOperator = 116,
		
		CStyleCastExpression = 117,
		
		CompoundLiteralExpression = 118,
		
		InitListExpression = 119,
		
		AddrLabelExpression = 120,
		
		StatementExpression = 121,
		
		GenericSelectionExpression = 122,
		
		GNUNullExpression = 123,
		
		CXXStaticCastExpression = 124,
		
		CXXDynamicCastExpression = 125,
		
		CXXReinterpretCastExpression = 126,
		
		CXXConstCastExpression = 127,
		
		CXXFunctionalCastExpression = 128,
		
		CXXTypeidExpression = 129,
		
		CXXBoolLiteralExpression = 130,
		
		CXXNullPtrLiteralExpression = 131,
		
		CXXThisExpression = 132,
		
		CXXThrowExpression = 133,
		
		CXXNewExpression = 134,
		
		CXXDeleteExpression = 135,
		
		UnaryExpression = 136,
		
		ObjCStringLiteral = 137,
		
		ObjCEncodeExpression = 138,
		
		ObjCSelectorExpression = 139,
		
		ObjCProtocolExpression = 140,
		
		ObjCBridgedCastExpression = 141,
		
		PackExpansionExpression = 142,
		
		SizeOfPackExpression = 143,
		
		LambdaExpression = 144,
		
		ObjCBoolLiteralExpression = 145,
		
		LastExpression = ObjCBoolLiteralExpression,
		
		FirstStatement = 200,
		UnexposedStatement = 200,
		
		LabelStatement = 201,
		
		CompoundStatement = 202,
		
		CaseStatement = 203,
		
		DefaultStatement = 204,
		
		IfStatement = 205,
		
		SwitchStatement = 206,
		
		WhileStatement = 207,
		
		DoStatement = 208,
		
		ForStatement = 209,
		
		GotoStatement = 210,
		
		IndirectGotoStatement = 211,
		
		ContinueStatement = 212,
		
		BreakStatement = 213,
		
		ReturnStatement = 214,
		
		AsmStatement = 215,
		
		ObjCAtTryStatement = 216,
		
		ObjCAtCatchStatement = 217,
		
		ObjCAtFinallyStatement = 218,
		
		ObjCAtThrowStatement = 219,
		
		ObjCAtSynchronizedStatement = 220,
		
		ObjCAutoreleasePoolStatement = 221,
		
		ObjCForCollectionStatement = 222,
		
		CXXCatchStatement = 223,
		
		CXXTryStatement = 224,
		
		CXXForRangeStatement = 225,
		
		SEHTryStatement = 226,
		
		SEHExceptStatement = 227,
		
		SEHFinallyStatement = 228,
		
		NullStatement = 230,
		
		DeclarationStatement = 231,
		
		LastStatement = DeclarationStatement,
		
		TranslationUnit = 300,
		
		FirstAttribute = 400,
		UnexposedAttribute = 400,
		
		IBActionAttribute = 401,
		IBOutletAttribute = 402,
		IBOutletCollectionAttribute = 403,
		CXXFinalAttribute = 404,
		CXXOverrideAttribute = 405,
		AnnotateAttribute = 406,
		AsmLabelAttribute = 407,
		LastAttribute = AsmLabelAttribute,
		 
		PreprocessingDirective = 500,
		MacroDefinition = 501,
		MacroExpansion = 502,
		MacroInstantiation = MacroExpansion,
		InclusionDirective = 503,
		FirstPreprocessing = PreprocessingDirective,
		LastPreprocessing = InclusionDirective
	}

	[Flags]
	public enum TranslationUnitFlags
	{
		None = 0x0,
		DetailedPreprocessingRecord = 0x01,
		Incomplete = 0x02,
		PrecompiledPreamble = 0x04,
		CacheCompletionResults = 0x08,
		ForSerialization = 0x10,
		CXXChainedPCH = 0x20,
		SkipFunctionBodies = 0x40,
		IncludeBriefCommentsInCodeCompletion = 0x80
	}

	[Flags]
	public enum SaveTranslationUnitFlags
	{
		None = 0x0
	}

	[Flags]
	public enum ReparseTranslationUnitFlags
	{
		None = 0x0
	}

	public enum SaveError
	{
		None = 0,
		Unknown = 1,
		TranslationErrors = 2,
		InvalidTU = 3

	}

	public enum ResourceUsageKind
	{
		AST = 1,
		Identifiers = 2,
		Selectors = 3,
		GlobalCompletionResults = 4,
		SourceManagerContentCache = 5,
		ASTSideTables = 6,
		SourceManagerMembufferMalloc = 7,
		SourceManagerMembufferMMap = 8,
		ExternalASTSourceMembufferMalloc = 9,
		ExternalASTSourceMembufferMMap = 10,
		Preprocessor = 11,
		PreprocessingRecord = 12,
		SourceManagerDataStructures = 13,
		PreprocessorHeaderSearch = 14,
		MemoryInBytesBegin = AST,
		MemoryInBytesEnd,
		First = AST,
		Last = PreprocessorHeaderSearch

	}

	[Flags]
	public enum CodeCompleteFlags
	{
		None = 0,
		IncludeMacros = 0x01,
		IncludeCodePatterns = 0x02,
		IncludeBriefComments = 0x04
	}

	public enum CompletionContext
	{
		Unexposed = 0,
		AnyType = 1 << 0,
		AnyValue = 1 << 1,
		ObjCObjectValue = 1 << 2,
		ObjCSelectorValue = 1 << 3,
		CXXClassTypeValue = 1 << 4,
		DotMemberAccess = 1 << 5,
		ArrowMemberAccess = 1 << 6,
		ObjCPropertyAccess = 1 << 7,
		EnumTag = 1 << 8,
		UnionTag = 1 << 9,
		StructTag = 1 << 10,
		ClassTag = 1 << 11,
		Namespace = 1 << 12,
		NestedNameSpecifier = 1 << 13,
		ObjCInterface = 1 << 14,
		ObjCProtocol = 1 << 15,
		ObjCCategory = 1 << 16,
		ObjCInstanceMessage = 1 << 17,
		ObjCClassMessage = 1 << 18,
		ObjCSelectorName = 1 << 19,
		MacroName = 1 << 20,
		NaturalLanguage = 1 << 21,
		Unknown = ((1 << 22) - 1)
	}

	public enum LinkageKind
	{
		Invalid,
		NoLinkage,
		Internal,
		UniqueExternal,
		External
	}

	public enum LanguageKind
	{
		Invalid = 0,
		C,
		ObjC,
		CPlusPlus

	}

	public enum TypeKind
	{
		Invalid = 0,
		Unexposed = 1,
		Void = 2,
		Bool = 3,
		CharU = 4,
		UChar = 5,
		Char16 = 6,
		Char32 = 7,
		UShort = 8,
		UInt = 9,
		ULong = 10,
		ULongLong = 11,
		UInt128 = 12,
		CharS = 13,
		SChar = 14,
		WChar = 15,
		Short = 16,
		Int = 17,
		Long = 18,
		LongLong = 19,
		Int128 = 20,
		Float = 21,
		Double = 22,
		LongDouble = 23,
		NullPtr = 24,
		Overload = 25,
		Dependent = 26,
		ObjCId = 27,
		ObjCClass = 28,
		ObjCSel = 29,
		FirstBuiltin = Void,
		LastBuiltin = ObjCSel,
		Complex = 100,
		Pointer = 101,
		BlockPointer = 102,
		LValueReference = 103,
		RValueReference = 104,
		Record = 105,
		Enum = 106,
		Typedef = 107,
		ObjCInterface = 108,
		ObjCObjectPointer = 109,
		FunctionNoProto = 110,
		FunctionProto = 111,
		ConstantArray = 112,
		Vector = 113,
		IncompleteArray = 114,
		VariableArray = 115,
		DependentSizedArray = 116,
		MemberPointer = 117
	}

	public enum CallingConvention
	{
		Default = 0,
		C = 1,
		X86StdCall = 2,
		X86FastCall = 3,
		X86ThisCall = 4,
		X86Pascal = 5,
		AAPCS = 6,
		AAPCSVfp = 7,
		PnaclCall = 8,
		IntelOclBicc = 9,
		X64Win64 = 10,
		X64SysV = 11,
		Invalid = 100,
		Unexposed = 200
	}

	public enum RefQualifierKind
	{
		None = 0,
		LValue,
		RValue

	}

	public enum CXXAccessSpecifier
	{
		Invalid,
		Public,
		Protected,
		Private
	}

	public enum CompletionChunkKind
	{
		Optional,
		TypedText,
		Text,
		Placeholder,
		Informative,
		CurrentParameter,
		LeftParen,
		RightParen,
		LeftBracket,
		RightBracket,
		LeftBrace,
		RightBrace,
		LeftAngle,
		RightAngle,
		Comma,
		ResultType,
		Colon,
		SemiColon,
		Equal,
		HorizontalSpace,
		VerticalSpace
	}

	public enum DiagnosticDisplayOptions
	{
		DisplaySourceLocation = 0x01,
		DisplayColumn = 0x02,
		DisplaySourceRanges = 0x04,
		DisplayOption = 0x08,
		DisplayCategoryId = 0x10,
		DisplayCategoryName = 0x20

	}

	public enum IndexOptFlags
	{
		None = 0x0,
		SuppressRedundantRefs = 0x1,
		IndexFunctionLocalSymbols = 0x2,
		IndexImplicitTemplateInstantiations = 0x4,
		SuppressWarnings = 0x8,
		SkipParsedBodiesInSession = 0x10
	}

	public enum ChildVisitResult
	{
		Break,
		Continue,
		Recurse
	}

	public enum CommentKind
	{
		Null = 0,
		Text = 1,
		InlineCommand = 2,
		HTMLStartTag = 3,
		HTMLEndTag = 4,
		Paragraph = 5,
		BlockCommand = 6,
		ParamCommand = 7,
		TParamCommand = 8,
		VerbatimBlockCommand = 9,
		VerbatimBlockLine = 10,
		VerbatimLine = 11,
		FullComment = 12
	}
	public enum CommentInlineCommandRenderKind
	{
		Normal,
		Bold,
		Monospaced,
		Emphasized

	}

	public enum CommentParamPassDirection
	{
		In,
		Out,
		InOut

	}

	public enum CompilationDatabaseError
	{
		NoError = 0,
		CanNotLoadDatabase = 1

	}

	[Flags]
	public enum NameRefFlags
	{
		WantQualifier = 0x1,
		WantTemplateArgs = 0x2,
		WantSinglePiece = 0x4

	}

	public enum DiagnosticSeverity
	{
		Ignored = 0,
		Note = 1,
		Warning = 2,
		Error = 3,
		Fatal = 4

	}

	public enum LoadDiagError
	{
		None = 0,
		Unknown = 1,
		CannotLoad = 2,
		InvalidFile = 3

	}

	public enum  	TokenKind
	{
		Punctuation,
		Keyword,
		Identifier,
		Literal,
		Comment

	}

	public enum VisitorResult
	{
		Break,
		Continue

	}

	public enum FindResult
	{
		Success = 0,
		Invalid = 1,
		VisitBreak = 2

	}

	public enum IndexEntityKind
	{
		Unexposed = 0,
		Typedef = 1,
		Function = 2,
		Variable = 3,
		Field = 4,
		EnumConstant = 5,
		ObjCClass = 6,
		ObjCProtocol = 7,
		ObjCCategory = 8,
		ObjCInstanceMethod = 9,
		ObjCClassMethod = 10,
		ObjCProperty = 11,
		ObjCIvar = 12,
		Enum = 13,
		Struct = 14,
		Union = 15,
		CXXClass = 16,
		CXXNamespace = 17,
		CXXNamespaceAlias = 18,
		CXXStaticVariable = 19,
		CXXStaticMethod = 20,
		CXXInstanceMethod = 21,
		CXXConstructor = 22,
		CXXDestructor = 23,
		CXXConversionFunction = 24,
		CXXTypeAlias = 25,
		CXXInterface = 26
	}

	public enum IndexEntityLanguage
	{
		None = 0,
		C = 1,
		ObjC = 2,
		CXX = 3

	}

	public enum IndexEntityCxxTemplateKind
	{
		NonTemplate = 0,
		Template = 1,
		TemplatePartialSpecialization = 2,
		TemplateSpecialization = 3

	}

	public enum IndexAttributeKind
	{
		Unexposed = 0,
		IBAction = 1,
		IBOutlet = 2,
		IBOutletCollection = 3

	}

	public enum IndexDeclInfoFlags
	{
		Skipped = 0x1
	}

	public enum IndexObjCContainerKind
	{
		ForwardRef = 0,
		Interface = 1,
		Implementation = 2

	}

	public enum IndexEntityRefKind
	{
		Direct = 1,
		Implicit = 2

	}
}
