using System.Text;

using JustRoguelite.Skills;

namespace JustRoguelite.Devtools.Editor
{
    public class SkillDataForm : DataForm
    {
        protected override string Header => "Skill Data";
        public SkillDataForm()
        {
            AddField(new TextInputField("Name"));
            AddField(new TextInputField("Description"));
            AddField(new MultiInputField("Values"));
            AddField(new EnumInputField("Damage Type", typeof(DamageType)));
            AddField(new EnumInputField("Skill Type", typeof(SkillType)));
        }

        internal void SetValues(Skill skill, SkillType sType)
        {
            fields[0].Value.Append(skill.name);
            fields[1].Value.Append(skill.description);

            StringBuilder values = new();
            foreach (var value in skill.values)
            {
                values.Append(value);
                values.Append(" ");
            }
            if (values.Length > 0)
                values.Remove(values.Length - 1, 1);

            fields[2].Value.Append(values.ToString());
            fields[3].Value.Append(skill.damageType);
            fields[4].Value.Append(sType);
        }

        // public override Dictionary<string, string> GetValues()
        // {
        //     var baseDict = base.GetValues()!;

        //     baseDict["Damage Type"] = baseDict["Damage Type (0 - Physical, 1 - Magical)"] == "0" ? "PHYSICAL" : "MAGICAL";
        //     baseDict["Skill Type"] = baseDict["Skill Type (0 - Attack, 1 - Defend, 2 - Defend, 3 - Special)"] switch
        //     {
        //         "0" => "ATTACK",
        //         "1" => "SPELL",
        //         "2" => "DEFEND",
        //         "3" => "SPECIAL",
        //         _ => "ATTACK"
        //     };
        //     baseDict.Remove("Damage Type (0 - Physical, 1 - Magical)");
        //     baseDict.Remove("Skill Type (0 - Attack, 1 - Defend, 2 - Defend, 3 - Special)");
        //     return baseDict;
        // }
    }
}
