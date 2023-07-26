using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    public class RoomDesireWorker_BeautyAvg : RoomDesireWorker
    {
        int minScoreStage;
        public RoomDesireWorker_BeautyAvg(RoomDesire parent) : base(parent)
        {
            minScoreStage = parent.desireTier + 1;
        }

        public override bool IsMet(Pawn pawn, Room room)
        {
            float score = room.GetStat(RoomDesireDefOf.Beauty);

            return score >= RoomDesireDefOf.Beauty.scoreStages[minScoreStage].minScore;
        }
    }
}
