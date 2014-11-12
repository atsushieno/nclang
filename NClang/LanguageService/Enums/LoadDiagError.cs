using System;

namespace NClang
{
	/// <summary>
	/// Describes the kind of error that occurred (if any) in a call to
	/// <c>clang_loadDiagnostics</c>.
	/// </summary>
	public enum LoadDiagError
	{
		/// <summary>
		/// Indicates that no error occurred.
		/// </summary>
		None = 0,

		/// <summary>
		/// Indicates that an unknown error occurred while attempting to
		/// deserialize diagnostics.
		/// </summary>
		Unknown = 1,

		/// <summary>
		/// Indicates that the file containing the serialized diagnostics
		/// could not be opened.
		/// </summary>
		CannotLoad = 2,

		/// <summary>
		/// Indicates that the serialized diagnostics file is invalid or
		/// corrupt.
		/// </summary>
		InvalidFile = 3
	}
}
