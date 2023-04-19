using Scripts.EnemyScripts;
using UnityEngine;

namespace Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] [Range(1, 30)] private int _frameRate = 10;
        [SerializeField] private Sprite[] _sprites;

        private SpriteRenderer _renderer;
        private float _secondsPerFrame;
        private int _currentSprite;
        private float _nextFrameTime;
        private bool _isPlaying = true;

        private void OnEnable()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
        }

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _secondsPerFrame = 1f / _frameRate;

            StartAnimation();
        }

        private void Update()
        {
            if (_nextFrameTime > Time.time)
                return;

            if (_currentSprite >= _sprites.Length)
                return;

            _renderer.sprite = _sprites[_currentSprite];
            _nextFrameTime += _secondsPerFrame;
            _currentSprite++;
        }

        private void OnBecameVisible()
        {
            enabled = _isPlaying;
        }

        private void OnBecameInvisible()
        {
            enabled = false;
        }

        private void StartAnimation()
        {
            _nextFrameTime = Time.time + _secondsPerFrame;
            enabled = true;
            _isPlaying = true;
            _currentSprite = 0;
        }
    }
}