using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using UnityEngine.UI;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    class Gizmo_DesireTracker : Command
    {
        internal Pawn pawn;

        public Gizmo_DesireTracker(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public override void ProcessInput(UnityEngine.Event ev)
        {
            base.ProcessInput(ev);

            if (Find.WindowStack.WindowOfType<Window_RoomDesire>() == null)
            {
                Window_RoomDesire newWindow = new Window_RoomDesire(pawn);
                Find.WindowStack.Add(newWindow);
            }
        }
    }
}
