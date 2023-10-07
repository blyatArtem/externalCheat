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
using CSLiba.Game.Structs;

namespace CSLiba.Game.Features
{
    public static class EspSkeleton
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.skeletonsEnemy)
                return;
            foreach (Entity entity in data.Entities)
            {
                if (!entity.IsAlive() || entity.basePtr == data.localPlayer.basePtr || (!Configuration.current.onTeam && entity.team == data.localPlayer.team))
                {
                        continue;
                }

                Draw(g, entity, data);
            }
        }

        private static void Draw(Graphics graphics, Entity entity, GameData data)
        {
            IBrush brush = entity.spottedByMask ?
                (Configuration.current.spottedSkeletonEnemyColor.X >= 0 ? graphics.CreateSolidBrush(Configuration.current.spottedSkeletonEnemyColor.X, Configuration.current.spottedSkeletonEnemyColor.Y, Configuration.current.spottedSkeletonEnemyColor.Z, Configuration.current.spottedSkeletonEnemyColor.W) : Overlay.GetRainbowBrush(graphics, Configuration.current.spottedSkeletonEnemyColor.W)) :
                (Configuration.current.skeletonEnemyColor.X >= 0 ? graphics.CreateSolidBrush(Configuration.current.skeletonEnemyColor.X, Configuration.current.skeletonEnemyColor.Y, Configuration.current.skeletonEnemyColor.Z, Configuration.current.skeletonEnemyColor.W) : Overlay.GetRainbowBrush(graphics, Configuration.current.skeletonEnemyColor.W));

            for (int i = 0; i < entity.skeletonCount; i++)
            {
                var (from, to) = entity.Skeleton[i];

                if (from == to || from < 0 || to < 0 || from >= 128 || to >= 128)
                {
                    continue;
                }
                DrawPolylineWorld(graphics, data, brush, entity.BonesPos[from], entity.BonesPos[to]);
            }
        }

        private static void DrawPolylineWorld(Graphics graphics, GameData data, IBrush brush, params Vector3[] verticesWorld)
        {
            var verticesScreen = verticesWorld.Select(v => data.localPlayer.MatrixViewProjectionViewport.Transform(v)).ToArray();
            DrawPolylineScreen(graphics, verticesScreen, brush);
        }

        private static void DrawPolylineScreen(Graphics graphics, Vector3[] vertices, IBrush color)
        {
            if (vertices.Length < 2 || vertices.Any(v => !v.IsValidScreen()))
            {
                return;
            }
            graphics.DrawLine(color, new Line(vertices[0].X, vertices[0].Y, vertices[1].X, vertices[1].Y), 2);
        }
    }
}
