using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundSound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _music;
        
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.priority = 0;
            _audioSource.playOnAwake = false;
        }

        private void Start()
        {
            int randomIndex = Random.Range(0, _music.Length);
            _audioSource.clip = _music[randomIndex];
            _audioSource.Play();
        }
    }
}