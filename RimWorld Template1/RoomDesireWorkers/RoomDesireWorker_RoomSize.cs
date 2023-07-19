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
        int desireTier;
        public RoomDesireWorker_RoomSize(RoomDesire parent) : base(parent)
        {
            desireTier = parent.desireTier;
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            float score = room.GetStat(RoomDesireDefOf.Space);

            return score >= RoomDesireDefOf.Space.scoreStages[desireTier].minScore;
        }
    }
}
