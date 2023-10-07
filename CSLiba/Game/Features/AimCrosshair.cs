using CSLiba.Game.Objects;
using CSLiba.Core;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSLiba.UI.ConfigSystem;

namespace CSLiba.Game.Features
{
    public static class AimCrosshair
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.aimCrosshair)
                return;

            if (!SmokeHelper.aiming)
            {
                IBrush brush = Configuration.current.aimCrosshairColor.X >= 0 ? g.CreateSolidBrush((int)Configuration.current.aimCrosshairColor.X, (int)Configuration.current.aimCrosshairColor.Y, (int)Configuration.current.aimCrosshairColor.Z, (int)Configuration.current.aimCrosshairColor.W) :
                    Overlay.GetRainbowBrush(g, Configuration.current.aimCrosshairColor.W);
                Vector3 crosshairPos = GetPositionScreen(g, data.localPlayer);
                g.DrawCrosshair(brush, new GameOverlay.Drawing.Point(crosshairPos.X + 0.5f, crosshairPos.Y + 0.5f), Configuration.current.aimCrosshairSize, Configuration.current.aimCrosshairStroke, Configuration.current.aimCrosshairStyle);
            }
            else
            {
                g.DrawCrosshair(g.CreateSolidBrush(255, 0, 0, 255), new GameOverlay.Drawing.Point(Overlay.window.Width / 2 + 0.5f, Overlay.window.Height / 2 + 0.5f), 4, 4, CrosshairStyle.Dot);
            }
        }

        public static Vector3 GetPositionScreen(Graphics g, LocalPlayer localPLayer)
        {
            var aspectRatio = (double)g.Width / g.Height;
            var player = localPLayer;
            var fovY = ((double)player.Fov).DegreeToRadian();
            var fovX = fovY * aspectRatio;
            var punchX = ((double)player.AimPunchAngle.X * 2f).DegreeToRadian();
            var punchY = ((double)player.AimPunchAngle.Y * 2f).DegreeToRadian();
            var pointClip = new Vector3
            (
                (float)(-punchY / fovX),
                (float)(-punchX / fovY),
                0
            );
            return player.MatrixViewport.Transform(pointClip);
        }
    }
}
