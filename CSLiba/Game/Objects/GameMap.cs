using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Objects
{
    [Serializable]
    public class GameMap
    {
        public GameMap()
        {

        }

        public GameMap(string name, List<SmokeHelperPoint> points)
        {
            this.name = name;
            helperPoints = points;
        }

        public string name;

        public List<SmokeHelperPoint> helperPoints;
    }
}
