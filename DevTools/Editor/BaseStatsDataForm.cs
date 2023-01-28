using JustRoguelite.Characters;

namespace JustRoguelite.Devtools.Editor
{
    public class BaseStatsDataForm : DataForm
    {
        protected override string Header => "Base Stats Data";
        public BaseStatsDataForm()
        {
            AddField(new NumericInputField("Max Health"));
            AddField(new NumericInputField("Speed"));
            AddField(new NumericInputField("Physical Resistance"));
            AddField(new NumericInputField("Magical Resistance"));
        }

        internal void SetValues(CharacterStats stats)
        {
            fields[0].Value.Append(stats.maxHP);
            fields[1].Value.Append(stats.speed);
            fields[2].Value.Append(stats.physicalResistance);
            fields[3].Value.Append(stats.magicalResistance);
        }

        // public override Dictionary<string, string> GetValues()
        // {
        //     var baseDict = base.GetValues()!;

        //     baseDict["Character Type"] = baseDict["Character Type (0 - Player, 1 - Enemy, 2 - Neutral)"] switch
        //     {
        //         "0" => "PLAYER",
        //         "1" => "ENEMY",
        //         "2" => "NEUTRAL",
        //         _ => "BASE"
        //     };
        //     baseDict.Remove("Character Type (0 - Player, 1 - Enemy, 2 - Neutral)");
        //     return baseDict;
        // }
    }
}
