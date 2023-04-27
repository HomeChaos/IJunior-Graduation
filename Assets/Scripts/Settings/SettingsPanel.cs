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
        [SerializeField] private GameObject _panelForBlocking;
        [SerializeField] private CanvasGroup _canvasGroup;

        private readonly float _maxAlpha = 1;
        private readonly float _minAlpha = 0;
        
        private Tweener _tweener;

        private void Start()
        {
            _soundSlider.onValueChanged.AddListener(SoundVolumeChanged);
            _sfxSlider.onValueChanged.AddListener(SfxVolumeChanged);
            _save.onClick.AddListener(Save);
            _reset.onClick.AddListener(ResetPlayerData);

            SetCurrentValueInSlider();
        }

        private void OnDisable()
        {
            _soundSlider.onValueChanged.RemoveListener(SoundVolumeChanged);
            _sfxSlider.onValueChanged.RemoveListener(SfxVolumeChanged);
            _save.onClick.RemoveListener(Save);
            _reset.onClick.RemoveListener(ResetPlayerData);
            _tweener.Kill();
        }

        public void Show()
        {
            SetActive(true);
            _tweener = _canvasGroup.DOFade(_maxAlpha, _timeToShow);
        }

        private void SetActive(bool value)
        {
            _panelForBlocking.SetActive(value);
            _save.enabled = value;
            _reset.enabled = value;
            _soundSlider.enabled = value;
            _sfxSlider.enabled = value;
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
            _tweener = _canvasGroup.DOFade(_minAlpha, _timeToShow);
            SetActive(false);
        }

        private void SoundVolumeChanged(float newValue)
        {
            _playerData.SoundVolume = newValue;
        }

        private void SfxVolumeChanged(float newValue)
        {
            _playerData.SfxVolume = newValue;
        }
    }
}