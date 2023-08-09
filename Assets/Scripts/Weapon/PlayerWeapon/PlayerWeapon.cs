using System.Collections;
using Scripts.Settings;
using UnityEngine;

namespace Scripts.Weapon.PlayerWeapon
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerWeapon : BulletPool
    {
        [SerializeField] private PlayerBullet _bullet;
        [SerializeField] private int _damage;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _rateOfFire;
        [SerializeField] private AudioClip _shootClip;

        private Coroutine _currentCoroutine;
        private float _timeUp;
        private AudioSource _source;

        public void Init(float rate)
        {
            _rateOfFire = rate;
            _source = GetComponent<AudioSource>();
            _source.volume = SoundSettings.Instance.SfxVolume;
        }

        public void StartShoot()
        {
            StopShoot();
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
                    InstantiateBullet(_bullet, _shootPoint, _damage);
                    _source.PlayOneShot(_shootClip);
                    _timeUp = Time.time + _rateOfFire;
                }

                yield return waitForEndOfFrame;
            }
        }
    }
}