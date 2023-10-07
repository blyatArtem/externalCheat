using CSLiba.Game.Features;
using CSLiba.Game.Objects;
using CSLiba.Core;
using GameOverlay.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CSLiba.UI.ConfigSystem;
using CSLiba.Imports;
using CSLiba.Game.Structs;
using System.Runtime.InteropServices;

namespace CSLiba.Game
{
    public class GameData
    {
        public static GameData data;

        public static void Initialize(Graphics g)
        {
            data = new GameData();
            data.localPlayer = new LocalPlayer();
            data.Entities = new List<Entity>();
            for (int i = 0; i < 32; i++)
            {
                data.Entities.Add(new Entity(i));
            }

            data.threads.Add(new GameThread(new Action(delegate ()
            {
                Triggerbot.FrameAction(data);
            }), "Triggerbot", g));

            data.threads.Add(new GameThread(new Action(delegate ()
            {
                AimAssist.FrameAction(data);
            }), "Aimbot", g));

            data.threads.Add(new GameThread(new Action(delegate ()
            {
                Bhop.FrameAction(data);
            }), "Bhop", g));

            data.threads.Add(new GameThread(new Action(delegate ()
            {
                EspWorld.FrameAction(data);
            }), "World list", g));

            data.threads.Add(new GameThread(new Action(delegate ()
            {
                RankViewer.FrameAction(data);
            }), "World list", g));
        }

        public static void FrameAction(Graphics g, int frameCount)
        {
            g.ClearScene(Color.Transparent);

            if (frameCount == 1)
            {
                data.threads.ForEach(x => x.RememberValue());
            }

            int clientPtr = Memory.Read<int>(Memory.engineBase + Offsets.dwClientState);
            string mapName = Memory.ReadString(clientPtr + Offsets.dwClientState_Map, 0x80, Encoding.UTF8);

            //data.stopwatch.Start();

            if (Memory.Read<int>(clientPtr + Offsets.dwClientState_State) == 6)
            {
                data.localPlayer.Update();

                foreach (Entity entity in data.Entities)
                    entity.Update();

                //World

                if (AimAssist.whToggle)
                {
                    EspWorld.FrameAction(g, data);
                    HitDrawer.FrameAction(g, data);
                    EspHitboxes.FrameAction(g, data);
                    EspSkeleton.FrameAction(g, data);
                    EspBoxes.FrameAction(g, data);
                    EspString.FrameAction(g, data);
                    SmokeHelper.FrameAction(g, data, mapName);
                }

                //Game screen
                FovDrawer.FrameAction(g, data);
                AimCrosshair.FrameAction(g, data);

                //Screen
                EspRadar.FrameAction(g, data);
                RankViewer.FrameAction(g, data);
                SpectatorList.FrameAction(g, data);
                EspBombTimer.FrameAction(g, data);

                string str = "";
                if (Configuration.current.aimToggleKey.Count > 0)
                    str += "Aim: " + (AimAssist.aimToggle ? "ON " : "OFF ") + string.Join(" ", Configuration.current.aimToggleKey) + "\n";
                if (Configuration.current.whToggleKey.Count > 0)
                    str += "WH: " + (AimAssist.whToggle ? "ON " : "OFF ") + string.Join(" ", Configuration.current.whToggleKey);

                if (str != "")
                    g.DrawText(Overlay.fontText, Overlay.brushText, 10, 10, str);
            }
            //g.DrawText(Overlay.fontText, Overlay.brushText, 500, 10, g.FPS.ToString());

            //data.stopwatch.Stop();

            //long elapsedMilliseconds = data.stopwatch.ElapsedMilliseconds;

            //if (Configuration.displayThreads)
            //{
            //    Point point = new Point(0, 0);
            //    float offset = 30f;
            //    int index = 1;
            //    float height = 10f;
            //    g.FillRectangle(g.CreateSolidBrush(0, 0, 0, 255), 0, 0, 800, point.Y + ((height + offset) * (1 + data.threads.Count)));
            //    g.DrawText(Overlay.fontIcons, Overlay.brushText, point, $"FPS: {g.FPS} FrameAction: " + elapsedMilliseconds.ToString() + "ms");
            //    foreach (GameThread thr in data.threads)
            //    {
            //        g.DrawText(Overlay.fontIcons, thr.brush, point.X, point.Y + ((height + offset) * index), thr.name + ": " + thr.elapsedMilliseconds.ToString() + "ms");
            //        int i = 2;
            //        float preValue = 0;
            //        foreach (float value in thr.msList)
            //        {
            //            g.DrawLine(thr.brush, point.X + 230 + ((i - 1) * 7), 40f + point.Y + ((height + offset) * (index + 1) - (height + offset)) - preValue, point.X + 230 + (i * 7), 40f + point.Y + ((height + offset) * (index + 1) - (height + offset)) - value, 2);
            //            i++;
            //            preValue = value;
            //        }
            //        index++;
            //    }

            //    data.stopwatch.Reset();
            //}
        }

        public static void SendCommand(string command)
        {
            try
            {
                // Copy into unmanaged memory
                IntPtr buffer = Marshal.StringToHGlobalAnsi(command);

                // Get console handle
                var m_hEngine = User32.FindWindowA("Valve001", 0);

                // Create console input struct
                COPYDATASTRUCT copyData = new COPYDATASTRUCT();

                copyData.dwData = IntPtr.Zero;
                copyData.lpData = buffer;
                copyData.cbData = command.Length + 1;

                IntPtr copyDataBuff = IntPtrAlloc(copyData);

                // WM_COPYDATA = 0x004A;
                User32.SendMessage(m_hEngine, 0x004A, IntPtr.Zero, copyDataBuff);

                IntPtrFree(ref copyDataBuff);
                IntPtrFree(ref buffer);
            }
            catch
            {

            }
        }
        private static IntPtr IntPtrAlloc<T>(T param)
        {
            IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }
        private static void IntPtrFree(ref IntPtr preAllocated)
        {
            if (IntPtr.Zero == preAllocated)
                throw (new NullReferenceException("Go Home"));
            Marshal.FreeHGlobal(preAllocated);
            preAllocated = IntPtr.Zero;
        }

        public LocalPlayer localPlayer;

        private Stopwatch stopwatch = new Stopwatch();
        private List<GameThread> threads = new List<GameThread>();
        public List<Entity> Entities;
    }
}
