using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRoguelite.Characters
{
    internal class CharacterStats
    {
        public int maxHP;
        public int speed;

        public int physicalResistance;
        public int magicalResistance;

        public CharacterStats(int maxHP = 10, int speed = 1, int physicalResistance = 0, int magicalResistance = 0) 
        {
            this.maxHP = maxHP;
            this.speed = speed;
            this.physicalResistance = physicalResistance;
            this.magicalResistance = magicalResistance;
        }
    }
}
