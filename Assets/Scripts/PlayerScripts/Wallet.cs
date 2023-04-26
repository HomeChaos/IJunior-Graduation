using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.PlayerScripts
{
    public class Wallet : MonoBehaviour
    {
        private int _money;

        public event UnityAction<int, int> OnMoneyChange;

        public void Init(int money)
        {
            _money = money;
        }

        private void Start()
        {
            OnMoneyChange?.Invoke(_money,0);
        }

        public void AddMoney(int value)
        {
            _money += value;
            OnMoneyChange?.Invoke(_money, value);
        }
    }
}