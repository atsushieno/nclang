using System;

namespace NClang
{
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
}
