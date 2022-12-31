using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
