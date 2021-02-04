// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   AudioClientBufferFlags.cs
// 
// 
//   Description:
//      The Audio Client Buffer Flags
//
// *****************************************************************************************************
using System;

namespace CoreAudioLib.Enums
{
    /// <summary>
    /// The Audio Client Buffer Flags
    /// </summary>
    [Flags]
    enum AudioClientBufferFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// AUDCLNT_BUFFERFLAGS_DATA_DISCONTINUITY
        /// </summary>
        DataDiscontinuity = 0x1,
        /// <summary>
        /// AUDCLNT_BUFFERFLAGS_SILENT
        /// </summary>
        Silent = 0x2,
        /// <summary>
        /// AUDCLNT_BUFFERFLAGS_TIMESTAMP_ERROR
        /// </summary>
        TimestampError = 0x4
    }
}
