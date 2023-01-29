using JustRoguelite.Characters;
using JustRoguelite.Skills;
using JustRoguelite.Items;

namespace JustRoguelite.Utility
{
    internal class ObjectManager
    {
        private CharactersList _playerCharacters = new();
        public CharactersList PlayerCharacters { get { return _playerCharacters; } }
        private CharactersList _enemyCharacters { get; } = new();
        public CharactersList EnemyCharacters { get { return _enemyCharacters; } }
        private CharactersList _neutralCharacters { get; } = new();
        public CharactersList NeutralCharacters { get { return _neutralCharacters; } }

        public T? Instantiate<T>(CharacterData characterData) where T : CharacterBase
        {
            var c = Instantiate(characterData);
            if (c == null)
                return null;

            return (T)(object)c;
        }

        public CharacterBase? Instantiate(CharacterData characterData) 
        {
            switch (characterData.characterType) 
            {
                case CharacterType.PLAYER:
                    CharacterPlayer p = new(characterData);
                    _playerCharacters.Add(p);
                    Logger.Instance().Info($"Created CharacterPlayer (id = {characterData.id})", "ObjectManager.Instantiate(CharacterData, CharacterType)");
                    return p;

                case CharacterType.ENEMY:
                    CharacterEnemy e = new(characterData);
                    _enemyCharacters.Add(e);
                    Logger.Instance().Info($"Created CharacterEnemy (id = {characterData.id})", "ObjectManager.Instantiate(CharacterData, CharacterType)");
                    return e;

                case CharacterType.NEUTRAL:
                    CharacterNeutral c = new(characterData);
                    _neutralCharacters.Add(c);
                    Logger.Instance().Info($"Created CharacterNeutral (id = {characterData.id})", "ObjectManager.Instantiate(CharacterData, CharacterType)");
                    return c;

                default:
                    Logger.Instance().Error($"Wrong Character Type (id = {characterData.id})", "ObjectManager.Instantiate(CharacterData, CharacterType)");
                    return null;
            }
        }

        public CharacterBase? TryToFind(uint id, CharacterType characterType) 
        {
            switch (characterType)
            {
                case CharacterType.PLAYER:
                    return _playerCharacters.GetItem((int)id);
                case CharacterType.ENEMY:
                    return _enemyCharacters.GetItem((int)id);
                case CharacterType.NEUTRAL:
                    return _neutralCharacters.GetItem((int)id);
            }

            return null;
        }

        public CharacterBase? FindOrInstantiate(CharacterData characterData) 
        {
            CharacterBase? character = TryToFind(characterData.id, characterData.characterType);

            if (character == null)
                return Instantiate(characterData);

            return character;
        }

        public Skill Instantiate(SkillData skillData) 
        {
            Skill skill = new(skillData);

            return skill;
        }

        public Item Instantiate(ItemData itemData) 
        {
            Item item = new(itemData);

            return item;
        }
    }
}
