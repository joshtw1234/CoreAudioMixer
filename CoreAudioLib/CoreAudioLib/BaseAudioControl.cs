using CoreAudioLib.Enums;
using CoreAudioLib.NativeCore;
using System;
using System.Runtime.InteropServices;

namespace CoreAudioLib
{
    abstract class BaseAudioControl
    {
        #region Core API structures
        /// <summary>
        /// MSFT Core API structures
        /// </summary>
        protected IMMDevice _audioDevice;
        protected AudioDataFlow _audioDataFlow;
        #endregion

        /// <summary>
        /// The Constructor
        /// </summary>
        /// <param name="audioFlow"></param>
        public BaseAudioControl(IMMDevice audioDevice, AudioDataFlow audioFlow)
        {
            _audioDataFlow = audioFlow;
            _audioDevice = audioDevice;
        }
        /// <summary>
        /// The UnInitialize Audio
        /// </summary>
        public virtual void UninitializeAudio()
        {
           
            if (null != _audioDevice)
            {
                Marshal.ReleaseComObject(_audioDevice);
                _audioDevice = null;
            }
           
            GC.SuppressFinalize(this);
        }
    }
}
