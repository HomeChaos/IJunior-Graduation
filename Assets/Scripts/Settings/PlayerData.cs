using System;
using Scripts.PlayerScripts;
using Scripts.Utils;
using Scripts.Weapon.PlayerWeapon;
using UnityEngine;

namespace Scripts.Settings
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private SoundSettings _soundSettings;
        [SerializeField] private PlayerWeapon _playerWeapon;
        
        private const string SoundVolumeKey = "SoundVolume";
        private const string SfxVolumeKey = "SfxVolume";
        private const string WandSpeedKey = "WandSpeed";
        private const string MoneyKey = "MoneyKey";
        private const string HealthCountKey = "HealthCount";

        private const float _volumeDefault = 0.5f;
        private const float _wandSpeedDefault = 0.35f;
        private const float _minWandSpeed = 0.10f;
        private const float _maxWandSpeed = 0.35f;
        private const int _monetDefault = 0;
        private const int _healthCountDefault = 3;
        private const int _healthMultiplicity = 3;

        private float _soundVolume = 1f;
        private float _sfxVolume = 1f;
        private float _wandSpeed = 1f;
        private int _money = 0;
        private int _healthCount = 3;

        public float SoundVolume
        {
            get => _soundVolume;
            set
            {
                if (0 <= value && value <= 1)
                {
                    _soundVolume = value;
                    _soundSettings.Init(_soundVolume, _sfxVolume);
                }
                else
                {
                    throw new ArgumentException($"value error in {nameof(SoundVolume)}");
                }
            }
        }
        
        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                if (0 <= value && value <= 1)
                {
                    _sfxVolume = value;
                    _soundSettings.Init(_soundVolume, _sfxVolume);
                }
                else
                {
                    throw new ArgumentException($"value error in {nameof(SfxVolume)}");
                }
            }
        }
        
        public float WandSpeed
        {
            get => _wandSpeed;
            set
            {
                if (_minWandSpeed <= value && value <= _maxWandSpeed)
                    _wandSpeed = value;
                else
                    throw new ArgumentException($"value error in {nameof(WandSpeed)}");
            }
        }
        
        public int Money
        {
            get => _money;
            set
            {
                if (0 <= value)
                    _money = value;
                else
                    throw new ArgumentException($"value error in {nameof(Money)}");
            }
        }
        
        public int HealthCount
        {
            get => _healthCount;
            set
            {
                if (value % _healthMultiplicity == 0)
                    _healthCount = value;
                else
                    throw new ArgumentException($"value error in {nameof(HealthCount)}");
            }
        }

        private void SavePlayerData()
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, _soundVolume);
            PlayerPrefs.SetFloat(SfxVolumeKey, _sfxVolume);
            PlayerPrefs.SetFloat(WandSpeedKey, _wandSpeed);
            PlayerPrefs.SetInt(MoneyKey, _money);
            PlayerPrefs.SetInt(HealthCountKey, _healthCount);
            
            PlayerPrefs.Save();
        }

        private void LoadPlayerData()
        {
            _soundVolume = PlayerPrefs.HasKey(SoundVolumeKey) ? PlayerPrefs.GetFloat(SoundVolumeKey) : _volumeDefault;
            _sfxVolume = PlayerPrefs.HasKey(SfxVolumeKey) ? PlayerPrefs.GetFloat(SfxVolumeKey) : _volumeDefault;
            _wandSpeed = PlayerPrefs.HasKey(WandSpeedKey) ? PlayerPrefs.GetFloat(WandSpeedKey) : _wandSpeedDefault;
            _money = PlayerPrefs.HasKey(MoneyKey) ? PlayerPrefs.GetInt(MoneyKey) : _monetDefault;
            _healthCount = PlayerPrefs.HasKey(HealthCountKey) ? PlayerPrefs.GetInt(HealthCountKey) : _healthCountDefault;
        }

        [ContextMenu("Reset Player Data")]
        public void ResetPlayerData()
        {
            _soundVolume = _volumeDefault;
            _sfxVolume = _volumeDefault;
            _wandSpeed = _wandSpeedDefault;
            _money = _monetDefault;
            _healthCount = _healthCountDefault;
            
            SavePlayerData();
        }

        private void Awake()
        {
            LoadPlayerData();

            if (_player != null)
            {
                _player.Init(_healthCount);
            }
            
            if (_playerWeapon != null)
            {
                _playerWeapon.Init(_wandSpeed);
            }

            if (_wallet != null)
            {
                _wallet.Init(_money);
                _wallet.OnMoneyChange += OnMoneyChange;
            }
            
            _soundSettings.Init(_soundVolume, _sfxVolume);
        }

        private void OnMoneyChange(int currentMoney, int addMoney)
        {
            _money = currentMoney;
        }

        private void OnDisable()
        {
            SavePlayerData();
        }
    }
}