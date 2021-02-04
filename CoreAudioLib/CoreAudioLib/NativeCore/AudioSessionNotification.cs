using CoreAudioLib.Enums;

namespace CoreAudioLib.NativeCore
{
    class AudioSessionNotification : IAudioSessionNotification
    {
        public COMResult OnSessionCreated(IAudioSessionControl newSession)
        {
            return COMResult.S_OK;
        }
    }
}
