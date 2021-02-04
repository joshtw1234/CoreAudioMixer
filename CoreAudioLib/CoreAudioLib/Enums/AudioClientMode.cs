// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   AudioClientMode.cs
// 
// 
//   Description:
//      The Audio Client Mode
//
// *****************************************************************************************************
namespace CoreAudioLib.Enums
{
    /// <summary>
    /// The Audio Client Mode
    /// </summary>
    enum AudioClientMode
    {
        /// <summary>
        /// AUDCLNT_SHAREMODE_SHARED,
        /// </summary>
        Shared,
        /// <summary>
        /// AUDCLNT_SHAREMODE_EXCLUSIVE
        /// </summary>
        Exclusive
    }
}
