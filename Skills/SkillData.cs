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
    }
}
