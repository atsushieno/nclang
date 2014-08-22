using System;

namespace NClang
{
    /// <summary>
    /// Describes the calling convention of a function type
    /// </summary>
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
}
