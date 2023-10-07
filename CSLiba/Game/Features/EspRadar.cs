using CSLiba.Core;
using CSLiba.Game.Enumerations;
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
    public static class EspRadar
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.radar)
                return;
            DrawRadar(g);

            lock (EspWorld.block)
                foreach (WorldItem item in EspWorld.grenades)
                    DrawItem(g, item, data);

            foreach (Entity entity in data.Entities)
            {
                if (entity.IsAlive())
                {
                    if (Configuration.current.onTeam || entity.team != data.localPlayer.team)
                        DrawEntity(g, entity, data);
                }
            }
            DrawRadarLines(g);
        }

        private static void DrawRadar(Graphics g)
        {
            borderBrush = Overlay.GetBrushByVector(g, Configuration.current.radarBorderColor);
            if (!Configuration.current.radarRounded)
            {
                g.FillRectangle(Overlay.GetBrushByVector(g, Configuration.current.radarBackgroundColor), new GameOverlay.Drawing.Rectangle(radarPos.X, radarPos.Y, radarPos.X + radarSize.X, radarPos.Y + radarSize.Y));
                g.DrawRectangle(borderBrush, radarPos.X, radarPos.Y, radarPos.X + radarSize.X, radarPos.Y + radarSize.Y, 2);

            }
            else
            {
                g.FillEllipse(Overlay.GetBrushByVector(g, Configuration.current.radarBackgroundColor), new GameOverlay.Drawing.Ellipse(RadarCenter.X, RadarCenter.Y, radarSize.X / 2, radarSize.Y / 2));
                g.DrawEllipse(borderBrush, RadarCenter.X, RadarCenter.Y, radarSize.X / 2, radarSize.Y / 2, 2);
            }
        }

        private static void DrawRadarLines(Graphics g)
        {
            g.FillCircle(Overlay.GetBrushByVector(g, Configuration.current.radarLocalPlayerColor), new Circle(RadarCenter.X, RadarCenter.Y, Configuration.current.radarEntityRadius));
            if (!Configuration.current.radarRounded)
            {
                g.DrawLine(borderBrush, new Line(RadarCenter.X, RadarCenter.Y, radarPos.X, radarPos.Y), 1);
                g.DrawLine(borderBrush, new Line(RadarCenter.X, RadarCenter.Y, radarPos.X + radarSize.X, radarPos.Y), 1);
            }
            else
            {
                Vector2 lineStart = new Vector2(RadarCenter.X, RadarCenter.Y);
                Vector2 lineEndLeft = new Vector2(radarPos.X, radarPos.Y);
                Vector2 lineEndRight = new Vector2(radarPos.X + radarSize.X, radarPos.Y);

                lineEndLeft.X += radarSize.X / 6.8f;
                lineEndLeft.Y += radarSize.Y / 6.8f;

                lineEndRight.X -= radarSize.X / 6.8f;
                lineEndRight.Y += radarSize.Y / 6.8f;

                g.DrawLine(borderBrush, new Line(lineStart.X, lineStart.Y, lineEndLeft.X, lineEndLeft.Y), 1);
                g.DrawLine(borderBrush, new Line(lineStart.X, lineStart.Y, lineEndRight.X, lineEndRight.Y), 1);
            }
        }

        private static void DrawEntity(Graphics g, Entity entity, GameData data)
        {
            Vector2 myPos = new Vector2(data.localPlayer.origin.X, data.localPlayer.origin.Y);
            Vector2 entityWorldPos = new Vector2(myPos.X - entity.origin.X, myPos.Y - entity.origin.Y);
            entityWorldPos /= Configuration.current.radarScale / 10f + 1;
            entityWorldPos.X *= -1;
            entityWorldPos += RadarCenter;
            Vector2 entityRadarPos = TransformPosToRadar(entityWorldPos, data.localPlayer.ViewAngles.Y - 90);

            if (entityRadarPos == Vector2.Zero)
                return;

            g.FillCircle(Overlay.GetBrushByVector(g, Configuration.current.radarEnemyColor), new Circle(entityRadarPos.X, entityRadarPos.Y, Configuration.current.radarEntityRadius));
        }

        private static void DrawItem(Graphics g, WorldItem entity, GameData data)
        {
            Vector2 myPos = new Vector2(data.localPlayer.origin.X, data.localPlayer.origin.Y);
            Vector2 entityWorldPos = new Vector2(myPos.X - entity.position.X, myPos.Y - entity.position.Y);
            entityWorldPos /= Configuration.current.radarScale / 10f + 1;
            entityWorldPos.X *= -1;
            entityWorldPos += RadarCenter;
            Vector2 entityRadarPos = TransformPosToRadar(entityWorldPos, data.localPlayer.ViewAngles.Y - 90);

            if (entityRadarPos == Vector2.Zero)
                return;

            if (entity.text == "")
                g.FillCircle(Overlay.GetBrushByVector(g, Configuration.current.radarGrenadesColor), new Circle(entityRadarPos.X, entityRadarPos.Y, 2));
            else
            {
                g.DrawText(Overlay.fontRadarIcons, Overlay.GetBrushByVector(g, Configuration.current.radarGrenadesColor), entityRadarPos.X, entityRadarPos.Y, entity.text);
            }
        }

        public static Vector2 TransformPosToRadar(Vector2 pointToRotate, float angle)
        {
            Vector2 rotatedPoint = new Vector2();
            angle = (float)(angle * (Math.PI / (float)180));

            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            rotatedPoint.X = cosTheta * (pointToRotate.X - RadarCenter.X) - sinTheta * (pointToRotate.Y - RadarCenter.Y);
            rotatedPoint.Y = sinTheta * (pointToRotate.X - RadarCenter.X) + cosTheta * (pointToRotate.Y - RadarCenter.Y);

            rotatedPoint.X += RadarCenter.X;
            rotatedPoint.Y += RadarCenter.Y;

            if (!Configuration.current.radarRounded)
            {

                if (rotatedPoint.X > radarPos.X + radarSize.X)
                    rotatedPoint.X = radarPos.X + radarSize.X;

                if (rotatedPoint.Y > radarPos.Y + radarSize.Y)
                    rotatedPoint.Y = radarPos.Y + radarSize.Y;

                if (rotatedPoint.X < radarPos.X)
                    rotatedPoint.X = radarPos.X;

                if (rotatedPoint.Y < radarPos.Y)
                    rotatedPoint.Y = radarPos.Y;
            }
            else
            {
                if (Math.Pow(rotatedPoint.X - RadarCenter.X, 2) + Math.Pow(rotatedPoint.Y - RadarCenter.Y, 2) >= Math.Pow(radarSize.X / 2, 2))
                    return Vector2.Zero;
            }

            return rotatedPoint;
        }

        private static Vector2 RadarCenter
        {
            get => radarPos + radarSize / 2;
        }

        private static Vector2 radarPos
        {
            get => new Vector2(Configuration.current.radarX, Configuration.current.radarY);
        }

        private static Vector2 radarSize
        {
            get => new Vector2(Configuration.current.radarW, Configuration.current.radarH);
        }

        private static IBrush borderBrush;
    }
}