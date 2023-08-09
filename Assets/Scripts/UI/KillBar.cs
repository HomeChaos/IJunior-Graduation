using System.Collections;
using Scripts.EnemyScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class KillBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Image _skull;
        [SerializeField] private float _timeOfAnimation = 0.5f;
        [SerializeField] private Sprite[] _skillSprites;

        private Coroutine _coroutine;
        
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
            
            if (_coroutine == null)
                _coroutine = StartCoroutine(StartAnimation());
        }
        
        private IEnumerator StartAnimation()
        {
            var waitForSeconds = new WaitForSeconds(_timeOfAnimation);
            int currentSprite = 0;

            while (currentSprite < _skillSprites.Length)
            {
                _skull.sprite = _skillSprites[currentSprite];
                currentSprite++;

                yield return waitForSeconds;
            }
            
            _skull.sprite = _skillSprites[0];

            _coroutine = null;
        }
    }
}