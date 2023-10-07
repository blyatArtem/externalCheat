using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Objects
{
    public class WorldItem
    {
        public WorldItem(Vector3 positionScreen, string text)
        {
            this.position = positionScreen;
            this.text = text;
        }

        public Vector4 color;
        public Vector3 position;
        public string text;

    }
}
