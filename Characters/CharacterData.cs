using JustRoguelite.Utility;
using System.Text.Json;

namespace JustRoguelite.Characters
{
    internal class CharacterData
    {
        public uint id;
        public string name;
        public string description;
        public CharacterStats characterBaseStats;
        public CharacterType characterType;

        public List<uint> skillIDs = new();
        public List<uint> itemIDs = new();

        public CharacterData(uint ID, string name, string description, CharacterStats characterBaseStats, CharacterType characterType)
        {
            this.id = ID;
            this.name = name;
            this.description = description;
            this.characterBaseStats = characterBaseStats;
            this.characterType = characterType;
        }

        public CharacterData(uint ID, string name, string description, CharacterStats characterBaseStats, CharacterType characterType, List<uint> skillIDs, List<uint> itemIDs)
        {
            this.id = ID;
            this.name = name;
            this.description = description;
            this.characterBaseStats = characterBaseStats;
            this.characterType = characterType;

            if (skillIDs != null && skillIDs.Count() > 0)
                this.skillIDs = skillIDs;
            if (itemIDs != null && itemIDs.Count() > 0)
                this.itemIDs = itemIDs;
        }

        public void DebugPrint()
        {
            Logger.Instance().Info($"CharacterData([{id}], {name}, {description}, {characterType})", "CharacterData - DebugPrint()");
        }


        internal Dictionary<string, string> AsDict()
        {
            Dictionary<string, string> dict = new();

            dict.Add("Id", id.ToString());
            dict.Add("Name", name);
            dict.Add("Description", description);
            dict.Add("Base Stats ID", characterBaseStats.GetID().ToString());
            dict.Add("Character Type", characterType.ToString());

            return dict;
        }

        internal static CharacterData FromDict(Dictionary<string, string> characterDataDict, List<CharacterStats> characterStatsList)
        {
            uint id = uint.Parse(characterDataDict["Id"]);
            string name = characterDataDict["Name"];
            string description = characterDataDict["Description"];
            uint baseStatsID = uint.Parse(characterDataDict["Base Stats ID"]);
            CharacterType characterType = (CharacterType)System.Enum.Parse(typeof(CharacterType), characterDataDict["Character Type"]);

            CharacterStats? baseStats = characterStatsList.Find(x => x.GetID() == baseStatsID);
            if (baseStats == null)
            {
                baseStats = new CharacterStats();
            }

            return new CharacterData(id, name, description, baseStats, characterType);
        }
    }
}
