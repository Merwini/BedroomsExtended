using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    class RoomDesireWorker_RoomSize : RoomDesireWorker
    {
        public RoomDesireWorker_RoomSize(RoomDesire parent) : base(parent)
        {
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            int minimumSize = parent.def.minimumQuantity;
            return RoomDesireDefOf.Space.Worker.GetScore(room) >= minimumSize;
        }
    }
}
