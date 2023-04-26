
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Settings
{
    public class SoundSettings : MonoBehaviour
    {
        private float _musicVolume;
        private float _sfxVolume;
        
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;

        public event UnityAction<float, float> OnVolumeChanged; 

        public void Init(float music, float sfx)
        {
            _musicVolume = music;
            _sfxVolume = sfx;
            OnVolumeChanged?.Invoke(_musicVolume, _sfxVolume);
        }
    }
}