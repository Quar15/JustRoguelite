namespace JustRoguelite.Characters
{
    internal class CharacterStats
    {
        public uint maxHP;
        public sbyte speed;

        public sbyte physicalResistance;
        public sbyte magicalResistance;

        public CharacterStats(uint maxHP = 10, sbyte speed = 1, sbyte physicalResistance = 0, sbyte magicalResistance = 0) 
        {
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
    }
}
