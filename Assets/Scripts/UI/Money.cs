using System;
using Scripts.PlayerScripts;
using TMPro;
using UnityEngine;

namespace Scripts.UI
{
    public class Money : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Wallet wallet;

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
        }
    }
}