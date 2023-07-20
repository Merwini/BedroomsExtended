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
        internal int minimumDesiresMetPerTier = 3;
        internal int generatedDesiresPerTier = 5;

        private HashSet<Trait> traitCacheSet;
        private List<List<RoomDesire>> roomDesireListList = new List<List<RoomDesire>>
        {
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>()
        };
        HashSet<RoomDesire> roomDesireHashSet;

        public RoomDesireSet(Pawn pawn)
        {
            this.pawn = pawn;
            roomDesireHashSet = new HashSet<RoomDesire>();
            traitCacheSet = CacheTraits(pawn.story.traits.allTraits);
            //TODO change minimumDesiresMetPerTier if has matching trait
            //TODO same for generatedDesiresPerTier
            //TODO generate desires


        }

        public void GenerateDesires()
        {
            //todo adjust i to account for easygoing or picky pawns
            for (int i = 0; i < roomDesireListList.Count; i++)
            {
                int selectedDesires = 0;
                roomDesireListList[i].Union(ReturnDesiresFromUpgrades(i, generatedDesiresPerTier - roomDesireListList[i].Count));
                



            }
        }

        public List<RoomDesire> ReturnDesiresFromUpgrades(int desireTier, int desiresDesired)
        {
            List<RoomDesire> possibleDesires = new List<RoomDesire>();
            List<RoomDesire> selectedDesires = new List<RoomDesire>();
            int desiresSelected = 0;

            //attempt to select desires that are upgradesof existing ones
            foreach (RoomDesire desire in RoomDesireMain.desiresByTier[desireTier])
            {
                foreach (RoomDesire desire2 in desire.upgradesFrom)
                {
                    if (roomDesireHashSet.Contains(desire2))
                    {
                        possibleDesires.Add(desire);
                    }
                }
            }
            possibleDesires.Shuffle();
            for (int i = 0; i < possibleDesires.Count; i++)
            {
                if (desiresSelected == desiresDesired)
                    break;

                if (TrySelectDesire(possibleDesires[i]))
                {
                    selectedDesires.Add(possibleDesires[i]);
                    desiresSelected++;
                }
            }
            return selectedDesires;
        }

        public bool TrySelectDesire(RoomDesire desire)
        {
            bool selected = false;
            foreach (RoomDesire desire2 in roomDesireHashSet)
            {
                if (!desire2.incompatibleWith.Contains(desire))
                {
                    return roomDesireHashSet.Add(desire);
                }
            }
            return selected;
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
            for (int i = 0; i < 5; i++)
            {
                int desiresMetForTier = 0;
                for (int j = 0; j < roomDesireListList[i].Count; j++)
                {
                    if (roomDesireListList[i][j].IsMet(pawn, room))
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

        public List<Dictionary<RoomDesire, bool>> GetDesiresMetCache(Room room)
        {
            //TODO 
            List<Dictionary<RoomDesire, bool>> dictList = new List<Dictionary<RoomDesire, bool>>
            {
                new Dictionary<RoomDesire, bool>(),
                new Dictionary<RoomDesire, bool>(),
                new Dictionary<RoomDesire, bool>(),
                new Dictionary<RoomDesire, bool>(),
                new Dictionary<RoomDesire, bool>()
            };
            for (int i = 0; i < roomDesireListList.Count; i++)
            {
                foreach (RoomDesire rd in roomDesireListList[i])
                {
                    dictList[i].Add(rd, rd.IsMet(pawn,room));
                }
            }

            return dictList;
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
