using Scripts.UI;
using UnityEngine;

namespace Scripts.PlayerScripts
{
    public class GameExit : MonoBehaviour
    {
        [SerializeField] private GameOver _gameOver;
        
        private PlayerInput _input;

        private void OnEnable()
        {
            _input = new PlayerInput();
            _input.Enable();
            
            _input.Player.Exit.performed += context => OnExit();
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Exit.performed -= context => OnExit();
        }

        private void OnExit()
        {
            _gameOver.OnExitGame();
        }
    }
}