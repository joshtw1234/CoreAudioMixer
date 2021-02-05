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

    public delegate void SliderValueChangeCallBack(int sessionOrFlow, double oldV, double newV);

    abstract class BaseSliderModel : NotifyBase
    {
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
        protected string _sliderValue;
        /// <summary>
        /// Get or Set Slider value
        /// </summary>
        public virtual string SliderValue
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
        public SliderValueChangeCallBack OnSliderValueChange { get; set; }
    }
    class SliderModel : BaseSliderModel
    {
        public CoreAudioLib.Enums.AudioDataFlow AudioFLow { get; set; }
        public override string SliderValue
        {
            get => base.SliderValue;
            set
            {
                if (null != _sliderValue)
                {
                    if (_sliderValue.Equals(value)) return;
                }
                OnSliderValueChange?.Invoke((int)AudioFLow, double.Parse(_sliderValue), double.Parse(value));
                base.SliderValue = value;
            }
        }
    }
    class SessionSliderModel : BaseSliderModel
    {
        public uint SessionPid { get; set; }
        public override string SliderValue
        {
            get => base.SliderValue;
            set
            {
                if (null != _sliderValue)
                {
                    if (_sliderValue.Equals(value)) return;
                }
                OnSliderValueChange?.Invoke((int)SessionPid, double.Parse(_sliderValue), double.Parse(value));
                base.SliderValue = value;
            }
        }
    }
}
