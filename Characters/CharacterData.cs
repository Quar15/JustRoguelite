using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRoguelite.Characters
{
    internal class CharacterData
    {
        public uint ID;
        public string name;
        public string description;
        public CharacterStats characterBaseStats;
        public CharacterStats characterStats;
        public CharacterType characterType;

        public List<uint> skillIDs = new();
        public List<uint> itemIDs = new();

        public CharacterData(uint ID, string name, string description, CharacterStats characterBaseStats, CharacterStats characterStats, CharacterType characterType) 
        {
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.characterBaseStats = characterBaseStats;
            this.characterStats = characterStats;
            this.characterType = characterType;
        }

        public CharacterData(uint ID, string name, string description, CharacterStats characterBaseStats, CharacterStats characterStats, CharacterType characterType, List<uint> skillIDs, List<uint> itemIDs)
        {
            this.ID = ID;
            this.name = name;
            this.description = description;
            this.characterBaseStats = characterBaseStats;
            this.characterStats = characterStats;
            this.characterType = characterType;

            if(skillIDs != null && skillIDs.Count() > 0)
                this.skillIDs = skillIDs;
            if(itemIDs != null && itemIDs.Count() > 0)
                this.itemIDs = itemIDs;
        }
    }
}
