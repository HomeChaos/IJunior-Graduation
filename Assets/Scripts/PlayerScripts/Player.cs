using Scripts.Components;
using Scripts.Settings;
using Scripts.Weapon.PlayerWeapon;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.PlayerScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private PlayerWeapon _playerWeapon;
        [SerializeField] private ParticleSystem _hitParticle;
        [SerializeField] private AudioClip _hitClip;

        private readonly int _isRunningKey = Animator.StringToHash("isRunnig");
        private readonly int _deadKey = Animator.StringToHash("Dead");

        private Vector2 _currentDirection;
        private Animator _animator;
        private AudioSource _audioSource;
        private int _health;
        
        public event UnityAction OnDying;
        public event UnityAction<int> OnHealthChange;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            OnHealthChange?.Invoke(_health);
            _audioSource.volume = SoundSettings.Instance.SfxVolume;
        }

        private void FixedUpdate()
        {
            transform.Translate(_currentDirection * _speed * Time.deltaTime);
            _animator.SetBool(_isRunningKey, _currentDirection != Vector2.zero);
        }

        public void Init(int health)
        {
            if (health <= 0)
                throw new System.ArgumentOutOfRangeException("Health cannot be lower than one");
            
            _health = health;
        }

        public void SetDirection(Vector2 direction)
        {
            _currentDirection = direction;
        }

        public void SetShooting(bool isShooting)
        {
            if (isShooting)
                _playerWeapon.StartShoot();
            else
                _playerWeapon.StopShoot();
        }

        public void TakeDamage(int damage)
        {
            Debug.Log($"Take damage: {damage}");
            //return;
            
            if (_health <= 0)
                return;

            ApplyDamage(damage);

            if (_health <= 0)
            {
                OnDying?.Invoke();
                _animator.SetTrigger(_deadKey);
            }
        }

        private void ApplyDamage(int damage)
        {
            _health -= damage;
            OnHealthChange?.Invoke(_health);
            _hitParticle.Play();
            _audioSource.PlayOneShot(_hitClip);
        }
    }
}