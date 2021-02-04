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
            return COMResult.S_OK;
        }

        public COMResult OnStateChanged([In] AudioSessionState state)
        {
            return COMResult.S_OK;
        }
    }
}
