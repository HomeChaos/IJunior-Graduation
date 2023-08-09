using System.Collections;
using Scripts.Components;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.EnemyScripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] private TypesOfEnemies _enemyType;
        [SerializeField] private AudioClip _burnSound;

        private readonly int _burnKey = Animator.StringToHash("Burn");
        
        private Transform _target;
        private IEnumerator _currentState;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Specification _specification;
        private int _health;
        private EnemySoundsComponent _enemySounds;
        private bool _isStopped;

        public event UnityAction<EnemyBase, int> OnDie;

        public TypesOfEnemies EnemyType => _enemyType;

        protected Transform Target => _target;
        protected Specification Specification => _specification;
        protected Animator Animator => _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual void Init(Transform target, EnemySpecifications specifications, EnemySoundsComponent enemySounds, Vector2 newPosition)
        {
            if (_specification == null)
                _specification = specifications.GetSpecification(_enemyType);

            if (_enemySounds == null)
                _enemySounds = enemySounds;
            
            _health = _specification.Health;
            _target = target;
            transform.position = newPosition;
            _isStopped = false;
        }

        public void TakeDamage(int damage)
        {
            if (damage < 0)
            {
                throw new System.ArgumentException("Damage cannot be negative");
            }

            _health -= damage;
            
            if (_health <= 0)
            {
                OnDie?.Invoke(this, _specification.Reward);
                StartState(Stop());
                
                _animator.SetTrigger(_burnKey);
                _enemySounds.PlaySound(_burnSound);
            }
        }

        public void Freeze()
        {
            _isStopped = true;
            StartState(Stop());
        }

        public void UnFreeze()
        {
            _isStopped = false;
            StartState(GoToTarget());
        }

        protected abstract IEnumerator GoToTarget();

        protected abstract IEnumerator AttackTarget();

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

        private IEnumerator Stop()
        {
            if (_currentState != null) 
                StopCoroutine(_currentState);

            yield return null;
        }

        // Calling in animation
        private void StartEnemy()
        {
            if (_isStopped == false)
                StartState(GoToTarget());
        }

        // Calling in animation
        private void Hold()
        {
            gameObject.SetActive(false);
        }
    }
}