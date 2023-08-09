using System;

namespace Scripts.Settings
{
    public class SoundSettings
    {
        public static SoundSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SoundSettings();
                }

                return _instance;
            }
        }

        private static SoundSettings _instance;

        private float _musicVolume;
        private float _sfxVolume;

        public event Action OnVolumeChanged;

        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;

        public void ChangeValue(float music, float sfx)
        {
            _musicVolume = music;
            _sfxVolume = sfx;
            OnVolumeChanged?.Invoke();
        }
    }
}