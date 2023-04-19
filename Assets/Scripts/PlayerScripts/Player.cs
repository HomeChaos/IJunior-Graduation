using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Components;
using Scripts.PlayerWeapon;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.PlayerScripts
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Weapon _weapon;

        private readonly int IsRunningKey = Animator.StringToHash("isRunnig");
        
        private Vector2 _currentDirection;
        private Animator _animator;
        private int _health = 100;

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
            ConsoleTools.LogSuccess("УРОН ГЕРОЮ", 14);
            _health -= damage;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            transform.Translate(_currentDirection * _speed * Time.deltaTime);
            _animator.SetBool(IsRunningKey, _currentDirection != Vector2.zero);
        }
    }
}