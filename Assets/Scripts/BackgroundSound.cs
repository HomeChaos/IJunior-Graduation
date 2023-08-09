using Scripts.Settings;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundSound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _musicTracks;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.priority = 0;
            _audioSource.playOnAwake = false;
        }

        private void OnEnable()
        {
            SoundSettings.Instance.OnVolumeChanged += OnVolumeChanged;
        }

        private void OnDisable()
        {
            SoundSettings.Instance.OnVolumeChanged -= OnVolumeChanged;
        }

        private void Start()
        {
            int randomIndex = Random.Range(0, _musicTracks.Length);
            _audioSource.clip = _musicTracks[randomIndex];
            _audioSource.volume = SoundSettings.Instance.MusicVolume;
            _audioSource.Play();
        }

        private void OnVolumeChanged()
        {
            _audioSource.volume = SoundSettings.Instance.MusicVolume;
        }
    }
}