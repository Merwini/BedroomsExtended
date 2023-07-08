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

        public void ActivateTraitDesires()
        {
            //select as many trait-associated desires as possible for each desire tier
        }

        public void ActivateGeneDesires()
        {
            if (ModsConfig.BiotechActive)
                return;

            //select as many gene-associated desires as possible for each desire tier
            //will require BioTech
            //maybe this takes priority over Trait desires?
        }

        public void ActivateIdeoDesires()
        {
            if (!ModsConfig.IdeologyActive)
                return;

            //select as many ideologion-associated desires as possible for each desire tier
            //will require Ideology
        }

        public void ActivateTitleDesires()
        {
            if (!ModsConfig.RoyaltyActive)
                return;

            //select as many royalty title-associated desires as possible for each desire tier
            //will require royalty
            //higher titles will probably raise the desire slots for each tier
        }

        public void ActivateGenericDesires()
        {
            //fill remaining desire slots for each desire tier with generic desires
        }
    }
}
