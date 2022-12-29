using System;
using JustRoguelite.Utility;

namespace JustRoguelite.Characters
{
    internal class CharactersList : IGameList<CharacterBase>
    {
        private List<CharacterBase> _characterList = new();

        public CharactersList()
        {
            _characterList = new List<CharacterBase>();
        }

        public CharactersList(List<CharacterBase> characters)
        {
            _characterList.AddRange(characters);
        }

        public void Add(CharacterBase newCharacter) { _characterList.Add(newCharacter); }
        public void Add(IEnumerable<CharacterBase> characters) { _characterList.AddRange(characters); }
        public void Remove(CharacterBase c) { _characterList.Remove(c); }
        public bool Remove(int id)
        {
            CharacterBase? s = GetItem(id);
            return s == null ? false : _characterList.Remove(s);
        }

        public CharacterBase? GetItem(int CharacterBaseID)
        {
            for (int i = 0; i < _characterList.Count; ++i)
            {
                if (_characterList[i].GetID() == CharacterBaseID)
                    return _characterList[i];
            }

            return null;
        }

        public CharacterBase[] GetAll() { return _characterList.ToArray(); }

        public void DebugPrintList()
        {
            Logger.Instance().Info($"CharactersList[{_characterList.Count}]:", "CharactersList.DebugPrintList()");
            for (int i = 0; i < _characterList.Count; ++i)
            {
                _characterList[i].DebugLog();
            }
        }
    }
}
