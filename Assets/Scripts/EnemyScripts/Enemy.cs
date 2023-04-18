using System;
using Scripts.Components;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.EnemyScripts
{
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private TypesOfEnemies _type;
        [SerializeField] private Transform _target;
        
        [SerializeField] private string _name;
        [SerializeField] private int _health;
        [SerializeField] private float _speed;
        [SerializeField] private int _damage;
        [SerializeField] private float _speedDamage;
        [SerializeField] private int _cost;

        private Specification _specification;

        public void Init(Transform target, EnemySpecifications specifications)
        {
            
        }

        public void TakeDamage(int damage)
        {
           
        }

        private void GetSpecification(EnemySpecifications specifications)
        {
            
        }
    }
}