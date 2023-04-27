using System.Collections.Generic;
using Scripts.PlayerScripts;
using UnityEngine;

namespace Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameObject _heartTemplate;
        [SerializeField] private GameObject _conteiner;

        private readonly int _healthMultiplicity = 3;
        
        private List<Heart> _hearts;

        private void Start()
        {
            _hearts = new List<Heart>();
        }

        private void OnEnable()
        {
            _player.OnHealthChange += OnHealthChange;
        }

        private void OnDisable()
        {
            _player.OnHealthChange -= OnHealthChange;
        }

        private void OnHealthChange(int health)
        {
            if (health <= 0)
            {
                RemoveAllHearts();
                return;
            }

            int requiredNumberOfHearts = health / _healthMultiplicity + (health % _healthMultiplicity == 0 ? 0 : 1);
            
            if (requiredNumberOfHearts < _hearts.Count)
                RemoveHearts(requiredNumberOfHearts);
            else if (requiredNumberOfHearts > _hearts.Count)
                AddNewHeart(requiredNumberOfHearts);

            SetValueOfLastHeart(health);
        }

        private void RemoveAllHearts()
        {
            RemoveHearts(0);
        }

        private void RemoveHearts(int currentValue)
        {
            while (_hearts.Count != currentValue)
            {
                Heart heart = _hearts[^1];
                _hearts.Remove(heart);
                Destroy(heart.gameObject);
            }
        }

        private void AddNewHeart(int currentValue)
        {
            while (_hearts.Count < currentValue)
            {
                Heart newHeart = Instantiate(_heartTemplate, _conteiner.transform).GetComponent<Heart>();
                _hearts.Add(newHeart);
            }
        }

        private void SetValueOfLastHeart(int health)
        {
            int valueOfLastHeart = health % _healthMultiplicity;
            _hearts[^1].ChangeState((HeartState)valueOfLastHeart);
        }
    }
}