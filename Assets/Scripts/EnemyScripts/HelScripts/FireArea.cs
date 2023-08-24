using System;
using System.Collections;
using Scripts.PlayerScripts;
using UnityEditor;
using UnityEngine;

namespace Scripts.EnemyScripts.HelScripts
{
    public class FireArea : MonoBehaviour
    {
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _distance = 3f;
        [SerializeField] private float _timer;
        [SerializeField] private float _timeToFade;

        [Space]
        [Header("Particle")]
        [SerializeField] private ParticleSystem _area;
        [SerializeField] private ParticleSystem _burst;

        private Transform _target;
        private Player _player;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, _distance);
        }

        public void StartTimer(Transform target)
        {
            _area.Play();
            _target = target;
            StartCoroutine(WaitTimer());
        }

        private IEnumerator WaitTimer()
        {
            yield return new WaitForSeconds(_timer);
            CheckDistanceToDamage();
        }

        private void CheckDistanceToDamage()
        {
            _burst.Play();
            _area.Stop();
            var distance = Vector3.Distance(_target.position, transform.position);
            
            if (distance <= _distance)
            {
                if (_player == null)
                {
                    _player = _target.gameObject.GetComponent<Player>();
                }
 
                _player.TakeDamage(_damage);
            }

            StartCoroutine(WaitFade());
        }
        
        private IEnumerator WaitFade()
        {
            yield return new WaitForSeconds(_timeToFade);
            gameObject.SetActive(false);
        }
    }
}