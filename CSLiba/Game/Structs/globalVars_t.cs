using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct globalVars_t
    {
        public float m_flRealTime;
        public int m_nFrameCount;
        public float m_flAbsoluteFrameTime;
        public float m_flAbsoluteFrameStartTimeStddev;
        public float m_flCurTime;
        public float m_flFrameTime;
        public int m_iMaxClients;
        public int m_iTickCount;
        public float m_flIntervalPerTick;
    }
}
