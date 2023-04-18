using UnityEngine;

namespace Scripts.PlayerScripts
{
    public class Input : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private PlayerInput _input;

        private void OnEnable()
        {
            _input = new PlayerInput();
            _input.Enable();

            _input.Player.Move.performed += context => Movement();
            _input.Player.Move.canceled += context => Movement();
            
            _input.Player.Shoot.started += context => OnStartShoot();
            _input.Player.Shoot.canceled += context => OnEndShoot();
        }

        private void OnDisable()
        {
            _input.Player.Move.performed -= context => Movement();
            _input.Player.Move.canceled -= context => Movement();
            
            _input.Player.Shoot.started -= context => OnStartShoot();
            _input.Player.Shoot.canceled -= context => OnEndShoot();
        }

        private void OnStartShoot()
        {
            _player.SetShooting(isShooting:true);
        }

        private void OnEndShoot()
        {
            _player.SetShooting(isShooting:false);
        }

        private void Movement()
        {
            var direction = _input.Player.Move.ReadValue<Vector2>();
            _player.SetDirection(direction);
        }
    }
}