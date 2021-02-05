using AppCoreAudioAPIDemo.Models.Structures;
using CoreAudioLib.Enums;
using CoreAudioLib.NativeCore;
using CoreAudioLib.Structures;
using System;
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

        private void OnDeviceMicVolumeCallBack(AudioVolumeNotificationData cData)
        {
            _audioDeviceCollection[1].AudioSlider.SliderValue = Math.Ceiling(cData.MasterVolume * AppCoreAudioConstants.VALUE_MAX).ToString();
        }

        private void OnDeviceSpeakerVolumeCallBack(AudioVolumeNotificationData cData)
        {
            _audioDeviceCollection.First().AudioSlider.SliderValue = Math.Ceiling(cData.MasterVolume * AppCoreAudioConstants.VALUE_MAX).ToString();
        }


        private void OnSliderValueChanged(AudioDataFlow audioFlow, double oldV, double newV)
        {
            var setValue = newV / AppCoreAudioConstants.VALUE_MAX;
            switch(audioFlow)
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
                        AudioFLow = AudioDataFlow.eRender,
                        MaxValue = "100",
                        MinValue = "0",
                        SliderValue = Math.Ceiling(_audioDevice.GetSpeakerVolumeValue() * AppCoreAudioConstants.VALUE_MAX).ToString(),
                        Step = "1",
                        OnSliderValueChange = OnSliderValueChanged
                    }
                    
                },
                new ModelAudioSlider(){
                    ImageSource = AppCoreAudioConstants.IMG_HEADPHONE,
                    ButtonContent = AppCoreAudioConstants.IMG_HEADPHONE_MUTE,
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
