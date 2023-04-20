using Scripts.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] private Button _restart;
        [SerializeField] private Button _exit;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Player _player;

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
            Time.timeScale = 0;
            _panel.SetActive(true);
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
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}