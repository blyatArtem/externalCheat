using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSLiba.Game.Objects;
using CSLiba.Core;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using CSLiba.UI.ConfigSystem.ConfigStructs;
using CSLiba.UI.ConfigSystem;
using CSLiba.Imports;
using CSLiba.Game.Structs;

namespace CSLiba.Game.Features
{
    public static class Triggerbot
    {
        public static void FrameAction(GameData data)
        {
            if (data.localPlayer.activeWeapon != null)
            {
                SerializebleWeapon weapon =
                    data.localPlayer.activeWeapon.IsPistol() ? Configuration.current.pistol :
                    data.localPlayer.activeWeapon.IsSMG() ? Configuration.current.smg :
                    data.localPlayer.activeWeapon.IsHeavy() ? Configuration.current.heavy :
                    data.localPlayer.activeWeapon.IsRifle() ? Configuration.current.rifle :
                    (data.localPlayer.activeWeapon.IsAutSniper() || data.localPlayer.activeWeapon.IsSniper()) ? Configuration.current.sniper : null;

                if (weapon == null || !weapon.triggerbot || weapon.triggerbotKey.Where(x => User32.GetAsyncKeyState(x) < 0).ToList().Count == 0)
                    return;

                UpdateTrigger(data);
            }
        }

        private static void UpdateTrigger(GameData data)
        {
            LocalPlayer player = data.localPlayer;
            if (player.AimDirection.Length() < 0.001d)
                return;

            Line3D aimRayWorld = new Line3D(player.EyePosition, player.EyePosition + player.AimDirection * 8192);

            foreach (Entity entity in data.Entities)
            {
                if (!entity.IsAlive() || entity.basePtr == player.basePtr)
                    continue;
                if (!Configuration.current.onTeam && entity.team == data.localPlayer.team)
                    continue;

                entity.UpdateStudioHitBoxes();
                entity.UpdateBonesMatricesAndPos();

                int hitBoxId = IntersectsHitBox(aimRayWorld, entity);
                if (hitBoxId >= 0)
                    Mouse.LeftClick();
            }
        }

        private static int IntersectsHitBox(Line3D aimRayWorld, Entity entity)
        {
            for (var hitBoxId = 0; hitBoxId < entity.StudioHitBoxSet.numhitboxes; hitBoxId++)
            {
                var hitBox = entity.StudioHitBoxes[hitBoxId];
                var boneId = hitBox.bone;
                if (boneId < 0 || boneId > 128 || hitBox.radius <= 0)
                {
                    continue;
                }

                // intersect capsule
                var matrixBoneModelToWorld = entity.BonesMatrices[boneId];
                var boneStartWorld = matrixBoneModelToWorld.Transform(hitBox.bbmin);
                var boneEndWorld = matrixBoneModelToWorld.Transform(hitBox.bbmax);
                var boneWorld = new Line3D(boneStartWorld, boneEndWorld);
                var (p0, p1) = aimRayWorld.ClosestPointsBetween(boneWorld, true);
                var distance = (p1 - p0).Length();
                if (distance < hitBox.radius * 0.9f)
                {
                    // intersects
                    return hitBoxId;
                }
            }

            return -1;
        }
    }
}
