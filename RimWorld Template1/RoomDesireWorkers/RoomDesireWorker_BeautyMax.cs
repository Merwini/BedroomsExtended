using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    class RoomDesireWorker_BeautyMax : RoomDesireWorker
    {
        int minimumBeauty;
        public RoomDesireWorker_BeautyMax(RoomDesire parent) : base(parent)
        {
            minimumBeauty = parent.def.minimumQuantity;
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            bool isMet = false;
            CompPersonalizedBedroom comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            foreach (Thing thing in comp.thingsInRoomCache)
            {
                if (thing is Building building && building.GetStatValue(StatDefOf.Beauty) >= minimumBeauty)
                {
                    isMet = true;
                    break;
                }
            }

            return isMet;
        }
    }
}
