using System;

using JustRoguelite.Utility;

namespace JustRoguelite.Items
{
    public enum ItemType { USABLE, EQUIPABLE, QUEST };

    internal class Item : IIdentifiable
    {
        private static uint _nextID;
        private uint _ID;
        public uint GetID() { return _ID; }
        public void SetID(uint ID) { _ID = ID; }

        public string name;
        public string description;
        public int value;

        private ItemType _itemType = ItemType.USABLE;
        public ItemType GetItemType() { return _itemType; }
        public void SetItemType(ItemType type) { _itemType = type; }

        public Item(string name = "Item", string description = "", int value = 0, ItemType itemType = ItemType.USABLE) 
        {
            _nextID++;
            _ID = _nextID;

            this.name = name;
            this.description = description;
            this.value = value;
            this._itemType = itemType;
        }

        public Item(ItemData itemData) 
        {
            _ID = itemData.id;
            _nextID = itemData.id + 1;
            this.name = itemData.name;
            this.description = itemData.description;
            this.value = itemData.value;
            this._itemType = itemData.itemType;
        }

        public void DebugLog(string? localization = null) 
        {
            Logger.Instance().Info($"Skill(\n\t\tID = {_ID}, Name = '{name}', \n\t\tDescription = '{description}'\n\t)", localization == null ? "Item.DebugLog()" : $"Item.DebugLog() -> {localization}");
        }

        internal Dictionary<string, string> ToDict()
        {
            Dictionary<string, string> itemDataDict = new();
            itemDataDict.Add("id", _ID.ToString());
            itemDataDict.Add("name", name);
            itemDataDict.Add("description", description);
            itemDataDict.Add("value", value.ToString());
            itemDataDict.Add("itemType", _itemType.ToString());

            return itemDataDict;
        }
    }
}
