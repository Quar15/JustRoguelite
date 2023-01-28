using System.Collections.Generic;

using JustRoguelite.Utility;

namespace JustRoguelite.Characters
{
    internal class CharacterPlayer : CharacterBase
    {
        public CharacterPlayer(string name, string description, CharacterStats characterBaseStats) : base(name, description, characterBaseStats)
        {
            SetCharacterType(CharacterType.PLAYER);
        }

        public CharacterPlayer(CharacterData characterData) : base(characterData) { }

        public override bool ExecuteTurn()
        {
            Logger.Instance().Info("Executing Player Turn", "CharacterPlayer.ExecuteTurn()");
            return true;
        }

        internal CharacterPlayer(Dictionary<string, string> charDict) : base(charDict) { }
    }
}
