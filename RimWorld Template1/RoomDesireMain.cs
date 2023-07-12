using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    [StaticConstructorOnStartup]
    class RoomDesireMain
    {
        static HashSet<RoomDesire> allDesiresSet = new HashSet<RoomDesire>();

        static HashSet<RoomDesire> tierOne = new HashSet<RoomDesire>();
        static HashSet<RoomDesire> tierTwo = new HashSet<RoomDesire>();
        static HashSet<RoomDesire> tierThree = new HashSet<RoomDesire>();
        static HashSet<RoomDesire> tierFour = new HashSet<RoomDesire>();
        static HashSet<RoomDesire> tierFive = new HashSet<RoomDesire>();

        static HashSet<TerrainDef> terrainHashSet = new HashSet<TerrainDef>();

        static RoomDesireMain()
        {
            FillTerrainHashSet();
            InstantiateAllDesires();
            //TODO fill upgradesFrom
            //TODO backfill satisfiers
        }

        //I think making a HashSet will save cycles in the long run
        internal static void FillTerrainHashSet()
        {
            foreach (TerrainDef td in DefDatabase<TerrainDef>.AllDefs)
            {
                terrainHashSet.Add(td);
            }
        }

        static void InstantiateAllDesires()
        {
            foreach (RoomDesireDef rdd in DefDatabase<RoomDesireDef>.AllDefsListForReading)
            {
                RoomDesire rd = new RoomDesire(rdd);
                allDesiresSet.Add(rd);
                SortDesireTier(rd);
            }
        }

        static void SortDesireTier(RoomDesire rd)
        {
            switch (rd.desireTier)
            {
                case 1:
                    tierOne.Add(rd);
                    break;
                case 2:
                    tierTwo.Add(rd);
                    break;
                case 3:
                    tierThree.Add(rd);
                    break;
                case 4:
                    tierFour.Add(rd);
                    break;
                case 5:
                    tierFive.Add(rd);
                    break;
            }
        }

        static void BackFillSatisfiers()
        {
            foreach (RoomDesire rd in tierFive)
            {
                foreach (RoomDesire rdu in rd.upgradesFrom)
            }
        }
    }
}
