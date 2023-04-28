using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Weapon
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        
        private List<Bullet> _bullets = new List<Bullet>();
        
        protected void InstantiateBullet(Bullet bullet, Transform shootPoint, int damage)
        {
            Bullet newBullet;

            if (TryGetBullet(bullet.Owner, out newBullet))
            {
                newBullet.gameObject.SetActive(true);
            }
            else
            {
                newBullet = Instantiate(bullet, _container.transform).GetComponent<Bullet>();
                _bullets.Add(newBullet);
                newBullet.SetNewDamage(damage);
            }
            
            newBullet.Init(shootPoint.transform.position, shootPoint.transform.rotation);
        }
        
        private bool TryGetBullet(BulletOwner owner, out Bullet newBullet)
        {
            newBullet = _bullets.FirstOrDefault(x => x.gameObject.activeSelf == false && x.Owner == owner);
            return newBullet != null;
        }
    }
}