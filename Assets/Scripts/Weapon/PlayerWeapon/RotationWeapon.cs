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
            MousePosition = GetMousePosition();
            float angle = Vector3.Angle(InitialVector, MousePosition) * GetCurrentSide();
            return angle;
        }
    }
}