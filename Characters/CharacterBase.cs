using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JustRoguelite.Utility;
using JustRoguelite.Skills;

namespace JustRoguelite.Characters
{
    public enum CharacterType { BASE, PLAYER, ENEMY, NEUTRAL };

    internal class CharacterBase : IIdentifiable
    {
        private static uint _nextID;
        private uint _ID;
        public uint GetID() { return _ID; }
        public void SetID(uint ID) { _ID = ID; }

        private string _name;
        private CharacterStats _characterBaseStats;
        private CharacterStats _characterStats;
        private int _battleID;
        private CharacterType _characterType;
        private int _HP;
        private int _EXP;

        public List<SkillList> skillLists = new();

        public System.Func<CharacterBase, CharacterBase, Skill, bool> turnExecuted;

        public CharacterBase(string name, CharacterStats characterBaseStats) 
        {
            _nextID++;
            _ID = _nextID;

            _name = name;
            _characterBaseStats = characterBaseStats;
            _characterType = CharacterType.BASE;

            _characterStats = new();
            _characterStats.maxHP = _characterBaseStats.maxHP;
            _HP = (int)_characterBaseStats.maxHP;
            _characterStats.speed = _characterBaseStats.speed;
            _characterStats.physicalResistance = _characterBaseStats.physicalResistance;
            _characterStats.magicalResistance = _characterBaseStats.magicalResistance;
            _EXP = 0;

            turnExecuted += TurnExecutedLog;

            Logger.Instance().Info($"Initialized character [HP: {GetHP()}/{GetMaxHP()}]", "CharacterBase.CharacterBase()");
        }

        public string GetName() { return _name; }
        public int GetBattleID() { return _battleID; }
        public void SetBattleID(int ID) { _battleID = ID; }
        public CharacterType GetCharacterType() { return _characterType; }
        public int GetHP() { return _HP; }
        public float GetHPPercentage() { return (GetHP() / GetMaxHP()); }
        public bool IsAlive() { return GetHP() > 0; }
        public int GetMaxHP() { return (int)_characterStats.maxHP; }
        public int GetSpeed() { return _characterStats.speed; }

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
    }
}
