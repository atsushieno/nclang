using System;

namespace NClang
{
    /// <summary>
    /// Categorizes how memory is being used by a translation unit.
    /// </summary>
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
        MemoryInBytesEnd = PreprocessorHeaderSearch,

        First = AST,
        Last = PreprocessorHeaderSearch
    }
}
