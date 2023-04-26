using Scripts.Shop;
using UnityEngine;

namespace Scripts.UI
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Shop", order = 52)]
    public class ShopItems : ScriptableObject
    {
        [SerializeField] private ShopItem[] _assortment;

        public ShopItem[] Assortment => _assortment;
    }

    [System.Serializable]
    public class ShopItem
    {
        [SerializeField] private ItemTypes _itemTypes;
        [SerializeField] private Pricing[] _pricings;

        public ItemTypes ItemTypes => _itemTypes;
        public Pricing[] Pricings => _pricings;
    }

    [System.Serializable]
    public class Pricing
    {
        [SerializeField] private float _value;
        [SerializeField] private int _price;

        public float Value => _value;
        public int Price => _price;
    }
}