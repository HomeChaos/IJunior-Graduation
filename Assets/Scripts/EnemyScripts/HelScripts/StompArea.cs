using System.Collections;
using Scripts.PlayerScripts;
using UnityEngine;

namespace Scripts.EnemyScripts.HelScripts
{
    public class StompArea : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _distance = 3f;

        private bool _canDoDamage;
        private Transform _center;
        private Transform _target;
        private Player _player;
        private Coroutine _coroutine;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _distance);
        }

        public void Init(Transform center, Transform target)
        {
            _center = center;
            _target = target;
        }

        public void StartAttack()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _canDoDamage = true;
            _coroutine = StartCoroutine(CheckDistanceToDamage());
        }

        public void StopAttack()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = null;
        }

        private IEnumerator CheckDistanceToDamage()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            
            while (_canDoDamage)
            {
                var distance = Vector3.Distance(_target.position, _center.position);

                if (distance <= _distance)
                {
                    if (_player == null)
                    {
                        _player = _target.gameObject.GetComponent<Player>();
                    }

                    _player.TakeDamage(_damage);
                    _canDoDamage = false;
                }

                yield return waitForEndOfFrame;
            }
        }
    }
}