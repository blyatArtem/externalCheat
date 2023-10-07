using CSLiba.UI.ConfigSystem.ConfigStructs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CSLiba.UI.ConfigSystem
{
    [Serializable]
    public class Configuration
    {
        public Configuration()
        {
            pistol = new SerializebleWeapon();
            smg = new SerializebleWeapon();
            heavy = new SerializebleWeapon();
            rifle = new SerializebleWeapon();
            sniper = new SerializebleWeapon();
        }

        public static void Save()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "cfg files (*.cfg)|*cfg|Configuration files (*.*)|*.*";
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\content\configs";
            saveFileDialog.FilterIndex = 2;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream file = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate);
                file.Close();
                XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                {
                    serializer.Serialize(writer, current);
                }
            }
        }

        public static bool Load(out string fileName)
        {
            fileName = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\content\configs";
            openFileDialog.Filter = "cfg files (*.cfg)|*cfg|Configuration files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                    {
                        current = (Configuration)serializer.Deserialize(fs);
                    }
                    fileName = Path.GetFileName(openFileDialog.FileName);
                    return true;
                }
                catch
                {
                }
            }
            return false;
        }

        internal static Configuration current;
        internal static bool displayThreads;

        #region other

        public FormTheme theme = FormTheme.Default;

        #endregion

        #region crosshair

        public bool aimCrosshair;
        public Vector4 aimCrosshairColor = new Vector4(255, 0, 0, 255);
        public GameOverlay.Drawing.CrosshairStyle aimCrosshairStyle = GameOverlay.Drawing.CrosshairStyle.Dot;
        public int aimCrosshairSize = 3, aimCrosshairStroke = 2;

        #endregion

        #region aim

        public SerializebleWeapon pistol, smg, heavy, rifle, sniper;
        public Vector4 fovColor = new Vector4(255, 0, 0, 50);

        #endregion

        #region worldEsp

        public Vector4 weaponsEspColor = new Vector4(255, 0, 0, 255), hitsEspColor = new Vector4(255, 0, 0, 255);
        public bool hitDrawer, bombTimer, groundedWeapons, smokeHelper;

        #endregion

        #region esp

        public int boxesEnemyType;
        public bool boxesEnemy, skeletonsEnemy, hitboxesEnemy;
        public Vector4
            boxEnemyColor = new Vector4(255, 0, 0, 255),
            skeletonEnemyColor = new Vector4(255, 0, 100, 255),
            spottedBoxEnemyColor = new Vector4(0, 255, 0, 255),
            spottedSkeletonEnemyColor = new Vector4(0, 255, 100, 255),
            hitboxEnemyColor = new Vector4(255, 0, 0, 255),
            spottedHitboxEnemyColor = new Vector4(0, 255, 100, 255);

        public bool health, lines;
        public int healthSide;

        #endregion

        #region espString

        public bool nicknames, distance, scopeWarning, defuseWarning, weapons, healthString;

        #endregion

        #region misc

        public bool spectators, rankViewer, fov, bhop, onTeam;
        public List<Keys> bhopKey = new List<Keys>();
        public List<Keys> aimToggleKey = new List<Keys>();
        public List<Keys> whToggleKey = new List<Keys>();

        #endregion

        #region radarEsp

        public Vector4
            radarBackgroundColor = new Vector4(0, 0, 0, 127),
            radarBorderColor = new Vector4(255, 255, 255, 255),
            radarLocalPlayerColor = new Vector4(50, 200, 255, 255),
            radarGrenadesColor = new Vector4(140, 140, 140, 180),
            radarEnemyColor = new Vector4(255, 0, 0, 255);

        public bool radar, grenades, radarRounded;
        public int radarX = 10, radarY = 820, radarW = 200, radarH = 200, radarScale = 15, radarEntityRadius = 4;

        #endregion

        #region spectatorList

        public int specX = 500, specY = 500, specW = 100;
        public Vector4 specBackgroundColor = new Vector4(0, 0, 0, 127);
        #endregion

    }
}
