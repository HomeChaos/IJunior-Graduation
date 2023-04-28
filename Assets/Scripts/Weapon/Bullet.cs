using System;
using UnityEngine;

namespace Scripts.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletOwner _owner;
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _direction;
        private float _correctionZAngle = 90;
        private int _damage = 1;

        public BulletOwner Owner => _owner;

        protected int Damage => _damage;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = 0;
        }

        private void Update()
        {
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }

        public void SetNewDamage(int newDamage)
        {
            if (newDamage < 0)
                throw new ArgumentException(nameof(SetNewDamage));
            
            _damage = newDamage;
        }

        public void Init(Vector3 transformPosition, Quaternion transformRotation)
        {
            transform.position = transformPosition;
            transform.rotation = transformRotation;
            transform.Rotate(0, 0, _correctionZAngle, Space.Self);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {

        }
    }
}