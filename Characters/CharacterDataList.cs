using JustRoguelite.Utility;

namespace JustRoguelite.Characters
{
    internal class CharacterDataList : IGameList<CharacterData>
    {
        private List<CharacterData> _characterDataList = new();

        public void Add(CharacterData item)
        {
            _characterDataList.Add(item);
        }

        public void Add(IEnumerable<CharacterData> items)
        {
            _characterDataList.AddRange(items);
        }

        public void DebugPrintList()
        {
            foreach (CharacterData cd in _characterDataList) 
                cd.DebugPrint();
        }

        public CharacterData[] GetAll()
        {
            return _characterDataList.ToArray();
        }

        public CharacterData? GetItem(int id)
        {
            for (int i = 0; i < _characterDataList.Count; ++i)
            {
                if (_characterDataList[i].id == id)
                    return _characterDataList[i];
            }

            return null;
        }

        public void Remove(CharacterData item)
        {
            _characterDataList.Remove(item);
        }

        public bool Remove(int id)
        {
            CharacterData? cd = GetItem(id);

            if (cd == null)
                return false;

            Remove(cd);
            return true;
        }
    }
}
