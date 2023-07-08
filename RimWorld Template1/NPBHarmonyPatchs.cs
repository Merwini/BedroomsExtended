using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;

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

                    if (codes[i].opcode == OpCodes.Ldsfld
                        && codes[i].operand.ToString().Contains("Impressiveness"))
                    {
                        //todo
                        startIndex = i;
                        foundStartIndex = true;
                        continue;
                    }

                    if (foundStartIndex)
                    {
                        for (int j = i; j < codes.Count; j++)
                        {
                            if (codes[j].opcode == OpCodes.Callvirt
                                && codes[j].operand.ToString().Contains("GetScoreStageIndex"))
                            {
                                endIndex = j;
                                foundEndIndex = true;
                                break;
                            }
                        }
                        continue;
                    }
                }

                if (startIndex >= 0 && endIndex >= 0)
                {
                    //make new instructions
                    List<CodeInstruction> newCodes = new List<CodeInstruction>();
                    //load Pawn on stack
                    newCodes.Add(new CodeInstruction(OpCodes.Ldarg_0));
                    //load Bed on stack
                    newCodes.Add(new CodeInstruction(OpCodes.Ldarg_1));
                    //call helper method, will return an int
                    newCodes.Add(new CodeInstruction(OpCodes.Call, methodHelper));
                    //next CodeInstruction should be stloc.s 6 from vanilla instructions, which will use the int the helper method returns

                    //remove old instructions
                    codes.RemoveRange(startIndex, (endIndex - startIndex + 1));

                    //insert new instructions
                    codes.InsertRange(startIndex, newCodes);
                }

                return codes.AsEnumerable();
            }
        }

        public static int Toils_LayDown_ApplyBedThoughts_Helper(Pawn actor, Building_Bed bed)
        {
            CompPersonalizedBedroom comp = actor.TryGetComp<CompPersonalizedBedroom>();
            if (comp == null)
            {
                CompPersonalizedBedroom newComp = new CompPersonalizedBedroom();
                actor.AllComps.Add(newComp);
                comp = newComp;
                comp.Initialize(new CompProperties_PersonalizedBedroom());
            }

            return comp.ReturnThoughtStage(bed);
        }
    }
}
