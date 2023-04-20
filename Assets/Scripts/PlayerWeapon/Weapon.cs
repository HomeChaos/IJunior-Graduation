using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.PlayerWeapon
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private GameObject _container;
        [SerializeField] private float _rateOfFire;

        private Coroutine _currentCoroutine;
        private List<Bullet> _bullets;
        private float _timeUp;

        private void Start()
        {
            _bullets = new List<Bullet>(); 
        }

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
                    InstantiateBullet();
                    _timeUp = Time.time + _rateOfFire;
                }

                yield return waitForEndOfFrame;
            }
        }

        private void InstantiateBullet()
        {
            Bullet newBullet;

            if (TryGetBullet(out newBullet))
            {
                newBullet.gameObject.SetActive(true);
            }
            else
            {
                newBullet = Instantiate(_bullet, _container.transform).GetComponent<Bullet>();
                _bullets.Add(newBullet);
            }
            
            newBullet.Init(_shootPoint.transform.position, _shootPoint.transform.rotation);
        }

        private bool TryGetBullet(out Bullet newBullet)
        {
            newBullet = _bullets.FirstOrDefault(x => x.gameObject.activeSelf == false);
            return newBullet != null;
        }
    }
}