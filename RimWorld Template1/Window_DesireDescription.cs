using UnityEngine;
using Verse;


namespace nuff.PersonalizedBedrooms
{
    public class Window_DesireDescription : Window
    {
        private string additionalInfo;

        public Window_DesireDescription(string additionalInfo)
        {
            this.additionalInfo = additionalInfo;
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
            draggable = false;
            doCloseButton = false;
            doCloseX = false;
            preventCameraMotion = false;
            soundAppear = null;
            soundClose = null;
            onlyOneOfTypeAllowed = false;
            closeOnAccept = false;
        }

        public override Vector2 InitialSize
        {
            get
            {
                Vector2 textSize = Text.CalcSize(additionalInfo);
                return new Vector2(textSize.x + 20f, textSize.y + 20f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            Widgets.DrawBoxSolid(inRect, Color.black);
            Text.Anchor = TextAnchor.MiddleCenter;
            GUI.color = Color.white;
            Widgets.Label(inRect, additionalInfo);
        }
    }
}
