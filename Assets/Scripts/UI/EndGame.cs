using System;
using Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using IJunior.TypedScenes;

namespace Scripts.UI
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exit;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Player _player;
        [SerializeField] private Image _background;
        [SerializeField] private float _time = 10f;
        [SerializeField] private Curtain _curtain;

        private Coroutine _coroutine;
        private Tweener _tweener;

        private delegate void LoadScene();

        private void OnEnable()
        {
            _restart.onClick.AddListener(RestartGame);
            _exit.onClick.AddListener(ExitGame);
            _player.Dying += OnDying;
        }

        private void OnDisable()
        {
            _restart.onClick.RemoveListener(RestartGame);
            _exit.onClick.RemoveListener(ExitGame);
            _player.Dying -= OnDying;
        }

        private void OnDying()
        {
            _panel.SetActive(true);
            _tweener = _background.DOFade(1, _time);
        }

        private void ExitGame()
        {
            LoadScene loadMainMenu = () => IJunior.TypedScenes.MainMenu.Load();
            _curtain.AnimationOver += () => LoadNextScene(loadMainMenu);
            _curtain.ShowCurtain();
        }

        private void RestartGame()
        {
            LoadScene loadGame = () => IJunior.TypedScenes.Game.Load();
            _curtain.AnimationOver += () => LoadNextScene(loadGame);
            _curtain.ShowCurtain();
        }

        private void LoadNextScene(LoadScene loadScene)
        {
            _curtain.AnimationOver -= () => LoadNextScene(loadScene);
            loadScene();
        }

        private void OnDestroy()
        {
            _tweener.Kill();
        }
    }
}