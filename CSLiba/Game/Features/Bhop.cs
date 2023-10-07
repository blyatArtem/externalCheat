using CSLiba.Core;
using CSLiba.Imports;
using CSLiba.UI.ConfigSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLiba.Game.Features
{
    public static class Bhop
    {
        public static void FrameAction(GameData data)
        {
            if (!Configuration.current.bhop)
                return;
            if (Configuration.current.bhopKey.Where(x => User32.GetAsyncKeyState(x) != 0).ToList().Count > 0)
            {
                int localPlayerPtr = Memory.Read<int>(Memory.clientBase + Offsets.dwLocalPlayer);
                int flag = Memory.Read<int>(localPlayerPtr + Offsets.m_fFlags);

                if (flag == 257 || flag == 263)
                {
                    Mouse.Scroll();
                    Thread.Sleep(15);
                }
            }
        }
    }
}
