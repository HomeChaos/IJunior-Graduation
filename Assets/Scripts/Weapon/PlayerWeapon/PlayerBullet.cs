using Scripts.EnemyScripts;
using UnityEngine;

namespace Scripts.Weapon.PlayerWeapon
{
    public class PlayerBullet : Bullet
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out EnemyBase enemy))
            {
                enemy.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
            else if (other.GetComponent<Wall>())
            {
                gameObject.SetActive(false);
            }
        }
    }
}