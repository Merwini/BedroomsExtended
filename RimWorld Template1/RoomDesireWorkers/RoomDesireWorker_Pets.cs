using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    public class RoomDesireWorker_Pets : RoomDesireWorker
    {
        public RoomDesireWorker_Pets(RoomDesire parent) : base(parent)
        {
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            CompPersonalizedBedroom comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            List<Thing> thingsInRoom = comp.thingsInRoomCache;
            int petsRequired = parent.def.minimumQuantity;
            int petsFound = 0;

            for (int i = 0; i < thingsInRoom.Count; i++)
            {
                Thing thing = thingsInRoom[i];

                if (thing is Building_Bed bed 
                    && bed.def.building.bed_humanlike == false
                    && bed.OwnersForReading.Count != 0)
                {
                    petsFound++;
                }
            }


            return petsFound >= petsRequired;
        }
    }
}