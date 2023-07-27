﻿using System;
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
        internal static Dictionary<RoomDesireDef,RoomDesire> desiresDictionary = new Dictionary<RoomDesireDef, RoomDesire>();

        internal static List<HashSet<RoomDesire>> desiresByTier = new List<HashSet<RoomDesire>>
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
            //TODO handle incompatiblewith
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
            List<RoomDesireDef> allDesireDefs = DefDatabase<RoomDesireDef>.AllDefsListForReading;
            foreach (RoomDesireDef rdd in allDesireDefs)
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

        //First attempt fails due to exception: Collection was modified
        /*
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
        */

        static void FrontFillUpgrades()
        {
            for (int i = 0; i < desiresByTier.Count; i++)
            {
                foreach (RoomDesire rd in desiresByTier[i])
                {
                    List<RoomDesireDef> rddList = rd.def.upgradesFrom;
                    if (rddList?.Count > 0)
                    {
                        // Create a separate collection to store the items to add to upgradesFrom
                        HashSet<RoomDesire> upgradesToAdd = new HashSet<RoomDesire>();
                        for (int j = 0; j < rddList.Count; j++)
                        {
                            upgradesToAdd.Add(desiresDictionary.TryGetValue(rddList[j]));
                        }

                        // Add the items from upgradesToAdd to the upgradesFrom set
                        foreach (RoomDesire rd2 in upgradesToAdd)
                        {
                            rd.upgradesFrom.Add(rd2);
                        }

                        // After adding the immediate upgrades, add all upgrades from the upgradesFrom sets of those upgrades
                        foreach (RoomDesire rd2 in upgradesToAdd)
                        {
                            rd.upgradesFrom.UnionWith(rd2.upgradesFrom);
                        }
                    }
                }
            }
        }

        static void BackFillSatisfiers()
        {
            for (int i = 4; i >= 0; i--)
            {
                foreach (RoomDesire rd in desiresByTier[i])
                {
                    foreach (RoomDesire rd2 in rd.upgradesFrom)
                    {
                        rd2.satisfyingThingsExpanded.UnionWith(rd.satisfyingThingsExpanded);
                        rd2.satisfyingTerrainsExpanded.UnionWith(rd.satisfyingTerrainsExpanded);
                    }
                }
            }
        }
    }
}
