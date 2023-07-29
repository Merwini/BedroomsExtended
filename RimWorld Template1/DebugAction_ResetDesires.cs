using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    public static class DebugActions
    {
        [DebugAction("Personalized Bedrooms", "Reset Desires", false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void ResetDesires(Pawn pawn)
        {
            CompPersonalizedBedroom comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            if (comp != null)
            {
                comp.Reset();
            }
            DebugActionsUtility.DustPuffFrom(pawn);
        }
    }
}
