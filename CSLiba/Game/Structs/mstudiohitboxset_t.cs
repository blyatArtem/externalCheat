using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct mstudiohitboxset_t
    {
        public int sznameindex;
        public int numhitboxes;
        public int hitboxindex;
    }
}
