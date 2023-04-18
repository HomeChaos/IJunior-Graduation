using System;
using System.Collections;
using UnityEngine;

namespace Scripts.PlayerWeapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private float _rateOfFire;

        private Coroutine _currentCoroutine;
        private float _timeUp;

        public void StartShoot()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(Shoot());
        }

        public void StopShoot()
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
        }

        private IEnumerator Shoot()
        {
            var waitForEndOfFrame = new WaitForEndOfFrame();
            bool isShooting = true;

            while (isShooting)
            {
                if (_timeUp <= Time.time)
                {
                    var bullet = Instantiate(_bullet, transform.position, transform.rotation);
                    bullet.GetComponent<Bullet>().Init(transform.rotation);

                    _timeUp = Time.time + _rateOfFire;
                }

                yield return waitForEndOfFrame;
            }
        }
    }
}