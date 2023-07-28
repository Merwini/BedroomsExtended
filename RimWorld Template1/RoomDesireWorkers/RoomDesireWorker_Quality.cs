using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    public class RoomDesireWorker_Quality : RoomDesireWorker
    {
        public RoomDesireWorker_Quality(RoomDesire parent) : base(parent)
        {
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            CompPersonalizedBedroom comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            List<Thing> thingsInRoom = comp.thingsInRoomCache;
            QualityCategory minimumQuality = parent.def.minimumQuality;
            for (int i = 0; i < thingsInRoom.Count; i++)
            {
                if (thingsInRoom[i].TryGetComp<CompQuality>() is CompQuality compQuality)
                {
                    if (compQuality.Quality < minimumQuality)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
