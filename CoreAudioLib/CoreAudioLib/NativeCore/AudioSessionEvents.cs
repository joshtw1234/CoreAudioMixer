using CoreAudioLib.Enums;
using System;
using System.Runtime.InteropServices;

namespace CoreAudioLib.NativeCore
{
    struct JoshAudioSessionStruct
    {
        public ISimpleAudioVolume SimpleAudioControl { get; set; }
        public AudioSessionEvents AudioSessionEventClass { get; set; }
    }
    class AudioSessionEvents : IAudioSessionEvents
    {
        public uint ProcessID { get; set; }
        public string DisplayName { get; set; }

        CallBacks.AudioSessionVolumeChangeCallBack _volumeCallBack;
        CallBacks.AudioSessionStateCallBack _stateCallBack;

        public void RegisterSessionVolumeCallBack(CallBacks.AudioSessionVolumeChangeCallBack callBack)
        {
            _volumeCallBack += callBack;
        }

        public void UnRegisterSessionVolumeCallBack(CallBacks.AudioSessionVolumeChangeCallBack callBack)
        {
            if (null == _volumeCallBack) return;

            _volumeCallBack -= callBack;
        }

        public void RegisterSessionStateCallBack(CallBacks.AudioSessionStateCallBack callBack)
        {
            _stateCallBack += callBack;
        }

        public void UnRegisterSessionStateCallBack(CallBacks.AudioSessionStateCallBack callBack)
        {
            if (null == _volumeCallBack) return;

            _stateCallBack -= callBack;
        }

        public COMResult OnChannelVolumeChanged([In, MarshalAs(UnmanagedType.U4)] uint channelCount, [In, MarshalAs(UnmanagedType.SysInt)] IntPtr newVolumes, [In, MarshalAs(UnmanagedType.U4)] uint channelIndex, [In] ref Guid eventContext)
        {
            return COMResult.S_OK;
        }

        public COMResult OnDisplayNameChanged([In, MarshalAs(UnmanagedType.LPWStr)] string displayName, [In] ref Guid eventContext)
        {
            return COMResult.S_OK;
        }

        public COMResult OnGroupingParamChanged([In] ref Guid groupingId, [In] ref Guid eventContext)
        {
            return COMResult.S_OK;
        }

        public COMResult OnIconPathChanged([In, MarshalAs(UnmanagedType.LPWStr)] string iconPath, [In] ref Guid eventContext)
        {
            return COMResult.S_OK;
        }

        public COMResult OnSessionDisconnected([In] AudioSessionDisconnectReason disconnectReason)
        {
            return COMResult.S_OK;
        }

        public COMResult OnSimpleVolumeChanged([In, MarshalAs(UnmanagedType.R4)] float volume, [In, MarshalAs(UnmanagedType.Bool)] bool isMuted, [In] ref Guid eventContext)
        {
            _volumeCallBack?.Invoke(ProcessID, volume, isMuted);
            return COMResult.S_OK;
        }

        public COMResult OnStateChanged([In] AudioSessionState state)
        {
            _stateCallBack?.Invoke(ProcessID, (int)state);
            return COMResult.S_OK;
        }
    }
}
