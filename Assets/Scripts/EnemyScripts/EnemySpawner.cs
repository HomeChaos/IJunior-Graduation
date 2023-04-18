using System;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpecifications _specifications;
        [SerializeField] private GameObject[] _enemyPrefab;
        [SerializeField] private Transform _target;

        private int index = 0;
        
        [ContextMenu("Spawn")]
        private void Spawn()
        {
            var GOenemy = Instantiate(_enemyPrefab[index], transform.position, Quaternion.identity);
            var enemy = GOenemy.GetComponent<Enemy>();
            enemy.Init(_target, _specifications);
            index++;
        }

        private void Start()
        {
            Spawn();
        }
    }
}