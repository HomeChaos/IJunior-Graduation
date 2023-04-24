using Scripts.Components;
using Scripts.Weapon.PlayerWeapon;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.PlayerScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Wallet))]
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private PlayerWeapon _playerWeapon;
        [SerializeField] private int _health = 1;
        [SerializeField] private ParticleSystem _hitParticle;
        [SerializeField] private AudioClip _hitClip;

        private readonly int IsRunningKey = Animator.StringToHash("isRunnig");
        private readonly int DeadKey = Animator.StringToHash("Dead");

        private Vector2 _currentDirection;
        private Animator _animator;
        private Wallet _wallet;
        private AudioSource _audioSource;

        public Wallet Wallet => _wallet;
        public event UnityAction Dying;
        public event UnityAction<int> OnHealthChange; 

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
            if (_health <= 0)
            {
                return;
            }

            _health -= damage;
            OnHealthChange?.Invoke(_health);
            _hitParticle.Play();
            _audioSource.PlayOneShot(_hitClip);

            if (_health <= 0)
            {
                Dying?.Invoke();
                _animator.SetTrigger(DeadKey);
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _wallet = GetComponent<Wallet>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            OnHealthChange?.Invoke(_health);
        }

        private void FixedUpdate()
        {
            transform.Translate(_currentDirection * _speed * Time.deltaTime);
            _animator.SetBool(IsRunningKey, _currentDirection != Vector2.zero);
        }
    }
}