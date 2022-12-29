using System;

using JustRoguelite.Utility;

namespace JustRoguelite.Skills
{
    internal class SkillList : IGameList<Skill>
    {
        private List<Skill> _skillList = new();
        private SkillType _skillType;
        public SkillType SkillListType { get { return _skillType; } }

        public SkillList(SkillType skillType) 
        {
            _skillList = new List<Skill>();
            _skillType = skillType;
        }

        public SkillList(SkillType skillType, List<Skill> skills) 
        {
            _skillType = skillType;
            _skillList.AddRange(skills);
        }

        public void Add(Skill newSkill) { _skillList.Add(newSkill); }
        public void Add(IEnumerable<Skill> skills) { _skillList.AddRange(skills);}
        public void Remove(Skill s) { _skillList.Remove(s); }
        public bool Remove(int id) 
        {
            Skill? s = GetItem(id);
            return s == null ? false : _skillList.Remove(s);
        }

        public Skill? GetItem(int skillID) 
        {
            for(int i=0; i<_skillList.Count; ++i) 
            {
                if(_skillList[i].GetID() == skillID)
                    return _skillList[i];
            }

            return null;
        }

        public Skill[] GetAll() { return _skillList.ToArray(); }

        public void DebugPrintList() 
        {
            Logger.Instance().Info($"SkillList[{_skillList.Count}]:", "SkillList.DebugPrintList()");
            for (int i=0; i< _skillList.Count; ++i) 
            {
                _skillList[i].DebugLog();
            }
        }
    }
}
