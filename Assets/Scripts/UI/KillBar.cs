using Scripts.EnemyScripts;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class KillBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private EnemySpawner _enemySpawner;

        private void OnEnable()
        {
            _enemySpawner.OnKill += OnKillScoreChange;
        }

        private void OnDisable()
        {
            _enemySpawner.OnKill -= OnKillScoreChange;
        }

        private void OnKillScoreChange(int newValue)
        {
            _text.text = $"{newValue}";
        }
    }
}