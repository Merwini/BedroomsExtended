using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Verse;

namespace nuff.PersonalizedBedrooms
{
    class Window_RoomDesire : Window
    {
        internal Pawn pawn;

        internal CompPersonalizedBedroom comp;

        internal Room room;

        internal List<Dictionary<RoomDesire, bool>> dictList;

        int desiresNeeded;


        public enum DesireTierStatus
        {
            ACTIVE,
            INACTIVE
        }

        internal string[] desireTierStrings =
        {
            "One", "Two", "Three", "Four", "Five"
        };

        public Window_RoomDesire(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public override void PreOpen()
        {
            base.PreOpen();
            //TODO recalculate which desires are met and which aren't. Cache the results so they aren't continually recalculated while the window is open
            comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            room = pawn.ownership.OwnedBed.GetRoom();
            dictList = comp.roomDesireSet.GetDesiresMetCache(room);
            desiresNeeded = comp.roomDesireSet.minimumDesiresMetPerTier;
        }

        public override void DoWindowContents(Rect inRect)
        {
            int displayDesiresTotal = 0;
            int displayDesiresMet = 0;
            string displayTierStatus = "ERROR";
            string displayTierColor = "ERROR";

            Listing_Standard list = new Listing_Standard();
            list.Begin(inRect);
            Verse.Text.Font = GameFont.Medium;
            GUI.color = Color.white;
            list.Label("Bedroom Desires for " + pawn.Name);
            Verse.Text.Font = GameFont.Small;

            for (int i = 0; i < dictList.Count; i++)
            {
                foreach (KeyValuePair<RoomDesire, bool> entry in dictList[i])
                {
                    displayDesiresTotal++;
                    if (entry.Value == true)
                        displayDesiresMet++;
                }
                if (displayDesiresMet >= desiresNeeded)
                {
                    displayTierStatus = "ACTIVE";
                    displayTierColor = "<color=green>";
                }
                else
                {
                    displayTierStatus = "INACTIVE";
                    displayTierColor = "<color=red>";
                }
                string tierLabel = $"<color=white>Tier {desireTierStrings[i]} desires. ({displayDesiresMet.ToString()}/{displayDesiresTotal.ToString()} met. Status: </color>{displayTierColor}{displayTierStatus}</color>";
                list.Label(tierLabel);

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
                    list.Label(entry.Key.label);
                }
            }
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            //TODO I forgot what I want this to do
        }
    }
}
