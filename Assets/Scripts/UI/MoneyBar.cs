using System;
using System.Collections;
using Scripts.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MoneyBar : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Wallet wallet;
        [SerializeField] private Image _coin;
        [SerializeField] private float _timeOfAnimation = 0.5f;
        [SerializeField] private Sprite[] _coinSprites;

        private Coroutine _coroutine;
        
        private void OnEnable()
        {
            wallet.OnMoneyChange += OnMoneyChange;
        }

        private void OnDisable()
        {
            wallet.OnMoneyChange -= OnMoneyChange;
        }

        private void OnMoneyChange(int currentMoney, int reward)
        {
            _text.text = $"{currentMoney}: +{reward}";

            if (_coroutine == null)
                _coroutine = StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            var waitForSeconds = new WaitForSeconds(_timeOfAnimation);
            int currentSprite = 0;

            while (currentSprite < _coinSprites.Length)
            {
                _coin.sprite = _coinSprites[currentSprite];
                currentSprite++;

                yield return waitForSeconds;
            }
            
            _coin.sprite = _coinSprites[0];

            _coroutine = null;
        }
    }
}