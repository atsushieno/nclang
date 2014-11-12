using System;

namespace NClang
{
    /// <summary>
    /// Describes the availability of a particular entity, which indicates
    /// whether the use of this entity will result in a warning or error due to
    /// it being deprecated or unavailable.
    /// </summary>
    public enum AvailabilityKind
    {
        /// <summary>
        /// The entity is available.
        /// </summary>
        Available,

        /// <summary>
        /// The entity is available, but has been deprecated (and its use is
        /// not recommended).
        /// </summary>
        Deprecated,

        /// <summary>
        /// The entity is not available; any use of it will be an error.
        /// </summary>
        NotAvailable,

        /// <summary>
        /// The entity is available, but not accessible; any use of it will be
        /// an error.
        /// </summary>
        NotAccessible
    }
}
