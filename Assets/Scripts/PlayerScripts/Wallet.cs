using Scripts.Utils;
using UnityEngine;

namespace Scripts.PlayerScripts
{
    public class Wallet : MonoBehaviour
    {
        private int _money;

        public void AddMoney(int value)
        {
            _money += value;
            ConsoleTools.LogSuccess($"Money: {_money}");
        }
    }
}