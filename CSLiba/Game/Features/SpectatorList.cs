using CSLiba.Core;
using CSLiba.Game.Enumerations;
using CSLiba.Game.Objects;
using CSLiba.UI.ConfigSystem;
using GameOverlay.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLiba.Game.Features
{
    public static class SpectatorList
    {
        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.spectators)
                return;

            List<Entity> spectators = GetSpectators(data);
            if (spectators.Count == 0)
                return;

            int i = 1;
            float maxWidth = Configuration.current.specW;
            float offset = 5;
            float offsetHeight = 13f;
            g.FillRectangle(Overlay.GetBrushByVector(g, Configuration.current.specBackgroundColor), new Rectangle(Configuration.current.specX, Configuration.current.specY, Configuration.current.specX + maxWidth + offset * 2, Configuration.current.specY + (spectators.Count + 2) * (offsetHeight + offset)));
            foreach (Entity entity in spectators)
            {
                string modeStr = "";
                ObsMode obsMode = (ObsMode)Memory.Read<int>(entity.basePtr + Offsets.m_iObserverMode);
                if (obsMode == ObsMode.DEATHCAM)
                {
                    modeStr = "(Death cam) ";
                }
                else if (obsMode == ObsMode.FREEZECAM)
                {
                    modeStr = "(Freeze cam) ";
                }
                else if (obsMode == ObsMode.FIXED)
                {
                    modeStr = "(Fixed) ";
                }
                else if (obsMode == ObsMode.CHASE)
                {
                    modeStr = "(3rd person) ";
                }
                else if (obsMode == ObsMode.ROAMING)
                {
                    modeStr = "(No clip) ";
                }
                IBrush brush = entity.team == Team.Terrorists ? Overlay.brushT : entity.team == Team.CounterTerrorists ? Overlay.brushCT : Overlay.brushText;
                g.DrawText(Overlay.fontText, brush, new Point(Configuration.current.specX + offset, Configuration.current.specY + (i * (offsetHeight + offset))), modeStr + entity.nickname);
                i++;
            }
        }

        //private static List<Entity> GetSpectators(GameData data)
        //{
        //    List<Entity> entities = new List<Entity>();

        //    int localObserverTarget = Memory.Read<int>(Memory.clientBase + Offsets.dwEntityList + ((Memory.Read<int>(data.localPlayer.basePtr + Offsets.m_hObserverTarget) & 0xFFF) - 1) * 0x10);

        //    foreach (Entity entity in data.Entities)
        //    {
        //        if (entity.basePtr == 0 || !entity.lifeState /*|| entity.dormant || entity.team == Enumerations.Team.Unknown*/)
        //            continue;
        //        int entity_observer_target = Memory.Read<int>(Memory.clientBase + Offsets.dwEntityList + ((Memory.Read<int>(entity.basePtr + Offsets.m_hObserverTarget) & 0xFFF) - 1) * 0x10);
        //        if (entity_observer_target == data.localPlayer.basePtr || entity_observer_target == localObserverTarget)
        //            entities.Add(entity);
        //    }
        //    return entities;
        //}

        private static List<Entity> GetSpectators(GameData data)
        {
            List<Entity> entities = new List<Entity>();

            foreach (Entity entity in data.Entities)
            {
                if (entity.basePtr == 0 || !entity.lifeState)
                    continue;
                int entity_observer_target = Memory.Read<int>(Memory.clientBase + Offsets.dwEntityList + ((Memory.Read<int>(entity.basePtr + Offsets.m_hObserverTarget) & 0xFFF) - 1) * 0x10);
                if (entity_observer_target == data.localPlayer.basePtr)
                    entities.Add(entity);
            }
            return entities;
        }
    }
}
