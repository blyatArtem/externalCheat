using CSLiba.Core;
using CSLiba.Game.Features;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Objects
{
    public class SmokeHelperPoint
    {
        public SmokeHelperPoint()
        {

        }

        public SmokeHelperPoint(Vector3 startPos, Vector3 throwPos, string endPoint, bool leftClick = true, bool jump = false, bool run = false)
        {
            this.leftClick = leftClick;
            this.jump = jump;
            this.run = run;

            this.startPos = startPos;
            this.throwPos = throwPos;
            this.endPoint = endPoint;
            startPos.Z = 1;
            throwPos.Z = 1;
        }

        public void Draw(Graphics g, LocalPlayer player)
        {
            float distance = Vector3.Distance(player.origin, startPos);
            if (distance < 600 && player.activeWeapon.definitionIndex == Enumerations.ItemDefinitionIndex.WEAPON_SMOKEGRENADE)
            {
                Vector3 screenPoint = player.MatrixViewProjectionViewport.Transform(startPos);
                if (screenPoint.IsValidScreen())
                {
                    g.FillCircle(Overlay.brushText, new Circle(screenPoint.X, screenPoint.Y, 3));
                    g.DrawCircle(Overlay.brushOutline, new Circle(screenPoint.X, screenPoint.Y, 5), 3);
                    g.DrawText(Overlay.fontText, Overlay.brushOutline, screenPoint.X + 15 + 2, screenPoint.Y + 15 + 2, endPoint);
                    g.DrawText(Overlay.fontText, Overlay.brushText, screenPoint.X + 15, screenPoint.Y + 15, endPoint);
                }

                if (distance < 12)
                {
                    Vector3 screenThrowPoint = player.MatrixViewProjectionViewport.Transform(throwPos);
                    if (screenThrowPoint.IsValidScreen())
                    {
                        SmokeHelper.aiming = true;
                        g.FillCircle(Overlay.brushOutline, new Circle(screenThrowPoint.X, screenThrowPoint.Y, 4));
                        g.FillCircle(Overlay.brushText, new Circle(screenThrowPoint.X, screenThrowPoint.Y, 2));
                        g.DrawImage(leftClick ? Overlay.mouseLeft : Overlay.mouseRight, screenThrowPoint.X + 10, screenThrowPoint.Y - 16);

                        if (jump || run)
                        {
                            string str = run ? "run" : "";
                            str += jump ? " jump" : "";
                            g.DrawText(Overlay.fontText, Overlay.brushOutline, screenThrowPoint.X + 72, screenThrowPoint.Y + 8, str);
                            g.DrawText(Overlay.fontText, Overlay.brushText, screenThrowPoint.X + 70, screenThrowPoint.Y + 6, str);
                        }
                    }

                    Vector3 playerPosition = player.MatrixViewProjectionViewport.Transform(new Vector3(player.origin.X, player.origin.Y, player.origin.Z));

                    if (playerPosition.IsValidScreen())
                    {
                        g.DrawCircle(Overlay.brushRed, playerPosition.X, playerPosition.Y, 4, 3);
                    }
                }
            }
        }

        public bool leftClick, jump, run;
        public Vector3 startPos, throwPos;
        public string endPoint;

    }
}
