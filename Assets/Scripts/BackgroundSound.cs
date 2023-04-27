using Scripts.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundSound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _musicTracks;
        
        private AudioSource _audioSource;
        private SoundSettings _soundSettings;

        private void Awake()
        {
            _soundSettings = SoundUtils.FindSoundSettings();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.priority = 0;
            _audioSource.playOnAwake = false;
        }

        private void OnEnable()
        {
            _soundSettings.OnVolumeChanged += OnVolumeChanged;
        }

        private void OnDisable()
        {
            _soundSettings.OnVolumeChanged -= OnVolumeChanged;
        }

        private void Start()
        {
            int randomIndex = Random.Range(0, _musicTracks.Length);
            _audioSource.clip = _musicTracks[randomIndex];
            _audioSource.volume = _soundSettings.MusicVolume;
            _audioSource.Play();
        }

        private void OnVolumeChanged()
        {
            _audioSource.volume = _soundSettings.MusicVolume;
        }
    }
}