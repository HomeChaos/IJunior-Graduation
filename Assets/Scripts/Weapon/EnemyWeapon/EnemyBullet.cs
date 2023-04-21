using Scripts.PlayerScripts;
using UnityEngine;

namespace Scripts.Weapon.EnemyWeapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBullet : Bullet
    {
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
            else if (other.GetComponent<Wall>())
            {
                gameObject.SetActive(false);
            }
        }
    }
}