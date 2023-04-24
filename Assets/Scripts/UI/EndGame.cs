using Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

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
        [SerializeField] private Animator _curtainAnimator;
        
        private readonly int ShowKey = Animator.StringToHash("Show");

        private Coroutine _coroutine;

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
            _background.DOFade(1, _time);
        }

        private void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void RestartGame()
        {
            _curtainAnimator.SetTrigger(ShowKey);
        }
    }
}