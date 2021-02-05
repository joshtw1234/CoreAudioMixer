// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   AudioCallBacks.cs
// 
// 
//   Description:
//      The Audio CallBacks
//
// *****************************************************************************************************
using CoreAudioLib.Enums;

namespace CoreAudioLib.CallBacks
{
    /// <summary>
    /// Audio Volume Call back
    /// </summary>
    /// <param name="audioFlow"></param>
    /// <param name="value"></param>
    public delegate void AudioVolumePeekCallBack(AudioDataFlow audioFlow, float value);
    /// <summary>
    /// Audio Pulse Call Back
    /// </summary>
    /// <param name="highValue"></param>
    /// <param name="lowValue"></param>
    public delegate void AudioPulseCallBack(double highValue, double lowValue);
    /// <summary>
    /// Audio Volume Change call Back
    /// </summary>
    /// <param name="highValue"></param>
    /// <param name="lowValue"></param>
    public delegate void AudioVolumeChangeCallBack(NativeCore.AudioVolumeNotificationData cData);
    /// <summary>
    /// The Audio Device state change call back
    /// </summary>
    /// <param name="audioDeviceData"></param>
    public delegate void AudioDeviceStateChangeCallBack(Structures.AudioDeviceChangeData audioDeviceData);

    public delegate void AudioSessionVolumeChangeCallBack(uint spid, float volume, bool isMute);
}
