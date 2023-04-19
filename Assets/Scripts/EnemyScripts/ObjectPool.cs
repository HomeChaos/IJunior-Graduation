using System.Collections.Generic;
using UnityEngine;

namespace Scripts.EnemyScripts
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        
        private List<GameObject> _pool = new List<GameObject>();
        
        
    }
}