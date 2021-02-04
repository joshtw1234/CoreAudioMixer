// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   MMDeviceEnumeratorFactory.cs
// 
// 
//   Description:
//      The MMDeviceEnumeratorFactory implement Class     
//
// *****************************************************************************************************
using System;

namespace CoreAudioLib.NativeCore
{
    /// <summary>
    /// The MMDeviceEnumeratorFactory
    /// </summary>
    class MMDeviceEnumeratorFactory
    {
        /// <summary>
        /// Create Instance
        /// </summary>
        /// <returns></returns>
        public static IMMDeviceEnumerator CreateInstance()
        {
            return (IMMDeviceEnumerator)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("BCDE0395-E52F-467C-8E3D-C4579291692E"))); // a MMDeviceEnumerator
        }
    }
}
