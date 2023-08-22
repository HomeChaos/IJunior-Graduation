using UnityEngine;

namespace Scripts.EnemyScripts.HelScripts
{
    public class FireArea : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public void StartAttack()
        {
            _spriteRenderer.enabled = true;
        }

        public void StopAttack()
        {
            _spriteRenderer.enabled = false;
        }
    }
}