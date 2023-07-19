using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    class RoomDesireWorker_Thing : RoomDesireWorker
    {
        public RoomDesireWorker_Thing(RoomDesire parent) : base(parent)
        {
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            List<Thing> things = room.ContainedAndAdjacentThings;
            HashSet<ThingDef> satisfyingThings = parent.satisfyingThingsExpanded;
            int minQuantity = parent.def.minimumQuantity;
            if (minQuantity <= 0)
                minQuantity = 1;
            int foundQuantity = 0;

            QualityCategory minimumQuality = parent.def.minimumQuality;

            foreach (Thing thing in things)
            {
                if (satisfyingThings.Contains(thing.def))
                {
                    if (thing.TryGetComp<CompQuality>() is CompQuality compQuality)
                    {
                        if (compQuality.Quality >= minimumQuality)
                        {
                            foundQuantity++;
                        }
                    }
                    else
                    {
                        foundQuantity++;
                    }
                }
            }

            return foundQuantity >= minQuantity;
        }
    }
}
