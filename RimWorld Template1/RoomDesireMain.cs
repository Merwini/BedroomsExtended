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
        static Dictionary<RoomDesireDef,RoomDesire> desiresDictionary = new Dictionary<RoomDesireDef, RoomDesire>();

        static List<HashSet<RoomDesire>> desiresByTier = new List<HashSet<RoomDesire>>
        {
            new HashSet<RoomDesire>(),
            new HashSet<RoomDesire>(),
            new HashSet<RoomDesire>(),
            new HashSet<RoomDesire>(),
            new HashSet<RoomDesire>()
        };

        internal static HashSet<TerrainDef> terrainHashSet = new HashSet<TerrainDef>();

        static RoomDesireMain()
        {
            FillTerrainHashSet();
            InstantiateAllDesires();
            FrontFillUpgrades();
            BackFillSatisfiers();
            //TODO fill upgradesFrom
            //TODO backfill satisfiers
        }

        //I think making a HashSet will save cycles in the long run. Also necessary due to implied defs created at runtime
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
                desiresDictionary.Add(rdd,rd);
                SortDesireTier(rd);
            }
        }

        static void SortDesireTier(RoomDesire rd)
        {
            switch (rd.desireTier)
            {
                case 1:
                    desiresByTier[0].Add(rd);
                    break;
                case 2:
                    desiresByTier[1].Add(rd);
                    break;
                case 3:
                    desiresByTier[2].Add(rd);
                    break;
                case 4:
                    desiresByTier[3].Add(rd);
                    break;
                case 5:
                    desiresByTier[4].Add(rd);
                    break;
            }
        }

        static void FrontFillUpgrades()
        {
            for (int i = 0; i < desiresByTier.Count; i++)
            {
                foreach (RoomDesire rd in desiresByTier[i])
                {
                    List<RoomDesireDef> rddList = rd.def.upgradesFrom;
                    if (rddList?.Count > 0)
                    {
                        for (int j = 0; j < rddList.Count; j++)
                        {
                            rd.upgradesFrom.Add(desiresDictionary.TryGetValue(rddList[j]));
                        }
                        foreach (RoomDesire rd2 in rd.upgradesFrom)
                        {
                            rd.upgradesFrom.UnionWith(rd2.upgradesFrom);
                        }
                    }
                }
            }
        }

        static void BackFillSatisfiers()
        {
            for (int i = 4; i <= 0; i--)
            {
                foreach (RoomDesire rd in desiresByTier[i])
                {
                    foreach (RoomDesire rd2 in rd.upgradesFrom)
                    {
                        rd.satisfyingThingsExpanded.UnionWith(rd2.satisfyingThingsExpanded);
                        rd.satisfyingTerrainsExpanded.UnionWith(rd2.satisfyingTerrainsExpanded);
                    }
                }
            }
        }
    }
}
