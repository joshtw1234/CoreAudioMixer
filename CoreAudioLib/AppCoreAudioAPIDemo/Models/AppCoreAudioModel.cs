﻿using AppCoreAudioAPIDemo.Models.Structures;
using CoreAudioLib.Enums;
using CoreAudioLib.NativeCore;
using CoreAudioLib.Structures;
using System;
using System.Collections.ObjectModel;
using System.Linq;

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

        private void OnDeviceMicVolumeCallBack(AudioVolumeNotificationData cData)
        {
            _audioDeviceCollection[1].AudioSlider.SliderValue = Math.Ceiling(cData.MasterVolume * AppCoreAudioConstants.VALUE_MAX).ToString();
        }

        private void OnDeviceSpeakerVolumeCallBack(AudioVolumeNotificationData cData)
        {
            _audioDeviceCollection.First().AudioSlider.SliderValue = Math.Ceiling(cData.MasterVolume * AppCoreAudioConstants.VALUE_MAX).ToString();
        }
        #endregion

        public ObservableCollection<ModelAudioSlider> GetAudioDeviceCollection()
        {
            return _audioDeviceCollection = new ObservableCollection<ModelAudioSlider>()
            {
                new ModelAudioSlider(){ 
                    ImageSource = AppCoreAudioConstants.IMG_HEADPHONE, 
                    ButtonContent = AppCoreAudioConstants.IMG_HEADPHONE_MUTE, 
                    AudioSlider = new SliderModel()
                    { 
                        MaxValue = "100",
                        MinValue = "0",
                        SliderValue = Math.Ceiling(_audioDevice.GetSpeakerVolumeValue() * AppCoreAudioConstants.VALUE_MAX).ToString(),
                        Step = "1",

                    } 
                },
                new ModelAudioSlider(){
                    ImageSource = AppCoreAudioConstants.IMG_HEADPHONE,
                    ButtonContent = AppCoreAudioConstants.IMG_HEADPHONE_MUTE,
                    AudioSlider = new SliderModel()
                    {
                        MaxValue = "100",
                        MinValue = "0",
                        SliderValue = Math.Ceiling(_audioDevice.GetMicrophoneVolumeValue() * AppCoreAudioConstants.VALUE_MAX).ToString(),
                        Step = "1",

                    }
                },
            };
        }

        public ObservableCollection<ModelAudioSlider> GetAudioSessionCollection()
        {
            return _audioSessionCollection = new ObservableCollection<ModelAudioSlider>()
            {
                new ModelAudioSlider(){ ImageSource = AppCoreAudioConstants.IMG_HEADPHONE, ButtonContent = AppCoreAudioConstants.IMG_HEADPHONE_MUTE},
                new ModelAudioSlider(){ ImageSource = AppCoreAudioConstants.IMG_HEADPHONE, ButtonContent = AppCoreAudioConstants.IMG_HEADPHONE_MUTE}
            };
        }
    }
}
