using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct mstudiobbox_t
    {
        public int bone;
        public int group;                   // intersection group
        public Vector3 bbmin;               // bounding box
        public Vector3 bbmax;
        public int szhitboxnameindex;       // offset to the name of the hitbox.
        public fixed int unused[3];
        public float radius;                // when radius is -1 it's box, otherwise it's capsule (cylinder with spheres on the end)
        public fixed int pad[4];
    }
}
