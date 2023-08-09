using System.Collections;
using Scripts.Weapon;
using Scripts.Weapon.EnemyWeapon;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    public class EnemyMag : EnemyBase
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private float _minDistance = 4f;
        [SerializeField] private float _maxDistance = 10f;
        [SerializeField] private GameObject _shootPoint;
        [SerializeField] private EnemyWeapon _weapon;

        private readonly int _attackKey = Animator.StringToHash("Attack");

        private EnemyBulletPool _pool;

        public override void Init(Transform target, EnemySpecifications specifications, EnemySoundsComponent enemySounds, Vector2 newPosition)
        {
            base.Init(target, specifications, enemySounds, newPosition);
            
            _weapon.Init(target);
            _pool = EnemyBulletPool.Instance;
        }

        protected override IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();

            while (IsCorrectDistance() == false)
            {
                Vector3 direction = GetCorrectDirection();
                UpdateSpriteRender(direction);
                transform.Translate(direction.normalized * Specification.Speed * Time.deltaTime);
                yield return waitForEndOfFrame;
            }

            StartState(AttackTarget());
        }

        protected override IEnumerator AttackTarget()
        {
            LookToTarget();
            var waitForSeconds = new WaitForSeconds(Specification.SpeedDamage);

            while (IsCorrectDistance())
            {
                LookToTarget();
                _pool.Shoot(_bullet, _shootPoint.transform, Specification.Damage);
                Animator.SetTrigger(_attackKey);

                yield return waitForSeconds;
            }
            
            StartState(GoToTarget());
        }

        private bool IsCorrectDistance()
        {
            var currentDistance = Vector3.Distance(Target.position, transform.position);
            return _minDistance < currentDistance && currentDistance < _maxDistance;
        }

        private Vector3 GetCorrectDirection()
        {
            float currentDistance = Vector3.Distance(Target.position, transform.position);
            return currentDistance > _maxDistance ? Target.position - transform.position : transform.position - Target.position;
        }

        private void LookToTarget()
        {
            var duraction = Target.position - transform.position;
            UpdateSpriteRender(duraction);
        }
    }
}