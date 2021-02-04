// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   COMResult.cs
// 
// 
//   Description:
//      The COM Result
//
// *****************************************************************************************************
namespace CoreAudioLib.Enums
{
    /// <summary>
    /// The COM Result
    /// </summary>
    enum COMResult : uint
    {
        E_ACCESSDENIED = 0x80070005,//Access denied.
        E_FAIL = 0x80004005,//	Unspecified error.
        E_INVALIDARG = 0x80070057,//	Invalid parameter value.
        E_OUTOFMEMORY = 0x8007000E,//	Out of memory.
        E_POINTER = 0x80004003,//	NULL was passed incorrectly for a pointer value.
        E_UNEXPECTED = 0x8000FFFF,//	Unexpected condition.
        S_OK = 0x0,//	Success.
        S_FALSE = 0x1,//	Success.
    }
}
