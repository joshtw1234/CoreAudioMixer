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
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CoreAudioLib
{
    /// <summary>
    /// The AudioControl
    /// </summary>
    class AudioControl : BaseAudioControl
    {
        // REFERENCE_TIME time units per second and per millisecond
        private const int REFTIMES_PER_SEC = 10000000;
        private const int REFTIMES_PER_MILLISEC = 10000;
        private bool isAudioPeekMonitoring = false;

        uint _channelCount = 0;

        /// <summary>
        /// Guid for Audio Client
        /// </summary>
        Guid _sessionGuid;

        protected IAudioClient _audioClient;
        protected IAudioMeterInformation _audioMeter;
        protected IAudioEndpointVolume _audioEndpointVolume;
        protected ClassAudioEndPointVolumeCallBack classCallBack;
        protected WAVEFORMATEXTENSION waveFormat;
        /// <summary>
        /// The Audio Call Back
        /// </summary>
        AudioVolumePeekCallBack _audioCallBack;

        /// <summary>
        /// The Audio pulse Call back
        /// </summary>
        AudioPulseCallBack _audiopulseCallBack;

        public AudioControl(IMMDevice audioDevice, AudioDataFlow audioFlow) : base(audioDevice, audioFlow)
        {
            //Initialize Call Back
            classCallBack = new ClassAudioEndPointVolumeCallBack();
            InitializeAudioClient();
        }

        /// <summary>
        /// The Initialize Audio Client
        /// </summary>
        /// <param name="audioFlow"></param>
        /// <param name="_deviceEnumerator"></param>
        private void InitializeAudioClient()
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
            //Initialize Audio Client.
            _sessionGuid = new Guid();
            result = _audioClient.GetMixFormat(out waveFormat);
            AudioClientStreamFlags streamFlag = AudioClientStreamFlags.None;
            if (_audioDataFlow == AudioDataFlow.eRender) streamFlag = AudioClientStreamFlags.Loopback;
            result = _audioClient.Initialize(AudioClientMode.Shared, streamFlag, 10000000, 0, waveFormat, ref _sessionGuid);
            result = _audioClient.Start();
            //Change wave format here
            SetupWaveFormat(waveFormat);
            
            result = _audioEndpointVolume.GetChannelCount(out _channelCount);
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

        public override void UninitializeAudio()
        {
            if (null != _audioEndpointVolume) _audioEndpointVolume.UnregisterControlChangeNotify(classCallBack);
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
            if (null != waveFormat)
            {
                waveFormat = null;
            }
            base.UninitializeAudio();
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
