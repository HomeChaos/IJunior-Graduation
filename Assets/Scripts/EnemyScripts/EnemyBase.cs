using System;
using System.Collections;
using Scripts.Components;
using Scripts.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.EnemyScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] private TypesOfEnemies _enemyType;
        [SerializeField] private AudioClip _burnSound;

        private readonly int BurnKey = Animator.StringToHash("Burn");
        
        private Transform _target;
        private IEnumerator _currentState;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Specification _specification;
        private AudioSource _audioSource;
        private SoundSettings _soundSettings;
        private int _health;

        public event UnityAction<EnemyBase, int> OnDie;

        public TypesOfEnemies EnemyType => _enemyType;

        protected Transform Target => _target;
        protected Specification Specification => _specification;
        protected Animator Animator => _animator;

        public virtual void Init(Transform target, EnemySpecifications specifications, Vector2 newPosition)
        {
            if (_specification == null)
                _specification = specifications.GetSpecification(_enemyType);
            
            _health = _specification.Health;
            _target = target;
            transform.position = newPosition;
        }
        
        public void TakeDamage(int damage)
        {
            _health -= damage;
            
            if (_health <= 0)
            {
                OnDie?.Invoke(this, _specification.Reward);
                StartState(Stop());
                _animator.SetTrigger(BurnKey);
                _audioSource.PlayOneShot(_burnSound);
            }
        }

        protected abstract IEnumerator GoToTarget();
        
        protected abstract IEnumerator AttackTarget();

        protected virtual IEnumerator Stop()
        {
            if (_currentState != null) 
                StopCoroutine(_currentState);

            yield return null;
        }

        protected void UpdateSpriteRender(Vector3 direction)
        {
            if (direction.x > 0)
            {
                _spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
        }

        protected void StartState(IEnumerator coroutine)
        {
            if (_currentState != null) 
                StopCoroutine(_currentState);

            _currentState = coroutine;
            StartCoroutine(coroutine);
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _soundSettings = SoundUtils.FindSoundSettings();
        }

        private void Start()
        {
            float volumeCorrection = 0.5f;
            _audioSource.volume = _soundSettings.SfxVolume / volumeCorrection;
        }

        protected void StartEnemy()
        {
            StartState(GoToTarget());
        }

        private void Hold()
        {
            gameObject.SetActive(false);
        }
    }
}