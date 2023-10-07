using CSLiba.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CSLiba.Game.Objects
{
    public class FaceitInfo
    {
        public FaceitInfo()
        {

        }

        public void Update(string steamId)
        {
            if (previousSteamId != steamId)
                RefreshValues(steamId);
            previousSteamId = steamId;
        }

        private void RefreshValues(string steamId)
        {
            long id = SteamIdTo64(steamId);

            Request request = new Request("https://faceitfinder.com/profile/" + id.ToString());

            request.Host = "faceitfinder.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.0.0 Safari/537.36";
            request.ContentType = "application/x-protobuf";

            request.Post();

            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";

            request.Get();

            if (request.Response.Contains("FaceIt account not found") || request.Response.Contains("Players not found!"))
            {
                matches = 0;
                elo = 0;
                level = 0;
            }
            else
            {
                matches = ParseValue(request.Response, "Matches");
                elo = ParseValue(request.Response, "ELO");
                level = GetLevelByElo(elo);
            }
        }

        long SteamIdTo64(string Id32)
        {
            Match match = Regex.Match(Id32, @"^STEAM_[0-5]:[01]:\d+$", RegexOptions.IgnoreCase);

            if (!match.Success)
                return 0;

            string[] split = Id32.Split(':');

            long v = 76561197960265728;
            long y = long.Parse(split[1]);
            long z = long.Parse(split[2]);

            return (z * 2) + v + y;
        }


        /// <param name="property">Matches, ELO, K/D</param>
        /// <returns></returns>
        private int ParseValue(string response, string property)
        {
            string s = $"class=\"account-faceit-stats-single\">{property}: <strong>";
            int start = response.IndexOf(s) + s.Length;
            int end = response.IndexOf("</", start);
            return int.Parse(response.Substring(start, end - start));
        }

        public static int GetLevelByElo(int elo)
        {
            if (elo < 801)
                return 1;
            else if (elo < 951)
                return 2;
            else if (elo < 1101)
                return 3;
            else if (elo < 1251)
                return 4;
            else if (elo < 1401)
                return 5;
            else if (elo < 1551)
                return 6;
            else if (elo < 1701)
                return 7;
            else if (elo < 1851)
                return 8;
            else if (elo < 2001)
                return 9;
            return 10;
        }

        public int level, elo, matches;
        public string previousSteamId;

    }
}
