using JustRoguelite.Utility;

namespace JustRoguelite.Skills
{
    internal class SkillDataList : IGameList<SkillData>
    {
        private List<SkillData> _skillDataList = new();

        public SkillDataList()
        {
        }

        public SkillDataList(List<SkillData> skillDataList)
        {
            _skillDataList = skillDataList;
        }

        public void Add(SkillData item)
        {
            _skillDataList.Add(item);
        }

        public void Add(IEnumerable<SkillData> items)
        {
            _skillDataList.AddRange(items);
        }

        public void DebugPrintList()
        {
            foreach (SkillData sd in _skillDataList)
            {
                sd.DebugPrint();
            }
        }

        public SkillData[] GetAll()
        {
            return _skillDataList.ToArray();
        }

        public SkillData? GetItem(int id)
        {
            for (int i = 0; i < _skillDataList.Count; ++i)
            {
                if (_skillDataList[i].id == id)
                    return _skillDataList[i];
            }

            return null;
        }

        public void Remove(SkillData item)
        {
            _skillDataList.Remove(item);
        }

        public bool Remove(int id)
        {
            SkillData? sd = GetItem(id);

            if (sd == null)
                return false;

            Remove(sd);
            return true;
        }
    }
}
