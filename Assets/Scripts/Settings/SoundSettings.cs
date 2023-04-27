using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Settings
{
    public class SoundSettings : MonoBehaviour
    {
        private float _musicVolume;
        private float _sfxVolume;
        
        public event UnityAction OnVolumeChanged;

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