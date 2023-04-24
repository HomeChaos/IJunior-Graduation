using System.Collections;
using Scripts.Weapon;
using Scripts.Weapon.EnemyWeapon;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    public class EnemyMag : EnemyBase
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private GameObject _container;
        [SerializeField] private float _minDistance = 4f;
        [SerializeField] private float _maxDistance = 10f;
        [SerializeField] private GameObject _shootPoint;
        [SerializeField] private EnemyRotationWeapon _weapon;

        private readonly int AttackKey = Animator.StringToHash("Attack");
        private readonly float _delayBeforeAttack = 0.4f;

        private EnemyBulletPool _pool;

        public override void Init(Transform target, EnemySpecifications specifications, Vector2 newPosition)
        {
            base.Init(target, specifications, newPosition);
            _weapon.Init(target);

            if (_pool == null)
                _pool = FindObjectOfType<EnemyBulletPool>();
        }

        protected override IEnumerator GoToTarget()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            bool isCorrectDistance = false;

            while (isCorrectDistance == false)
            {
                var currentDistance = Vector3.Distance(Target.position, transform.position);

                if (currentDistance < _minDistance || currentDistance > _maxDistance)
                {
                    Vector3 direction = currentDistance > _maxDistance
                        ? Target.position - transform.position
                        : transform.position - Target.position;

                    UpdateSpriteRender(direction);
                    transform.Translate(direction.normalized * Specification.Speed * Time.deltaTime);
                    yield return waitForEndOfFrame;
                }
                else
                {
                    isCorrectDistance = true;
                }
            }

            StartState(AttackTarget());
        }

        protected override IEnumerator AttackTarget()
        {
            LookToTarget();
            var waitForSeconds = new WaitForSeconds(Specification.SpeedDamage);
            var currentDistance = Vector3.Distance(Target.position, transform.position);

            yield return new WaitForSeconds(_delayBeforeAttack);
            
            while (_minDistance < currentDistance && currentDistance < _maxDistance)
            {
                LookToTarget();
                _pool.Shoot(_bullet, _shootPoint.transform, Specification.Damage);
                Animator.SetTrigger(AttackKey);

                yield return waitForSeconds;
                currentDistance = Vector3.Distance(Target.position, transform.position);
            }
            
            StartState(GoToTarget());
        }

        private void LookToTarget()
        {
            var duraction = Target.position - transform.position;
            UpdateSpriteRender(duraction);
        }
    }
}