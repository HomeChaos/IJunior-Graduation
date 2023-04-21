using UnityEngine;

namespace Scripts.Weapon.EnemyWeapon
{
    public class EnemyRotationWeapon : MonoBehaviour
    {
        private readonly int LeftSide = -1;
        private readonly int RightSide = 1;

        private Transform _target;
        private Vector2 _initialVector = Vector2.up;

        public void Init(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target != null)
            {
                float z = GetCorrectRotationZ();
                transform.rotation = Quaternion.Euler(0, 0, z);
            }
        }

        private float GetCorrectRotationZ()
        {
            float angle = Vector3.Angle(_initialVector, _target.position - transform.position) * GetCurrentSide();
            return angle;
        }

        private int GetCurrentSide()
        {
            return (_target.position - transform.position).x >= _initialVector.x ? LeftSide : RightSide;
        }
    }
}