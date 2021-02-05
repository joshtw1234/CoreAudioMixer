using CoreAudioLib.Enums;
using CoreAudioLib.NativeCore;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CoreAudioLib
{
    class AudioSessionControl : BaseAudioControl
    {
        IAudioSessionManager2 _audioSessionManager2;
        Dictionary<IAudioSessionControl, JoshAudioSessionStruct> _audioSessionEventDict;

        public AudioSessionControl(IMMDevice audioDevice, AudioDataFlow audioFlow) : base(audioDevice, audioFlow)
        {
            InitializeAudioSessions();
        }

        void InitializeAudioSessions()
        {
            if (_audioDataFlow == AudioDataFlow.eCapture) return;

            //Get Session manager
            var result = _audioDevice.Activate(typeof(IAudioSessionManager2).GUID, ClsCtx.ALL, IntPtr.Zero, out object obj);
            _audioSessionManager2 = (IAudioSessionManager2)obj;

            _audioSessionEventDict = new Dictionary<IAudioSessionControl, JoshAudioSessionStruct>();
            //Get all session process IDs
            //refer https://gist.github.com/sverrirs/d099b34b7f72bb4fb386
            // enumerate sessions for on this device
            _audioSessionManager2.GetSessionEnumerator(out IAudioSessionEnumerator sessionEnumerator);
            int count;
            sessionEnumerator.GetCount(out count);

            // search for an audio session with the required process-id
            JoshAudioSessionStruct audioStruc;
            AudioSessionEvents ssEvent = null;
            for (int i = 0; i < count; ++i)
            {
                IAudioSessionControl ctl = null;
                IAudioSessionControl2 ctl2 = null;

                var revv = sessionEnumerator.GetSession(i, out ctl);
                ssEvent = new AudioSessionEvents();
                audioStruc = new JoshAudioSessionStruct();
                revv = ctl.RegisterAudioSessionNotification(ssEvent);
                ctl2 = ctl as IAudioSessionControl2;
                // NOTE: we could also use the app name from ctl.GetDisplayName()
                revv = ctl2.GetProcessId(out uint cpid);
                ssEvent.ProcessID = cpid;

                var pos = System.Diagnostics.Process.GetProcessById((int)cpid);
                ssEvent.DisplayName = pos.ProcessName;

                audioStruc.AudioSessionEventClass = ssEvent;
                audioStruc.SimpleAudioControl = ctl2 as ISimpleAudioVolume;
                //revv = ctl2.GetDisplayName(out string names);
                //Keep session in memory for opreation it later
                _audioSessionEventDict.Add(ctl, audioStruc);

            }
            //Register event for get session creation
            _audioSessionManager2.RegisterSessionNotification(new AudioSessionNotification());
        }

        public override void UninitializeAudio()
        {
           
            if (null != _audioSessionEventDict)
            {
                foreach (var ctl in _audioSessionEventDict)
                {
                    ctl.Key.UnregisterAudioSessionNotification(ctl.Value.AudioSessionEventClass);
                    Marshal.ReleaseComObject(ctl.Key);
                }
            }

            if (null != _audioSessionManager2)
            {
                Marshal.ReleaseComObject(_audioSessionManager2);
            }
            base.UninitializeAudio();
        }
    }
}
