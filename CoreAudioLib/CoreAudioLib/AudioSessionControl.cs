using CoreAudioLib.Enums;
using CoreAudioLib.NativeCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CoreAudioLib
{
    class AudioSessionControl : BaseAudioControl
    {
        IAudioSessionManager2 _audioSessionManager2;
        Dictionary<IAudioSessionControl, JoshAudioSessionStruct> _audioSessionEventDict;

        public List<AudioSessionDataStruct> AudioSessionsPid { get; private set; }

        public AudioSessionControl(IMMDevice audioDevice, AudioDataFlow audioFlow) : base(audioDevice, audioFlow)
        {
            AudioSessionsPid = new List<AudioSessionDataStruct>();
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
            string iconSaveDir = $@"C:\Users\JoshOMEN\Documents\";
            string[] fileEntries = System.IO.Directory.GetFiles(iconSaveDir);
            foreach (var iconStr in fileEntries)
            {
                if (iconStr.Contains("ico"))
                {
                    System.IO.File.Delete(iconStr);
                }
            }
            for (int i = 0; i < count; ++i)
            {
                try
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
                    string fullPath = pos.MainModule.FileName;
                    var icon = System.Drawing.Icon.ExtractAssociatedIcon(fullPath);
                    string iconSavedPath = $"{iconSaveDir}{ssEvent.DisplayName}.ico";
                    // Save it to disk, or do whatever you want with it.
                    using (var stream = new System.IO.FileStream(iconSavedPath, System.IO.FileMode.CreateNew))
                    {
                        icon.Save(stream);
                    }
                    ssEvent.RegisterSessionVolumeCallBack(OnSessionChangeCallBack);
                    AudioSessionsPid.Add(new AudioSessionDataStruct() { SeesionPid = cpid, SessionIconPath = iconSavedPath });

                    audioStruc.AudioSessionEventClass = ssEvent;
                    audioStruc.SimpleAudioControl = ctl2 as ISimpleAudioVolume;
                    //revv = ctl2.GetDisplayName(out string names);
                    //Keep session in memory for opreation it later
                    _audioSessionEventDict.Add(ctl, audioStruc);
                }
                catch(Exception ex)
                {

                }
            }
            //Register event for get session creation
            _audioSessionManager2.RegisterSessionNotification(new AudioSessionNotification());
        }

        private void OnSessionChangeCallBack(uint spid, float volume, bool isMute)
        {
            var sControl = AudioSessionsPid.FirstOrDefault(x => x.SeesionPid == spid);
            sControl.SessionVolumeChangeCallBack?.Invoke(spid, volume, isMute);
        }

        public override void UninitializeAudio()
        {
           
            if (null != _audioSessionEventDict)
            {
                foreach (var ctl in _audioSessionEventDict)
                {
                    ctl.Value.AudioSessionEventClass.UnRegisterSessionVolumeCallBack(OnSessionChangeCallBack);
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

        public void SetVolume(uint spid, double value)
        {
            var sControl = _audioSessionEventDict.Values.FirstOrDefault(x => x.AudioSessionEventClass.ProcessID == spid);
            sControl.SimpleAudioControl.SetMasterVolume((float)value, Guid.Empty);
        }

        public double GetVolume(uint spid)
        {
            var sControl = _audioSessionEventDict.Values.FirstOrDefault(x => x.AudioSessionEventClass.ProcessID == spid);
            sControl.SimpleAudioControl.GetMasterVolume(out float value);
            return value;
        }

        public void SetMuted(uint spid, bool isMuted)
        {
            var sControl = _audioSessionEventDict.Values.FirstOrDefault(x => x.AudioSessionEventClass.ProcessID == spid);
            sControl.SimpleAudioControl.SetMute(isMuted, Guid.Empty);
        }
        public bool GetIsMuted(uint spid)
        {
            var sControl = _audioSessionEventDict.Values.FirstOrDefault(x => x.AudioSessionEventClass.ProcessID == spid);
            sControl.SimpleAudioControl.GetMute(out bool isMuted);
            return isMuted;
        }
    }
}
