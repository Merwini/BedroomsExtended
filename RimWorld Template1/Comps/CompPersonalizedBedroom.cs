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

        public int desireSlots = 5;

        public float cachedBedroomWealth = -1;
        public int cachedThoughtStage = -1;

        internal int minimumDesiresMetPerTier = 3;
        internal int generatedDesiresPerTier = 5;

        internal Pawn pawn;

        internal bool desiresGenerated;

        private HashSet<Trait> traitCacheSet;

        private List<List<RoomDesire>> roomDesireListList = new List<List<RoomDesire>>
        {
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>(),
            new List<RoomDesire>()
        };

        HashSet<RoomDesire> roomDesireHashSet = new HashSet<RoomDesire>();

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            pawn = this.parent as Pawn;
        }

        public void DoLogic()
        {
            traitCacheSet = CacheTraits();
            //TODO change minimumDesiresMetPerTier if has matching trait
            //TODO same for generatedDesiresPerTier
            //TODO generate desires
            GenerateDesires();
            desiresGenerated = true;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            List<Gizmo> result = new List<Gizmo>();
            if (desiresGenerated)
            {
                result.Add(new Gizmo_DesireTracker(this.parent as Pawn));
            }
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
                cachedThoughtStage = GetScoreStage(room);
                cachedBedroomWealth = currentBedroomWealth;
            }
            return cachedThoughtStage;
        }

        public void Reset()
        {
            for (int i = 0; i < roomDesireListList.Count; i++)
            {
                cachedBedroomWealth = -1;
                cachedThoughtStage = -1;
                roomDesireHashSet.Clear();
                roomDesireListList[i].Clear();
                GenerateDesires();
            }
        }

        public void GenerateDesires()
        {
            //todo adjust i to account for easygoing or picky pawns
            for (int i = 0; i < roomDesireListList.Count; i++)
            {
                roomDesireListList[i] = roomDesireListList[i].Union(ReturnDesiresFromUpgrades(i, generatedDesiresPerTier - roomDesireListList[i].Count)).ToList();
                roomDesireListList[i] = roomDesireListList[i].Union(ReturnDesiresFromRandom(i, generatedDesiresPerTier - roomDesireListList[i].Count)).ToList();
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

        public List<RoomDesire> ReturnDesiresFromRandom(int desireTier, int desiresDesired)
        {
            List<RoomDesire> possibleDesires = new List<RoomDesire>();
            List<RoomDesire> selectedDesires = new List<RoomDesire>();
            int desiresSelected = 0;

            //TODO logic
            //TODO early return if desiresDesired == 0?

            //populate list of possibleDesires, check against already selected desires for imcompatibility
            foreach (RoomDesire desire in RoomDesireMain.desiresByTier[desireTier])
            {
                //this block is not needed, incompatibility is checked during TrySelectDesire
                /*
                bool canAdd = true;
                foreach (RoomDesire desire2 in roomDesireHashSet)
                {
                    if (desire2.incompatibleWith.Contains(desire))
                    {
                        canAdd = false;
                        break;
                    }
                }
                if (canAdd)
                {
                    possibleDesires.Add(desire);
                }
                */
                possibleDesires.Add(desire);
            }

            //shuffle the list and select the correct number of desires
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
            foreach (RoomDesire desire2 in roomDesireHashSet)
            {
                if (desire2.incompatibleWith.Contains(desire))
                {
                    return false;
                }
            }
            return roomDesireHashSet.Add(desire);
        }

        public HashSet<Trait> CacheTraits()
        {
            HashSet<Trait> traitsHashSet = new HashSet<Trait>();
            if (pawn?.story?.traits != null)
            {
                List<Trait> traitsList = pawn.story.traits.allTraits;
                traitsHashSet.UnionWith(traitsList);
            }
            return traitsHashSet;
        }

        public void CheckIfTraitsChanged()
        {
            HashSet<Trait> currentTraits = CacheTraits();
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
                    dictList[i].Add(rd, rd.IsMet(pawn, room));
                }
            }

            return dictList;
        }

        public void HashSetToLists()
        {
            foreach (RoomDesire desire in roomDesireHashSet)
            {
                roomDesireListList[desire.desireTier - 1].Add(desire);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref desireSlots, "desireSlots", 5);
            Scribe_Values.Look(ref cachedBedroomWealth, "cachedBedroomWealth", -1);
            Scribe_Values.Look(ref cachedThoughtStage, "cachedThoughtStage", -1);
            Scribe_Values.Look(ref desiresGenerated, "desiresGenerated", false);
            List<string> roomDesireDefNames = new List<string>();
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                foreach (RoomDesire desire in roomDesireHashSet)
                {
                    roomDesireDefNames.Add(desire.def.defName);
                }
                Scribe_Collections.Look(ref roomDesireDefNames, "roomDesireHashSet", LookMode.Value);
            }
            else if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
                Scribe_Collections.Look(ref roomDesireDefNames, "roomDesireHashSet", LookMode.Value);
                roomDesireHashSet.Clear();
                foreach (string defName in roomDesireDefNames)
                {
                    RoomDesireDef desireDef = DefDatabase<RoomDesireDef>.GetNamed(defName, errorOnFail: false);
                    if (desireDef != null)
                    {
                        RoomDesire desire = desireDef.GetRoomDesire();
                        if (desire != null)
                        {
                            roomDesireHashSet.Add(desire);
                        }
                    }
                }
                HashSetToLists();
            }
        }
    }
}
