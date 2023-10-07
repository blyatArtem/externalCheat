using CSLiba.Core;
using CSLiba.Game.Objects;
using CSLiba.UI.ConfigSystem;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Features
{
    public static class HitDrawer
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            foreach(Hit hit in hits)
            {
                hit.Draw(g, data);
            }
            hits.Where(x => x.timer < 0).ToList().ForEach(x => hits.Remove(x));
        }

        public static List<Hit> hits = new List<Hit>();
    }
}
