using System;
using System.Collections;
using Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpecifications _specifications;
        [SerializeField] private WaveSettings _wavesSettings;
        [SerializeField] private Transform _target;

        private int _currentWaveNumber = 0;
        private float _timeAfterLastSpawn = 0;

        [ContextMenu("! StartSpawn")]
        private void StartSpawn()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            bool isSpawnerWork = true;
            
            while (isSpawnerWork)
            {
                ConsoleTools.LogInfo($"Start new Wave: {_currentWaveNumber+1}");
                foreach (var pack in _wavesSettings.Waves[_currentWaveNumber].Packs)
                {
                    for (int i = 0; i < pack.Count; i++)
                    {
                        InstantiateEnemy(pack.EnemyTemplate);
                        yield return new WaitForSeconds(0.3f);
                    }
                    
                    yield return new WaitForSeconds(_wavesSettings.Waves[_currentWaveNumber].Delay);
                }

                _currentWaveNumber++;
                if (_currentWaveNumber >= _wavesSettings.Waves.Count)
                {
                    ConsoleTools.LogError("Spawner отработал! Сброс на последнюю волну");
                    _currentWaveNumber--;
                }
            }
        }

        private void InstantiateEnemy(GameObject template)
        {
            Enemy enemy = Instantiate(template, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.Init(_target, _specifications);
        }

    }
}