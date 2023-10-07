using CSLiba.Game.Enumerations;
using CSLiba.Game.Objects;
using CSLiba.UI.ConfigSystem;
using GameOverlay.Drawing;
using CSLiba.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Features
{
    public static class EspString
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            foreach (Entity entity in data.Entities)
            {
                if (!entity.IsAlive() || entity.basePtr == data.localPlayer.basePtr || entity.dormant || entity.Box.IsZero() || (!Configuration.current.onTeam && entity.team == data.localPlayer.team))
                {
                    continue;
                }
                DrawString(g, entity, data);
                DrawIcons(g, entity, data);
            }
        }

        public static void DrawString(Graphics g, Entity entity, GameData data)
        {
            string str = "";

            if (Configuration.current.healthString)
                str += "hp: " + entity.health.ToString() + "\n";
            if (Configuration.current.nicknames)
                str += entity.nickname + "\n";
            if (Configuration.current.distance)
                str += Math.Round(Vector3.Distance(data.localPlayer.origin, entity.origin) * 0.0254f, 0).ToString() + "m\n";
            if (Configuration.current.scopeWarning)
                str += entity.scoped ? "scoped\n" : "";
            if (Configuration.current.defuseWarning)
                str += entity.defusing ? "defusing\n" : "";


            if (str != "")
            {
                g.DrawText(Overlay.fontText, Overlay.brushOutline, new GameOverlay.Drawing.Point(entity.Box.Right + 15 + 2, entity.Box.Top + 2), str);
                g.DrawText(Overlay.fontText, Overlay.brushText, new GameOverlay.Drawing.Point(entity.Box.Right + 15, entity.Box.Top), str);
            }
        }

        public static void DrawIcons(Graphics g, Entity entity, GameData data)
        {
            if (!Configuration.current.weapons)
                return;
            string activeWeaponStr = entity.activeWeapon.GetWeaponIcon();
            if (activeWeaponStr != "")
            {
                if (entity.activeWeapon.ammo != -1)
                    activeWeaponStr += entity.activeWeapon.ammo.ToString() + "/" + entity.activeWeapon.ammo2;
                g.DrawText(Overlay.fontIcons, Overlay.brushOutline, new GameOverlay.Drawing.Point(entity.Box.Left + 2, entity.Box.Bottom + 15 + 2), activeWeaponStr);
                g.DrawText(Overlay.fontIcons, Overlay.brushText, new GameOverlay.Drawing.Point(entity.Box.Left, entity.Box.Bottom + 15), activeWeaponStr);
            }
        }
    }
}
