using System;

namespace NClang
{
	/// <summary>
	/// Describes the kind of type
	/// </summary>
	public enum TypeKind
	{
		/// <summary>
		/// Reprents an invalid type (e.g., where no type is available).
		/// </summary>
		Invalid = 0,

		/// <summary>
		/// A type whose specific kind is not exposed via this
		/// interface.
		/// </summary>
		Unexposed = 1,

		/* Builtin types */

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
		Float128 = 30,
		Half = 31,
		Float16 = 32,
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
		MemberPointer = 117,
		Auto = 118, Elaborated = 119, Pipe = 120,
  OCLImage1dRO = 121, OCLImage1dArrayRO = 122, OCLImage1dBufferRO = 123, OCLImage2dRO = 124,
  OCLImage2dArrayRO = 125, OCLImage2dDepthRO = 126, OCLImage2dArrayDepthRO = 127, OCLImage2dMSAARO = 128,
  OCLImage2dArrayMSAARO = 129, OCLImage2dMSAADepthRO = 130, OCLImage2dArrayMSAADepthRO = 131, OCLImage3dRO = 132,
  OCLImage1dWO = 133, OCLImage1dArrayWO = 134, OCLImage1dBufferWO = 135, OCLImage2dWO = 136,
  OCLImage2dArrayWO = 137, OCLImage2dDepthWO = 138, OCLImage2dArrayDepthWO = 139, OCLImage2dMSAAWO = 140,
  OCLImage2dArrayMSAAWO = 141, OCLImage2dMSAADepthWO = 142, OCLImage2dArrayMSAADepthWO = 143, OCLImage3dWO = 144,
  OCLImage1dRW = 145, OCLImage1dArrayRW = 146, OCLImage1dBufferRW = 147, OCLImage2dRW = 148,
  OCLImage2dArrayRW = 149, OCLImage2dDepthRW = 150, OCLImage2dArrayDepthRW = 151, OCLImage2dMSAARW = 152,
  OCLImage2dArrayMSAARW = 153, OCLImage2dMSAADepthRW = 154, OCLImage2dArrayMSAADepthRW = 155, OCLImage3dRW = 156,
  OCLSampler = 157, OCLEvent = 158, OCLQueue = 159, OCLReserveID = 160 
	}
}
