﻿using System;
using System.Collections;
using Scripts.Components;
using Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.EnemyScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private TypesOfEnemies _enemyType;
        [SerializeField] private float _minDistanceForTarget = 1f;

        private readonly int AttackKey = Animator.StringToHash("Attack");
        private readonly int BurnKey = Animator.StringToHash("Burn");
        private readonly int RaiseKey = Animator.StringToHash("ReRaise");

        private IEnumerator _currentState;
        private IEnumerator _current;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Transform _target;
        private Specification _specification;
        private int _health;

        public TypesOfEnemies EnemyType => _enemyType;
        public event UnityAction<Enemy, int> OnDie; 
        

        public void Init(Transform target, EnemySpecifications specifications, Vector2 newPosition)
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            
            if (_specification == null)
                _specification = specifications.GetSpecification(_enemyType);

            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _health = _specification.Health;
            _target = target;
            transform.position = newPosition;
            //_animator.SetTrigger(RaiseKey);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            
            if (_health <= 0)
            {
                OnDie?.Invoke(this, _specification.Reward);
                StartState(Stop());
                _animator.SetTrigger(BurnKey);
                //Destroy(gameObject);
            }
        }

        private void StartEnemy()
        {
            StartState(GoToTarget());
        }

        private bool IsTargetFarAway()
        {
            var x = Vector3.Distance(_target.position, transform.position);
            return x >= _minDistanceForTarget;
        }
        
        private void UpdateSpriteRender(Vector3 direction)
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
        
        private IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (IsTargetFarAway())
            {
                var direction = _target.position - transform.position;
                UpdateSpriteRender(direction);
                transform.Translate(direction.normalized * _specification.Speed * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
            
            StartState(AttackTarget());
        }
        
        private IEnumerator AttackTarget()
        {
            var waitForSeconds = new WaitForSeconds(_specification.SpeedDamage);
            IDamageable playerHealth = _target.GetComponent<IDamageable>();
            
            while (IsTargetFarAway() == false)
            {
                playerHealth.TakeDamage(_specification.Damage);
                _animator.SetTrigger(AttackKey);
                
                yield return waitForSeconds;
            }
            
            StartState(GoToTarget());
        }
        
        private IEnumerator Stop()
        {
            if (_currentState != null) 
                StopCoroutine(_currentState);

            yield return null;
        }

        private void StartState(IEnumerator coroutine)
        {
            if (_currentState != null) 
                StopCoroutine(_currentState);

            _currentState = coroutine;
            StartCoroutine(coroutine);
        }

        private void Hold()
        {
            gameObject.SetActive(false);
        }
    }
}