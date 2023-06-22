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
    public class RoomDesireDef : Def
    {
        List<ThingDef> satisfyingThings;

        QualityCategory minimumQuality;
    }
}