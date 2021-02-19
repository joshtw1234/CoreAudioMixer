﻿using AppCoreAudioAPIDemo.Models.Structures;
using CoreAudioLib.Enums;
using CoreAudioLib.NativeCore;
using CoreAudioLib.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AppCoreAudioAPIDemo.Models
{
    class AppCoreAudioModel : IAppCoreAudioModel
    {
        ObservableCollection<ModelAudioSlider> _audioDeviceCollection;
        ObservableCollection<ModelAudioSlider> _audioSessionCollection;

        CoreAudioLib.AudioDeviceController _audioDevice;
        public AppCoreAudioModel()
        {
            InitializeAudioDevice();
        }
        #region Privates
        private void InitializeAudioDevice()
        {
            _audioDevice = new CoreAudioLib.AudioDeviceController();
            _audioDevice.InitializeAudioControls(AudioDataFlow.eAll, string.Empty, string.Empty);
            _audioDevice.RegisterSpeakerVolumeCallBack(OnDeviceSpeakerVolumeCallBack);
            _audioDevice.RegisterMicrophoneVolumeCallBack(OnDeviceMicVolumeCallBack);
            _audioDevice.RegisterAudioDeviceStateCallBack(OnDeviceStateCallBack);
        }
        private void UnInitializeAudioDevice()
        {
            _audioDevice.UnRegisterMicrophoneVolumeCallBack(OnDeviceMicVolumeCallBack);
            _audioDevice.UnRegisterSpeakerVolumeCallBack(OnDeviceSpeakerVolumeCallBack);
            _audioDevice.UnRegisterAudioDeviceStateCallBack(OnDeviceStateCallBack);
            _audioDevice.UnInitializeAudioControls();
        }
        private void OnDeviceStateCallBack(AudioDeviceChangeData audioDeviceData)
        {
            switch(audioDeviceData.DeviceState)
            {
                case AudioDeviceState.DEVICE_STATE_ACTIVE:
                case AudioDeviceState.DEVICE_STATE_NOTPRESENT:
                case AudioDeviceState.DEVICE_STATE_DISABLED:
                case AudioDeviceState.DEVICE_STATE_UNPLUGGED:
                    UnInitializeAudioDevice();
                    InitializeAudioDevice();
                    break;
                default:
                    break;
            }
        }

        private string GetMutedImage(bool isMuted)
        {
            string spMuteImg = AppCoreAudioConstants.IMG_HEADPHONE_MUTE;
            if (!isMuted)
            {
                spMuteImg = AppCoreAudioConstants.IMG_HEADPHONE;
            }
            return spMuteImg;
        }

        private void OnDeviceMicVolumeCallBack(AudioVolumeNotificationData cData)
        {
            _audioDeviceCollection[1].AudioSlider.SliderValue = Math.Ceiling(cData.MasterVolume * AppCoreAudioConstants.VALUE_MAX).ToString();
            _audioDeviceCollection[1].ButtonContent.MenuImage = GetMutedImage(cData.Muted);
        }

        private void OnDeviceSpeakerVolumeCallBack(AudioVolumeNotificationData cData)
        {
            _audioDeviceCollection.First().AudioSlider.SliderValue = Math.Ceiling(cData.MasterVolume * AppCoreAudioConstants.VALUE_MAX).ToString();
            _audioDeviceCollection.First().ButtonContent.MenuImage = GetMutedImage(cData.Muted);
        }


        private void OnSliderValueChanged(int audioFlow, double oldV, double newV)
        {
            var setValue = newV / AppCoreAudioConstants.VALUE_MAX;
            if (audioFlow < 10)
            {
                switch ((AudioDataFlow)audioFlow)
                {
                    case AudioDataFlow.eRender:
                        _audioDevice.SetSpeakerVolumeValue(setValue);
                        break;
                    case AudioDataFlow.eCapture:
                        _audioDevice.SetMicrophoneVolumeValue(setValue);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _audioDevice.SetSessionVolume((uint)audioFlow, setValue);
            }
        }
        #endregion
        const string SPEAKERNAME = "Speaker";
        const string MICROPHONENAME = "Mic";
        private ButtonCommandHandler _btnCommandHandler;
        private ButtonCommandHandler BtnCommandHandler => _btnCommandHandler ?? new ButtonCommandHandler(OnAudioDeviceButtonClick, true);
        public ObservableCollection<ModelAudioSlider> GetAudioDeviceCollection()
        {
            return _audioDeviceCollection = new ObservableCollection<ModelAudioSlider>()
            {
                new ModelAudioSlider(){
                    MenuImage = AppCoreAudioConstants.IMG_HEADPHONE,
                    ButtonContent = new ButtonMenuItem()
                    {
                        MenuImage = GetMutedImage(_audioDevice.GetSpeakerIsMuted()),
                        MenuCommand = BtnCommandHandler,
                        MenuData = SPEAKERNAME
                    },
                    AudioSlider = new SliderModel()
                    {
                        AudioFLow = AudioDataFlow.eRender,
                        MaxValue = "100",
                        MinValue = "0",
                        SliderValue = Math.Ceiling(_audioDevice.GetSpeakerVolumeValue() * AppCoreAudioConstants.VALUE_MAX).ToString(),
                        Step = "1",
                        OnSliderValueChange = OnSliderValueChanged
                    }
                    
                },
                new ModelAudioSlider(){
                    MenuImage = AppCoreAudioConstants.IMG_HEADPHONE,
                    ButtonContent = new ButtonMenuItem()
                    {
                        MenuImage = GetMutedImage(_audioDevice.GetMicrophoneIsMuted()),
                        MenuCommand = BtnCommandHandler,
                        MenuData = MICROPHONENAME
                    },
                    AudioSlider = new SliderModel()
                    {
                        AudioFLow = AudioDataFlow.eCapture,
                        MaxValue = "100",
                        MinValue = "0",
                        SliderValue = Math.Ceiling(_audioDevice.GetMicrophoneVolumeValue() * AppCoreAudioConstants.VALUE_MAX).ToString(),
                        Step = "1",
                        OnSliderValueChange = OnSliderValueChanged
                    }
                },
            };
        }

        private void OnAudioDeviceButtonClick(object obj)
        {
            var audioObj = _audioDeviceCollection.First();
            bool isMicrophone = obj.ToString().Equals(MICROPHONENAME);
            if (isMicrophone)
            {
                audioObj = _audioDeviceCollection[1];
            }
            bool isMuted = audioObj.ButtonContent.MenuImage.Equals(AppCoreAudioConstants.IMG_HEADPHONE);
            audioObj.ButtonContent.MenuImage = GetMutedImage(isMuted);
            if (isMicrophone)
            {
                _audioDevice.SetMicrophoneMute(isMuted);
            }
            else
            {
                _audioDevice.SetSpeakerMute(isMuted);
            }
        }

        public ObservableCollection<ModelAudioSlider> GetAudioSessionCollection()
        {
            _audioSessionCollection = new ObservableCollection<ModelAudioSlider>();
            var SessionList = _audioDevice.GetAudioSessionsPid();
            foreach(var sess in SessionList)
            {
                var mmm = new ModelAudioSlider()
                {
                    MenuImage = sess.SessionIconPath,
                    ButtonContent = new ButtonMenuItem()
                    {
                        MenuImage = GetMutedImage(_audioDevice.GetSessionMuted(sess.SeesionPid)),
                        MenuCommand = new ButtonCommandHandler(OnSessionButtonClick, true),
                        MenuData = sess.SeesionPid.ToString()
                    },
                    AudioSlider = new SessionSliderModel()
                    {
                        SessionPid = sess.SeesionPid,
                        MaxValue = "100",
                        MinValue = "0",
                        SliderValue = Math.Ceiling(_audioDevice.GetSessionVolume(sess.SeesionPid) * AppCoreAudioConstants.VALUE_MAX).ToString(),
                        Step = "1",
                        OnSliderValueChange = OnSliderValueChanged
                    }
                };
                sess.SessionVolumeChangeCallBack = OnSessionVolumeChangeCallBack;
                _audioSessionCollection.Add(mmm);
            }
            return _audioSessionCollection;
        }

        private void OnSessionButtonClick(object obj)
        {
            int id = Convert.ToInt32(obj);
            var sControl = _audioSessionCollection.FirstOrDefault(x => (x.AudioSlider as SessionSliderModel).SessionPid == id);
            bool isMuted = sControl.ButtonContent.MenuImage.Equals(AppCoreAudioConstants.IMG_HEADPHONE);
            sControl.ButtonContent.MenuImage = GetMutedImage(isMuted);
            _audioDevice.SetSessionMuted((uint)id, isMuted);
        }

        private void OnSessionVolumeChangeCallBack(uint spid, float volume, bool isMute)
        {
            var sControl = _audioSessionCollection.FirstOrDefault(x => (x.AudioSlider as SessionSliderModel).SessionPid == spid);
            sControl.AudioSlider.SliderValue = Math.Ceiling(volume * AppCoreAudioConstants.VALUE_MAX).ToString();
        }
    }
}
