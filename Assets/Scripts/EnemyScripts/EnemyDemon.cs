using System.Collections;
using Scripts.Components;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    public class EnemyDemon : EnemyBase
    {
        [SerializeField] private float _minDistanceForTarget = 1f;
        
        private readonly int _attackKey = Animator.StringToHash("Attack");
        private readonly float _delayBeforeAttack = 0.2f;        
        
        protected override IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (IsTargetFarAway())
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
            var waitAfterAttack = new WaitForSeconds(Specification.SpeedDamage);
            IDamageable targetHealth = Target.GetComponent<IDamageable>();

            if (targetHealth == null)
                throw new System.NullReferenceException("The target must have an IDamageable interface");
            
            yield return new WaitForSeconds(_delayBeforeAttack);
            
            while (IsTargetFarAway() == false)
            {
                targetHealth.TakeDamage(Specification.Damage);
                Animator.SetTrigger(_attackKey);
                
                yield return waitAfterAttack;
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