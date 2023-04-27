using System.Collections.Generic;
using System.Linq;
using Scripts.Settings;
using Scripts.Shop;
using TMPro;
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
            _healthItem = UpdatePrice(_playerData.HealthCount, ItemTypes.Health);
            _wandItem = UpdatePrice(_playerData.WandSpeed, ItemTypes.Wand);
            
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
            SetItemPrice(_healthItem, _healthText, _updateHealthButton);
            SetItemPrice(_wandItem, _wandText, _updateWandButton);

            _money.text = $"Money: {_playerData.Money}";
        }

        private void SetItemPrice(Item item, TMP_Text text, Button button)
        {
            if (item.NextValue != 0)
            {
                text.text = $"Current: {item.CurrentValue}\nNext: {item.NextValue}\nCost: {item.Price}";
            }
            else
            {
                text.text = $"Current: {_healthItem.CurrentValue}\nMax";
                button.enabled = false;
            }
        }

        private Item UpdatePrice(float currentValue, ItemTypes itemTypes)
        {
            GetNextPrice(itemTypes, currentValue, out float nextValue, out int nextPrice);
            
            return new Item(currentValue, nextValue, nextPrice);
        }

        private void GetNextPrice(ItemTypes itemTypes, float currentValue, out float nextValue, out int nextPrice)
        {
            nextValue = 0;
            nextPrice = 0;
            
            ShopItem assortment = _shopItems.Assortment.FirstOrDefault(x => x.ItemTypes == itemTypes);
            
            if (assortment == null)
            {
                throw new System.NullReferenceException("The store could not find the right product");
            }

            IReadOnlyList<PriceTag> prices = assortment.PricуTags;

            for (int i = 0; i < prices.Count; i++)
            {
                if (prices[i].Value == currentValue)
                {
                    int nextIndex = i + 1;
                    
                    if (nextIndex < prices.Count)
                    {
                        nextValue = prices[nextIndex].Value;
                        nextPrice = prices[nextIndex].Price;
                    }
                    
                    break;
                }
            }
        }

        private void BuyHealth()
        {
            if (_playerData.Money - _healthItem.Price >= 0)
            {
                _playerData.Money -= _healthItem.Price;
                _playerData.HealthCount = (int)_healthItem.NextValue;
                _healthItem = UpdatePrice(_playerData.HealthCount, ItemTypes.Health);
                ShowItemsPrice();
            }
        }

        private void BuyWand()
        {
            if (_playerData.Money - _wandItem.Price >= 0)
            {
                _playerData.Money -= _wandItem.Price;
                _playerData.WandSpeed = _wandItem.NextValue;
                _wandItem = UpdatePrice(_playerData.WandSpeed, ItemTypes.Wand);
                ShowItemsPrice();
            }
        }
    }

    public class Item
    {
        public Item(float currentValue, float nextValue, int price)
        {
            CurrentValue = currentValue;
            NextValue = nextValue;
            Price = price;
        }

        public float CurrentValue { get; private set; }
        public float NextValue { get; private set;}
        public int Price { get; private set;}
    }
}