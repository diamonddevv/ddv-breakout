using Breakout.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Breakout.Util.HighScoreManager;

namespace Breakout.Game.Achievements
{
    internal class AchievementManager
    {
        private const string FILENAME = "data/achievements.save";
        private const string SAVE_FORMAT_VERSION = "ddvb_ACHV";

        public static readonly Dictionary<string, AchievementData> achievementDict = new Dictionary<string, AchievementData>();
        public static Dictionary<string, float> ACHIEVEMENT_CACHE = new Dictionary<string, float>();

        public static SaveManager ACHIEVEMENT_SAVE = new SaveManager(FILENAME, SAVE_FORMAT_VERSION, (line) =>
        {
            string[] s = line.Split(':');

            string id = s[0];
            float f = float.Parse(s[1]);

            ACHIEVEMENT_CACHE.Add(id, f);
        });
        

        public struct AchievementData
        {
            public string Name;
            public string Description;

            public string id;
        }

        public static readonly AchievementData
            ACHV_SHINYFISH = new AchievementData() { Name = "It's Shiny!", Description = "See a Shiny Fish", id = "shinyFish" },
            ACHV_FASTBALL = new AchievementData() { Name = "Fastball", Description = "Get over over 100 points at the max ball speed", id = "fastball" };

        private static void Add(AchievementData data)
        {
            achievementDict.Add(data.id, data);
        }

        public static void Prepare()
        {
            ACHIEVEMENT_CACHE.Clear();

            Add(ACHV_SHINYFISH);
            Add(ACHV_FASTBALL);

            Fetch();
        }

        public static void Fetch()
        {
            ACHIEVEMENT_CACHE.Clear();
            ACHIEVEMENT_SAVE.Fetch();
            ACHIEVEMENT_SAVE.write();
        }

        public static float GetAchivementValue(AchievementData data)
        {
            Fetch();
            ACHIEVEMENT_CACHE.TryGetValue(data.id, out var v);
            return v;
        }
        public static void SetAchievement(AchievementData data, float v)
        {
            ACHIEVEMENT_SAVE.addLine($"{data.id}:{v}");
            ACHIEVEMENT_SAVE.write();
            Fetch();
        }
        public static void ChangeAchievementBy(AchievementData data, float change)
        {
            float f = GetAchivementValue(data);
            SetAchievement(data, f + change);
        }
    }
}
