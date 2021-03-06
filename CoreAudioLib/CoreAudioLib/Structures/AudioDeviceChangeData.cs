﻿// *****************************************************************************************************
//   Copyright (c) 2019-2020, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   AudioDeviceChangeData.cs
// 
// 
//   Description:
//      The structure for Audio Device Change Data
//
// *****************************************************************************************************
namespace CoreAudioLib.Structures
{
    /// <summary>
    /// The structure for Audio Device Change Data
    /// </summary>
    public struct AudioDeviceChangeData
    {
        /// <summary>
        /// Audio Device State
        /// </summary>
        public Enums.AudioDeviceState DeviceState;
        /// <summary>
        /// Audio End point Jack Sub Type.
        /// </summary>
        public string PKEY_AudioEndpoint_JackSubType;
        /// <summary>
        /// Audio End point Interface.
        /// </summary>
        public string PKEY_AudioEndPoint_Interface;
        /// <summary>
        /// Audio End point Name.
        /// </summary>
        public string PKEY_AudioEndpoint_Name;
        /// <summary>
        /// Audio End point Full Name.
        /// </summary>
        public string PKEY_audioendpoint_full_name;
        /// <summary>
        /// Audio End point HW ID.
        /// </summary>
        public string PKEY_AudioEndpoint_HWID;
        /// <summary>
        /// Audio End point Info.
        /// </summary>
        public string PKEY_AudioEndpoint_Info;
    }
}
