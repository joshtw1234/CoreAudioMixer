// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   PropertyKey.cs
// 
// 
//   Description:
//      The Key for property      
//
// *****************************************************************************************************
using System;

namespace CoreAudioLib.Structures
{
    /// <summary>
    /// The PropertyKey
    /// </summary>
    struct PropertyKey
    {
        /// <summary>
        /// Format ID
        /// </summary>
        public Guid formatId;
        /// <summary>
        /// Property ID
        /// </summary>
        public int propertyId;
        /// <summary>
        /// <param name="formatId"></param>
        /// <param name="propertyId"></param>
        /// </summary>
        public PropertyKey(Guid formatId, int propertyId)
        {
            this.formatId = formatId;
            this.propertyId = propertyId;
        }
    }
}
