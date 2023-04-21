using System;
using UnityEngine;


namespace Scripts
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private void Start()
        {
            
        }

        private void Update()
        {
            var direction = _player.position - transform.position;
            Debug.Log($"1 direction: {direction}");

            direction = transform.position - _player.position;
            Debug.Log($"2 direction: {direction}");
        }
    }
}