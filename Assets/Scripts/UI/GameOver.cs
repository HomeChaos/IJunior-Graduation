using Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

namespace Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exit;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Player _player;
        [SerializeField] private Image _background;
        [SerializeField] private float _time = 10f;
        [SerializeField] private Curtain _curtain;

        private readonly float _maxAlpha = 1;
        
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

        private void OnDestroy()
        {
            _tweener.Kill();
        }

        public void OnExitGame()
        {
            ExitGame();
        }

        private void OnDying()
        {
            _panel.SetActive(true);
            _tweener = _background.DOFade(_maxAlpha, _time);
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
    }
}