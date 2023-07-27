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

        public List<RoomDesireDef> upgradesFrom;

        public List<RoomDesireDef> incompatibleWith;

        public List<ThingDef> satisfyingThings;

        //changed to strings, so that TerrainTemplateDefs can be included as well. Will be parsed into Defs when the Desire is instantiated
        public List<string> satisfyingTerrains;

        public QualityCategory minimumQuality = QualityCategory.Awful;

        public int minimumQuantity;

        public List<TraitDef> associatedTraits;

        public List<GeneDef> associatedGenes;

        public List<RoyalTitleDef> associatedTitles;

        public int desireTier;

        public RoomDesire registeredDesire;

        public RoomDesire GetRoomDesire()
        {
            return this.registeredDesire;
        }
    }
}