
using JustRoguelite.Characters;
using JustRoguelite.Devtools.Terminal;

namespace JustRoguelite.Devtools.Editor
{
    public class CharacterDataForm : DataForm
    {
        protected override string Header => "Character Data";
        public CharacterDataForm()
        {
            AddField(new TextInputField("Name"));
            AddField(new TextInputField("Description"));
            AddField(new NumericInputField("Max Health"));
            AddField(new NumericInputField("Speed"));
            AddField(new NumericInputField("Physical Resistance"));
            AddField(new NumericInputField("Magical Resistance"));
            AddField(new NumericInputField("Character Type (0 - Player, 1 - Enemy, 2 - Neutral)"));
        }

        internal void SetValues(CharacterBase character)
        {
            fields[0].Value.Append(character.GetName());
            fields[1].Value.Append(character.GetDescription());
            fields[2].Value.Append(character.GetMaxHP());
            fields[3].Value.Append(character.GetSpeed());
            fields[4].Value.Append(character.GetPhysicalResistance());
            fields[5].Value.Append(character.GetMagicalResistance());
            fields[6].Value.Append(character.GetCharacterType() == CharacterType.PLAYER ? 0 : character.GetCharacterType() == CharacterType.ENEMY ? 1 : 2);
        }

        public override Dictionary<string, string> GetValues()
        {
            var baseDict = base.GetValues()!;

            baseDict["Character Type"] = baseDict["Character Type (0 - Player, 1 - Enemy, 2 - Neutral)"] switch
            {
                "0" => "PLAYER",
                "1" => "ENEMY",
                "2" => "NEUTRAL",
                _ => "BASE"
            };
            return baseDict;
        }
    }
}