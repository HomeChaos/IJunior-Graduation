using Scripts.Settings;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    [RequireComponent(typeof(AudioSource))]
    public class EnemySoundsComponent : MonoBehaviour
    {
        private AudioSource _audioSource;

        private bool _isPlaying => _audioSource.isPlaying;

        public void Init()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            float volumeCorrection = 0.5f;
            _audioSource.volume = SoundSettings.Instance.SfxVolume / volumeCorrection;
        }

        public void PlaySound(AudioClip clip)
        {
            if (_isPlaying == false)
            {
                _audioSource.clip = clip;
                _audioSource.Play();
            }
        }
    }
}