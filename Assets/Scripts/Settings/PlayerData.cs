using System;
using Scripts.PlayerScripts;
using Scripts.Weapon.PlayerWeapon;
using UnityEngine;

namespace Scripts.Settings
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private bool _isInitializeObjects = true;
        [SerializeField] private Player _player;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private PlayerWeapon _playerWeapon;
        
        private const string SoundVolumeKey = "SoundVolume";
        private const string SfxVolumeKey = "SfxVolume";
        private const string WandSpeedKey = "WandSpeed";
        private const string MoneyKey = "MoneyKey";
        private const string HealthCountKey = "HealthCount";

        private const float VolumeDefault = 0.5f;
        private const float WandSpeedDefault = 0.35f;
        private const float MinWandSpeed = 0.10f;
        private const float MaxWandSpeed = 0.35f;
        private const int MonetDefault = 0;
        private const int HealthCountDefault = 3;
        private const int HealthMultiplicity = 3;

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
                    SoundSettings.Instance.ChangeValue(_soundVolume, _sfxVolume);
                }
                else
                {
                    throw new ArgumentException($"Value error in {nameof(SoundVolume)}.");
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
                    SoundSettings.Instance.ChangeValue(_soundVolume, _sfxVolume);
                }
                else
                {
                    throw new ArgumentException($"Value error in {nameof(SfxVolume)}.");
                }
            }
        }
        
        public float WandSpeed
        {
            get => _wandSpeed;
            set
            {
                if (MinWandSpeed <= value && value <= MaxWandSpeed)
                    _wandSpeed = value;
                else
                    throw new ArgumentException($"Value error in {nameof(WandSpeed)}.");
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
                    throw new ArgumentException($"Value error in {nameof(Money)}.");
            }
        }
        
        public int HealthCount
        {
            get => _healthCount;
            set
            {
                if (value % HealthMultiplicity == 0)
                    _healthCount = value;
                else
                    throw new ArgumentException($"Value error in {nameof(HealthCount)}.");
            }
        }

        private void Awake()
        {
            LoadPlayerData();
            
            if (_isInitializeObjects)
                InitializeObjects();

            SoundSettings.Instance.ChangeValue(_soundVolume, _sfxVolume);
        }

        private void OnDisable()
        {
            SavePlayerData();
        }

        public void ResetPlayerData()
        {
            _soundVolume = VolumeDefault;
            _sfxVolume = VolumeDefault;
            _wandSpeed = WandSpeedDefault;
            _money = MonetDefault;
            _healthCount = HealthCountDefault;
            
            SavePlayerData();
        }

        private void InitializeObjects()
        {
            _player.Init(_healthCount);
            _playerWeapon.Init(_wandSpeed);    
            _wallet.Init(_money);
            _wallet.OnMoneyChange += OnMoneyChange;
            //
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
            _soundVolume = PlayerPrefs.HasKey(SoundVolumeKey) ? PlayerPrefs.GetFloat(SoundVolumeKey) : VolumeDefault;
            _sfxVolume = PlayerPrefs.HasKey(SfxVolumeKey) ? PlayerPrefs.GetFloat(SfxVolumeKey) : VolumeDefault;
            _wandSpeed = PlayerPrefs.HasKey(WandSpeedKey) ? PlayerPrefs.GetFloat(WandSpeedKey) : WandSpeedDefault;
            _money = PlayerPrefs.HasKey(MoneyKey) ? PlayerPrefs.GetInt(MoneyKey) : MonetDefault;
            _healthCount = PlayerPrefs.HasKey(HealthCountKey) ? PlayerPrefs.GetInt(HealthCountKey) : HealthCountDefault;
        }

        private void OnMoneyChange(int currentMoney, int addMoney)
        {
            _money = currentMoney;
        }
    }
}