using System;
using Scripts.EnemyScripts;
using UnityEngine;

namespace Scripts.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _damage = 1;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _direction;
        private float _correctionZAngle = 90;

        protected int Damage => _damage;

        public void SetNewDamage(int newDamage)
        {
            if (newDamage < 0)
                throw new ArgumentException(nameof(SetNewDamage));
            _damage = newDamage;
        }
        
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = 0;
        }

        public void Init(Vector3 transformPosition, Quaternion transformRotation)
        {
            transform.position = transformPosition;
            transform.rotation = transformRotation;
            transform.Rotate(0, 0, _correctionZAngle, Space.Self);
        }

        private void Update()
        {
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {

        }
    }
}