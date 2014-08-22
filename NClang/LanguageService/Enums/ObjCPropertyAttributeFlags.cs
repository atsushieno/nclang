using System;

namespace NClang
{
    /// <summary>
    /// Property attributes for a <see cref="CursorKind.ObjCPropertyDeclaration"/>.
    /// </summary>
    [Flags]
    public enum ObjCPropertyAttributeFlags
    {
        NoAttr = 0x00,
        ReadOnly = 0x01,
        Getter = 0x02,
        Assign = 0x04,
        ReadWrite = 0x08,
        Retain = 0x10,
        Copy = 0x20,
        NonAtomic = 0x40,
        Setter = 0x80,
        Atomic = 0x100,
        Weak = 0x200,
        Strong = 0x400,
        UnsafeU7nretained = 0x800
    }
}
