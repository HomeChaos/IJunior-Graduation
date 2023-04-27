using UnityEngine;

namespace Scripts.Weapon.PlayerWeapon
{
    public class RotationWeapon : MouseRotator
    {
        private void Update()
        {
            float z = GetCorrectRotationZ();
            transform.rotation = Quaternion.Euler(0, 0, z);
        }

        private float GetCorrectRotationZ()
        {
            Vector2 mousePosition = GetMousePosition();
            float angle = Vector3.Angle(InitialVector, mousePosition) * GetCurrentSide();
            return angle;
        }
    }
}