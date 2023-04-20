using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.PlayerScripts;
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

        private readonly float _delayBeforeSpawning = 0.3f;
        
        private List<Enemy> _pool;
        private int _currentWaveNumber = 0;
        private int _currentSpawned;

        private float minPositionX;
        private float maxPositionX;
        private float minPositionY;
        private float maxPositionY;


        private void Start()
        {
            StartSpawn();
        }

        [ContextMenu("! StartSpawn")]
        private void StartSpawn()
        {
            _pool = new List<Enemy>();

            var spawnZoneSize = _spawnZone.transform.localScale;
            minPositionX = -spawnZoneSize.x / 2f;
            maxPositionX = spawnZoneSize.x / 2f;
            minPositionY = -spawnZoneSize.y / 2;
            maxPositionY = spawnZoneSize.y / 2f;

            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            bool isSpawnerWork = true;
            var waitDelayBeforeSpawning = new WaitForSeconds(_delayBeforeSpawning);
            
            while (isSpawnerWork)
            {
                var waitForNextPack = new WaitForSeconds(_wavesSettings.Waves[_currentWaveNumber].DelayBetweenPackets);
                
                foreach (var pack in _wavesSettings.Waves[_currentWaveNumber].Packs)
                {
                    for (int i = 0; i < pack.Count; i++)
                    {
                        if (_currentSpawned <= _maxCountOfEnemy)
                        {
                            InstantiateEnemy(pack.EnemyTemplate);
                            _currentSpawned++;
                            yield return waitDelayBeforeSpawning;
                        }
                    }
                    
                    yield return waitForNextPack;
                }

                _currentWaveNumber++;
                
                if (_currentWaveNumber >= _wavesSettings.Waves.Count)
                {
                    _currentWaveNumber--;
                }
            }
        }

        private void InstantiateEnemy(GameObject template)
        {
            var typeOfNewEnemy = template.GetComponent<Enemy>().EnemyType;

            var newPositionX = Random.Range(minPositionX, maxPositionX);
            var newPositionY = Random.Range(minPositionY, maxPositionY);

            Enemy enemyForSpawn;
            
            if (TryGetEnemy(typeOfNewEnemy, out enemyForSpawn))
            {
                enemyForSpawn.gameObject.SetActive(true);
            }
            else
            {
                enemyForSpawn = Instantiate(template, _container.transform).GetComponent<Enemy>();
                _pool.Add(enemyForSpawn);
            }
            
            enemyForSpawn.Init(_target, _specifications, new Vector2(newPositionX, newPositionY));
            enemyForSpawn.OnDie += OnEnemyDying;
        }

        private bool TryGetEnemy(TypesOfEnemies enemyEnemyType, out Enemy enemy)
        { 
            enemy = _pool.FirstOrDefault(x => x.EnemyType == enemyEnemyType && x.gameObject.activeSelf == false);
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