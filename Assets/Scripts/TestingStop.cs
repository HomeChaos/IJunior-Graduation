using System;
using Scripts.EnemyScripts;
using UnityEngine;

namespace Scripts
{
    public class TestingStop : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private void Start()
        {
            Invoke(nameof(StopEnemy), 10f);
        }

        private void StopEnemy()
        {
            _enemySpawner.FreezeEnemys();
            Invoke(nameof(StartEnemy), 10f);
        }

        private void StartEnemy()
        {
            _enemySpawner.UnFreezeEnemys();
        }
    }
}