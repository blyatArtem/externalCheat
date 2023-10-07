using CSLiba.Game.Objects;
using CSLiba.Imports;
using CSLiba.UI.ConfigSystem;
using GameOverlay.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLiba.Game.Features
{
    public static class RankViewer
    {
        public static void FrameAction(GameData data)
        {
            if (!Configuration.current.rankViewer)
                return;
            foreach (Entity entity in data.Entities)
            {
                if (entity.basePtr != 0)
                {
                    if (entity.steamID != "BOT")
                    {
                        entity.faceitInfo.Update(entity.steamID);
                    }
                }
            }
            Thread.Sleep(1000);
        }

        public static void FrameAction(Graphics g, GameData data)
        {
            if (!Configuration.current.rankViewer)
                return;
            if (User32.GetAsyncKeyState(Keys.Tab) == 0)
                return;
            List<Entity> terrorists = new List<Entity>();
            List<Entity> counterTerrorists = new List<Entity>();
            foreach (Entity entity in data.Entities)
            {
                if (entity.basePtr != 0)
                {
                    if (entity.steamID != "BOT")
                    {
                        if (entity.team == Enumerations.Team.Terrorists)
                        {
                            terrorists.Add(entity);
                        }
                        else if (entity.team == Enumerations.Team.CounterTerrorists)
                        {
                            counterTerrorists.Add(entity);
                        }
                    }
                }
            }

            float itemWidth = 450;
            float itemheight = 55;
            float offset = 5f;
            Point startPos = new Point(g.Width - (itemWidth + offset) - 10, -itemheight - 10);
            Rectangle itemRect = new Rectangle(startPos.X, startPos.Y, startPos.X + itemWidth + 40, startPos.Y + itemheight * (terrorists.Count + counterTerrorists.Count + 2) + offset + offset * (terrorists.Count + counterTerrorists.Count) + itemheight + 20);
            g.FillRoundedRectangle(Overlay.brushOutline, new RoundedRectangle(itemRect, 25));
            int index = 1;
            foreach (Entity entity in terrorists)
            {
                DrawScoreboardItem(g, entity, index);
                index++;
            }
            DrawScoreboardAVG(g, index, true, terrorists);
            index++;
            foreach (Entity entity in counterTerrorists)
            {
                DrawScoreboardItem(g, entity, index, false);
                index++;
            }
            DrawScoreboardAVG(g, index, false, counterTerrorists);
        }

        private static void DrawScoreboardAVG(Graphics g, int index, bool tSide, List<Entity> entities)
        {
            float itemWidth = 450;
            float itemheight = 55;
            float offset = 5f;

            Point startPos = new Point(g.Width - (itemWidth + offset), -itemheight);
            Rectangle itemRect = new Rectangle(startPos.X, startPos.Y + itemheight * index + offset + offset * index, startPos.X + itemWidth, startPos.Y + itemheight * index + offset + offset * index + itemheight);

            //g.FillRoundedRectangle(Overlay.brushOutline, new RoundedRectangle(itemRect, itemheight / 2));

            List<Entity> penis = entities.Where(x => x.faceitInfo.level != 0).ToList();
            int rankId = 0;
            int faceitLevel = 0;
            int wins = 0;
            int faceitMatches = 0;
            int faceitElo = 0;
            if (entities.Count > 0)
            {
                rankId = (int)Math.Round(entities.Average(x => x.rank));
                wins = (int)Math.Round(entities.Average(x => x.wins));
            }
            if (penis.Where(x => x.faceitInfo.level > 0).ToList().Count > 0)
            {
                faceitElo = (int)Math.Round(penis.Average(x => x.faceitInfo.elo));
                faceitMatches = (int)Math.Round(penis.Average(x => x.faceitInfo.matches));
                faceitLevel = FaceitInfo.GetLevelByElo(faceitElo);
            }


            string rankName = GetRankNameFromNumber(rankId);
            string text =
                $"Average {(tSide ? "terrorists" : "counter-terrorists")}\n" +
                $"wins: {wins}, rank: {rankName}\n" +
                $"faceit matches: {faceitMatches}, faceit elo: {faceitElo}";

            g.DrawText(Overlay.fontText, tSide ? Overlay.brushT : Overlay.brushCT, new Point(itemRect.Left + 15, itemRect.Top + 5), text);
            g.DrawImage(GetRankImageFromNumber(rankId), new Rectangle(itemRect.Right - 110, itemRect.Top + 10, itemRect.Right - 15, itemRect.Bottom - 10));
            if (faceitLevel > 0)
                g.DrawImage(GetLevelImage(faceitLevel), new Rectangle(itemRect.Right - 110 - 55, itemRect.Top + 5, itemRect.Right - 110 - 5, itemRect.Bottom - 5));
        }

        //201x80
        private static void DrawScoreboardItem(Graphics g, Entity entity, int index, bool tSide = true)
        {
            float itemWidth = 450;
            float itemheight = 55;
            float offset = 5f;

            Point startPos = new Point(g.Width - (itemWidth + offset), -itemheight);
            Rectangle itemRect = new Rectangle(startPos.X, startPos.Y + itemheight * index + offset + offset * index, startPos.X + itemWidth, startPos.Y + itemheight * index + offset + offset * index + itemheight);

            g.FillRoundedRectangle(tSide ? Overlay.brushTBG : Overlay.brushCTBG, new RoundedRectangle(itemRect.Left, itemRect.Top, itemRect.Right + itemheight / 2, itemRect.Bottom, itemheight / 2));


            string rankName = GetRankNameFromNumber(entity.rank);
            string text =
                $"{entity.nickname}\n" +
                $"wins: {entity.wins}, rank: {rankName}\n" +
                $"faceit matches: {entity.faceitInfo.matches}, faceit elo: {entity.faceitInfo.elo}";

            g.DrawText(Overlay.fontText, tSide ? Overlay.brushT : Overlay.brushCT, new Point(itemRect.Left + 15, itemRect.Top + 5), text);
            g.DrawImage(GetRankImageFromNumber(entity.rank), new Rectangle(itemRect.Right - 110, itemRect.Top + 10, itemRect.Right - 15, itemRect.Bottom - 10));
            if (entity.faceitInfo.elo != 0)
                g.DrawImage(GetLevelImage(entity.faceitInfo.level), new Rectangle(itemRect.Right - 110 - 55, itemRect.Top + 5, itemRect.Right - 110 - 5, itemRect.Bottom - 5));
        }
        public static string GetRankNameFromNumber(int RankID)
        {
            if (RankID >= 0 && RankID <= RankArray.Length)
            {
                return RankArray[RankID];
            }

            return "";
        }

        private static Image GetRankImageFromNumber(int RankID)
        {
            if (RankID >= 0 && RankID <= RankArray.Length)
            {
                Image[] imageArray =
                {
                    Overlay.ur,
                    Overlay.s1,
                    Overlay.s2,
                    Overlay.s3,
                    Overlay.s4,
                    Overlay.s5,
                    Overlay.s6,
                    Overlay.n1,
                    Overlay.n2,
                    Overlay.n3,
                    Overlay.n4,
                    Overlay.a1,
                    Overlay.a2,
                    Overlay.a3,
                    Overlay.bs,
                    Overlay.ber,
                    Overlay.lem,
                    Overlay.sup,
                    Overlay.gl,
                };
                return imageArray[RankID];
            }
            return Overlay.ur;
        }

        private static Image GetLevelImage(int level)
        {
            Image[] imageArray =
            {
                    Overlay.l1,
                    Overlay.l2,
                    Overlay.l3,
                    Overlay.l4,
                    Overlay.l5,
                    Overlay.l6,
                    Overlay.l7,
                    Overlay.l8,
                    Overlay.l9,
                    Overlay.l10,
                };
            return imageArray[level - 1];
        }

        private static string[] RankArray = new string[] {
        "Unranked", "Silver I", "Silver II", "Silver III", "Silver IV", "Silver Elite", "Silver Elite Master",
        "Gold Nova I", "Gold Nova II", "Gold Nova III","Gold Nova Master",
        "Master Guardian I", "Master Guardian II", "Master Guardian Elite",
        "Distinguished Master Guardian", "Legendary Eagle", "Legendary Eagle master", "Supreme", "Global Elite"};

    }
}
