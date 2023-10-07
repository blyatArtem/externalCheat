using CSLiba.Core;
using CSLiba.UI.ConfigSystem;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Objects
{
    public class Hit
    {
        public Hit(int dmg, Vector3 pos)
        {
            this.dmg = dmg;
            this.timer = 2f;
            this.point = pos;
        }

        public void Draw(Graphics g, GameData data)
        {
            point.Z += 1f;
            timer -= 0.04f;

            Vector3 point2d = data.localPlayer.MatrixViewProjectionViewport.Transform(point);
            if (point2d.IsValidScreen())
            {
                g.DrawText(Overlay.fontText, Configuration.current.hitsEspColor.X >= 0 ? g.CreateSolidBrush(Configuration.current.hitsEspColor.X, Configuration.current.hitsEspColor.Y, Configuration.current.hitsEspColor.Z, Configuration.current.hitsEspColor.W) : Overlay.GetRainbowBrush(g, Configuration.current.hitsEspColor.W), new GameOverlay.Drawing.Point(point2d.X, point2d.Y), $"-{dmg}");
            }
        }

        public Vector3 point;

        public float timer;
        public int dmg;
    }
}
