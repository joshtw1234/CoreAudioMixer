using System.ComponentModel;

namespace AppCoreAudioAPIDemo.Models.Structures
{
    class SliderModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
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
                _sliderValue = value;
                OnPropertyRaised("SliderValue");
            }
        }
    }
}
