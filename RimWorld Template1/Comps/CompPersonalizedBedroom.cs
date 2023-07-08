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

        public RoomDesireSet roomDesireSet;

        public HashSet<RoomDesireDef> possibleDesires;
        public HashSet<RoomDesireDef> activeDesires;


        public int desireSlots;

        public int cachedBedroomWealth = -1;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            roomDesireSet = new RoomDesireSet(this.parent as Pawn);
        }

        public int ReturnThoughtStage(Building_Bed bed)
        {
            int scoreStageIndex = 0;
            //logic
            return scoreStageIndex;
        }
}
