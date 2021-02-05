﻿// *****************************************************************************************************
//   Copyright (c) 2019, HP Development Company, L.P. All rights reserved.
//   This software contains confidential and proprietary information of HP.
//   The user of this software agrees not to disclose, disseminate or copy
//   such Confidential Information and shall use the software only in accordance
//   with the terms of the license agreement the user entered into with HP.
// 
//   AudioControl.cs
// 
// 
//   Description:
//      The Audio Control class
// 
//  
// *****************************************************************************************************
using CoreAudioLib.CallBacks;
using CoreAudioLib.Enums;
using CoreAudioLib.NativeCore;
using CoreAudioLib.Structures;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CoreAudioLib
{
    /// <summary>
    /// The AudioControl
    /// </summary>
    class AudioControl
    {
        // REFERENCE_TIME time units per second and per millisecond
        private const int REFTIMES_PER_SEC = 10000000;
        private const int REFTIMES_PER_MILLISEC = 10000;
        private bool isAudioPeekMonitoring = false;

        #region Core API structures
        /// <summary>
        /// MSFT Core API structures
        /// </summary>
        IMMDevice _audioDevice;
        IAudioClient _audioClient;
        IAudioMeterInformation _audioMeter;
        AudioDataFlow _audioDataFlow;
        IAudioEndpointVolume _audioEndpointVolume;
        IAudioSessionControl _audioSessionControl;
        IAudioSessionManager2 _audioSessionManager2;
        Dictionary<IAudioSessionControl, JoshAudioSessionStruct> _audioSessionEventDict;
        ClassAudioEndPointVolumeCallBack classCallBack;
        WAVEFORMATEXTENSION waveFormat;
        #endregion

        uint _channelCount = 0;

        /// <summary>
        /// Guid for Audio Client
        /// </summary>
        Guid _sessionGuid;

        /// <summary>
        /// The Audio Call Back
        /// </summary>
        AudioVolumePeekCallBack _audioCallBack;

        /// <summary>
        /// The Audio pulse Call back
        /// </summary>
        AudioPulseCallBack _audiopulseCallBack;

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="audioFlow"></param>
        public AudioControl(IMMDevice audioDevice, AudioDataFlow audioFlow)
        {
            _audioDataFlow = audioFlow;
            _audioDevice = audioDevice;
            //Initialize Call Back
            classCallBack = new ClassAudioEndPointVolumeCallBack();
            InitializeAudioClasses();
        }
        /// <summary>
        /// Initialize Audio Classes
        /// </summary>
        private void InitializeAudioClasses()
        {
            //Get Audio Client from device
            COMResult result = _audioDevice.Activate(typeof(IAudioClient).GUID, 0, IntPtr.Zero, out object obj);
            _audioClient = (IAudioClient)obj;
            //Get Audio Meter from device
            result = _audioDevice.Activate(typeof(IAudioMeterInformation).GUID, 0, IntPtr.Zero, out obj);
            _audioMeter = (IAudioMeterInformation)obj;
            //Get Audio End Point
            result = _audioDevice.Activate(typeof(IAudioEndpointVolume).GUID, 0, IntPtr.Zero, out obj);
            _audioEndpointVolume = (IAudioEndpointVolume)obj;
            _audioEndpointVolume.RegisterControlChangeNotify(classCallBack);
            /*
             * TODO:Add end point check logic here for make sure the audio device setting is correct.
             * like _audioEndpointVolume.QueryHardwareSupport(out uint mask);
             */
            InitializeAudioClient();

            if (_audioDataFlow == AudioDataFlow.eCapture) return;
            
            //Get Session manager
            result = _audioDevice.Activate(typeof(IAudioSessionManager2).GUID, ClsCtx.ALL, IntPtr.Zero, out obj);
            _audioSessionManager2 = (IAudioSessionManager2)obj;

            _audioSessionEventDict = new Dictionary<IAudioSessionControl, JoshAudioSessionStruct>();
            //Get all session process IDs
            //refer https://gist.github.com/sverrirs/d099b34b7f72bb4fb386
            // enumerate sessions for on this device
            _audioSessionManager2.GetSessionEnumerator(out IAudioSessionEnumerator sessionEnumerator);
            int count;
            sessionEnumerator.GetCount(out count);
            
            // search for an audio session with the required process-id
            JoshAudioSessionStruct audioStruc;
            AudioSessionEvents ssEvent = null;
            for (int i = 0; i < count; ++i)
            {
                IAudioSessionControl ctl = null;
                IAudioSessionControl2 ctl2 = null;

                var revv = sessionEnumerator.GetSession(i, out ctl);
                ssEvent = new AudioSessionEvents();
                audioStruc = new JoshAudioSessionStruct();
                revv = ctl.RegisterAudioSessionNotification(ssEvent);
                ctl2 = ctl as IAudioSessionControl2;
                // NOTE: we could also use the app name from ctl.GetDisplayName()
                revv = ctl2.GetProcessId(out uint cpid);
                ssEvent.ProcessID = cpid;

                var pos = System.Diagnostics.Process.GetProcessById((int)cpid);
                ssEvent.DisplayName = pos.ProcessName;

                audioStruc.AudioSessionEventClass = ssEvent;
                audioStruc.SimpleAudioControl = ctl2 as ISimpleAudioVolume;
                //revv = ctl2.GetDisplayName(out string names);
                //Keep session in memory for opreation it later
                _audioSessionEventDict.Add(ctl, audioStruc);

            }
            //Register event for get session creation
            _audioSessionManager2.RegisterSessionNotification(new AudioSessionNotification());

        }



        /// <summary>
        /// The Initialize Audio Client
        /// </summary>
        /// <param name="audioFlow"></param>
        /// <param name="_deviceEnumerator"></param>
        private void InitializeAudioClient()
        {
            //Initialize Audio Client.
            _sessionGuid = new Guid();
            var result = _audioClient.GetMixFormat(out waveFormat);
            AudioClientStreamFlags streamFlag = AudioClientStreamFlags.None;
            if (_audioDataFlow == AudioDataFlow.eRender) streamFlag = AudioClientStreamFlags.Loopback;
            result = _audioClient.Initialize(AudioClientMode.Shared, streamFlag, 10000000, 0, waveFormat, ref _sessionGuid);
            result = _audioClient.Start();
            //Change wave format here
            SetupWaveFormat(waveFormat);
            
            result = _audioEndpointVolume.GetChannelCount(out _channelCount);
            //_audioClient.GetService(typeof(IAudioSessionControl).GUID, out object sessionObj);
            //_audioSessionControl = (IAudioSessionControl)sessionObj;
            //var rev = _audioSessionControl.GetDisplayName(out string displayName);
            //rev = _audioSessionControl.RegisterAudioSessionNotification(new AudioSessionEvents());
        }


        /// <summary>
        /// The UnInitialize Audio
        /// </summary>
        public void UninitializeAudio()
        {
            if (null != _audioEndpointVolume)  _audioEndpointVolume.UnregisterControlChangeNotify(classCallBack);
            if (null != _audioClient) _audioClient.Stop();
            if (null != _audioMeter)
            {
                //For wait audio meter thread stop. then release memory
                Thread.Sleep(100);
                Marshal.ReleaseComObject(_audioMeter);
                _audioMeter = null;
            }
            if (null != _audioClient)
            {
                Marshal.ReleaseComObject(_audioClient);
                _audioClient = null;
            }
            if (null != _audioEndpointVolume)
            {
                Marshal.ReleaseComObject(_audioEndpointVolume);
                _audioEndpointVolume = null;
            }
            if (null != _audioDevice)
            {
                Marshal.ReleaseComObject(_audioDevice);
                _audioDevice = null;
            }
            if (null != waveFormat)
            {
                waveFormat = null;
            }
            if (null != _audioSessionEventDict)
            {
                foreach (var ctl in _audioSessionEventDict)
                {
                    ctl.Key.UnregisterAudioSessionNotification(ctl.Value.AudioSessionEventClass);
                    Marshal.ReleaseComObject(ctl.Key);
                }
            }

            if (null != _audioSessionManager2)
            {
                Marshal.ReleaseComObject(_audioSessionManager2);
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The Setup Wave Format
        /// </summary>
        /// <param name="_waveFormat"></param>
        private void SetupWaveFormat(WAVEFORMATEXTENSION _waveFormat)
        {
            switch (_waveFormat.WFormatTag)
            {
                case WaveFormat.Extensible:
                    _waveFormat.WBitsPerSample = 16;
                    _waveFormat.NBlockAlign = (ushort)(_waveFormat.NChannels * _waveFormat.WBitsPerSample / 8);
                    _waveFormat.NAvgBytesPerSec = _waveFormat.NBlockAlign * _waveFormat.NSamplesPerSec;
                    break;
            }
        }

     

        /// <summary>
        /// The start monitor
        /// </summary>
        public void StartPeekMonitor()
        {
            if (!isAudioPeekMonitoring)
            {
                try
                {
                    isAudioPeekMonitoring = true;
                    Task.Factory.StartNew(() => { AudioVolumePeekMonitorWork(); });
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"StartMonitor Error {ex.Message}");
                }
            }
        }

        /// <summary>
        /// The stop monitor
        /// </summary>
        public void StopPeekMonitor()
        {
            if (isAudioPeekMonitoring) isAudioPeekMonitoring = false;
        }
        /// <summary>
        /// The Audio Monitor work
        /// </summary>
        private void AudioVolumePeekMonitorWork()
        {
            while(isAudioPeekMonitoring)
            {
                _audioMeter.GetPeakValue(out float peak);
                _audioCallBack?.Invoke(_audioDataFlow, peak);
                Thread.Sleep(100);
            }
            _audioCallBack?.Invoke(_audioDataFlow, 0);
        }
        /// <summary>
        /// Get Mute.
        /// </summary>
        /// <returns></returns>
        public bool GetMuted()
        {
            bool isMute = false;
            var result =  _audioEndpointVolume.GetMute(out isMute);
            return isMute;
        }
        /// <summary>
        /// Set Mute
        /// </summary>
        /// <param name="v"></param>
        public void SetMuted(bool v)
        {
            _audioEndpointVolume.SetMute(v, Guid.Empty);
        }
        /// <summary>
        /// Get Master Volume
        /// </summary>
        /// <returns></returns>
        public double GetMasterVolume()
        {
            float level = 0.0f;
            if (_audioEndpointVolume != null)
            {
                _audioEndpointVolume.GetMasterVolumeLevelScalar(out level);
                
            }
            return (double)level;
        }
        /// <summary>
        /// Set Master Volume
        /// </summary>
        /// <param name="newValue"></param>
        public void SetMasterVolumeScalar(double newValue)
        {
            if (_audioEndpointVolume == null) return;
            _audioEndpointVolume.SetMasterVolumeLevelScalar((float)newValue, Guid.Empty);
        }
        /// <summary>
        /// Get Channel Value
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public float GetChannelValue(uint channelName)
        {
            if (channelName >= _channelCount) return 0;
            _audioEndpointVolume.GetChannelVolumeLevelScalar(channelName, out float level);
            return level;
        }
        /// <summary>
        /// Set Channel Value
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="v"></param>
        public void SetChannelValue(uint channelName, float v)
        {
            if (channelName >= _channelCount) return;
            _audioEndpointVolume.SetChannelVolumeLevelScalar(channelName, v, Guid.Empty);
        }

        #region Register Call Backs
        /// <summary>
        /// The register Audio Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterAudioVolumePeekCallBack(AudioVolumePeekCallBack callBack)
        {
            _audioCallBack += callBack;
        }
        /// <summary>
        /// UnRegister Audio Peek Call Back
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterAudioVolumePeekCallBack(AudioVolumePeekCallBack callBack)
        {
            _audioCallBack -= callBack;
        }

        /// <summary>
        /// The register Audio Pulse Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void RegisterAudioPulseCallBack(AudioPulseCallBack callBack)
        {
            _audiopulseCallBack += callBack;
        }
        /// <summary>
        /// The register Audio Pulse Call back
        /// </summary>
        /// <param name="callBack"></param>
        public void UnRegisterAudioPulseCallBack(AudioPulseCallBack callBack)
        {
            _audiopulseCallBack -= callBack;
        }
        /// <summary>
        /// The Volume Change call back
        /// </summary>
        /// <param name="_callBack"></param>
        public void RegisterVolumeChangeCallBack(AudioVolumeChangeCallBack _callBack)
        {
            classCallBack.RegisterVolumeChangeCallBack(_callBack);
        }
        /// <summary>
        /// UnRegister Volume Change Call back
        /// </summary>
        /// <param name="_callBack"></param>
        public void UnRegisterVolumeChangeCallBack(AudioVolumeChangeCallBack _callBack)
        {
            classCallBack.UnRegisterVolumeChangeCallBack(_callBack);
        }
        #endregion
    }
}
