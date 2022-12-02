using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptid.Logic
{
    [System.Serializable]
    public class UnitState
    {
        public string ownerId;
        public string unitId;
        public UnitType type;
        public int posX, posZ;
        public int health;
        public bool moved;
        public bool attacked;
        public int maxDmg = 1;
        public int minDmg = 2;
    }
}