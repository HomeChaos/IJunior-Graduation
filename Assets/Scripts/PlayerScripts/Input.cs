using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

            _input.Player.Move.performed += Movement;
            _input.Player.Move.canceled += Movement;

            _input.Player.Shoot.started += OnStartShoot;
            _input.Player.Shoot.canceled += OnEndShoot;
        }

        private void OnDisable()
        {
            _input.Disable();
            
            _input.Player.Move.performed -= Movement;
            _input.Player.Move.canceled -= Movement;

            _input.Player.Shoot.started -= OnStartShoot;
            _input.Player.Shoot.canceled -= OnEndShoot;
        }

        private void OnStartShoot(InputAction.CallbackContext context)
        {
            _player.SetShooting(isShooting:true);
        }

        private void OnEndShoot(InputAction.CallbackContext context)
        {
            _player.SetShooting(isShooting: false);
        }

        private void Movement(InputAction.CallbackContext context)
        {
            var direction = _input.Player.Move.ReadValue<Vector2>();
            _player.SetDirection(direction);
        }
    }
}