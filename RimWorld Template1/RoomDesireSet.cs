using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    class RoomDesireSet
    {
        Pawn pawn;
        int minimumDesiresMetPerTier = 3;
        int generatedDesiresPerTier = 5;

        private HashSet<Trait> traitCacheSet;
        private List<List<RoomDesire>> roomDesireListList = new List<List<RoomDesire>>
        {
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>()
        };

        public RoomDesireSet(Pawn pawn)
        {
            this.pawn = pawn;
            traitCacheSet = CacheTraits(pawn.story.traits.allTraits);
            //TODO change minimumDesiresMetPerTier if has matching trait
            //TODO same for generatedDesiresPerTier
        }

        public HashSet<Trait> CacheTraits(List<Trait> list)
        {
            HashSet<Trait> traitSet = new HashSet<Trait>();
            for (int i = 0; i < list.Count; i++)
            {
                traitSet.Add(list[i]);
            }
            return traitSet;
        }

        public void CheckIfTraitsChanged()
        {
            HashSet<Trait> currentTraits = CacheTraits(pawn.story.traits.allTraits);
            bool traitsMatch = true;
            if (traitCacheSet.Count == currentTraits.Count)
            {
                foreach (Trait trait in currentTraits)
                {
                    if (!traitCacheSet.Contains(trait))
                    {
                        traitsMatch = false;
                    }
                }
            }
            else
            {
                traitsMatch = false;
            }

            if (!traitsMatch)
            {
                HandleTraitMismatch(currentTraits);
            }

            //TODO patch pawn birthday to call this method
        }

        public void HandleTraitMismatch(HashSet<Trait> currentTraits)
        {
            List<Trait> missingTraits = new List<Trait>();
            List<Trait> addedTraits = new List<Trait>();

            foreach (Trait trait in currentTraits)
            {
                if (!traitCacheSet.Contains(trait))
                {
                    addedTraits.Add(trait);
                }
            }
            foreach (Trait trait in traitCacheSet)
            {
                if (!currentTraits.Contains(trait))
                {
                    missingTraits.Add(trait);
                }
            }

            //TODO remove Desires associated with missingTraits
            //TODO add Desires associated with addedTraits
            //TODO notify player of change
        }

        public int GetScoreStage(Room room)
        {
            int scoreStage = 0;
            bool previousTierMet = true;
            for (int i = 0; i < 5; i++)
            {
                int desiresMetForTier = 0;
                for (int j = 0; j < roomDesireListList[i].Count; j++)
                {
                    if (roomDesireListList[i][j].Met)
                    {
                        desiresMetForTier++;
                    }
                }
                if (desiresMetForTier >= minimumDesiresMetPerTier)
                {
                    scoreStage++;
                    continue;
                }
                else
                {
                    break;
                }

            }
            return scoreStage;
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
