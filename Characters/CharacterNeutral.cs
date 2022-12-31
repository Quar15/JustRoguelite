using System;
using JustRoguelite.Utility;

namespace JustRoguelite.Characters
{
    internal class CharacterNeutral : CharacterBase
    {
        public CharacterNeutral(string name, string description, CharacterStats characterBaseStats) : base(name, description, characterBaseStats)
        {
            SetCharacterType(CharacterType.NEUTRAL);
        }

        public override bool ExecuteTurn()
        {
            Logger.Instance().Info("Executing NPC Turn", "CharacterNeutral.ExecuteTurn()");
            return true;
        }
    }
}
