using CSLiba.Core;
using CSLiba.UI.ConfigSystem;
using CSLiba.UI.ConfigSystem.ConfigStructs;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Features
{
    public static class FovDrawer
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.fov || data.localPlayer.activeWeapon == null)
                return;

            IBrush brush = Configuration.current.fovColor.X >= 0 ? g.CreateSolidBrush(Configuration.current.fovColor.X, Configuration.current.fovColor.Y, Configuration.current.fovColor.Z, Configuration.current.fovColor.W) : Overlay.GetRainbowBrush(g, Configuration.current.fovColor.W);
            SerializebleWeapon weapon =
                data.localPlayer.activeWeapon.IsPistol() ? Configuration.current.pistol :
                data.localPlayer.activeWeapon.IsSMG() ? Configuration.current.smg :
                data.localPlayer.activeWeapon.IsHeavy() ? Configuration.current.heavy :
                data.localPlayer.activeWeapon.IsRifle() ? Configuration.current.rifle :
                (data.localPlayer.activeWeapon.IsAutSniper() || data.localPlayer.activeWeapon.IsSniper()) ? Configuration.current.sniper : null;

            if (weapon == null)
                return;
            Vector3 aimPos = AimCrosshair.GetPositionScreen(g, data.localPlayer);
            g.DrawCircle(brush, new Circle(new GameOverlay.Drawing.Point(aimPos.X, aimPos.Y), (float)weapon.fov * 5.6f), 2);

            Vector3 target = AimAssist.FrameAction(data, true);
            if (target == Vector3.Zero)
                return;
            target = data.localPlayer.MatrixViewProjectionViewport.Transform(target);
            if (!target.IsValidScreen())
                return;
            g.DrawCircle(brush, new Circle(new GameOverlay.Drawing.Point(target.X, target.Y), (float)weapon.fov * 7), 2);
            g.DrawLine(brush, aimPos.X, aimPos.Y, target.X, target.Y, 2);
        }
    }
}
