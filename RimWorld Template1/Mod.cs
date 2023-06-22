using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    [StaticConstructorOnStartup]
    public class PersonalizedBedrooms : Mod
    {
        public PersonalizedBedrooms(ModContentPack content) : base(content)
        {

        }

        public override string SettingsCategory()
        {
            return "Personalized Bedrooms";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
        }
    }
}
