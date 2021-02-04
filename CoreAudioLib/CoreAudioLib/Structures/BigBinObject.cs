// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   BigBinObject.cs
// 
// 
//   Description:
//      The Big Bin Object
//
// *****************************************************************************************************
using System;

namespace CoreAudioLib.Structures
{
    /// <summary>
    /// For Big binary Object
    /// </summary>
    struct BigBinObject
    {
        /// <summary>
        /// Length of binary object.
        /// </summary>
        public int Length;
        /// <summary>
        /// Pointer to buffer storing data.
        /// </summary>
        public IntPtr Data;
    }
}
