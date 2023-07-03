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
        List<ThingDef> satisfyingThings;

        QualityCategory minimumQuality;

        List<TraitDef> associatedTraits;

        List<GeneDef> associatedGenes;

        List<RoyalTitleDef> associatedTitles;
    }
}