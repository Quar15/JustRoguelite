using System;
using System.Collections.Generic;
using System.Text.Json;

using JustRoguelite.Utility;
using JustRoguelite.Skills;

namespace JustRoguelite.Characters
{
    public enum CharacterType { BASE, PLAYER, ENEMY, NEUTRAL };

    internal class CharacterBase : IIdentifiable
    {
        private static uint _nextID;
        protected uint _ID;
        public uint GetID() { return _ID; }
        public void SetID(uint ID) { _ID = ID; }

        protected string _name;
        protected string _description;
        protected CharacterStats _characterBaseStats;
        protected CharacterStats _characterStats;
        protected int _battleID;
        protected CharacterType _characterType;
        protected int _HP;
        protected int _EXP;

        public List<SkillList> skillLists = new();

        public Func<CharacterBase, CharacterBase, Skill, bool> turnExecuted;

        public CharacterBase(string name, string description, CharacterStats characterBaseStats)
        {
            _nextID++;
            _ID = _nextID;

            _name = name;
            _description = description;
            _characterBaseStats = characterBaseStats;
            _characterType = CharacterType.BASE;

            _characterStats = new(_characterBaseStats);
            _HP = (int)_characterBaseStats.maxHP;
            _EXP = 0;

            turnExecuted += TurnExecutedLog;

            Logger.Instance().Info($"Initialized character [HP: {GetHP()}/{GetMaxHP()}]", "CharacterBase.CharacterBase(...)");
        }

        public CharacterBase(CharacterData characterData)
        {
            _ID = characterData.id;
            _nextID = characterData.id + 1;

            _name = characterData.name;
            _description = characterData.description;

            _characterBaseStats = characterData.characterBaseStats;
            _characterStats = characterData.characterBaseStats;
            _HP = (int)_characterBaseStats.maxHP;
            _EXP = 0;

            _characterType = characterData.characterType;

            foreach (uint skillID in characterData.skillIDs)
            {
                // @TODO: use factory to create skill
            }

            foreach (uint itemID in characterData.itemIDs)
            {
                // @TODO: use factory to create item
            }

            turnExecuted += TurnExecutedLog;

            Logger.Instance().Info($"Initialized character [HP: {GetHP()}/{GetMaxHP()}]", "CharacterBase.CharacterBase(CharacterData)");
        }

        public string GetName() { return _name; }
        public string GetDescription() { return _description; }
        public int GetBattleID() { return _battleID; }
        public void SetBattleID(int ID) { _battleID = ID; }
        public CharacterType GetCharacterType() { return _characterType; }
        public void SetCharacterType(CharacterType characterType) { _characterType = characterType; }
        public int GetHP() { return _HP; }
        public float GetHPPercentage() { return (GetHP() / GetMaxHP()); }
        public bool IsAlive() { return GetHP() > 0; }
        public int GetMaxHP() { return (int)_characterStats.maxHP; }
        public int GetSpeed() { return _characterStats.speed; }

        public int GetPhysicalResistance() { return _characterStats.physicalResistance; }

        public int GetMagicalResistance() { return _characterStats.magicalResistance; }

        public void DebugLog(string? localization = null)
        {
            Logger.Instance().Info($"CharacterBase(\n\t\tID = {_ID}, BattleID = {GetBattleID()}, Name = '{GetName()}', CharacterType = {GetCharacterType()}, \n\t\tHP = {GetHP()}, MaxHP = {GetMaxHP()}, EXP = {_EXP}\n\t)", localization == null ? "CharacterBase.DebugLog()" : $"CharacterBase.DebugLog() -> {localization}");
        }

        public void DealDMGToSelf(int dmgValue, DamageType dmgType)
        {
            int calcDmgValue = dmgValue;
            switch (dmgType)
            {
                case DamageType.PHYSICAL:
                    calcDmgValue = (int)Math.Ceiling(Math.Max(0, dmgValue * (10f / (10 + _characterStats.physicalResistance))));
                    break;
                case DamageType.MAGIC:
                    calcDmgValue = (int)Math.Ceiling(Math.Max(0, dmgValue * (10f / (10 + _characterStats.magicalResistance))));
                    break;
            }

            _HP = Math.Clamp(GetHP() - calcDmgValue, 0, GetMaxHP());
            Logger.Instance().Info($"Got hit [HP: {GetHP():D2}/{GetMaxHP()} (-{dmgValue}, reduced to -{calcDmgValue})]", "CharacterBase.GetHit()");
        }

        public void Heal(int healValue, bool force = false)
        {
            if (GetHP() <= 0 && !force)
            {
                Logger.Instance().Warning($"NOT Healed [HP: {GetHP():D2}/{GetMaxHP()} (+{healValue} - reduced to +0)]", "CharacterBase.Heal()");
                return;
            }

            int calcHealValue = healValue;

            _HP = Math.Clamp(GetHP() + calcHealValue, 0, GetMaxHP());
            Logger.Instance().Info($"Healed [HP: {GetHP():D2}/{GetMaxHP()} (+{healValue}, reduced to {calcHealValue})]", "CharacterBase.Heal()");
        }

        public void AddEXP(int exp) { _EXP += exp; }

        public virtual bool ExecuteTurn()
        {
            Logger.Instance().Warning("You should override ExecuteTurn() function!", "CharacterBase.ExecuteTurn()");

            return true;
        }

        public bool TurnExecutedLog(CharacterBase castingCharacter, CharacterBase targetCharacter, Skill skill)
        {
            Logger.Instance().Info($"Executed turn - skill casted: {skill.name} ({castingCharacter} -> {targetCharacter})", "CharacterBase.TurnExecutedLog(...)");

            return true;
        }

        internal Dictionary<string, string> ToDict()
        {
            Dictionary<string, string> characterData = new();
            characterData.Add("id", _ID.ToString());
            characterData.Add("name", _name);
            characterData.Add("description", _description);
            characterData.Add("characterBaseStats", JsonSerializer.Serialize(_characterBaseStats));
            characterData.Add("characterStats", JsonSerializer.Serialize(_characterStats));
            characterData.Add("battleID", _battleID.ToString());
            characterData.Add("characterType", _characterType.ToString());
            characterData.Add("HP", _HP.ToString());
            characterData.Add("EXP", _EXP.ToString());

            return characterData;
        }

        internal CharacterBase(Dictionary<string, string> charDict)
        {
            // _ID = uint.Parse(charDict["id"]);
            _ID = charDict.ContainsKey("id") ? uint.Parse(charDict["id"]) : _nextID++;
            _name = charDict["name"];
            _description = charDict["description"];
            _characterBaseStats = JsonSerializer.Deserialize<CharacterStats>(charDict["characterBaseStats"])!;
            _characterStats = JsonSerializer.Deserialize<CharacterStats>(charDict["characterStats"])!;
            _battleID = int.Parse(charDict["battleID"]);
            _characterType = (CharacterType)Enum.Parse(typeof(CharacterType), charDict["characterType"]);
            _HP = int.Parse(charDict["HP"]);
            _EXP = int.Parse(charDict["EXP"]);

            turnExecuted += TurnExecutedLog;
        }
    }
}
