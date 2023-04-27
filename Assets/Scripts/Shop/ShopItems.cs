using System.Collections.Generic;
using Scripts.Shop;
using UnityEngine;

namespace Scripts.UI
{
    [CreateAssetMenu(fileName = "ShopItems", menuName = "Shop", order = 52)]
    public class ShopItems : ScriptableObject
    {
        [SerializeField] private List<ShopItem> _assortment;

        public IReadOnlyList<ShopItem> Assortment => _assortment;
    }

    [System.Serializable]
    public class ShopItem
    {
        [SerializeField] private ItemTypes _itemTypes;
        [SerializeField] private List<PriceTag> _pricуTags;

        public ItemTypes ItemTypes => _itemTypes;
        public IReadOnlyList<PriceTag> PricуTags => _pricуTags;
    }

    [System.Serializable]
    public class PriceTag
    {
        [SerializeField] private float _value;
        [SerializeField] private int _price;

        public float Value => _value;
        public int Price => _price;
    }
}