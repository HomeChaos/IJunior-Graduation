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

        public override void Init(Transform target, EnemySpecifications specifications, Vector2 newPosition)
        {
            base.Init(target, specifications, newPosition);
            _weapon.Init(target);
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
            var waitForSeconds = new WaitForSeconds(2f);
            var currentDistance = Vector3.Distance(Target.position, transform.position);

            yield return new WaitForSeconds(_delayBeforeAttack);
            
            while (_minDistance < currentDistance && currentDistance < _maxDistance)
            {
                LookToTarget();
                
                var bullet = Instantiate(_bullet, _container.transform).GetComponent<Bullet>();
                bullet.gameObject.SetActive(true);
                bullet.Init(_shootPoint.transform.position, _shootPoint.transform.rotation);
                bullet.SetNewDamage(Specification.Damage);
                
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