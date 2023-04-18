using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    [CreateAssetMenu(fileName = "EnemySpecifications", menuName = "Enemy Specifications", order = 52)]
    public class EnemySpecifications : ScriptableObject
    {
        [SerializeField] private List<Specification> _specifications;

        public Specification GetSpecification(TypesOfEnemies enemies)
        {
            var a = _specifications.FirstOrDefault(x => x.Type == enemies);
            return a;
        }
    }

    [System.Serializable]
    public class Specification
    {
        [SerializeField] private TypesOfEnemies _type;
        [SerializeField] private string _name;
        [SerializeField] private int _health;
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _speedDamage;
        [SerializeField] private int _reward;
        
        public TypesOfEnemies Type => _type;
        public string Name => _name;
        public int Health => _health;
        public float Speed => _speed;
        public int Damage => _damage;
        public float SpeedDamage => _speedDamage;
        public int Reward => _reward;
    }
}