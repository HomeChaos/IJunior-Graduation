using System;
using Scripts.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundSound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _music;
        
        private AudioSource _audioSource;
        private SoundSettings _soundSettings;

        private void Awake()
        {
            _soundSettings = SoundUtils.FindSoundSettings();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.priority = 0;
            _audioSource.playOnAwake = false;
        }

        private void Start()
        {
            int randomIndex = Random.Range(0, _music.Length);
            _audioSource.clip = _music[randomIndex];
            _audioSource.volume = _soundSettings.MusicVolume;
            _soundSettings.OnVolumeChanged += OnVolumeChange;
            _audioSource.Play();
        }

        private void OnVolumeChange(float arg0, float arg1)
        {
            _audioSource.volume = _soundSettings.MusicVolume;
        }

        private void OnDisable()
        {
            _soundSettings.OnVolumeChanged -= OnVolumeChange;
        }
    }
}