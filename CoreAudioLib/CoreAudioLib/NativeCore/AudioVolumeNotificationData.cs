﻿// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   AUDIO_VOLUME_NOTIFICATION_DATA.cs
// 
// 
//   Description:
//      The structure for Audio Device Notification Data
//
// *****************************************************************************************************
using System;
using System.Runtime.InteropServices;

namespace CoreAudioLib.NativeCore
{
    /// <summary>
    /// Internal structure defining the layout of the AUDIO_VOLUME_NOTIFICATION_DATA structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct AUDIO_VOLUME_NOTIFICATION_DATA
    {
        /// <summary>
        /// guidEventContext.
        /// </summary>
        public Guid EventContext;

        /// <summary>
        /// bMuted.
        /// </summary>
        public bool Muted;

        /// <summary>
        /// fMasterVolume.
        /// </summary>
        public float MasterVolume;

        /// <summary>
        /// nChannels.
        /// </summary>
        public int Channels;

        /// <summary>
        /// afChannelVolumes[1].
        /// </summary>
        public float ChannelVolume0;

        /// <summary>
        /// Initializes a new instance of the <see cref="AUDIO_VOLUME_NOTIFICATION_DATA"/> struct.
        /// </summary>
        /// <param name="eventContext">The event context.</param>
        /// <param name="muted">Muted state.</param>
        /// <param name="masterVolume">Master volume level.</param>
        /// <param name="channels">Number of channels.</param>
        /// <param name="channelVolume0">The first channel volume.</param>
        public AUDIO_VOLUME_NOTIFICATION_DATA(Guid eventContext, bool muted, float masterVolume, int channels, float channelVolume0)
        {
            this.EventContext = eventContext;
            this.Muted = muted;
            this.MasterVolume = masterVolume;
            this.Channels = channels;
            this.ChannelVolume0 = channelVolume0;
        }
    }
    /// <summary>
    /// AUDIO_VOLUME_NOTIFICATION_DATA structure (defined in Endpointvolume.h).
    /// </summary>
    public class AudioVolumeNotificationData
    {
        /// <summary>
        /// Gets or sets the context value for the IAudioEndpointVolumeCallback::OnNotify method. This
        /// member is the value of the event-context GUID that was provided as an input parameter to the
        /// IAudioEndpointVolume method call that changed the endpoint volume level or muting state.
        /// </summary>
        public Guid EventContext { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the audio stream is currently muted.
        /// </summary>
        public bool Muted { get; set; }

        /// <summary>
        /// Gets or sets the current master volume level of the audio stream. The volume level is normalized
        /// to the range from 0.0 to 1.0, where 0.0 is the minimum volume level and 1.0 is the maximum level.
        /// </summary>
        public float MasterVolume { get; set; }

        /// <summary>
        /// Gets or sets the number of channels in the audio stream.
        /// </summary>
        public int Channels { get; set; }

        /// <summary>
        /// Gets or sets an array of channel volumes.
        /// </summary>
        public float[] ChannelVolume { get; set; }

        /// <summary>
        /// Marshals a native AUDIO_VOLUME_NOTIFICATION_DATA structure to a <see cref="AudioVolumeNotificationData"/> object.
        /// </summary>
        /// <param name="ptr">A pointer to an AUDIO_VOLUME_NOTIFICATION_DATA structure.</param>
        /// <returns>The marshaled <see cref="AudioVolumeNotificationData"/> object.</returns>
        public static AudioVolumeNotificationData MarshalFromPtr(IntPtr ptr)
        {
            AUDIO_VOLUME_NOTIFICATION_DATA data = (AUDIO_VOLUME_NOTIFICATION_DATA)Marshal.PtrToStructure(ptr, typeof(AUDIO_VOLUME_NOTIFICATION_DATA));
            IntPtr channelVolumesPtr = new IntPtr(ptr.ToInt64() + Marshal.OffsetOf<AUDIO_VOLUME_NOTIFICATION_DATA>("ChannelVolume0").ToInt64());

            // Read dynamic channel volumes array
            float[] channelVolume = new float[data.Channels];
            Marshal.Copy(channelVolumesPtr, channelVolume, 0, data.Channels);

            // Construct the complete AudioVolumeNotificationData object
            AudioVolumeNotificationData notificationData = new AudioVolumeNotificationData()
            {
                EventContext = data.EventContext,
                Muted = data.Muted,
                MasterVolume = data.MasterVolume,
                Channels = data.Channels,
                ChannelVolume = channelVolume,
            };

            return notificationData;
        }
    }
}
