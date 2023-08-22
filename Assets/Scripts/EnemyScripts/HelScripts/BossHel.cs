using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.EnemyScripts.HelScripts
{
    public class BossHel : EnemyBase
    {
        [SerializeField] private float _delayBeforeAttack = 3f;
        [SerializeField] private float _timeToAttack = 5f;
        [SerializeField] private float _minDistanceForTarget = 1f;
        [SerializeField] private float _timeToStomp = 0.3f;
        [SerializeField] private float _timeToFireArena = 4.5f;
        [SerializeField] private StompArea _stompArea;
        [SerializeField] private FireArea _fireArea;

        private float _timeSinceLastAttack = 0f;
        
        private readonly int _fireAreaKey = Animator.StringToHash("FireArea");
        private readonly int _stompKey = Animator.StringToHash("Stomp");
        private readonly int _summonKey = Animator.StringToHash("Summon");
        
        public override void Init(Transform target, EnemySpecifications specifications, EnemySoundsComponent enemySounds, Vector2 newPosition)
        {
            base.Init(target, specifications, enemySounds, newPosition);
            _stompArea.Init(transform, target);
        }

        protected override IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            _timeSinceLastAttack = 0f;
            
            while (IsTargetFarAway() && IsTimeToAttack() == false)
            {
                Vector3 direction = Target.position - transform.position;
                UpdateSpriteRender(direction);
                transform.Translate(direction.normalized * Specification.Speed * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
            
            StartState(AttackTarget());
        }

        protected override IEnumerator AttackTarget()
        {
            if (IsTargetFarAway())
            {

                int randomAttack = Random.Range(0, 2);

                if (randomAttack == 0)
                {
                    
                }
                else
                {
                    
                }
            }
            else
            {
                Animator.SetTrigger(_stompKey);
                Stop();
            }

            yield break;
        }

        private bool IsTimeToAttack()
        {
            _timeSinceLastAttack += Time.deltaTime;
            return _timeSinceLastAttack > _timeToAttack;
        }
        
        private bool IsTargetFarAway()
        {
            var distance = Vector3.Distance(Target.position, transform.position);
            return distance >= _minDistanceForTarget;
        }

        // Calling in animation
        private void AttackAnimationStopped()
        {
            StartState(GoToTarget());
        }

        // Calling in animation
        private void OnStompAction()
        {
            StartState(PerformStomp());
        }

        private void OnFireAreaAction()
        {
            StartState(PerformFireArea());
        }

        private IEnumerator PerformStomp()
        {
            _stompArea.StartAttack();
            yield return new WaitForSeconds(_timeToStomp);
            _stompArea.StopAttack();
        }

        private IEnumerator PerformFireArea()
        {
            _fireArea.StartAttack();
            yield return new WaitForSeconds(_timeToFireArena);
            _fireArea.StopAttack();
        }
    }
}