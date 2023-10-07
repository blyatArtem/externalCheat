using CSLiba.Core;
using CSLiba.Game.Structs;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLiba.Game.Objects
{
    public class LocalPlayer : EntityBase
    {
        public LocalPlayer()
        {
            activeWeapon = new Weapon(this);
        }

        public override int UpdateAdressBase()
        {
            return Memory.Read<int>(Memory.clientBase + Offsets.dwLocalPlayer);
        }

        public override bool Update()
        {
            if (!base.Update())
            {
                return false;
            }

            UpdateWeapon();

            UpdateMatrix();
            UpdateAngles();

            if (Fov == 0)
                Fov = 90;

            UpdateFields();

            return true;
        }

        private void UpdateFields()
        {
            AimDirection = GetAimDirection(ViewAngles, AimPunchAngle);
            IsScoped = Memory.Read<bool>(basePtr + Offsets.m_bIsScoped);
            globalVars = Memory.Read<globalVars_t>(Memory.engineBase + Offsets.dwGlobalVars);
            observerTarget = Memory.Read<int>(basePtr + Offsets.m_hObserverTarget);
            flag = Memory.Read<int>(basePtr + Offsets.m_fFlags);
        }

        private void UpdateMatrix()
        {
            Matrix MatrixViewProjection = Matrix.Transpose(Memory.Read<Matrix>(Memory.clientBase + Offsets.dwViewMatrix));
            MatrixViewport = Core.MathHelper.GetMatrixViewport(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            MatrixViewProjectionViewport = MatrixViewProjection * MatrixViewport;
        }

        private void UpdateAngles()
        {
            Vector3 ViewOffset = Memory.Read<Vector3>(basePtr + Offsets.m_vecViewOffset);
            EyePosition = origin + ViewOffset;
            ViewAngles = Memory.Read<Vector3>(Memory.Read<int>(Memory.engineBase + Offsets.dwClientState) + Offsets.dwClientState_ViewAngles);
            AimPunchAngle = Memory.Read<Vector3>(basePtr + Offsets.m_aimPunchAngle);
            Fov = Memory.Read<int>(basePtr + Offsets.m_iFOV);
        }

        private void UpdateWeapon()
        {
            activeWeapon.Update();
        }

        private static Vector3 GetAimDirection(Vector3 viewAngles, Vector3 aimPunchAngle)
        {
            var phi = (viewAngles.X + aimPunchAngle.X * 2f).DegreeToRadian();
            var theta = (viewAngles.Y + aimPunchAngle.Y * 2f).DegreeToRadian();

            // https://en.wikipedia.org/wiki/Spherical_coordinate_system
            return new Vector3
            (
                (float)(Math.Cos(phi) * Math.Cos(theta)),
                (float)(Math.Cos(phi) * Math.Sin(theta)),
                (float)-Math.Sin(phi)
            ).Normalized();
        }

        public Matrix MatrixViewport;

        public Matrix MatrixViewProjectionViewport;

        public Vector3 EyePosition;

        public Vector3 AimPunchAngle;

        public Vector3 ViewAngles;

        public Vector3 AimDirection;

        public globalVars_t globalVars;

        public int Fov;

        public bool IsScoped;

        public int observerTarget;

        public int flag;

        public Weapon activeWeapon;
    }
}