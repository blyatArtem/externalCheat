using CSLiba.Core;
using CSLiba.Game.Objects;
using CSLiba.Imports;
using CSLiba.UI.ConfigSystem;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSLiba.Game.Features
{
    public static class EspWorld
    {
        public static void FrameAction(GameData data)
        {
            if (!Configuration.current.groundedWeapons && !Configuration.current.bombTimer && !Configuration.current.grenades)
                return;
            lock (block)
            {
                bool bombPlanted = false;
                items.Clear();
                grenades.Clear();
                int entityListCount = Memory.Read<int>(Memory.clientBase + Offsets.dwEntityList + 4);
                for (int i = 0; i < 2048; i++)
                {
                    int entity = Memory.Read<int>(Memory.clientBase + Offsets.dwEntityList + i * 0x10);
                    if (entity == 0)
                        continue;
                    int classId = GetClassID(entity);
                    if (classId == 129)
                    {
                        bombPlanted = true;
                        //planted bomb
                        AddItem(entity, "", false);
                        AddGrenadeItem(entity, "", false);
                        bombTimer = Memory.Read<float>(entity + Offsets.m_flC4Blow) - GameData.data.localPlayer.globalVars.m_flCurTime;
                    }
                    else if (classId == 34)
                    {
                        //dropped bomb
                        AddItem(entity, "", false);
                        AddGrenadeItem(entity, "", false);
                    }
                    else if (classId == 53)
                    {
                        //defuser
                        AddItem(entity, "", false);
                    }
                    else if (classId == 1)
                    {
                        //Ak47
                        AddItem(entity, "", true);
                    }
                    else if (classId == 249)
                    {
                        //M4A4 or M4A1-S
                        AddItem(entity, "", true);
                    }
                    else if (classId == 265)
                    {
                        //SG553
                        AddItem(entity, "", true);
                    }
                    else if (classId == 240)
                    {
                        //Famas
                        AddItem(entity, "", true);
                    }
                    else if (classId == 244)
                    {
                        //Galil
                        AddItem(entity, "", true);
                    }
                    else if (classId == 232)
                    {
                        //AUG
                        AddItem(entity, "", true);
                    }
                    else if (classId == 233)
                    {
                        //AWP
                        AddItem(entity, "", true);
                    }
                    else if (classId == 267)
                    {
                        //ssg08
                        AddItem(entity, "", true);
                    }
                    else if (classId == 242)
                    {
                        //auto sniper (T)
                        AddItem(entity, "", true);
                    }
                    else if (classId == 261)
                    {
                        //auto sniper (CT)
                        AddItem(entity, "", true);
                    }
                    else if (classId == 269)
                    {
                        //tec9
                        AddItem(entity, "", true);
                    }
                    else if (classId == 246)
                    {
                        //usp-s
                        AddItem(entity, "", true);
                    }
                    else if (classId == 239)
                    {
                        //Dual berettas (elite)
                        AddItem(entity, "", true);
                    }
                    else if (classId == 241)
                    {
                        //57
                        AddItem(entity, "", true);
                    }
                    else if (classId == 258)
                    {
                        //250
                        AddItem(entity, "", true);
                    }
                    else if (classId == 258)
                    {
                        //CZ
                        AddItem(entity, "", true);
                    }
                    else if (classId == 46)
                    {
                        //deagle
                        AddItem(entity, "", true);
                    }
                    else if (classId == 245)
                    {
                        //glock
                        AddItem(entity, "", true);
                    }
                    else if (classId == 255)
                    {
                        //negev
                        AddItem(entity, "", true);
                    }
                    else if (classId == 247)
                    {
                        //m249
                        AddItem(entity, "", true);
                    }
                    else if (classId == 256)
                    {
                        //nova
                        AddItem(entity, "", true);
                    }
                    else if (classId == 260)
                    {
                        //sawed-off
                        AddItem(entity, "", true);
                    }
                    else if (classId == 251)
                    {
                        //MAG-7
                        AddItem(entity, "", true);
                    }
                    else if (classId == 273)
                    {
                        //XM-1024
                        AddItem(entity, "", true);
                    }
                    else if (classId == 254)
                    {
                        //mp-9
                        AddItem(entity, "", true);
                    }
                    else if (classId == 235)
                    {
                        //bizon
                        AddItem(entity, "", true);
                    }
                    else if (classId == 259)
                    {
                        //p90
                        AddItem(entity, "", true);
                    }
                    else if (classId == 250)
                    {
                        //mac-10
                        AddItem(entity, "", true);
                    }
                    else if (classId == 253)
                    {
                        //mp-7
                        AddItem(entity, "", true);
                    }
                    else if (classId == 271)
                    {
                        //ump
                        AddItem(entity, "", true);
                    }
                    else if (classId == 157 || classId == 114 || classId == 100 || classId == 9 || classId == 48)
                    {
                        AddGrenadeItem(entity, "", false);
                    }
                }
                if (!bombPlanted)
                    bombTimer = -1f;
            }
        }

        public static void FrameAction(Graphics g, GameData data)
        {
            if (Configuration.current.groundedWeapons)
                lock (block)
                {
                    foreach (WorldItem item in items)
                    {
                        if (item.position == Vector3.Zero || !item.position.IsValidScreen())
                            continue;
                        g.DrawText(Overlay.fontIcons, Overlay.GetBrushByVector(g, Configuration.current.weaponsEspColor), item.position.X, item.position.Y, item.text);
                    }
                }
        }

        private static void AddItem(int entity, string text, bool weapon)
        {
            Vector3 worldPos = Memory.Read<Vector3>(entity + Offsets.m_vecOrigin);
            if (worldPos == Vector3.Zero)
                return;
            items.Add(new WorldItem(GameData.data.localPlayer.MatrixViewProjectionViewport.Transform(worldPos), weapon ? text + "|" + Memory.Read<int>(entity + Offsets.m_iClip1) + "/" + Memory.Read<int>(entity + Offsets.m_iClip1 + Marshal.SizeOf(typeof(int)) * 2) : text));
        }

        private static void AddGrenadeItem(int entity, string text, bool weapon)
        {
            Vector3 worldPos = Memory.Read<Vector3>(entity + Offsets.m_vecOrigin);
            if (worldPos == Vector3.Zero)
                return;
            grenades.Add(new WorldItem(worldPos, text));
        }

        private static int GetClassID(int entityBase)
        {
            int one = Memory.Read<int>(entityBase + 0x8);
            int two = Memory.Read<int>(one + 0x4 * 2);
            int three = Memory.Read<int>(two + 0x1);
            return Memory.Read<int>(three + 0x14);
        }

        public static object block = new object();
        public static float bombTimer;

        public static List<WorldItem> items = new List<WorldItem>();
        public static List<WorldItem> grenades = new List<WorldItem>();
    }
}
