using System;
using System.Collections.Generic;
using GameOverlay.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSLiba.Core;
using Microsoft.Xna.Framework;
using CSLiba.UI.ConfigSystem;

namespace CSLiba.Game.Features
{
    public static class EspBombTimer
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.bombTimer)
                return;

            if (EspWorld.bombTimer <= 0)
                return;

            float percentage = EspWorld.bombTimer * 100 / 45;
            string bombTimerStr = Math.Round(EspWorld.bombTimer, 2).ToString();

            g.DrawVerticalProgressBar(Overlay.brushOutline, g.CreateSolidBrush(255, EspWorld.bombTimer * 255 / 45, 0), 0, 0, Overlay.window.Width, 5, 1, percentage);

            g.DrawText(Overlay.fontText, Overlay.brushText, Overlay.window.Width / 2 - 10, 25, bombTimerStr);
        }
    }
}
