using System;
using Scripts.EnemyScripts;
using Scripts.PlayerScripts;
using UnityEngine;

namespace Scripts.PlayerWeapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _direction;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = 0;
        }

        public void Init(Quaternion transformRotation)
        {
            transform.Rotate(0, 0, 90, Space.Self);
        }

        private void Update()
        {
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(1);
            }
            else if (other.GetComponent<Wall>())
            {
                Destroy(gameObject);
            }
        }
    }
}