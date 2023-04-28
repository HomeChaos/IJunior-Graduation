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
        private SoundSettings _soundSettings;

        private void Awake()
        {
            _soundSettings = SoundSettings.Instance;
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _source.volume = _soundSettings.SfxVolume;
        }

        public void Init(float rate)
        {
            _rateOfFire = rate;
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