using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
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
        }

        private void OnDisable()
        {
            _input.Player.Move.performed -= context => Movement();
            _input.Player.Move.canceled -= context => Movement();
        }

        private void Movement()
        {
            var direction = _input.Player.Move.ReadValue<Vector2>();
            _player.SetDirection(direction);
        }
    }
}