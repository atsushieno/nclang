using System;

namespace NClang
{
    /// <summary>
    /// Error codes for Compilation Database
    /// </summary>
    public enum CompilationDatabaseError
    {
        /// <summary>
        /// No error occurred
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Database can not be loaded
        /// </summary>
        CanNotLoadDatabase = 1
    }
}
