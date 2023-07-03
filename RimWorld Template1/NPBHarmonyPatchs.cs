using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;

namespace nuff.PersonalizedBedrooms
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony("nuff.rimworld.personalized_bedrooms");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Toils_LayDown))]
        [HarmonyPatch("ApplyBedThoughts")]
        public static class Toils_LayDown_ApplyBedThoughts_Patch
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                MethodInfo methodHelper = typeof(HarmonyPatches).GetMethod("Toils_LayDown_ApplyBedThoughts_Helper", BindingFlags.Public | BindingFlags.Static);
                int startIndex = -1;
                int endIndex = -1;
                bool foundStartIndex = false;
                bool foundEndIndex = false;

                for (int i = 0; i < codes.Count; i++)
                {
                    if (foundStartIndex && foundEndIndex)
                    {
                        break;
                    }

                    if (foundEndIndex)
                    {
                        //todo
                    }

                    if (!foundEndIndex)
                    {
                        endIndex = i;
                        foundEndIndex = true;
                        continue;
                    }
                }

                if (startIndex >= 0 && endIndex >= 0)
                {
                    //make new instructions
                    List<CodeInstruction> newCodes = new List<CodeInstruction>();

                    //remove old instions
                    codes.RemoveRange(startIndex, (endIndex - startIndex + 1));

                    //insert new instructions
                    codes.InsertRange(startIndex, newCodes);
                }

                return codes.AsEnumerable();
            }
        }

        public static void Toils_LayDown_ApplyBedThoughts_Helper()
        {

        }
    }
}
