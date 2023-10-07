using CSLiba.Game.Structs;
using CSLiba.Core;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CSLiba.Game.Enumerations;
using System.Diagnostics;
using CSLiba.Game.Features;
using CSLiba.UI.ConfigSystem;

namespace CSLiba.Game.Objects
{
    public class Entity : EntityBase
    {
        public Entity(int index)
        {
            this.index = index;
            StudioHitBoxes = new mstudiobbox_t[128];
            StudioBones = new mstudiobone_t[128];
            BonesMatrices = new Matrix[128];
            BonesPos = new Vector3[128];
            Skeleton = new (int, int)[128];
            activeWeapon = new Weapon(this);
            faceitInfo = new FaceitInfo();

        }

        public override int UpdateAdressBase()
        {
            return Memory.Read<int>(Memory.clientBase + Offsets.dwEntityList + index * 0x10);
        }
        public override bool IsAlive()
        {
            return base.IsAlive() && !dormant;
        }

        public override bool Update()
        {
            clientState = Memory.Read<int>(Memory.engineBase + Offsets.dwClientState);
            if (!base.Update())
            {
                    h = -1;
                return false;
            }

            UpdatePlayerInfo();
            UpdateCompetitiveInfo();

            dormant = Memory.Read<bool>(basePtr + Offsets.m_bDormant);

            UpdateHD();

            if (!IsAlive())
            {
                    h = -1;
                return true;
            }

            if (basePtr != Memory.Read<int>(Memory.clientBase + Offsets.dwLocalPlayer))
            {
                activeWeapon.Update();
                UpdateFields();
                UpdateStudioHdr();
                UpdateStudioHitBoxes();
                UpdateStudioBones();
                UpdateBonesMatricesAndPos();
                UpdateSkeleton();
                UpdateBoxes();
            }

            h = health;

            return true;
        }

        private void UpdateHD()
        {
            if (h != -1 && h > health)
            {
                if (GameData.data.localPlayer.team != team && Configuration.current.hitDrawer)
                    HitDrawer.hits.Add(new Hit(h - health, BonesPos[8]));
            }
        }

        private void UpdateFields()
        {
            spottedByMask = Memory.Read<bool>(basePtr + Offsets.m_bSpottedByMask);
            defusing = Memory.Read<bool>(basePtr + Offsets.m_bIsDefusing);

            scoped = activeWeapon != null && activeWeapon.basePtr != 0 && activeWeapon.IsSniper() && Memory.Read<bool>(basePtr + Offsets.m_bIsScoped);
        }

        private void UpdatePlayerInfo()
        {
            int CurrentUserInformationPointer = Memory.Read<int>(clientState + Offsets.dwClientState_PlayerInfo);
            int One = Memory.Read<int>(CurrentUserInformationPointer + 0x40);
            int Two = Memory.Read<int>(One + 0xC);
            int playerInformationStructure = Memory.Read<int>(Two + 0x28 + (index * 0x34));
            steamID = Memory.ReadString(playerInformationStructure + 148, 20, Encoding.UTF8);
            nickname = Memory.ReadString(playerInformationStructure + 0x10, 128, Encoding.UTF8);
        }

        private void UpdateCompetitiveInfo()
        {
            int playerResource = Memory.Read<int>(Memory.clientBase + Offsets.dwPlayerResource);
            wins = Memory.Read<int>(playerResource + Offsets.m_iCompetitiveWins + ((index + 1) * 4));
            rank = Memory.Read<int>(playerResource + Offsets.m_iCompetitiveRanking + ((index + 1) * 4));
        }

        private void UpdateBoxes()
        {
            Box = new GameOverlay.Drawing.Rectangle();
            Vector3 v3headPos = BonesPos[8];
            Vector3 v2HeadPos = GameData.data.localPlayer.MatrixViewProjectionViewport.Transform(v3headPos);
            Vector3 v3Pos = origin;
            Vector3 v2Pos = GameData.data.localPlayer.MatrixViewProjectionViewport.Transform(v3Pos);

            if (!v2HeadPos.IsValidScreen() || !v2Pos.IsValidScreen())
                return;

            float boxHeight = v2Pos.Y - v2HeadPos.Y;
            float boxWidth = (boxHeight / 2) * 1.25f;

            Box = new GameOverlay.Drawing.Rectangle(v2Pos.X - (boxWidth / 2), v2HeadPos.Y - (boxHeight / 8) + 1, v2Pos.X - (boxWidth / 2) + boxWidth, v2HeadPos.Y + boxHeight);
        }

        private void UpdateStudioHdr()
        {
            int addressToAddressStudioHdr = Memory.Read<int>(basePtr + Offsets.m_pStudioHdr);
            AddressStudioHdr = Memory.Read<int>(addressToAddressStudioHdr);
            StudioHdr = Memory.Read<studiohdr_t>(AddressStudioHdr);
        }

        public void UpdateStudioHitBoxes()
        {
            var addressHitBoxSet = AddressStudioHdr + StudioHdr.hitboxsetindex;
            StudioHitBoxSet = Memory.Read<mstudiohitboxset_t>(addressHitBoxSet);

            for (var i = 0; i < StudioHitBoxSet.numhitboxes; i++)
            {
                StudioHitBoxes[i] = Memory.Read<mstudiobbox_t>(addressHitBoxSet + StudioHitBoxSet.hitboxindex + i * Marshal.SizeOf<mstudiobbox_t>());
            }
        }

        private void UpdateStudioBones()
        {
            for (var i = 0; i < StudioHdr.numbones; i++)
            {
                StudioBones[i] = Memory.Read<mstudiobone_t>(AddressStudioHdr + StudioHdr.boneindex + i * Marshal.SizeOf<mstudiobone_t>());
            }
        }

        public void UpdateBonesMatricesAndPos()
        {
            int addressBoneMatrix = Memory.Read<int>(basePtr + Offsets.m_dwBoneMatrix);
            for (var boneId = 0; boneId < BonesPos.Length; boneId++)
            {
                var matrix = Memory.Read<matrix3x4_t>(addressBoneMatrix + boneId * Marshal.SizeOf<matrix3x4_t>());
                BonesMatrices[boneId] = matrix.ToMatrix();
                BonesPos[boneId] = new Vector3(matrix.m30, matrix.m31, matrix.m32);
            }
        }

        private void UpdateSkeleton()
        {
            var skeletonBoneId = 0;
            for (var i = 0; i < StudioHitBoxSet.numhitboxes; i++)
            {
                var hitbox = StudioHitBoxes[i];
                var bone = StudioBones[hitbox.bone];
                if (bone.parent >= 0 && bone.parent < StudioHdr.numbones)
                {
                    Skeleton[skeletonBoneId] = (hitbox.bone, bone.parent);
                    skeletonBoneId++;
                }
            }
            skeletonCount = skeletonBoneId;
        }

        public int clientState;

        public bool local;

        public int index;

        public bool dormant = true;

        public Weapon activeWeapon;

        public GameOverlay.Drawing.Rectangle Box { get; set; }

        #region entity files

        private int h = -1;
        public bool spottedByMask, scoped, defusing;

        #endregion

        #region players info

        public string nickname, steamID;
        public int wins, rank;

        #endregion

        #region hitboxes and bones

        private int AddressStudioHdr { get; set; }

        public studiohdr_t StudioHdr { get; private set; }

        public mstudiohitboxset_t StudioHitBoxSet { get; private set; }

        public mstudiobbox_t[] StudioHitBoxes { get; }

        public mstudiobone_t[] StudioBones { get; }

        public Matrix[] BonesMatrices { get; }

        public Vector3[] BonesPos { get; }

        public int skeletonCount;

        public (int from, int to)[] Skeleton { get; }

        public FaceitInfo faceitInfo;

        #endregion
    }
}
