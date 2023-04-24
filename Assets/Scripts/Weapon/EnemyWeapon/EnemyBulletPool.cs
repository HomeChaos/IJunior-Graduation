using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Weapon.EnemyWeapon
{
    public class EnemyBulletPool : MonoBehaviour
    {
        private List<Bullet> _bullets = new List<Bullet>();
        
        public void Shoot(Bullet bullet, Transform shootPoint, int damage)
        {
            Bullet newBullet;

            if (TryGetBullet(out newBullet))
            {
                newBullet.gameObject.SetActive(true);
            }
            else
            {
                newBullet = Instantiate(bullet, transform).GetComponent<Bullet>();
                _bullets.Add(newBullet);
                newBullet.SetNewDamage(damage);
            }
            
            newBullet.Init(shootPoint.transform.position, shootPoint.transform.rotation);
        }
        
        private bool TryGetBullet(out Bullet newBullet)
        {
            newBullet = _bullets.FirstOrDefault(x => x.gameObject.activeSelf == false);
            return newBullet != null;
        }
    }
}