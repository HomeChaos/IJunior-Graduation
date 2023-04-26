using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Settings
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [Space(10f)] 
        [SerializeField] private float _timeToShow;
        [SerializeField] private Slider _soundSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Button _save;
        [SerializeField] private Button _reset;
        [SerializeField] private GameObject _block;

        [SerializeField] private CanvasGroup _canvasGroup;
        
        private Tweener _tweener;


        public void Show()
        {
            _block.SetActive(true);
            _save.enabled = true;
            _reset.enabled = true;
            _soundSlider.enabled = true;
            _sfxSlider.enabled = true;
            _tweener = _canvasGroup.DOFade(1, _timeToShow);
            
        }

        private void Start()
        {
            _soundSlider.onValueChanged.AddListener(SoundVolumeCahnged);
            _sfxSlider.onValueChanged.AddListener(SfxVolumeChanged);
            _save.onClick.AddListener(Save);
            _reset.onClick.AddListener(ResetPlayerData);

            SetCurrentValueInSlider();
        }

        private void SetCurrentValueInSlider()
        {
            _soundSlider.value = _playerData.SoundVolume;
            _sfxSlider.value = _playerData.SfxVolume;
        }

        private void ResetPlayerData()
        {
            _playerData.ResetPlayerData();
            SetCurrentValueInSlider();
        }

        private void Save()
        {
            _tweener.Kill();
            _tweener = _canvasGroup.DOFade(0, _timeToShow);
            _save.enabled = false;
            _reset.enabled = false;
            _soundSlider.enabled = false;
            _sfxSlider.enabled = false;
            _block.SetActive(false);
        }

        private void SoundVolumeCahnged(float arg0)
        {
            _playerData.SoundVolume = _soundSlider.value;
        }

        private void SfxVolumeChanged(float arg0)
        {
            _playerData.SfxVolume = _sfxSlider.value;
        }

        private void OnDisable()
        {
            _soundSlider.onValueChanged.RemoveListener(SoundVolumeCahnged);
            _sfxSlider.onValueChanged.RemoveListener(SfxVolumeChanged);
            _save.onClick.RemoveListener(Save);
            _reset.onClick.RemoveListener(ResetPlayerData);
            _tweener.Kill();
        }
    }
}