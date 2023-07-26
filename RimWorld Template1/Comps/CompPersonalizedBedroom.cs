using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    [Serializable]
    class CompPersonalizedBedroom : ThingComp
    {
        CompProperties_PersonalizedBedroom Props => props as CompProperties_PersonalizedBedroom;

        internal RoomDesireSet roomDesireSet;

        public int desireSlots = 5;

        public float cachedBedroomWealth = -1;
        public int cachedThoughtStage = -1;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            roomDesireSet = new RoomDesireSet(this.parent as Pawn);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            List<Gizmo> result = new List<Gizmo>();
            result.Add(new Gizmo_DesireTracker(this.parent as Pawn));
            return result;
        }

        public int ReturnThoughtStage(Building_Bed bed)
        {
            //only need to recalculate ThoughtStage, which is expensive, if the room wealth has changed i.e. room contents have changed
            //edge case of room contents changing w/o changing wealth seems insignifcant at this time
            Room room = bed.GetRoom();
            float currentBedroomWealth = room.GetStat(RoomStatDefOf.Wealth);
            if (currentBedroomWealth != cachedBedroomWealth)
            {
                cachedThoughtStage = roomDesireSet.GetScoreStage(room);
                cachedBedroomWealth = currentBedroomWealth;
            }
            return cachedThoughtStage;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look(ref roomDesireSet, "roomDesireSet", this.parent as Pawn);
            Scribe_Values.Look(ref desireSlots, "desireSlots", 5);
            Scribe_Values.Look(ref cachedBedroomWealth, "cachedBedroomWealth", -1);
            Scribe_Values.Look(ref cachedThoughtStage, "cachedThoughtStage", -1);
        }
    }
}
