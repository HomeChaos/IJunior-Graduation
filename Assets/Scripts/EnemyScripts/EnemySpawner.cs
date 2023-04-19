using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.PlayerScripts;
using Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.EnemyScripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private GameObject _spawnZone;
        [SerializeField] private EnemySpecifications _specifications;
        [SerializeField] private WaveSettings _wavesSettings;
        [SerializeField] private int _maxCountOfEnemy = 25;
        [SerializeField] private Transform _target;

        private List<Enemy> _pool;
        private int _currentWaveNumber = 0;
        private int _currentSpawned;

        private float minPositionX;
        private float maxPositionX;
        private float minPositionY;
        private float maxPositionY;
        

        [ContextMenu("! StartSpawn")]
        private void StartSpawn()
        {
            _pool = new List<Enemy>();

            var spawnZoneSize = _spawnZone.transform.localScale;
            minPositionX = -spawnZoneSize.x / 2f;
            maxPositionX = spawnZoneSize.x / 2f;
            minPositionY = -spawnZoneSize.y / 2;
            maxPositionY = spawnZoneSize.y / 2f;
            Debug.Log($"x: {minPositionX}| {maxPositionX}; y: {minPositionY}|{maxPositionY}");
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
                        if (_currentSpawned <= _maxCountOfEnemy)
                        {
                            InstantiateEnemy(pack.EnemyTemplate);
                            _currentSpawned++;
                            yield return new WaitForSeconds(0.3f);
                        }
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
            var typeOfNewEnemy = template.GetComponent<Enemy>().EnemyType;

            var newPositionX = Random.Range(minPositionX, maxPositionX);
            var newPositionY = Random.Range(minPositionY, maxPositionY);

            if (TryGetEnemy(typeOfNewEnemy, out Enemy oldEnemy))
            {
                oldEnemy.gameObject.SetActive(true);
                oldEnemy.Init(_target, _specifications, new Vector2(newPositionX, newPositionY));
                oldEnemy.OnDie += OnEnemyDying;
            }
            else
            {
                Enemy newEnemy = Instantiate(template, _container.transform).GetComponent<Enemy>();
                newEnemy.Init(_target, _specifications, new Vector2(newPositionX, newPositionY));
                newEnemy.OnDie += OnEnemyDying;
                _pool.Add(newEnemy);
            }
        }

        private bool TryGetEnemy(TypesOfEnemies enemyEnemyType, out Enemy enemy)
        { 
            enemy = _pool.FirstOrDefault(x => x.EnemyType == enemyEnemyType && x.gameObject.active == false);
            return enemy != null;
        }

        private void OnEnemyDying(Enemy enemy, int reward)
        {
            enemy.OnDie -= OnEnemyDying;
            _target.gameObject.GetComponent<Player>().Wallet.AddMoney(reward);
            _currentSpawned--;
        }
    }
}