// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   PropVariantNative.cs
// 
// 
//   Description:
//      The MSFT ProvVariant structure.
//  
// *****************************************************************************************************
using System;
using System.Runtime.InteropServices;

namespace CoreAudioLib.Structures
{
    /// <summary>
    /// The PropVariantNative
    /// </summary>
    class PropVariantNative
    {
        [DllImport("ole32.dll")]
        internal static extern int PropVariantClear(ref PropVariant pvar);

        [DllImport("ole32.dll")]
        internal static extern int PropVariantClear(IntPtr pvar);
    }
}
