using CSLiba.Game.Enumerations;
using CSLiba.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameOverlay.Drawing;
using Microsoft.Xna.Framework;

namespace CSLiba.Game.Objects
{
    public abstract class EntityBase
    {
        public virtual bool Update()
        {
            basePtr = UpdateAdressBase();
            if (basePtr == 0)
                return false;

            lifeState = Memory.Read<bool>(basePtr + Offsets.m_lifeState);
            health = Memory.Read<int>(basePtr + Offsets.m_iHealth);
            team = (Team)Memory.Read<int>(basePtr + Offsets.m_iTeamNum);
            origin = Memory.Read<Vector3>(basePtr + Offsets.m_vecOrigin);
            return true;
        }

        public abstract int UpdateAdressBase();

        public virtual bool IsAlive()
        {
            return basePtr != 0 && !lifeState && health > 0 && (team == Team.Terrorists || team == Team.CounterTerrorists);
        }

        public int basePtr;

        public bool lifeState;

        public int health;

        public Vector3 origin;

        public Team team;
    }
}
