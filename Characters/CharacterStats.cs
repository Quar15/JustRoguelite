using JustRoguelite.Utility;
using JustRoguelite.Devtools.Editor;

namespace JustRoguelite.Characters
{
    internal class CharacterStats : IIdentifiable
    {
        private uint _id;
        private static uint _nextID;

        public uint maxHP;
        public sbyte speed;

        public sbyte physicalResistance;
        public sbyte magicalResistance;

        public CharacterStats(uint maxHP = 10, sbyte speed = 1, sbyte physicalResistance = 0, sbyte magicalResistance = 0, uint? ID = null)
        {
            this._id = ID == null ? _nextID++ : (uint)ID;
            this.maxHP = maxHP;
            this.speed = speed;
            this.physicalResistance = physicalResistance;
            this.magicalResistance = magicalResistance;
        }

        public CharacterStats(CharacterStats characterStats)
        {
            this.maxHP = characterStats.maxHP;
            this.speed = characterStats.speed;
            this.physicalResistance = characterStats.physicalResistance;
            this.magicalResistance = characterStats.magicalResistance;
        }

        public Dictionary<string, string> AsDict()
        {
            Dictionary<string, string> dict = new();
            dict.Add("ID", _id.ToString());
            dict.Add("Max Health", maxHP.ToString());
            dict.Add("Speed", speed.ToString());
            dict.Add("Physical Resistance", physicalResistance.ToString());
            dict.Add("Magical Resistance", magicalResistance.ToString());
            return dict;
        }

        internal static CharacterStats FromDict(Dictionary<string, string> dict)
        {
            CharacterStats stats = new();
            stats.SetID(uint.Parse(dict["ID"]));
            stats.maxHP = uint.Parse(dict["Max Health"]);
            stats.speed = sbyte.Parse(dict["Speed"]);
            stats.physicalResistance = sbyte.Parse(dict["Physical Resistance"]);
            stats.magicalResistance = sbyte.Parse(dict["Magical Resistance"]);

            stats.SetNextID(stats.GetID());
            return stats;
        }

        static public uint NextID()
        {
            return _nextID++;
        }

        public void SetNextID(uint ID)
        {
            _nextID = ID + 1;
        }

        public void SetID(uint ID)
        {
            _id = ID;
        }

        public uint GetID()
        {
            return _id;
        }

        public void DebugLog(string? localization)
        {
            Logger.Instance().Info($"CharacterStats:\n\t[ID: {_id}] [Max HP: {maxHP}] [Speed: {speed}] [Physical Resistance: {physicalResistance}] [Magical Resistance: {magicalResistance}]", localization);
        }
    }
}
