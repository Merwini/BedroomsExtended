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
    class AddCompToHumanlikes
    {
        static AddCompToHumanlikes()
        {
            for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
            {
                ThingDef def = DefDatabase<ThingDef>.AllDefsListForReading[i];

                if (def.race?.Humanlike ?? false)
                {
                    if (!def.comps.Any(c => c is CompProperties_PersonalizedBedroom))
                    {
                        CompProperties_PersonalizedBedroom props = new CompProperties_PersonalizedBedroom()
                        {
                            compClass = typeof(CompPersonalizedBedroom),
                        };
                        def.comps.Add(props);
                        props.ResolveReferences(def);
                        props.PostLoadSpecial(def);
                        Log.Message("Added personalized bedrooms to: " + def.defName);
                    }
                }
            }
        }
    }
}
