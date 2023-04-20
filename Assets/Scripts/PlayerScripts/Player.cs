using System;
using Scripts.Components;
using Scripts.PlayerWeapon;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.PlayerScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Wallet))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private int _health = 1;

        private readonly int IsRunningKey = Animator.StringToHash("isRunnig");

        private Vector2 _currentDirection;
        private Animator _animator;
        private Wallet _wallet;

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
                _weapon.StartShoot();
            else
                _weapon.StopShoot();
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            OnHealthChange?.Invoke(_health);
            
            if (_health <= 0)
                Dying?.Invoke();
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _wallet = GetComponent<Wallet>();
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