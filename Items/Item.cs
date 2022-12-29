using System;

using JustRoguelite.Utility;

namespace JustRoguelite.Items
{
    public enum ItemType { USABLE, EQUIPABLE, QUEST };

    internal class Item : IIdentifiable
    {
        private static int _nextID;
        private int _ID;
        public int GetID() { return _ID; }
        public void SetID(int ID) { _ID = ID; }

        public string name;
        public string description;

        private ItemType _itemType = ItemType.USABLE;
        public ItemType ItemUsageType { get { return _itemType; } }

        public Item(string name = "Item", string description = "") 
        {
            _nextID++;
            _ID = _nextID;

            this.name = name;
            this.description = description;
        }

        public void DebugLog(string? localization = null) 
        {
            Logger.Instance().Info($"Skill(\n\t\tID = {_ID}, Name = '{name}', \n\t\tDescription = '{description}'\n\t)", localization == null ? "Item.DebugLog()" : $"Item.DebugLog() -> {localization}");
        }
    }
}
