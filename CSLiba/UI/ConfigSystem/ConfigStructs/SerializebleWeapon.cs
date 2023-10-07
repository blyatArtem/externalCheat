using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSLiba.Game.Enumerations;

namespace CSLiba.UI.ConfigSystem.ConfigStructs
{
    [Serializable]
    public class SerializebleWeapon
    {
        public SerializebleWeapon()
        {
            //aimAssistKey = new List<Keys>() { Keys.XButton2, Keys.LButton };
            //triggerbotKey = new List<Keys>() { Keys.XButton2 };
        }

        public int aimBone = 8, xShake = 6, yShake = 6, xSpeed = 6, ySpeed = 6, rcsSmoothX = 10;
        public bool triggerbot, aimAssist, rcs, targetLock, shake, canFireCheck, jumpDelay, spottedAim, flashDelay;
        public float fov = 5;
        public List<Keys> aimAssistKey = new List<Keys>(), triggerbotKey = new List<Keys>();
    };
}
