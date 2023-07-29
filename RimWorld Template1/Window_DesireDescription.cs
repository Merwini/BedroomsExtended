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
            forcePause = false;
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
                //Vector2 textSize = Text.CalcSize(additionalInfo);
                //return new Vector2(textSize.x + 20f, textSize.y + 20f);
                return new Vector2(400, 400);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            GUI.color = Color.white;
            Widgets.Label(inRect, additionalInfo);

            Rect closeButtonRect = new Rect(inRect.width / 2f - CloseButSize.x / 2f, inRect.height - CloseButSize.y, CloseButSize.x, CloseButSize.y);
            if (Widgets.ButtonText(closeButtonRect, "Close"))
            {
                this.Close();
            }
        }
    }
}
