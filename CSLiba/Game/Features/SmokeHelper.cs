using CSLiba.Game.Objects;
using CSLiba.UI.ConfigSystem;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSLiba.Game.Features
{
    public static class SmokeHelper
    {
        public static bool Initialize()
        {
            try
            {
                FileStream file = new FileStream(Directory.GetCurrentDirectory() + @"\content\smokes.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(List<GameMap>));
                maps = (List<GameMap>)serializer.Deserialize(file);
                file.Close();
                if (maps == null)
                    maps = new List<GameMap>();
                return true;
            }
            catch
            {
                maps = new List<GameMap>();
            }
            return false;
        }

        public static void FrameAction(Graphics g, GameData data, string mapName)
        {
            aiming = false;
            if (!Configuration.current.smokeHelper)
                return;

            foreach(GameMap map in maps)
            {
                if (map.name == mapName)
                {
                    foreach(SmokeHelperPoint point in map.helperPoints)
                    {
                        point.Draw(g, data.localPlayer);
                    }
                }
            }
        }

        public static List<GameMap> maps = new List<GameMap>();
        public static bool aiming = false;

    }
}
