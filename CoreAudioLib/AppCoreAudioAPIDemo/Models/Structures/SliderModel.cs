using System.ComponentModel;

namespace AppCoreAudioAPIDemo.Models.Structures
{
    class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }

    public delegate void SliderValueChangeCallBack(CoreAudioLib.Enums.AudioDataFlow audioFlow, double oldV, double newV);
    class SliderModel : NotifyBase
    {
        public CoreAudioLib.Enums.AudioDataFlow AudioFLow { get; set; }
        /// <summary>
        /// The Max value
        /// </summary>
        public string MaxValue { get; set; }
        /// <summary>
        /// The Min Value
        /// </summary>
        public string MinValue { get; set; }
        /// <summary>
        /// The Step
        /// </summary>
        public string Step { get; set; }
        /// <summary>
        /// The Slider value
        /// </summary>
        private string _sliderValue;
        /// <summary>
        /// Get or Set Slider value
        /// </summary>
        public string SliderValue
        {
            get
            {
                return _sliderValue;
            }
            set
            {

                if (null == _sliderValue)
                {
                    _sliderValue = value;
                }
                else
                {
                    if (_sliderValue.Equals(value)) return;
                }

                OnSliderValueChange?.Invoke(AudioFLow, double.Parse(_sliderValue),double.Parse(value));
                _sliderValue = value;
                OnPropertyRaised("SliderValue");
            }
        }
        public SliderValueChangeCallBack OnSliderValueChange { get; set; }
    }
}
