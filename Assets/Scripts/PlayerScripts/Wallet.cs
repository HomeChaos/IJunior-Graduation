using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.PlayerScripts
{
    public class Wallet : MonoBehaviour
    {
        private int _money;

        public event UnityAction<int, int> OnMoneyChange;

        private void Start()
        {
            OnMoneyChange?.Invoke(0,0);
        }

        public void AddMoney(int value)
        {
            _money += value;
            OnMoneyChange?.Invoke(_money, value);
        }
    }
}