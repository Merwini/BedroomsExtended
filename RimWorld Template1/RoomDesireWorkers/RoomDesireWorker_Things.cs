using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    class RoomDesireWorker_Things : RoomDesireWorker
    {
        public RoomDesireWorker_Things(RoomDesire parent) : base(parent)
        {
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            CompPersonalizedBedroom comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            List<Thing> thingsInRoom = comp.thingsInRoomCache;
            List<ThingRequirement> thingRequirements = parent.thingRequirements;
            int requirementsTotal = thingRequirements.Count;
            Log.Warning("requirementsTotal: " + requirementsTotal.ToString());
            int requirementsMet = 0;
            for (int i = 0; i < requirementsTotal; i++)
            {
                ThingRequirement tr = thingRequirements[i];
                bool tagMode = tr.satisfyingThingsExpanded.EnumerableNullOrEmpty<ThingDef>();
                int quantityNeeded = tr.quantityNeeded;
                int quantityFound = 0;
                QualityCategory minimumQuality = tr.minimumQuality;
                for (int j = 0; j < thingsInRoom.Count; j++)
                {
                    Thing thing = thingsInRoom[j];
                    if (!tagMode)
                    {
                        Log.Warning("Debug 1");
                        if (tr.satisfyingThingsExpanded.Contains(thing.def))
                        {
                            if (thing.TryGetComp<CompQuality>() is CompQuality compQuality)
                            {
                                if (compQuality.Quality >= minimumQuality)
                                {
                                    quantityFound++;
                                }
                            }
                            else
                            {
                                quantityFound++;
                            }
                        }
                    }
                    else
                    {
                        Log.Warning("Debug 2");
                        List<string> thingTags = thing.def.tradeTags;
                        for (int k = 0; k < thingTags.Count; k++)
                        {
                            if (tr.satisfyingTags.Contains(thingTags[k]))
                            {
                                if (thing.TryGetComp<CompQuality>() is CompQuality compQuality)
                                {
                                    if (compQuality.Quality >= minimumQuality)
                                    {
                                        Log.Warning("Debug 3");
                                        quantityFound++;
                                    }
                                }
                                else
                                {
                                    Log.Warning("Debug 4");
                                    quantityFound++;
                                }
                            }
                        }
                    }
                }
                if (quantityFound >= quantityNeeded)
                {
                    Log.Warning("Debug 5");
                    requirementsMet++;
                }
                Log.Warning("TagMode: " + tagMode.ToString());
            }
            return requirementsMet == requirementsTotal;
        }
    }
}
