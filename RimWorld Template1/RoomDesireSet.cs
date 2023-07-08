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

        public HashSet<Trait> traitCacheSet;

        public RoomDesireSet(Pawn pawn)
        {
            this.pawn = pawn;
            traitCacheSet = CacheTraits(pawn.story.traits.allTraits);
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
    }
}
