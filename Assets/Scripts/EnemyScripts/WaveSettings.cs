using System.Collections.Generic;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    [CreateAssetMenu(fileName = "WavesSettings", menuName = "Enemys/Waves Settings", order = 52)]
    public class WaveSettings : ScriptableObject
    {
        [SerializeField] private List<Wave> _waves;

        public List<Wave> Waves => _waves;
    }

    [System.Serializable]
    public class Wave
    {
        [SerializeField] private List<Pack> _packs;
        [SerializeField] private float _delay;

        public List<Pack> Packs => _packs;
        public float Delay => _delay;
    }

    [System.Serializable]
    public class Pack
    {
        [SerializeField] private GameObject _enemyTemplate;
        [SerializeField] private int _count;

        public GameObject EnemyTemplate => _enemyTemplate;
        public int Count => _count;
    }
}