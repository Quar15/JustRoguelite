using JustRoguelite.Utility;

namespace JustRoguelite.Items
{
    internal class ItemData
    {
        public uint id;
        public string name;
        public string description;
        public int value;
        public ItemType itemType;

        public ItemData(uint id, string name, string description, int value, ItemType itemType) 
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.value = value;
            this.itemType = itemType;
        }

        public void DebugPrint()
        {
            Logger.Instance().Info($"ItemData([{id}], {name}, {description}, {value}, {itemType})", "ItemData - DebugPrint()");
        }

        internal Dictionary<string, string> ToDict()
        {
            Dictionary<string, string> itemDataDict = new();
            itemDataDict.Add("id", id.ToString());
            itemDataDict.Add("name", name);
            itemDataDict.Add("description", description);
            itemDataDict.Add("value", value.ToString());
            itemDataDict.Add("itemType", itemType.ToString());

            return itemDataDict;
        }
    }
}
