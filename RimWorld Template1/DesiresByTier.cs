using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    [StaticConstructorOnStartup]
    class DesiresByTier
    {
        static HashSet<RoomDesireDef> tierOne;
        static HashSet<RoomDesireDef> tierTwo;
        static HashSet<RoomDesireDef> tierThree;
        static HashSet<RoomDesireDef> tierFour;
        static HashSet<RoomDesireDef> tierFive;

        static DesiresByTier()
        {
            foreach (RoomDesireDef rdd in DefDatabase<RoomDesireDef>.AllDefsListForReading)
            {
                switch (rdd.desireTier)
                {
                    case 1:
                        tierOne.Add(rdd);
                        break;
                    case 2:
                        tierTwo.Add(rdd);
                        break;
                    case 3:
                        tierThree.Add(rdd);
                        break;
                    case 4:
                        tierFour.Add(rdd);
                        break;
                    case 5:
                        tierFive.Add(rdd);
                        break;
                }
            }
        }
    }
}
