using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Settings;
using UnityEngine;

namespace Scripts.Weapon.PlayerWeapon
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private GameObject _container;
        [SerializeField] private float _rateOfFire;
        [SerializeField] private AudioClip _shootClip;

        private Coroutine _currentCoroutine;
        private List<Bullet> _bullets;
        private float _timeUp;
        private AudioSource _source;
        private SoundSettings _soundSettings;

        private void Awake()
        {
            _bullets = new List<Bullet>();
            _soundSettings = SoundUtils.FindSoundSettings();
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
                    InstantiateBullet();
                    _source.PlayOneShot(_shootClip);
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