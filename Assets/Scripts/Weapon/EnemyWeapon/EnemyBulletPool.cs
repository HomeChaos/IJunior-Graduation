using UnityEngine;

namespace Scripts.Weapon.EnemyWeapon
{
    public class EnemyBulletPool : BulletPool
    {
        public static EnemyBulletPool Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void Shoot(Bullet bullet, Transform shootPoint, int damage)
        {
            InstantiateBullet(bullet, shootPoint, damage);
        }
    }
}