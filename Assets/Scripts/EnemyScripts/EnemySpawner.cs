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
        [SerializeField] private int _maxCountOfEnemy = 50;
        [SerializeField] private Transform _target;
        [SerializeField] private EnemySoundsComponent _enemySounds;

        private readonly float _delayBeforeSpawning = 0.3f;
        
        private List<EnemyBase> _pool;
        private Wallet _targetWallet;
        private int _currentWaveNumber = 0;
        private int _currentSpawned;
        private int _countOfKills = 0;
        private Coroutine _coroutine;

        private float _minPositionX;
        private float _maxPositionX;
        private float _minPositionY;
        private float _maxPositionY;

        public event Action<int> OnKill;

        private void Start()
        {
            _targetWallet = _target.GetComponent<Wallet>();
            _enemySounds.Init();
            
            if (_targetWallet == null)
                throw new System.NullReferenceException("The target must have a Wallet component");
            
            StartSpawn();
        }

        public void FreezeEnemys()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = null;
            
            foreach (var enemy in _pool)
            {
                if (enemy.gameObject.activeSelf)
                    enemy.Freeze();
            }
        }

        public void UnFreezeEnemys()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Spawn());
            
            foreach (var enemy in _pool)
            {
                if (enemy.gameObject.activeSelf)
                    enemy.UnFreeze();
            }
        }
        
        private void StartSpawn()
        {
            _pool = new List<EnemyBase>();

            Vector3 spawnZoneSize = _spawnZone.transform.localScale;
            _minPositionX = -spawnZoneSize.x / 2f;
            _maxPositionX = spawnZoneSize.x / 2f;
            _minPositionY = -spawnZoneSize.y / 2f;
            _maxPositionY = spawnZoneSize.y / 2f;

            _coroutine = StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            bool isSpawnerWork = true;
            var waitDelayBeforeSpawning = new WaitForSeconds(_delayBeforeSpawning);
            
            while (isSpawnerWork)
            {
                var waitForNextPack = new WaitForSeconds(_wavesSettings.Waves[_currentWaveNumber].DelayBetweenPackets);
                
                foreach (Pack pack in _wavesSettings.Waves[_currentWaveNumber].Packs)
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

                _currentWaveNumber = Mathf.Min(_wavesSettings.Waves.Count - 1, _currentWaveNumber + 1);
            }
        }

        private void InstantiateEnemy(GameObject template)
        {
            var typeOfNewEnemy = template.GetComponent<EnemyBase>().EnemyType;

            float newPositionX = Random.Range(_minPositionX, _maxPositionX);
            float newPositionY = Random.Range(_minPositionY, _maxPositionY);

            EnemyBase enemyForSpawn;
            
            if (TryGetEnemy(typeOfNewEnemy, out enemyForSpawn))
            {
                enemyForSpawn.gameObject.SetActive(true);
            }
            else
            {
                enemyForSpawn = Instantiate(template, _container.transform).GetComponent<EnemyBase>();
                _pool.Add(enemyForSpawn);
            }
            
            enemyForSpawn.Init(_target, _specifications, _enemySounds, new Vector2(newPositionX, newPositionY));
            enemyForSpawn.OnDie += OnEnemyDying;
        }

        private bool TryGetEnemy(TypesOfEnemies enemyType, out EnemyBase enemy)
        { 
            enemy = _pool.FirstOrDefault(x => x.EnemyType == enemyType && x.gameObject.activeSelf == false);
            return enemy != null;
        }

        private void OnEnemyDying(EnemyBase enemy, int reward)
        {
            enemy.OnDie -= OnEnemyDying;
            _countOfKills++;
            OnKill?.Invoke(_countOfKills);
            _targetWallet.AddMoney(reward);
            _currentSpawned--;
        }
    }
}