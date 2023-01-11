using JustRoguelite.Utility;

namespace JustRoguelite.Items
{
    internal class ItemDataList : IGameList<ItemData>
    {
        private List<ItemData> _itemDataList = new();

        public void Add(ItemData item)
        {
            _itemDataList.Add(item);
        }

        public void Add(IEnumerable<ItemData> items)
        {
            _itemDataList.AddRange(items);
        }

        public void DebugPrintList()
        {
            foreach (ItemData item in _itemDataList)
                item.DebugPrint();
        }

        public ItemData[] GetAll()
        {
            return _itemDataList.ToArray();
        }

        public ItemData? GetItem(int id)
        {
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                if (_itemDataList[i].id == id)
                    return _itemDataList[i];
            }

            return null;
        }

        public void Remove(ItemData item)
        {
            _itemDataList.Remove(item);
        }

        public bool Remove(int id)
        {
            ItemData? item = GetItem(id);

            if (item == null)
                return false;

            Remove(item);
            return true;
        }
    }
}
