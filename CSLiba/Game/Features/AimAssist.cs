using CSLiba.Core;
using CSLiba.Game.Enumerations;
using CSLiba.Game.Objects;
using CSLiba.Game.Structs;
using CSLiba.Imports;
using CSLiba.UI.ConfigSystem;
using CSLiba.UI.ConfigSystem.ConfigStructs;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSLiba.Game.Features
{
    public static class AimAssist
    {
        public static Vector3 FrameAction(GameData data, bool justCheck = false)
        {
            if (Configuration.current.aimToggleKey.Where(x => User32.GetAsyncKeyState(x) != 0).ToList().Count > 0 && !justCheck)
            {
                aimToggle = !aimToggle;
                Console.Beep(440, 300);
                Thread.Sleep(500);
                return Vector3.Zero;
            }

            if (Configuration.current.whToggleKey.Where(x => User32.GetAsyncKeyState(x) != 0).ToList().Count > 0 && !justCheck)
            {
                whToggle = !whToggle;
                Console.Beep(440, 300);
                Thread.Sleep(500);
            }

            if (!aimToggle)
                return Vector3.Zero;

            if (data.localPlayer.activeWeapon != null)
            {
                SerializebleWeapon weapon =
                    data.localPlayer.activeWeapon.IsPistol() ? Configuration.current.pistol :
                    data.localPlayer.activeWeapon.IsSMG() ? Configuration.current.smg :
                    data.localPlayer.activeWeapon.IsHeavy() ? Configuration.current.heavy :
                    data.localPlayer.activeWeapon.IsRifle() ? Configuration.current.rifle :
                    (data.localPlayer.activeWeapon.IsAutSniper() || data.localPlayer.activeWeapon.IsSniper()) ? Configuration.current.sniper : null;

                if (weapon == null || (!weapon.aimAssist || !CanShoot(data, weapon.canFireCheck)) && !justCheck)
                    return Vector3.Zero;

                if (weapon.aimAssistKey.Where(x => User32.GetAsyncKeyState(x) != 0).ToList().Count > 0 || justCheck)
                {

                    Vector3 viewAngles = data.localPlayer.ViewAngles;
                    Vector3 localEyePos = data.localPlayer.EyePosition;

                    List<Entity> entities = data.Entities.Where(x => x.IsAlive()).ToList();
                    if (!Configuration.current.onTeam)
                        entities = entities.Where(x => x.team != data.localPlayer.team).ToList();
                    if (weapon.spottedAim)
                        entities = entities.Where(x => x.spottedByMask).ToList();

                    Vector3 angleRCS = !weapon.rcs ? Vector3.Zero : data.localPlayer.AimPunchAngle * 2;
                    if (entities.Count > 0)
                    {
                        entities = entities.OrderBy(x => GetFov(CalcAngle(localEyePos, x.BonesPos[weapon.aimBone], viewAngles + angleRCS))).ToList();
                        foreach (Entity entity in entities)
                        {
                            if (justCheck)
                                return entity.BonesPos[weapon.aimBone];

                            if (target != -1 && weapon.targetLock)
                            {
                                if (target != entity.basePtr)
                                    continue;
                            }


                            int localPlayerPtr = Memory.Read<int>(Memory.clientBase + Offsets.dwLocalPlayer);

                            //if (weapon.jumpDelay)
                            //{
                            //    int flag = Memory.Read<int>(entity.basePtr + Offsets.m_fFlags);
                            //    if (flag != 769 && flag != 775)
                            //    {
                            //        return Vector3.Zero;
                            //    }
                            //}

                            if (weapon.flashDelay)
                            {
                                float flashedDuration = Memory.Read<float>(localPlayerPtr + Offsets.m_flFlashDuration);
                                if (flashedDuration > 0f)
                                {
                                    return Vector3.Zero;
                                }
                            }

                            float xRcsSmooth = weapon.rcsSmoothX / 10;
                            Vector3 angle = CalcAngle(localEyePos,
                            entity.BonesPos[weapon.aimBone],
                            viewAngles + new Vector3(angleRCS.X, weapon.rcsSmoothX > 10 ? angleRCS.Y / xRcsSmooth : angleRCS.Y, 0));

                            float fov = GetFov(angle);
                            if (fov < weapon.fov)
                            {
                                if (target == -1 && weapon.targetLock)
                                    target = entity.basePtr;

                                int xMove = -(int)(angle.Y * weapon.xSpeed);
                                int yMove = (int)(angle.X * weapon.ySpeed);

                                if (xMove > 0)
                                    xMove += 2;
                                else if (xMove != 0)
                                    xMove -= 2;

                                if (yMove > 0)
                                    yMove += 2;
                                else if (yMove != 0)
                                    yMove -= 2;

                                if (data.localPlayer.activeWeapon.IsSniper() & data.localPlayer.IsScoped)
                                {
                                    xMove *= 3;
                                    yMove *= 3;
                                }

                                int shakeOffsetX = 0;
                                int shakeOffsetY = 0;
                                if (weapon.shake)
                                {
                                    sinWaveShakeX += 0.3f;
                                    sinWaveShakeY += 0.1f;
                                    shakeOffsetX = (int)(Math.Sin(sinWaveShakeX) * weapon.xShake);
                                    shakeOffsetY = (int)(Math.Sin(sinWaveShakeY) * weapon.yShake);
                                }
                                Mouse.Move(xMove + shakeOffsetX, yMove + shakeOffsetY);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    target = -1;
                    sinWaveShakeX = 0;
                    sinWaveShakeY = 0;
                }
            }
            return Vector3.Zero;
        }

        public static Vector3 targetPos;

        private static bool CanShoot(GameData data, bool PrimaryAttackCheck = false)
        {
            bool flag1 = true;
            int weaponBase = data.localPlayer.activeWeapon.basePtr;
            if (PrimaryAttackCheck)
            {
                globalVars_t globalVars = data.localPlayer.globalVars;
                float a = NextPrimaryAttack(weaponBase);
                float b = globalVars.m_flCurTime;
                flag1 = a < b;
            }
            int ammo = Memory.Read<int>(weaponBase + Offsets.m_iClip1);
            bool flag2 = ammo > 0;
            bool flag3 = !Memory.Read<bool>(weaponBase + Offsets.m_bInReload);
            return flag1 && flag2 && flag3;
        }

        public static float NextPrimaryAttack(int weaponBase)
        {
            return Memory.Read<float>(weaponBase + Offsets.m_flNextPrimaryAttack);
        }

        private static float sinWaveShakeX;
        private static float sinWaveShakeY;

        private static int target = -1;

        private static double Hypot(float a, float b) => Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

        private static float GetFov(Vector3 angle) => (float)Math.Sqrt(Math.Pow(angle.X, 2) + (float)Math.Pow(angle.Y, 2));

        private static Vector3 ToAngle(Vector3 vec3) =>
            new Vector3(
                (float)(Math.Atan2(-vec3.Z, Hypot(vec3.X, vec3.Y)) * (180 / Math.PI)),
                (float)(Math.Atan2(vec3.Y, vec3.X) * (180f / Math.PI)),
                0);

        private static Vector3 CalcAngle(Vector3 localPos, Vector3 targerPos, Vector3 viewAngles) =>
            ToAngle(targerPos - localPos) - viewAngles;

        public static bool aimToggle = true, whToggle = true;
    }
}
