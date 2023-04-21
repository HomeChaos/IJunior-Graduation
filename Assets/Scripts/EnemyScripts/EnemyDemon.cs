using System.Collections;
using Scripts.Components;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    public class EnemyDemon : EnemyBase
    {
        [SerializeField] private float _minDistanceForTarget = 1f;
        
        private readonly int AttackKey = Animator.StringToHash("Attack");
        private readonly float _delayBeforeAttack = 0.2f;        
        
        protected override IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (IsTargetFarAway())
            {
                var direction = Target.position - transform.position;
                UpdateSpriteRender(direction);
                transform.Translate(direction.normalized * Specification.Speed * Time.deltaTime);
                yield return waitForEndOfFrame;
            }
            
            StartState(AttackTarget());
        }
        
        protected override IEnumerator AttackTarget()
        {
            var waitForSeconds = new WaitForSeconds(Specification.SpeedDamage);
            IDamageable playerHealth = Target.GetComponent<IDamageable>();
            
            yield return new WaitForSeconds(_delayBeforeAttack);
            
            while (IsTargetFarAway() == false)
            {
                playerHealth.TakeDamage(Specification.Damage);
                Animator.SetTrigger(AttackKey);
                
                yield return waitForSeconds;
            }
            
            StartState(GoToTarget());
        }
        
        private bool IsTargetFarAway()
        {
            var distance = Vector3.Distance(Target.position, transform.position);
            return distance >= _minDistanceForTarget;
        }
    }
}