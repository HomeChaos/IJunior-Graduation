using System;
using System.Linq;
using Scripts.Settings;
using Scripts.Shop;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Button _updateHealthButton;
        [SerializeField] private TMP_Text _healthText;
        [SerializeField] private Button _updateWandButton;
        [SerializeField] private TMP_Text _wandText;
        [SerializeField] private TMP_Text _money;
        [SerializeField] private Button _exitButton;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private ShopItems _shopItems;

        [SerializeField] private Curtain _curtain;

        private Item _healthItem;
        private Item _wandItem;
        
        private delegate void LoadScene();

        private void Start()
        {
            _healthItem = UpdateHealthPrice();
            _wandItem = UpdateWandPrice();
            
            ShowItemsPrice();

            _updateHealthButton.onClick.AddListener(BuyHealth);
            _updateWandButton.onClick.AddListener(BuyWand);
            _exitButton.onClick.AddListener(Exit);
        }

        private void OnDisable()
        {
            _updateHealthButton.onClick.RemoveListener(BuyHealth);
            _updateWandButton.onClick.RemoveListener(BuyWand);
            _exitButton.onClick.RemoveListener(Exit);
        }

        private void Exit()
        {
            LoadScene loadMainMenu = () => IJunior.TypedScenes.MainMenu.Load();
            _curtain.AnimationOver += () => LoadNextScene(loadMainMenu);
            _curtain.ShowCurtain();
        }
        
        private void LoadNextScene(LoadScene loadScene)
        {
            _curtain.AnimationOver -= () => LoadNextScene(loadScene);
            loadScene();
        }

        private void ShowItemsPrice()
        {
            if (_healthItem.NextValue != 0)
            {
                _healthText.text =
                    $"Current: {_healthItem.CurrentValue}\nNext: {_healthItem.NextValue}\nCost: {_healthItem.Price}";
            }
            else
            {
                _healthText.text = $"Current: {_healthItem.CurrentValue}\nMax";
                _updateHealthButton.enabled = false;
            }
            
            if (_wandItem.NextValue != 0)
            {
                _wandText.text =
                    $"Current: {_wandItem.CurrentValue}\nNext: {_wandItem.NextValue}\nCost: {_wandItem.Price}";
            }
            else
            {
                _wandText.text = $"Current: {_wandItem.CurrentValue}\nMax";
                _updateWandButton.enabled = false;
            }
            
            _money.text = $"Money: {_playerData.Money}";
        }

        private Item UpdateHealthPrice()
        {
            var currentHealth = _playerData.HealthCount;
            GetNextPrice(ItemTypes.Health, currentHealth, out float nextHealth, out int nextPrice);

            return new Item(currentHealth, nextHealth, nextPrice);
        }

        private Item UpdateWandPrice()
        {
            var currentWand = _playerData.WandSpeed;
            GetNextPrice(ItemTypes.Wand, currentWand, out float nextWand, out int nextPrice);

            return new Item(currentWand, nextWand, nextPrice);
        }

        private void GetNextPrice(ItemTypes itemTypes, float currentValue, out float nextValue, out int nextPrice)
        {
            nextValue = 0;
            nextPrice = 0;
            
            var assortment = _shopItems.Assortment.FirstOrDefault(x => x.ItemTypes == itemTypes).Pricings;
            
            for (int i = 0; i < assortment.Length; i++)
            {
                if (assortment[i].Value == currentValue)
                {
                    int nextIndex = i + 1;
                    if (nextIndex < assortment.Length)
                    {
                        nextValue = assortment[nextIndex].Value;
                        nextPrice = assortment[nextIndex].Price;
                        break;
                    }
                }
            }
        }

        private void BuyHealth()
        {
            if (_playerData.Money - _healthItem.Price >= 0)
            {
                _playerData.Money -= _healthItem.Price;
                _playerData.HealthCount = (int)_healthItem.NextValue;
                _healthItem = UpdateHealthPrice();
                ShowItemsPrice();
            }
        }

        private void BuyWand()
        {
            if (_playerData.Money - _wandItem.Price >= 0)
            {
                _playerData.Money -= _wandItem.Price;
                _playerData.WandSpeed = _wandItem.NextValue;
                _wandItem = UpdateWandPrice();
                ShowItemsPrice();
            }
        }
    }

    public class Item
    {
        private float _currentValue;
        private float _nextValue;
        private int _price;

        public Item(float currentValue, float nextValue, int price)
        {
            _currentValue = currentValue;
            _nextValue = nextValue;
            _price = price;
        }

        public float CurrentValue => _currentValue;
        public float NextValue => _nextValue;
        public int Price => _price;
    }
}