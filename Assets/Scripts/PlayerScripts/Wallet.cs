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
            OnMoneyChange?.Invoke(_money, 0);
        }

        public void Init(int money)
        {
            _money = money;
        }

        public void AddMoney(int value)
        {
            _money += value;
            OnMoneyChange?.Invoke(_money, value);
        }

        [ContextMenu("[!] Add money")]
        private void AddCheatMoney()
        {
            AddMoney(100_000);
        }
    }
}