// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   MMNotificationClient.cs
// 
// 
//   Description:
//      The IMMNotificationClient implement Class     
//
// *****************************************************************************************************
using CoreAudioLib.Enums;
using CoreAudioLib.Structures;
using System.Runtime.InteropServices;

namespace CoreAudioLib.NativeCore
{
    /// <summary>
    /// The MMNotificationClient
    /// </summary>
    class MMNotificationClient : IMMNotificationClient
    {
        public delegate void NotificationClientCallBack(string deviceId, AudioDeviceState newState);

        NotificationClientCallBack _deviceStateCallBack;
        public void RegisterAudioDeviceStateChange(NotificationClientCallBack callback)
        {
            _deviceStateCallBack += callback;
        }
        public void UnRegisterAudioDeviceStateChange(NotificationClientCallBack callback)
        {
            _deviceStateCallBack -= callback;
        }
        #region IMMNotificationClient interface Do Not Call from outside.
        public void OnDeviceStateChanged([MarshalAs(UnmanagedType.LPWStr)] string deviceId, [MarshalAs(UnmanagedType.I4)] AudioDeviceState newState)
        {
            _deviceStateCallBack?.Invoke(deviceId, newState);
        }

        public void OnDeviceAdded([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId)
        {
            //Do Nothing.
        }

        public void OnDeviceRemoved([MarshalAs(UnmanagedType.LPWStr)] string deviceId)
        {
            //Do Nothing.
        }

        public void OnDefaultDeviceChanged(AudioDataFlow flow, EndPointRole role, [MarshalAs(UnmanagedType.LPWStr)] string defaultDeviceId)
        {
            //Do Nothing.
        }

        public void OnPropertyValueChanged([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId, PropertyKey key)
        {
            //Do Nothing.
        }
        #endregion
    }
}
