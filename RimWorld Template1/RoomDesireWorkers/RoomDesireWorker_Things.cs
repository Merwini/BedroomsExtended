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
            List<Thing> thingsInRoom = room.ContainedAndAdjacentThings;
            List<ThingRequirement> thingRequirements = parent.requiredThings;
            int requirementsTotal = thingRequirements.Count;
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
                        List<string> thingTags = thing.def.tradeTags;
                        for (int k = 0; k < thingTags.Count; k++)
                        {
                            if (tr.satisfyingTags.Contains(thingTags[k]))
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
                    }
                }
                if (quantityFound >= quantityNeeded)
                {
                    requirementsMet++;
                }
            }

            return requirementsMet == requirementsTotal;
        }
    }
}
