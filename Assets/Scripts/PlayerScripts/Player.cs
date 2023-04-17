using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        private Vector2 _currentDirection;

        public void SetDirection(Vector2 direction)
        {
            _currentDirection = direction;
        }

        private void Awake()
        {
            
        }

        private void Update()
        {
            //transform.Translate(_currentDirection * _speed * Time.deltaTime);
            //_currentDirection = Vector2.zero;
        }

        private void FixedUpdate()
        {
            transform.Translate(_currentDirection * _speed * Time.deltaTime);
        }
    }
}