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
            AddField(new NumericInputField("Damage Type (0 - Physical, 1 - Magical)"));
            AddField(new NumericInputField("Skill Type (0 - Attack, 1 - Spell, 2 - Defend, 3 - Special)"));
        }

        internal void SetValues(Skill skill, SkillType sType)
        {
            fields[0].Value.Append(skill.name);
            fields[1].Value.Append(skill.description);
            fields[2].Value.Append(skill.damageType == DamageType.PHYSICAL ? 0 : 1);
            fields[3].Value.Append(sType == SkillType.ATTACK ? 0 : sType == SkillType.SPELL ? 1 : sType == SkillType.DEFEND ? 2 : 3);
        }


        public override Dictionary<string, string> GetValues()
        {
            var baseDict = base.GetValues()!;

            baseDict["Damage Type"] = baseDict["Damage Type (0 - Physical, 1 - Magical)"] == "0" ? "PHYSICAL" : "MAGICAL";
            baseDict["Skill Type"] = baseDict["Skill Type (0 - Attack, 1 - Defend, 2 - Defend, 3 - Special)"] switch
            {
                "0" => "ATTACK",
                "1" => "SPELL",
                "2" => "DEFEND",
                "3" => "SPECIAL",
                _ => "ATTACK"
            };
            return baseDict;
        }
    }
}
