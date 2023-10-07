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
    public static class EspBoxes
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            foreach (Entity entity in data.Entities)
            {
                if (!entity.IsAlive() || entity.basePtr == data.localPlayer.basePtr || (!Configuration.current.onTeam && entity.team == data.localPlayer.team) || entity.dormant)
                {
                    continue;
                }
                if (Configuration.current.lines)
                    DrawLines(g, entity);
                Draw(g, entity, data);
            }
        }

        private static void DrawLines(Graphics g, Entity entity)
        {
            if (entity.Box.IsZero())
                return;

            GameOverlay.Drawing.Point linesStartPoint = new GameOverlay.Drawing.Point(g.Width / 2, g.Width / 2);
            IBrush brush = entity.spottedByMask ?
                (Configuration.current.spottedBoxEnemyColor.X >= 0 ? g.CreateSolidBrush(Configuration.current.spottedBoxEnemyColor.X, Configuration.current.spottedBoxEnemyColor.Y, Configuration.current.spottedBoxEnemyColor.Z, Configuration.current.spottedBoxEnemyColor.W) : Overlay.GetRainbowBrush(g, Configuration.current.spottedBoxEnemyColor.W)) :
                (Configuration.current.boxEnemyColor.X >= 0 ? g.CreateSolidBrush(Configuration.current.boxEnemyColor.X, Configuration.current.boxEnemyColor.Y, Configuration.current.boxEnemyColor.Z, Configuration.current.boxEnemyColor.W) : Overlay.GetRainbowBrush(g, Configuration.current.boxEnemyColor.W));
            g.DrawLine(brush, new Line(linesStartPoint.X, linesStartPoint.Y, entity.Box.Right - (entity.Box.Width / 2), entity.Box.Bottom), 2);
        }

        private static void Draw(Graphics g, Entity entity, GameData data)
        {
            if (entity.Box.IsZero())
                return;

            IBrush brush = null;

            DrawHealth(g, entity);
            brush = entity.spottedByMask ?
                (Configuration.current.spottedBoxEnemyColor.X >= 0 ? g.CreateSolidBrush(Configuration.current.spottedBoxEnemyColor.X, Configuration.current.spottedBoxEnemyColor.Y, Configuration.current.spottedBoxEnemyColor.Z, Configuration.current.spottedBoxEnemyColor.W) : Overlay.GetRainbowBrush(g, Configuration.current.spottedBoxEnemyColor.W)) :
                (Configuration.current.boxEnemyColor.X >= 0 ? g.CreateSolidBrush(Configuration.current.boxEnemyColor.X, Configuration.current.boxEnemyColor.Y, Configuration.current.boxEnemyColor.Z, Configuration.current.boxEnemyColor.W) : Overlay.GetRainbowBrush(g, Configuration.current.boxEnemyColor.W));


            if (!Configuration.current.boxesEnemy)
                return;

            int boxType = Configuration.current.boxesEnemyType;

            if (boxType == 0)
                g.DrawRectangle(brush, entity.Box, 2);
            else if (boxType == 1)
                g.DrawRoundedRectangle(brush, entity.Box.Left, entity.Box.Top, entity.Box.Right, entity.Box.Bottom, entity.Box.Height / 6, 2);
            else if (boxType == 2)
                g.DrawRectangleEdges(brush, entity.Box.Left, entity.Box.Top, entity.Box.Right, entity.Box.Bottom, 2);
        }

        private static void DrawHealth(Graphics g, Entity entity)
        {
            if (!Configuration.current.health)
                return;

            if (Configuration.current.healthSide == 0)
            {
                g.DrawHorizontalProgressBar(g.CreateSolidBrush(0, 0, 0, 255), g.CreateSolidBrush(0, 0, 0), entity.Box.Left - 9, entity.Box.Top, entity.Box.Left - 4, entity.Box.Bottom, 1, 100);
                g.DrawHorizontalProgressBar(g.CreateSolidBrush(0, 0, 0, 255), g.CreateSolidBrush(ColorFromHealth(entity.health)), entity.Box.Left - 9, entity.Box.Top, entity.Box.Left - 4, entity.Box.Bottom, 1, entity.health);
            }
            else if (Configuration.current.healthSide == 1)
            {
                g.DrawHorizontalProgressBar(g.CreateSolidBrush(0, 0, 0, 255), g.CreateSolidBrush(0, 0, 0), entity.Box.Right + 4, entity.Box.Top, entity.Box.Right + 9, entity.Box.Bottom, 1, 100);
                g.DrawHorizontalProgressBar(g.CreateSolidBrush(0, 0, 0, 255), g.CreateSolidBrush(ColorFromHealth(entity.health)), entity.Box.Right + 4, entity.Box.Top, entity.Box.Right + 9, entity.Box.Bottom, 1, entity.health);
            }
            else if (Configuration.current.healthSide == 2)
            {
                g.DrawHorizontalProgressBar(g.CreateSolidBrush(0, 0, 0, 255), g.CreateSolidBrush(0, 0, 0), entity.Box.Left, entity.Box.Bottom + 4, entity.Box.Right, entity.Box.Bottom + 9, 1, 100);
                g.DrawVerticalProgressBar(g.CreateSolidBrush(0, 0, 0, 255), g.CreateSolidBrush(ColorFromHealth(entity.health)), entity.Box.Left, entity.Box.Bottom + 4, entity.Box.Right, entity.Box.Bottom + 9, 1, entity.health);
            }

        }
        private static GameOverlay.Drawing.Color ColorFromHealth(int health)
        {
            if (health > 59)
                return GameOverlay.Drawing.Color.Green;
            else if (health > 24)
                return new GameOverlay.Drawing.Color(255, 255, 0);
            return GameOverlay.Drawing.Color.Red;
        }
    }
}
