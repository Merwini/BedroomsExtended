using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Verse;
using RimWorld;

namespace nuff.PersonalizedBedrooms
{
    class Window_RoomDesire : Window
    {
        internal Pawn pawn;

        internal CompPersonalizedBedroom comp;

        internal Room room;

        internal List<Dictionary<RoomDesire, bool>> dictList;

        int desiresNeeded;

        string colorRed = "<color=#ff3333>";
        string colorGreen = "<color=#15b01a>";
        string colorYellow = "<color=#ffff14>";


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
        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(550, 850);
            }
        }
        public override void PreOpen()
        {
            base.PreOpen();
            //TODO recalculate which desires are met and which aren't. Cache the results so they aren't continually recalculated while the window is open
            comp = pawn.TryGetComp<CompPersonalizedBedroom>();
            if (!comp.desiresGenerated)
                comp.DoLogic();
            room = pawn?.ownership?.OwnedBed?.GetRoom() ?? null;
            if (room != null)
            {
                comp.thingsInRoomCache = room.ContainedAndAdjacentThings;
                dictList = comp.GetDesiresMetCache(room);
            }
            desiresNeeded = comp.minimumDesiresMetPerTier;
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

            if (room != null)
            {
                Verse.Text.Font = GameFont.Small;
                bool stillAvailable = true;
                for (int i = 0; i < dictList.Count; i++)
                {
                    displayDesiresTotal = 0;
                    displayDesiresMet = 0;

                    foreach (KeyValuePair<RoomDesire, bool> entry in dictList[i])
                    {
                        displayDesiresTotal++;
                        if (entry.Value)
                            displayDesiresMet++;
                    }
                    if (stillAvailable)
                    {
                        if (displayDesiresMet >= desiresNeeded)
                        {
                            displayTierStatus = "ACTIVE";
                            displayTierColor = colorGreen;
                        }
                        else
                        {
                            displayTierStatus = "INACTIVE";
                            displayTierColor = colorRed;
                            stillAvailable = false;
                        }
                    }
                    else
                    {
                        displayTierStatus = "UNAVAILABLE";
                        displayTierColor = colorYellow;
                    }

                    string tierLabel = $"<color=white>Tier {desireTierStrings[i]} desires. ({displayDesiresMet.ToString()}/{displayDesiresTotal.ToString()} met. Status: </color>{displayTierColor}{displayTierStatus}</color>";
                    list.Label(tierLabel);
                    foreach (KeyValuePair<RoomDesire, bool> entry in dictList[i])
                    {
                        Color color;
                        if (entry.Value)
                        {
                            //color = new Color(0.055f, 0.541f, 0.086f);
                            color = ColorLibrary.Green;
                        }
                        else
                        {
                            //color = new Color(0.714f, 0.008f, 0.020f);
                            color = ColorLibrary.RedReadable;
                        }
                        GUI.color = color;

                        // Draw the label
                        Rect labelRect = list.GetRect(Verse.Text.LineHeight);
                        //Rect iconRect = new Rect(labelRect.xMax - 30f, labelRect.y, 24f, 24f);
                        Widgets.Label(labelRect, entry.Key.label);

                        // Draw the icon
                        //Texture2D iconTexture = ContentFinder<Texture2D>.Get("UI/Icons/QuestionMark");
                        //GUI.DrawTexture(iconRect, iconTexture);
                        TooltipHandler.TipRegion(labelRect, entry.Key.def.description);


                        list.Gap(list.verticalSpacing);
                    }
                }
            }
            else
            {
                GUI.color = Color.red;
                list.Label("Error: pawn has no current bedroom");
            }

            Rect closeButtonRect = new Rect(inRect.width / 2f - CloseButSize.x / 2f, inRect.height - CloseButSize.y, CloseButSize.x, CloseButSize.y);
            if (Widgets.ButtonText(closeButtonRect, "Close"))
            {
                this.Close();
            }
            list.End();
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
            //TODO I forgot what I want this to do
        }
    }
}
