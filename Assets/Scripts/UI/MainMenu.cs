using Scripts.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private Curtain _curtain;
        
        private delegate void LoadScene();
        
        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartGame);
            _shopButton.onClick.AddListener(OnShop);
            _settingsButton.onClick.AddListener(OnSettings);
            _exitButton.onClick.AddListener(OnExit);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartGame);
            _shopButton.onClick.RemoveListener(OnShop);
            _settingsButton.onClick.RemoveListener(OnSettings);
            _exitButton.onClick.RemoveListener(OnExit);
        }

        private void OnStartGame()
        {
            LoadScene loadGame = () => IJunior.TypedScenes.Game.Load();
            _curtain.AnimationOver += () => LoadNextScene(loadGame);
            _curtain.ShowCurtain();
        }

        private void OnShop()
        {
            LoadScene loadShop = () => IJunior.TypedScenes.Shop.Load();
            _curtain.AnimationOver += () => LoadNextScene(loadShop);
            _curtain.ShowCurtain();
        }

        private void OnSettings()
        {
            _settingsPanel.Show();
        }

        private void OnExit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        
        private void LoadNextScene(LoadScene loadScene)
        {
            _curtain.AnimationOver -= () => LoadNextScene(loadScene);
            loadScene();
        }
    }
}