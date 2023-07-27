using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    public class ThingRequirement
    {
        public List<ThingDef> satisfyingThings;

        public HashSet<ThingDef> satisfyingThingsExpanded;

        public List<string> satisfyingTags;

        public int quantityNeeded = 1;

        public QualityCategory minimumQuality = QualityCategory.Awful;
    }
}
