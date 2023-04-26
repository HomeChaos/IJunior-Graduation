using System;
using System.Collections;
using Scripts.Settings;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _shop;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;
        [SerializeField] private SettingsPanel _settingsPanel;
        [SerializeField] private Curtain _curtain;
        
        private delegate void LoadScene();
        
        private void OnEnable()
        {
            _start.onClick.AddListener(OnStartGame);
            _shop.onClick.AddListener(OnShop);
            _settings.onClick.AddListener(OnSettings);
            _exit.onClick.AddListener(OnExit);
        }

        private void OnDisable()
        {
            _start.onClick.RemoveListener(OnStartGame);
            _shop.onClick.RemoveListener(OnShop);
            _settings.onClick.RemoveListener(OnSettings);
            _exit.onClick.RemoveListener(OnExit);
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