// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   CoreAudioAPIs.cs
// 
// 
//   Description:
//      MSFT Core APIs
//      https://docs.microsoft.com/en-us/windows/desktop/api/_coreaudio/
// 
//  
// *****************************************************************************************************
using CoreAudioLib.Enums;
using CoreAudioLib.Structures;
using System;
using System.Runtime.InteropServices;

namespace CoreAudioLib.NativeCore
{
    #region Core API Imports

    [ComImport]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceEnumerator
    {
        COMResult EnumAudioEndpoints(AudioDataFlow dataFlow, AudioDeviceState dwStateMask, out IMMDeviceCollection ppDevices);
        COMResult GetDefaultAudioEndpoint(AudioDataFlow dataFlow, EndPointRole role, out IMMDevice ppDevice);
        COMResult GetDevice(string id, out IMMDevice deviceName);

        COMResult RegisterEndpointNotificationCallback(IMMNotificationClient client);

        COMResult UnregisterEndpointNotificationCallback(IMMNotificationClient client);
    }

    [ComImport]
    [Guid("7991EEC9-7E89-4D85-8390-6C703CEC60C0"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMNotificationClient
    {
        /// <summary>
        /// Device State Changed
        /// </summary>
        void OnDeviceStateChanged([MarshalAs(UnmanagedType.LPWStr)] string deviceId, [MarshalAs(UnmanagedType.I4)] AudioDeviceState newState);

        /// <summary>
        /// Device Added
        /// </summary>
        void OnDeviceAdded([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId);

        /// <summary>
        /// Device Removed
        /// </summary>
        void OnDeviceRemoved([MarshalAs(UnmanagedType.LPWStr)] string deviceId);

        /// <summary>
        /// Default Device Changed
        /// </summary>
        void OnDefaultDeviceChanged(AudioDataFlow flow, EndPointRole role, [MarshalAs(UnmanagedType.LPWStr)] string defaultDeviceId);

        /// <summary>
        /// Property Value Changed
        /// </summary>
        /// <param name="pwstrDeviceId"></param>
        /// <param name="key"></param>
        void OnPropertyValueChanged([MarshalAs(UnmanagedType.LPWStr)] string pwstrDeviceId, PropertyKey key);
    }

    [Guid("C8ADBD64-E71E-48a0-A4DE-185C395CD317"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComImport]
    interface IAudioCaptureClient
    {
        COMResult GetBuffer(
            out IntPtr dataBuffer,
            out int numFramesToRead,
            out AudioClientBufferFlags bufferFlags,
            out long devicePosition,
            out long qpcPosition);

        COMResult ReleaseBuffer(int numFramesRead);

        COMResult GetNextPacketSize(out int numFramesInNextPacket);

    }

    [ComImport]
    [Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDeviceCollection
    {
        COMResult GetCount(out int numDevices);
        COMResult Item(int deviceNumber, out IMMDevice device);
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMMDevice
    {
        COMResult Activate([MarshalAs(UnmanagedType.LPStruct)] Guid iid, ClsCtx dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        COMResult OpenPropertyStore(StorageAccessMode stgmAccess, out IPropertyStore properties);

        COMResult GetId([MarshalAs(UnmanagedType.LPWStr)] out string id);

        COMResult GetState(out AudioDeviceState state);
    }
    [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPropertyStore
    {
        COMResult GetCount(out int propCount);
        COMResult GetAt(int property, out PropertyKey key);
        COMResult GetValue(ref PropertyKey key, out PropVariant value);
        COMResult SetValue(ref PropertyKey key, ref PropVariant value);
        COMResult Commit();
    }
    [Guid("C02216F6-8C67-4B5B-9D00-D008E73E0064"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioMeterInformation
    {
        COMResult GetPeakValue(out float pfPeak);
        COMResult GetMeteringChannelCount(ref UInt32 pnChannelCount);
        COMResult GetChannelsPeakValues(UInt32 u32ChannelCount, ref float afPeakValues);
        COMResult QueryHardwareSupport(ref UInt32 pdwHardwareSupportMask);
    }
    [Guid("1CB9AD4C-DBFA-4c32-B178-C2F568A703B2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioClient
    {
        COMResult Initialize(AudioClientMode ShareMode, AudioClientStreamFlags StreamFlags, Int64 hnsBufferDuration, Int64 hnsPeriodicity, WAVEFORMATEXTENSION pFormat, ref Guid AudioSessionGuid);
        COMResult GetBufferSize(ref UInt32 pNumBufferFrames);
        COMResult GetStreamLatency(ref Int64 phnsLatency);
        COMResult GetCurrentPadding(ref UInt32 pNumPaddingFrames);
        COMResult IsFormatSupported(int ShareMode, ref WAVEFORMATEXTENSION pFormat, out WAVEFORMATEXTENSION ppClosestMatch);
        COMResult GetMixFormat(out WAVEFORMATEXTENSION ppClosestMatch);
        COMResult GetDevicePeriod(ref Int64 phnsDefaultDevicePeriod, ref Int64 phnsMinimumDevicePeriod);
        COMResult Start();
        COMResult Stop();
        COMResult Reset();
        COMResult SetEventHandle(IntPtr eventHandle);
        //COMResult GetService(Guid riid, out object ppv);
        [PreserveSig]
        COMResult GetService([In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceId, [Out, MarshalAs(UnmanagedType.IUnknown)] out object interfacePointer);
    }

    [Guid("657804FA-D6AD-4496-8A60-352752AF4F89"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolumeCallback
    {
        void OnNotify([In] IntPtr pNotify);
    }

    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolume
    {
        [PreserveSig]
        COMResult RegisterControlChangeNotify([In][MarshalAs(UnmanagedType.Interface)] IAudioEndpointVolumeCallback pNotify);

        [PreserveSig]
        COMResult UnregisterControlChangeNotify([In][MarshalAs(UnmanagedType.Interface)] IAudioEndpointVolumeCallback pNotify);

        /// <summary>
        /// Gets a count of the channels in the audio stream.
        /// </summary>
        /// <param name="channelCount">The number of channels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetChannelCount(
            [Out][MarshalAs(UnmanagedType.U4)] out UInt32 channelCount);

        /// <summary>
        /// Sets the master volume level of the audio stream, in decibels.
        /// </summary>
        /// <param name="level">The new master volume level in decibels.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetMasterVolumeLevel(
            [In][MarshalAs(UnmanagedType.R4)] float level,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Sets the master volume level, expressed as a normalized, audio-tapered value.
        /// </summary>
        /// <param name="level">The new master volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetMasterVolumeLevelScalar(
            [In][MarshalAs(UnmanagedType.R4)] float level,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the master volume level of the audio stream, in decibels.
        /// </summary>
        /// <param name="level">The volume level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetMasterVolumeLevel(
            [Out][MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Gets the master volume level, expressed as a normalized, audio-tapered value.
        /// </summary>
        /// <param name="level">The volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetMasterVolumeLevelScalar(
            [Out][MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Sets the volume level, in decibels, of the specified channel of the audio stream.
        /// </summary>
        /// <param name="channelNumber">The channel number.</param>
        /// <param name="level">The new volume level in decibels.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetChannelVolumeLevel(
            [In][MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [In][MarshalAs(UnmanagedType.R4)] float level,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Sets the normalized, audio-tapered volume level of the specified channel in the audio stream.
        /// </summary>
        /// <param name="channelNumber">The channel number.</param>
        /// <param name="level">The new master volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetChannelVolumeLevelScalar(
            [In][MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [In][MarshalAs(UnmanagedType.R4)] float level,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the volume level, in decibels, of the specified channel in the audio stream.
        /// </summary>
        /// <param name="channelNumber">The zero-based channel number.</param>
		/// <param name="level">The volume level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetChannelVolumeLevel(
            [In][MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [Out][MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Gets the normalized, audio-tapered volume level of the specified channel of the audio stream.
        /// </summary>
        /// <param name="channelNumber">The zero-based channel number.</param>
		/// <param name="level">The volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetChannelVolumeLevelScalar(
            [In][MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [Out][MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Sets the muting state of the audio stream.
        /// </summary>
        /// <param name="isMuted">True to mute the stream, or false to unmute the stream.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult SetMute(
            [In][MarshalAs(UnmanagedType.Bool)] Boolean isMuted,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the muting state of the audio stream.
        /// </summary>
        /// <param name="isMuted">The muting state. True if the stream is muted, false otherwise.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetMute(
            [Out][MarshalAs(UnmanagedType.Bool)] out Boolean isMuted);

        /// <summary>
        /// Gets information about the current step in the volume range.
        /// </summary>
        /// <param name="step">The current zero-based step index.</param>
        /// <param name="stepCount">The total number of steps in the volume range.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetVolumeStepInfo(
            [Out][MarshalAs(UnmanagedType.U4)] out UInt32 step,
            [Out][MarshalAs(UnmanagedType.U4)] out UInt32 stepCount);

        /// <summary>
        /// Increases the volume level by one step.
        /// </summary>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult VolumeStepUp(
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Decreases the volume level by one step.
        /// </summary>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult VolumeStepDown(
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Queries the audio endpoint device for its hardware-supported functions.
        /// </summary>
        /// <param name="hardwareSupportMask">A hardware support mask that indicates the capabilities of the endpoint.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult QueryHardwareSupport(
            [Out][MarshalAs(UnmanagedType.U4)] out UInt32 hardwareSupportMask);

        /// <summary>
        /// Gets the volume range of the audio stream, in decibels.
        /// </summary>
		/// <param name="volumeMin">The minimum volume level in decibels.</param>
		/// <param name="volumeMax">The maximum volume level in decibels.</param>
		/// <param name="volumeStep">The volume increment level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        COMResult GetVolumeRange(
            [Out][MarshalAs(UnmanagedType.R4)] out float volumeMin,
            [Out][MarshalAs(UnmanagedType.R4)] out float volumeMax,
            [Out][MarshalAs(UnmanagedType.R4)] out float volumeStep);
    }
    /// <summary>
    /// Defines constants that indicate the current state of an audio session.
    /// </summary>
    /// <remarks>
    /// MSDN Reference: http://msdn.microsoft.com/en-us/library/dd370792.aspx
    /// </remarks>
    enum AudioSessionState
    {
        /// <summary>
        /// The audio session is inactive.
        /// </summary>
        AudioSessionStateInactive = 0,

        /// <summary>
        /// The audio session is active.
        /// </summary>
        AudioSessionStateActive = 1,

        /// <summary>
        /// The audio session has expired.
        /// </summary>
        AudioSessionStateExpired = 2
    }
    /// <summary>
    /// Windows CoreAudio IAudioSessionControl interface
    /// Defined in AudioPolicy.h
    /// </summary>
    [Guid("F4B1A599-7266-4319-A8CA-E70ACB11E8CD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionControl
    {
        /// <summary>
        /// Retrieves the current state of the audio session.
        /// </summary>
        /// <param name="state">Receives the current session state.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetState(
            [Out] out AudioSessionState state);

        /// <summary>
        /// Retrieves the display name for the audio session.
        /// </summary>
        /// <param name="displayName">Receives a string that contains the display name.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetDisplayName(
            [Out][MarshalAs(UnmanagedType.LPWStr)] out string displayName);

        /// <summary>
        /// Assigns a display name to the current audio session.
        /// </summary>
        /// <param name="displayName">A string that contains the new display name for the session.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult SetDisplayName(
            [In][MarshalAs(UnmanagedType.LPWStr)] string displayName,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Retrieves the path for the display icon for the audio session.
        /// </summary>
        /// <param name="iconPath">Receives a string that specifies the fully qualified path of the file that contains the icon.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetIconPath(
            [Out][MarshalAs(UnmanagedType.LPWStr)] out string iconPath);

        /// <summary>
        /// Assigns a display icon to the current session.
        /// </summary>
        /// <param name="iconPath">A string that specifies the fully qualified path of the file that contains the new icon.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult SetIconPath(
            [In][MarshalAs(UnmanagedType.LPWStr)] string iconPath,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Retrieves the grouping parameter of the audio session.
        /// </summary>
        /// <param name="groupingId">Receives the grouping parameter ID.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        int GetGroupingParam(
            [Out] out Guid groupingId);

        /// <summary>
        /// Assigns a session to a grouping of sessions.
        /// </summary>
        /// <param name="groupingId">The new grouping parameter ID.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult SetGroupingParam(
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid groupingId,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Registers the client to receive notifications of session events, including changes in the session state.
        /// </summary>
        /// <param name="client">A client-implemented <see cref="IAudioSessionEvents"/> interface.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult RegisterAudioSessionNotification(
            [In] IAudioSessionEvents client);

        /// <summary>
        /// Deletes a previous registration by the client to receive notifications.
        /// </summary>
        /// <param name="client">A client-implemented <see cref="IAudioSessionEvents"/> interface.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult UnregisterAudioSessionNotification(
            [In] IAudioSessionEvents client);
    }
    /// <summary>
    /// Windows CoreAudio IAudioSessionControl interface
    /// Defined in AudioPolicy.h
    /// </summary>
    [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionControl2 : IAudioSessionControl
    {
        /// <summary>
        /// Retrieves the current state of the audio session.
        /// </summary>
        /// <param name="state">Receives the current session state.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult GetState(
            [Out] out AudioSessionState state);

        /// <summary>
        /// Retrieves the display name for the audio session.
        /// </summary>
        /// <param name="displayName">Receives a string that contains the display name.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult GetDisplayName(
            [Out][MarshalAs(UnmanagedType.LPWStr)] out string displayName);

        /// <summary>
        /// Assigns a display name to the current audio session.
        /// </summary>
        /// <param name="displayName">A string that contains the new display name for the session.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult SetDisplayName(
            [In][MarshalAs(UnmanagedType.LPWStr)] string displayName,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Retrieves the path for the display icon for the audio session.
        /// </summary>
        /// <param name="iconPath">Receives a string that specifies the fully qualified path of the file that contains the icon.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult GetIconPath(
            [Out][MarshalAs(UnmanagedType.LPWStr)] out string iconPath);

        /// <summary>
        /// Assigns a display icon to the current session.
        /// </summary>
        /// <param name="iconPath">A string that specifies the fully qualified path of the file that contains the new icon.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult SetIconPath(
            [In][MarshalAs(UnmanagedType.LPWStr)] string iconPath,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Retrieves the grouping parameter of the audio session.
        /// </summary>
        /// <param name="groupingId">Receives the grouping parameter ID.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult GetGroupingParam(
            [Out] out Guid groupingId);

        /// <summary>
        /// Assigns a session to a grouping of sessions.
        /// </summary>
        /// <param name="groupingId">The new grouping parameter ID.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult SetGroupingParam(
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid groupingId,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Registers the client to receive notifications of session events, including changes in the session state.
        /// </summary>
        /// <param name="client">A client-implemented <see cref="IAudioSessionEvents"/> interface.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult RegisterAudioSessionNotification(
            [In] IAudioSessionEvents client);

        /// <summary>
        /// Deletes a previous registration by the client to receive notifications.
        /// </summary>
        /// <param name="client">A client-implemented <see cref="IAudioSessionEvents"/> interface.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult UnregisterAudioSessionNotification(
            [In] IAudioSessionEvents client);
        /// <summary>
        /// Retrieves the identifier for the audio session.
        /// </summary>
        /// <param name="retVal">Receives the session identifier.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetSessionIdentifier(
            [Out][MarshalAs(UnmanagedType.LPWStr)] out string retVal);

        /// <summary>
        /// Retrieves the identifier of the audio session instance.
        /// </summary>
        /// <param name="retVal">Receives the identifier of a particular instance.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetSessionInstanceIdentifier(
            [Out][MarshalAs(UnmanagedType.LPWStr)] out string retVal);

        /// <summary>
        /// Retrieves the process identifier of the audio session.
        /// </summary>
        /// <param name="retVal">Receives the process identifier of the audio session.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetProcessId(
            [Out] out UInt32 retVal);

        /// <summary>
        /// Indicates whether the session is a system sounds session.
        /// </summary>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult IsSystemSoundsSession();

        /// <summary>
        /// Enables or disables the default stream attenuation experience (auto-ducking) provided by the system.
        /// </summary>
        /// <param name="optOut">A variable that enables or disables system auto-ducking.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult SetDuckingPreference(bool optOut);
    }
    /// <summary>
    /// Windows CoreAudio IAudioSessionControl interface
    /// Defined in AudioPolicy.h
    /// </summary>
    [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionEvents
    {
        /// <summary>
        /// Notifies the client that the display name for the session has changed.
        /// </summary>
        /// <param name="displayName">The new display name for the session.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnDisplayNameChanged(
            [In][MarshalAs(UnmanagedType.LPWStr)] string displayName,
            [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the display icon for the session has changed.
        /// </summary>
        /// <param name="iconPath">The path for the new display icon for the session.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnIconPathChanged(
            [In][MarshalAs(UnmanagedType.LPWStr)] string iconPath,
            [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the volume level or muting state of the session has changed.
        /// </summary>
        /// <param name="volume">The new volume level for the audio session.</param>
        /// <param name="isMuted">The new muting state.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnSimpleVolumeChanged(
            [In][MarshalAs(UnmanagedType.R4)] float volume,
            [In][MarshalAs(UnmanagedType.Bool)] bool isMuted,
            [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the volume level of an audio channel in the session submix has changed.
        /// </summary>
        /// <param name="channelCount">The channel count.</param>
        /// <param name="newVolumes">An array of volumnes cooresponding with each channel index.</param>
        /// <param name="channelIndex">The number of the channel whose volume level changed.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnChannelVolumeChanged(
            [In][MarshalAs(UnmanagedType.U4)] UInt32 channelCount,
            [In][MarshalAs(UnmanagedType.SysInt)] IntPtr newVolumes, // Pointer to float array
            [In][MarshalAs(UnmanagedType.U4)] UInt32 channelIndex,
            [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the grouping parameter for the session has changed.
        /// </summary>
        /// <param name="groupingId">The new grouping parameter for the session.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnGroupingParamChanged(
            [In] ref Guid groupingId,
            [In] ref Guid eventContext);

        /// <summary>
        /// Notifies the client that the stream-activity state of the session has changed.
        /// </summary>
        /// <param name="state">The new session state.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnStateChanged(
            [In] AudioSessionState state);

        /// <summary>
        /// Notifies the client that the session has been disconnected.
        /// </summary>
        /// <param name="disconnectReason">The reason that the audio session was disconnected.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnSessionDisconnected(
            [In] AudioSessionDisconnectReason disconnectReason);
    }
    /// <summary>
    /// Defines constants that indicate a reason for an audio session being disconnected.
    /// </summary>
    /// <remarks>
    /// MSDN Reference: Unknown
    /// </remarks>
    enum AudioSessionDisconnectReason
    {
        /// <summary>
        /// The user removed the audio endpoint device.
        /// </summary>
        DisconnectReasonDeviceRemoval = 0,

        /// <summary>
        /// The Windows audio service has stopped.
        /// </summary>
        DisconnectReasonServerShutdown = 1,

        /// <summary>
        /// The stream format changed for the device that the audio session is connected to.
        /// </summary>
        DisconnectReasonFormatChanged = 2,

        /// <summary>
        /// The user logged off the WTS session that the audio session was running in.
        /// </summary>
        DisconnectReasonSessionLogoff = 3,

        /// <summary>
        /// The WTS session that the audio session was running in was disconnected.
        /// </summary>
        DisconnectReasonSessionDisconnected = 4,

        /// <summary>
        /// The (shared-mode) audio session was disconnected to make the audio endpoint device available for an exclusive-mode connection.
        /// </summary>
        DisconnectReasonExclusiveModeOverride = 5
    }
    /// <summary>
    /// Windows CoreAudio IAudioSessionManager interface
    /// Defined in AudioPolicy.h
    /// </summary>
    [Guid("BFA971F1-4D5E-40BB-935E-967039BFBEE4"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionManager
    {
        /// <summary>
        /// Retrieves an audio session control.
        /// </summary>
        /// <param name="sessionId">A new or existing session ID.</param>
        /// <param name="streamFlags">Audio session flags.</param>
        /// <param name="sessionControl">Receives an <see cref="IAudioSessionControl"/> interface for the audio session.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetAudioSessionControl(
            [In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId,
            [In][MarshalAs(UnmanagedType.U4)] UInt32 streamFlags,
            [Out][MarshalAs(UnmanagedType.Interface)] out IAudioSessionControl sessionControl);

        /// <summary>
        /// Retrieves a simple audio volume control.
        /// </summary>
        /// <param name="sessionId">A new or existing session ID.</param>
        /// <param name="streamFlags">Audio session flags.</param>
        /// <param name="audioVolume">Receives an <see cref="ISimpleAudioVolume"/> interface for the audio session.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetSimpleAudioVolume(
            [In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId,
            [In][MarshalAs(UnmanagedType.U4)] UInt32 streamFlags,
            [Out][MarshalAs(UnmanagedType.Interface)] out ISimpleAudioVolume audioVolume);
    }
    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionManager2 : IAudioSessionManager
    {
        /// <summary>
        /// Retrieves an audio session control.
        /// </summary>
        /// <param name="sessionId">A new or existing session ID.</param>
        /// <param name="streamFlags">Audio session flags.</param>
        /// <param name="sessionControl">Receives an <see cref="IAudioSessionControl"/> interface for the audio session.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult GetAudioSessionControl(
            [In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId,
            [In][MarshalAs(UnmanagedType.U4)] UInt32 streamFlags,
            [Out][MarshalAs(UnmanagedType.Interface)] out IAudioSessionControl sessionControl);

        /// <summary>
        /// Retrieves a simple audio volume control.
        /// </summary>
        /// <param name="sessionId">A new or existing session ID.</param>
        /// <param name="streamFlags">Audio session flags.</param>
        /// <param name="audioVolume">Receives an <see cref="ISimpleAudioVolume"/> interface for the audio session.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        new COMResult GetSimpleAudioVolume(
            [In, Optional][MarshalAs(UnmanagedType.LPStruct)] Guid sessionId,
            [In][MarshalAs(UnmanagedType.U4)] UInt32 streamFlags,
            [Out][MarshalAs(UnmanagedType.Interface)] out ISimpleAudioVolume audioVolume);

        [PreserveSig]
        COMResult GetSessionEnumerator(out IAudioSessionEnumerator sessionEnum);

        [PreserveSig]
        COMResult RegisterSessionNotification(IAudioSessionNotification sessionNotification);

        [PreserveSig]
        COMResult UnregisterSessionNotification(IAudioSessionNotification sessionNotification);

        [PreserveSig]
        COMResult RegisterDuckNotification(string sessionId, IAudioSessionNotification audioVolumeDuckNotification);

        [PreserveSig]
        COMResult UnregisterDuckNotification(IntPtr audioVolumeDuckNotification);
    }
    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionEnumerator
    {
        COMResult GetCount(out int sessionCount);

        COMResult GetSession(int sessionCount, out IAudioSessionControl session);
    }
    /// <summary>
    /// Windows CoreAudio IAudioSessionNotification interface
    /// Defined in AudioPolicy.h
    /// </summary>
    [Guid("641DD20B-4D41-49CC-ABA3-174B9477BB08"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionNotification
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newSession">session being added</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult OnSessionCreated(IAudioSessionControl newSession);
    }
    /// <summary>
    /// Windows CoreAudio ISimpleAudioVolume interface
    /// Defined in AudioClient.h
    /// </summary>
    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface ISimpleAudioVolume
    {
        /// <summary>
        /// Sets the master volume level for the audio session.
        /// </summary>
        /// <param name="levelNorm">The new volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult SetMasterVolume(
            [In][MarshalAs(UnmanagedType.R4)] float levelNorm,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Retrieves the client volume level for the audio session.
        /// </summary>
        /// <param name="levelNorm">Receives the volume level expressed as a normalized value between 0.0 and 1.0. </param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetMasterVolume(
            [Out][MarshalAs(UnmanagedType.R4)] out float levelNorm);

        /// <summary>
        /// Sets the muting state for the audio session.
        /// </summary>
        /// <param name="isMuted">The new muting state.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult SetMute(
            [In][MarshalAs(UnmanagedType.Bool)] bool isMuted,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Retrieves the current muting state for the audio session.
        /// </summary>
        /// <param name="isMuted">Receives the muting state.</param>
        /// <returns>An HRESULT code indicating whether the operation succeeded of failed.</returns>
        [PreserveSig]
        COMResult GetMute(
            [Out][MarshalAs(UnmanagedType.Bool)] out bool isMuted);
    }
    #endregion
}
