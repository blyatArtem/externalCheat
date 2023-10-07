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
    public static class EspHitboxes
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.hitboxesEnemy)
                return;
            foreach (Entity entity in data.Entities)
            {
                if (entity.Box.IsZero() || !entity.IsAlive() || entity.basePtr == data.localPlayer.basePtr || (!Configuration.current.onTeam && entity.team == data.localPlayer.team))
                {
                    continue;
                }
                IBrush brush = entity.spottedByMask ? Overlay.GetBrushByVector(g, Configuration.current.spottedHitboxEnemyColor) : Overlay.GetBrushByVector(g, Configuration.current.hitboxEnemyColor);
                Draw(g, entity, brush, data);
            }
        }

        private static void Draw(Graphics g, Entity entity, IBrush brush, GameData data)
        {
            for (int i = 0; i < entity.StudioHitBoxSet.numhitboxes; i++)
            {
                var hitbox = entity.StudioHitBoxes[i];
                if (hitbox.bone < 0 || hitbox.bone > 128)
                {
                    return;
                }

                if (hitbox.radius > 0)
                {
                    DrawHitBoxCapsule(g, entity, brush, i);
                }
            }
        }

        private static void DrawHitBoxCapsule(Graphics g, Entity entity, IBrush brush, int hitBoxId)
        {
            var hitbox = entity.StudioHitBoxes[hitBoxId];
            var matrixBoneModelToWorld = entity.BonesMatrices[hitbox.bone];

            var bonePos0World = matrixBoneModelToWorld.Transform(hitbox.bbmin);
            var bonePos1World = matrixBoneModelToWorld.Transform(hitbox.bbmax);


            DrawCapsuleWorld(g, brush, bonePos0World, bonePos1World, hitbox.radius, 6, 3);
        }

        public static void DrawCapsuleWorld(Graphics graphics, IBrush spotted, Vector3 start, Vector3 end, float radius, int segments, int layers)
        {
            var normal = end - start;
            normal.Normalize();

            var halfSphere0 = GetHalfSphere(start, -normal, radius, segments, layers);
            var halfSphere1 = GetHalfSphere(end, normal, radius, segments, layers);

            // world to screen + draw layered circles
            for (var i = 0; i < layers; i++)
            {
                for (var j = 0; j < segments + 1; j++)
                {
                    halfSphere0[i][j] = TransformWorldToScreen(halfSphere0[i][j]);
                    halfSphere1[i][j] = TransformWorldToScreen(halfSphere1[i][j]);
                }

                DrawPolylineScreen(graphics, spotted, halfSphere0[i]);
                DrawPolylineScreen(graphics, spotted, halfSphere1[i]);
            }

            // draw verticals of half-spheres (connect layered circles)
            var halfSphereTopScreen0 = TransformWorldToScreen(start - normal * radius);
            var halfSphereTopScreen1 = TransformWorldToScreen(end + normal * radius);
            var verticals0 = new Vector3[layers + 1];
            var verticals1 = new Vector3[layers + 1];
            for (var vertexId = 0; vertexId < segments + 1; vertexId++)
            {
                for (var layerId = 0; layerId < layers; layerId++)
                {
                    verticals0[layerId] = halfSphere0[layerId][vertexId];
                    verticals1[layerId] = halfSphere1[layerId][vertexId];
                }
                verticals0[layers] = halfSphereTopScreen0;
                verticals1[layers] = halfSphereTopScreen1;

                DrawPolylineScreen(graphics, spotted, verticals0);
                DrawPolylineScreen(graphics, spotted, verticals1);
            }

            // draw vertical cylinder edges between half-spheres
            DrawCylinderSidesWorld(graphics, spotted, start, end, radius, segments);
        }

        private static void DrawCylinderSidesWorld(this Graphics graphics, IBrush spotted, Vector3 start, Vector3 end, float radius, int segments)
        {
            var normal = end - start;
            normal.Normalize();

            var vertices0 = GetCircleVertices(start, normal, radius, segments);
            var vertices1 = GetCircleVertices(end, normal, radius, segments);

            for (var i = 0; i < vertices0.Length; i++)
            {
                vertices0[i] = TransformWorldToScreen(vertices0[i]);
                vertices1[i] = TransformWorldToScreen(vertices1[i]);

                DrawPolylineScreen(graphics, spotted, vertices0[i], vertices1[i]);
            }
        }

        private static Vector3[][] GetHalfSphere(Vector3 origin, Vector3 normal, float radius, int segments, int layers)
        {
            normal.Normalize();
            var verticesByLayer = new Vector3[layers][];
            for (var layerId = 0; layerId < layers; layerId++)
            {
                var radiusLayer = radius - layerId * (radius / layers);
                var originLayer = origin + normal * ((float)Math.Cos(Math.Asin(radiusLayer / radius)) * radius);
                verticesByLayer[layerId] = GetCircleVertices(originLayer, normal, radiusLayer, segments);
            }
            return verticesByLayer;
        }

        private static void DrawPolylineScreen(Graphics g, IBrush brush, params Vector3[] vertices)
        {
            if (vertices.Length < 2 || vertices.Any(v => !v.IsValidScreen()))
            {
                return;
            }

            int count = vertices.Length / 2;

            for (int i = 0; i < count; i += 2)
            {
                g.DrawLine(brush, vertices[i].X, vertices[i].Y, vertices[i + 1].X, vertices[i + 1].Y, 2);
            }
        }
        private static Vector3[] GetCircleVertices2D(Vector3 origin, double radius, int segments)
        {
            var vertices = new Vector3[segments + 1];
            var step = Math.PI * 2 / segments;
            for (var i = 0; i < segments; i++)
            {
                var theta = step * i;
                vertices[i] = new Vector3
                (
                    (float)(origin.X + radius * Math.Cos(theta)),
                    (float)(origin.Y + radius * Math.Sin(theta)),
                    0
                );
            }
            vertices[segments] = vertices[0];
            return vertices;
        }
        private static Vector3[] GetCircleVertices(Vector3 origin, Vector3 normal, double radius, int segments)
        {
            var matrixLocalToWorld = GetOrthogonalMatrix(normal, origin);
            var vertices = GetCircleVertices2D(new Vector3(), radius, segments);
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = matrixLocalToWorld.Transform(vertices[i]);
            }
            return vertices;
        }
        private static Matrix GetOrthogonalMatrix(Vector3 normal, Vector3 origin)
        {
            GetOrthogonalAxis(normal, out var xAxis, out var yAxis, out var zAxis);
            return GetMatrix(xAxis, yAxis, zAxis, origin);
        }
        private static float AngleTo(Vector3 vector, Vector3 other)
        {
            vector.Normalize();
            other.Normalize();
            return (float)Math.Acos(Vector3.Dot(vector, other));
        }

        public static void GetOrthogonalAxis(Vector3 normal, out Vector3 xAxis, out Vector3 yAxis, out Vector3 zAxis)
        {
            normal.Normalize();

            var axisZ = new Vector3(0, 0, 1);
            var angleToAxisZ = AngleTo(normal, axisZ);
            if (angleToAxisZ < Math.PI * 0.25 || angleToAxisZ > Math.PI * 0.75)
            {
                // too close to z-axis, use y-axis
                xAxis = Vector3.Cross(new Vector3(0, 1, 0), normal);
            }
            else
            {
                // use z-axis
                xAxis = Vector3.Cross(normal, axisZ);
            }
            xAxis.Normalize();

            yAxis = Vector3.Cross(normal, xAxis);
            yAxis.Normalize();

            zAxis = normal;
        }
        private static Matrix GetMatrix(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, Vector3 origin)
        {
            return new Matrix
            {
                M11 = xAxis.X,
                M12 = xAxis.Y,
                M13 = xAxis.Z,

                M21 = yAxis.X,
                M22 = yAxis.Y,
                M23 = yAxis.Z,

                M31 = zAxis.X,
                M32 = zAxis.Y,
                M33 = zAxis.Z,

                M41 = origin.X,
                M42 = origin.Y,
                M43 = origin.Z,
                M44 = 1,
            };
        }

        private static Vector3 TransformWorldToScreen(Vector3 value)
        {
            return GameData.data.localPlayer.MatrixViewProjectionViewport.Transform(value);
        }
    }
}
