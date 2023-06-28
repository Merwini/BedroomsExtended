using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    [StaticConstructorOnStartup]
    static class LinkTraitsWithDesires
    {
        static LinkTraitsWithDesires()
        {
            //on startup, check all RoomDesireDefs for associated traits
            //if it has an associated trait, add the trait to the dictionary as a key, and add the desire to the associated value list
        }

        internal static Dictionary<TraitDef, List<RoomDesireDef>> traitToDesireDictionary;
    }
}
