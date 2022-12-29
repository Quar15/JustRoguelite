using System;

using JustRoguelite.Utility;

namespace JustRoguelite.Items
{
    internal class Inventory : IGameList<Item>
    {
        private List<Item> _itemList = new();
        
        public Inventory() 
        {
            _itemList = new List<Item>();
        }

        public Inventory(List<Item> itemList)
        {
            _itemList.AddRange(itemList);
        }

        public void Add(Item item) { _itemList.Add(item); }
        public void Add(IEnumerable<Item> items) { _itemList.AddRange(items); }
        public void Remove(Item item) { _itemList.Remove(item);}
        public bool Remove(int id) 
        {
            Item? item = GetItem(id);

            return item == null ? false : _itemList.Remove(item);
        }

        public Item? GetItem(int itemID) 
        {
            foreach (Item item in _itemList)
            {
                if(item.GetID() == itemID) 
                    return item;
            }

            return null;
        }
        
        public Item[] GetAll() { return _itemList.ToArray(); }
        public List<Item> GetAllOfType(ItemType itemType) 
        {
            List<Item> itemsOfType = new();
            foreach (Item item in _itemList)
            {
                if (item.ItemUsageType == itemType)
                    itemsOfType.Add(item);
            }

            return itemsOfType;
        }

        public void DebugPrintList()
        {
            Logger.Instance().Info($"Inventory[{_itemList.Count}]:", "Inventory.DebugPrintList()");
            foreach (Item item in _itemList)
            {
                item.DebugLog();
            }
        }

    }
}
