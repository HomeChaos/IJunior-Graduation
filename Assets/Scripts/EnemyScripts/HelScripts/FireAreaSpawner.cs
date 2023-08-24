using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.EnemyScripts.HelScripts
{
    public class FireAreaSpawner : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _frequencyToSpawn;
        [SerializeField] private GameObject _areaTemplate;
        [SerializeField] private GameObject _container;

        private Transform _target;
        private Coroutine _coroutine;
        private List<FireArea> _pool;

        public void Init(Transform target)
        {
            _target = target;
            _pool = new List<FireArea>();
        }

        public void StartAttack()
        {
            _spriteRenderer.enabled = true;
            _coroutine = StartCoroutine(Spawn());
        }

        public void StopAttack()
        {
            _spriteRenderer.enabled = false;
            
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            _coroutine = null;
        }

        private IEnumerator Spawn()
        {
            bool isSpawnerWork = true;
            var waitForSeconds = new WaitForSeconds(_frequencyToSpawn);

            while (isSpawnerWork)
            {
                InstantiateFireArea();
                yield return waitForSeconds;
            }
        }

        private void InstantiateFireArea()
        {
            FireArea area;
            
            if (TryGetFireArea(out area))
            {
                area.gameObject.SetActive(true);
            }
            else
            {
                area = Instantiate(_areaTemplate).GetComponent<FireArea>();
                _pool.Add(area);
            }

            area.gameObject.transform.position = _target.position;
            area.StartTimer(_target);
        }

        private bool TryGetFireArea(out FireArea fireArea)
        {
            fireArea = _pool.FirstOrDefault(x => x.gameObject.activeInHierarchy == false);
            return fireArea != null;
        }
    }
}