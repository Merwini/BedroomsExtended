using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    class CompPersonalizedBedroom : ThingComp
    {
        CompProperties_PersonalizedBedroom Props => props as CompProperties_PersonalizedBedroom;

        private RoomDesireSet roomDesireSet;

        private HashSet<RoomDesireDef> possibleDesires;
        private HashSet<RoomDesireDef> activeDesires;


        public int desireSlots;

        public float cachedBedroomWealth = -1;
        public int cachedThoughtStage = -1;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            roomDesireSet = new RoomDesireSet(this.parent as Pawn);
        }

        public int ReturnThoughtStage(Building_Bed bed)
        {
            //only need to recalculate ThoughtStage, which is expensive, if the room wealth has changed i.e. room contents have changed
            //edge case of room contents changing w/o changing wealth seems insignifcant at this time
            float currentBedroomWealth = bed.GetRoom().GetStat(RoomStatDefOf.Wealth);
            if (currentBedroomWealth != cachedBedroomWealth)
            {
                cachedThoughtStage = roomDesireSet.GetScoreStage(bed.GetRoom());
                cachedBedroomWealth = currentBedroomWealth;
            }
            return cachedThoughtStage;
        }
    }
}
