using AppCoreAudioAPIDemo.Models.Structures;
using System.Collections.ObjectModel;

namespace AppCoreAudioAPIDemo.Models
{
    interface IAppCoreAudioModel
    {
        ObservableCollection<ModelAudioSlider> GetAudioDeviceCollection();
        ObservableCollection<ModelAudioSlider> GetAudioSessionCollection();
    }
}
