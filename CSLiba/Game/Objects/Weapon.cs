using CSLiba.Core;
using GameOverlay.Drawing;
using CSLiba.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CSLiba.Game.Objects
{
    public class Weapon : EntityBase
    {
        public Weapon(EntityBase owner)
        {
            this.owner = owner;
        }

        public override int UpdateAdressBase()
        {
            int ptr = Memory.Read<int>(owner.basePtr + Offsets.m_hActiveWeapon) & 0xFFF;
            return Memory.Read<int>(Memory.clientBase + Offsets.dwEntityList + (ptr - 1) * 0x10);
        }

        public override bool IsAlive()
        {
            return true;
        }

        public override bool Update()
        {
            basePtr = UpdateAdressBase();
            if (basePtr != 0)
            {
                ammo = Memory.Read<int>(basePtr + Offsets.m_iClip1);
                ammo2 = Memory.Read<int>(basePtr + Offsets.m_iClip1 + Marshal.SizeOf(typeof(int)) * 2);
                UpdateDefinitionIndex();
                return true;
            }
            definitionIndex = ItemDefinitionIndex.NULL;
            return true;
        }

        private void UpdateDefinitionIndex()
        {
            definitionIndex = (ItemDefinitionIndex)Memory.Read<short>(basePtr + Offsets.m_iItemDefinitionIndex);
        }

        public bool IsKnife()
        {
            return definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_T ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_BAYONET ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_GUT ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_KARAMBIT ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_FLIP ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_BUTTERFLY ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_TACTICAL ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_FALCHION ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_M9_BAYONET ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_SURVIVAL_BOWIE ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_PUSH ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_URSUS ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_GYPSY_JACKKNIFE ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_STILETTO ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_WIDOWMAKER ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_CSS ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_CORD ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_CANIS ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_SKELETON ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_OUTDOOR;
        }
        public bool IsSniper()
        {
            return definitionIndex == ItemDefinitionIndex.WEAPON_AWP ||
                definitionIndex == ItemDefinitionIndex.WEAPON_SSG08;
        }
        public bool IsAutSniper()
        {
            return definitionIndex == ItemDefinitionIndex.WEAPON_SCAR20 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_G3SG1;
        }
        public bool IsPistol()
        {
            return definitionIndex == ItemDefinitionIndex.WEAPON_HKP2000 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_USP_SILENCER ||
                definitionIndex == ItemDefinitionIndex.WEAPON_GLOCK ||
                definitionIndex == ItemDefinitionIndex.WEAPON_ELITE ||
                definitionIndex == ItemDefinitionIndex.WEAPON_P250 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_FIVESEVEN ||
                definitionIndex == ItemDefinitionIndex.WEAPON_TEC9 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_CZ75A ||
                definitionIndex == ItemDefinitionIndex.WEAPON_DEAGLE;
        }
        public bool IsHeavy()
        {
            return definitionIndex == ItemDefinitionIndex.WEAPON_NOVA ||
                definitionIndex == ItemDefinitionIndex.WEAPON_XM1014 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_SAWEDOFF ||
                definitionIndex == ItemDefinitionIndex.WEAPON_MAG7 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_M249 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_NEGEV;
        }
        public bool IsRifle()
        {
            return definitionIndex == ItemDefinitionIndex.WEAPON_FAMAS ||
                definitionIndex == ItemDefinitionIndex.WEAPON_GALILAR ||
                definitionIndex == ItemDefinitionIndex.WEAPON_M4A1 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_M4A1_SILENCER ||
                definitionIndex == ItemDefinitionIndex.WEAPON_AK47 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_AK47 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_AUG ||
                definitionIndex == ItemDefinitionIndex.WEAPON_SG556;
        }

        public bool IsSMG()
        {
            return definitionIndex == ItemDefinitionIndex.WEAPON_MP9 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_MAC10 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_MP7 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_MP5_SD ||
                definitionIndex == ItemDefinitionIndex.WEAPON_UMP45 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_P90 ||
                definitionIndex == ItemDefinitionIndex.WEAPON_BIZON;
        }

        public string GetWeaponIcon()
        {
            return GetWeaponIcon(definitionIndex);
        }

        public static string GetWeaponIcon(ItemDefinitionIndex definitionIndex)
        {
            if (definitionIndex == ItemDefinitionIndex.WEAPON_ELITE)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_FIVESEVEN)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_GLOCK)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_DEAGLE)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_AK47)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_AUG)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_AWP)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_FAMAS)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_G3SG1)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_GALILAR)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_M4A1)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_M4A1_SILENCER)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_MAC10)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_HKP2000)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_UMP45)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_XM1014)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_BIZON)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_MAG7)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_NEGEV)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_SAWEDOFF)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_TEC9)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_TASER)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_P250)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_MP7)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_MP9)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_NOVA)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_P90)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_SCAR20)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_SG556)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_MP5_SD)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_SSG08)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_FLASHBANG)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_HEGRENADE)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_SMOKEGRENADE)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_MOLOTOV)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_DECOY)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_INCGRENADE)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_C4)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_M249)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_USP_SILENCER)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_CZ75A)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_REVOLVER)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_BAYONET)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_GUT)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_KARAMBIT)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_M9_BAYONET)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_TACTICAL)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_FALCHION)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_SURVIVAL_BOWIE)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_BUTTERFLY)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_PUSH)
                return "";
            else if (definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_T ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_BAYONET ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_GUT ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_KARAMBIT ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_FLIP ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_BUTTERFLY ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_TACTICAL ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_FALCHION ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_M9_BAYONET ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_SURVIVAL_BOWIE ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_PUSH ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_URSUS ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_GYPSY_JACKKNIFE ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_STILETTO ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_WIDOWMAKER ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_CSS ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_CORD ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_CANIS ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_SKELETON ||
                definitionIndex == ItemDefinitionIndex.WEAPON_KNIFE_OUTDOOR)
                return "";
            return "";
        }

        public ItemDefinitionIndex definitionIndex;

        public int ammo;
        public int ammo2;

        private EntityBase owner;
    }
}
