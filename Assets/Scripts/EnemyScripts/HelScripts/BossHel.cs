using System;
using System.Collections;
using UnityEngine;

namespace Scripts.EnemyScripts.HelScripts
{
    public class BossHel : EnemyBase
    {
        [SerializeField] private float _delayBeforeAttack = 3f;
        [SerializeField] private float _timeToAttack = 5f;
        [SerializeField] private float _minDistanceForTarget = 1f;
        [SerializeField] private float _timeToStomp = 0.3f;
        [SerializeField] private StompArea _stompArea;

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
            Animator.SetTrigger(_stompKey);
            Stop();
            yield break;
            
            Debug.LogWarning("Проскочили STOP");
            
            if (IsTargetFarAway())
            {
                
                // если цель далеко:
                //     рандомная атака из двух
            }
            else
            {
                Animator.SetTrigger(_stompKey);
                Stop();
            }

            throw new System.NotImplementedException();
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

        private IEnumerator PerformStomp()
        {
            _stompArea.StartAttack();
            yield return new WaitForSeconds(_timeToStomp);
            _stompArea.StopAttack();
        }

        [ContextMenu("[!] Test attack")]
        private void StartAttackTest()
        {
            Animator.SetTrigger(_stompKey);
        }
    }
}