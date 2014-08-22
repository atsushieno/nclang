using System;

namespace NClang
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum GlobalOptionFlags
    {
        /// <summary>
        /// Used to indicate that no special CXIndex options are needed.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Used to indicate that threads that libclang creates for indexing
        /// purposes should use background priority.
        ///
        /// Affects #clang_indexSourceFile, #clang_indexTranslationUnit,
        /// #clang_parseTranslationUnit, #clang_saveTranslationUnit.
        /// </summary>
        ThreadBackgroundPriorityForIndexing = 0x1,

        /// <summary>
        /// Used to indicate that threads that libclang creates for editing
        /// purposes should use background priority.
        ///
        /// Affects #clang_reparseTranslationUnit, #clang_codeCompleteAt,
        /// #clang_annotateTokens
        /// </summary>
        ThreadBackgroundPriorityForEditing = 0x2,

        /// <summary>
        /// Used to indicate that all threads that libclang creates should use
        /// background priority.
        /// </summary>
        ThreadBackgroundPriorityForAll = ThreadBackgroundPriorityForIndexing | ThreadBackgroundPriorityForEditing
    }
}
