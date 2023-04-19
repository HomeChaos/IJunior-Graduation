using System;
using System.Collections;
using Scripts.Components;
using Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.EnemyScripts
{
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private TypesOfEnemies _enemyType;
        [SerializeField] private float _minDistanceForTarget = 1f;

        private readonly int AttackKey = Animator.StringToHash("Attack");

        private readonly int BurnKey = Animator.StringToHash("Burn");

        private readonly int RaiseKey = Animator.StringToHash("Raise");

        private IEnumerator _currentState;
        private IEnumerator _current;
        private Animator _animator;
        private Transform _target;
        private Specification _specification;

        [SerializeField] private int _health;

        public event UnityAction<int> OnDie; 

        public void Init(Transform target, EnemySpecifications specifications)
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            
            if (_specification == null)
                _specification = specifications.GetSpecification(_enemyType);
            
            _health = _specification.Health;
            _target = target;
            //_animator.Play(RaiseKey);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            
            if (_health <= 0)
            {
                OnDie?.Invoke(_specification.Reward);
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
        
        private IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (IsTargetFarAway())
            {
                var direction = _target.position - transform.position;
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