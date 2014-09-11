using System;

namespace NClang
{
    /// <summary>
    /// Describes the kind of error that occurred (if any) in a call to
    /// <c>clang_saveTranslationUnit()</c>.
    /// </summary>
    public enum SaveError
    {
        /// <summary>
        /// Indicates that no error occurred while saving a translation unit.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates that an unknown error occurred while attempting to save
        /// the file.
        ///
        /// This error typically indicates that file I/O failed when attempting to 
        /// write the file.
        /// </summary>
        Unknown = 1,

        /// <summary>
        /// Indicates that errors during translation prevented this attempt
        /// to save the translation unit.
        /// 
        /// Errors that prevent the translation unit from being saved can be
        /// extracted using <c>clang_getNumDiagnostics()</c> and <c>clang_getDiagnostic()</c>.
        /// </summary>
        TranslationErrors = 2,

        /// <summary>
        /// Indicates that the translation unit to be saved was somehow
        /// invalid (e.g., NULL).
        /// </summary>
        InvalidTU = 3
    }
}
