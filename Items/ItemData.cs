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

        internal Dictionary<string, string> AsDict()
        {
            Dictionary<string, string> dict = new();
            dict.Add("Id", id.ToString());
            dict.Add("Name", name);
            dict.Add("Description", description);
            dict.Add("Value", value.ToString());
            dict.Add("Item Type", itemType.ToString());
            return dict;
        }

        internal static ItemData FromDict(Dictionary<string, string> itemDataDict)
        {
            return new ItemData(
                uint.Parse(itemDataDict["Id"]),
                itemDataDict["Name"],
                itemDataDict["Description"],
                int.Parse(itemDataDict["Value"]),
                (ItemType)System.Enum.Parse(typeof(ItemType), itemDataDict["Item Type"])
            );
        }
    }
}
