using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptid.Shared
{
    public class RandomGenerator
    {
        private Random rand;

        public RandomGenerator()
        {
            rand = new Random(9999);
        }

        public int GenerateRandom(int min, int max)
        {
            return rand.Next(min, max);
        }
    }
}
