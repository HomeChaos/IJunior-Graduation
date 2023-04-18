using System;
using System.Collections;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float _minDistanceForTarget;
        
        private readonly int AttackKey = Animator.StringToHash("Attack");
        
        protected Transform Target;
        protected float Speed = 1f;
        protected float SpeedDamage = 1f;
        
        private IEnumerator _currentState;
        private IEnumerator _current;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        protected IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForFixedUpdate();

            while (true)
            {
                transform.Translate(Target.transform.position - transform.position.normalized * Speed * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
            
            StartState(AttackTarget());
        }

        private bool IsTargetFarAway()
        {
            return Vector3.Distance(transform.position, Target.position) > _minDistanceForTarget;
        }

        protected IEnumerator AttackTarget()
        {
            var waitForSeconds = new WaitForSeconds(SpeedDamage);

            while (IsTargetFarAway() == false)
            {
                _animator.SetTrigger(AttackKey);

                yield return waitForSeconds;
            }
            
            StartState(GoToTarget());
        }

        protected IEnumerator Stop()
        {
            if (_currentState != null) 
                StopCoroutine(_currentState);

            yield return null;
        }

        protected void StartState(IEnumerator coroutine)
        {
            if (_currentState != null) 
                StopCoroutine(_currentState);

            _currentState = coroutine;
            Debug.Log(coroutine);
            StartCoroutine(coroutine);
        }
    }
}