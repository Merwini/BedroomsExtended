using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    //todo
    public class RoomDesireDef : Def
    {
        //inherits defName from Def

        //inherits label from Def

        //inherits description from Def

        public Type workerClass;

        public RoomDesireDef upgradesFrom;

        public List<RoomDesireDef> incompatibleWith;

        public List<ThingDef> satisfyingThings;

        public QualityCategory minimumQuality;

        public List<TraitDef> associatedTraits;

        public List<GeneDef> associatedGenes;

        public List<RoyalTitleDef> associatedTitles;

        public int desireTier;
    }
}