using System;
using System.Windows.Input;

namespace AppCoreAudioAPIDemo.Models.Structures
{
    class MenuItem : NotifyBase
    {
        public ButtonCommandHandler MenuCommand { get; set; }
        public virtual string MenuData { get; set; }
        public virtual bool MenuVisibility { get; set; }
        public virtual string MenuImage { get; set; }
    }
    class ButtonMenuItem : MenuItem
    {
        private string _menuImage;
        public override string MenuImage
        {
            get => _menuImage;
            set
            {
                _menuImage = value;
                OnPropertyRaised("MenuImage");
            }
        }
    }
    class ModelAudioSlider : MenuItem
    {
        public MenuItem ButtonContent { get; set; }

        public BaseSliderModel AudioSlider { get; set; }
    }
}
