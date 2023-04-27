using Scripts.Components;
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
                IDamageable damageComponent = player;
                
                if (damageComponent == null)
                {
                    throw new System.NullReferenceException("The player must have an IDamageable interface");
                }
                
                damageComponent.TakeDamage(Damage);
                gameObject.SetActive(false);
            }
            else if (other.GetComponent<Wall>())
            {
                gameObject.SetActive(false);
            }
        }
    }
}