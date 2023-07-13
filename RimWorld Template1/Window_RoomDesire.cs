using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    class Window_RoomDesire : Window
    {
        internal Pawn pawn;

        internal List<Dictionary<RoomDesire, bool>> dictList;

        public Window_RoomDesire(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public override void PreOpen()
        {
            base.PreOpen();
            //TODO recalculate which desires are met and which aren't. Cache the results so they aren't continually recalculated while the window is open
            CompPersonalizedBedroom comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            Room room = pawn.ownership.OwnedBed.GetRoom();
            dictList = comp.roomDesireSet.GetDesiresMetCache(room);
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);
            Text.Font = GameFont.Medium;
            GUI.color = Color.white;
            list.Label("Bedroom Desires for " + pawn.Name);
            Text.Font = GameFont.Small;

            for (int i = 0; i < dictList.Count; i++)
            {
                foreach (KeyValuePair<RoomDesire, bool> entry in dictList[i])
                {
                    //TODO
                    if (entry.Value == true)
                    {
                        GUI.color = Color.green;
                    }
                    else
                    {
                        GUI.color = Color.red;
                    }
                }
            }

            list.Label("Tier one desires:");
            //TODO list tier one desires
            list.Gap();

            list.Label("Tier two desires:");
            //TODO list tier two desires
            list.Gap();

            list.Label("Tier three desires:");
            //TODO list tier three desires
            list.Gap();

            list.Label("Tier four desires:");
            //TODO list tier four desires
            list.Gap();

            list.Label("Tier five desires:");
            //TODO list tier five desires
            list.Gap();

            //TODO fill window with pawns desires and show which are met and which aren't
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            //TODO I forgot what I want this to do
        }
    }
}
