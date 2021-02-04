// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   ClassAudioEndPointVolumeCallBack.cs
// 
// 
//   Description:
//      The implementation for IAudioEndPointVolumeCallBack
//
// *****************************************************************************************************
using System;

namespace CoreAudioLib.NativeCore
{
    /// <summary>
    /// Implement this class prevent OnNotify function show up in public API
    /// </summary>
    class ClassAudioEndPointVolumeCallBack : IAudioEndpointVolumeCallback
    {
        /// <summary>
        /// For receive End point volume change, do not call this function out side
        /// </summary>
        /// <param name="pNotify"></param>
        public void OnNotify(IntPtr pNotify)
        {
            if (null == pNotify) return;

            callBack?.Invoke(AudioVolumeNotificationData.MarshalFromPtr(pNotify));
        }

        /// <summary>
        /// The Volume Change Call back
        /// </summary>
        CallBacks.AudioVolumeChangeCallBack callBack;
        /// <summary>
        /// Register Volume Change Call Back
        /// </summary>
        /// <param name="_callBack"></param>
        public void RegisterVolumeChangeCallBack(CallBacks.AudioVolumeChangeCallBack _callBack)
        {
            callBack += _callBack;
        }
        /// <summary>
        /// UnRegister Volume Change Call Back
        /// </summary>
        /// <param name="_callBack"></param>
        public void UnRegisterVolumeChangeCallBack(CallBacks.AudioVolumeChangeCallBack _callBack)
        {
            callBack -= _callBack;
        }
    }
}
