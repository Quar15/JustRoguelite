using System;
using System.Collections.Generic;
using JustRoguelite.Utility;

namespace JustRoguelite.Characters
{
    internal class CharacterEnemy : CharacterBase
    {
        public CharacterEnemy(string name, string description, CharacterStats characterBaseStats) : base(name, description, characterBaseStats)
        {
            SetCharacterType(CharacterType.ENEMY);
        }

        public CharacterEnemy(CharacterData characterData) : base(characterData) { }

        public override bool ExecuteTurn()
        {
            Logger.Instance().Info("Executing Enemy Turn", "CharacterEnemy.ExecuteTurn()");
            turnExecuted.Invoke(this, this, new());
            return true;
        }
    }
}
