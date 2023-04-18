using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.PlayerWeapon;
using UnityEngine;

namespace Scripts.PlayerScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Weapon _weapon;

        private readonly int IsRunningKey = Animator.StringToHash("isRunnig");
        
        private Vector2 _currentDirection;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

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

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            //transform.Translate(_currentDirection * _speed * Time.deltaTime);
            //_currentDirection = Vector2.zero;
        }

        private void FixedUpdate()
        {
            transform.Translate(_currentDirection * _speed * Time.deltaTime);
            _animator.SetBool(IsRunningKey, _currentDirection != Vector2.zero);
        }
    }
}