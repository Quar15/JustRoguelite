using JustRoguelite.Utility;

namespace JustRoguelite.Characters
{
    internal class CharacterPlayer : CharacterBase
    {
        public CharacterPlayer(string name, string description, CharacterStats characterBaseStats) : base(name, description, characterBaseStats)
        {
            SetCharacterType(CharacterType.PLAYER);
        }

        public override bool ExecuteTurn()
        {
            Logger.Instance().Info("Executing Player Turn", "CharacterPlayer.ExecuteTurn()");
            return true;
        }
    }
}
