using System.Text;
using JustRoguelite.Utility;

namespace JustRoguelite.Skills
{
    internal class SkillData
    {
        public uint id;
        public string name;
        public string description;
        public List<int> values = new();
        public DamageType damageType;

        public SkillType skillType;

        public SkillData(uint id, string name, string description, List<int> values, DamageType damageType, SkillType skillType)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.values = values;
            this.damageType = damageType;

            this.skillType = skillType;
        }

        public void DebugPrint()
        {
            Logger.Instance().Info($"SkillData([{id}], {name}, {description}, {damageType}, {skillType})", "SkillData - DebugPrint()");
        }

        internal Dictionary<string, string> AsDict()
        {
            Dictionary<string, string> dict = new();
            dict.Add("Id", id.ToString());
            dict.Add("Name", name);
            dict.Add("Description", description);
            dict.Add("Damage Type", damageType.ToString());

            var valuesStr = new StringBuilder();
            foreach (var value in values)
            {
                valuesStr.Append(value);
                valuesStr.Append(" ");
            }
            if (valuesStr.Length > 0)
                valuesStr.Remove(valuesStr.Length - 1, 1);

            dict.Add("Values", valuesStr.ToString());
            return dict;
        }

        internal static SkillData FromDict(Dictionary<string, string> skillDataDict)
        {
            var values = new List<int>();
            foreach (var value in skillDataDict["Values"].Split(' '))
            {
                values.Add(int.Parse(value));
            }

            return new SkillData(
                uint.Parse(skillDataDict["Id"]),
                skillDataDict["Name"],
                skillDataDict["Description"],
                values,
                (DamageType)System.Enum.Parse(typeof(DamageType), skillDataDict["Damage Type"]),
                (SkillType)System.Enum.Parse(typeof(SkillType), skillDataDict["Skill Type"])
            );
        }
    }
}
