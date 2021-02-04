using System.Collections.ObjectModel;

namespace AppCoreAudioAPIDemo.ViewModels
{
    class MainWindowViewModel
    {
        Models.IAppCoreAudioModel _model;
        public ObservableCollection<Models.Structures.ModelAudioSlider> AudioDeviceCollection { get; set; }
        public ObservableCollection<Models.Structures.ModelAudioSlider> AudioSessionCollection { get; set; }
        public string AudioTitleText { get; set; }
        public MainWindowViewModel()
        {
            _model = new Models.AppCoreAudioModel();
            AudioTitleText = "This is Josh";
            AudioDeviceCollection = _model.GetAudioDeviceCollection();
            AudioSessionCollection = _model.GetAudioSessionCollection();
        }

      
    }
}
