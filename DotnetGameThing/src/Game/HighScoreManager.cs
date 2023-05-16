using Breakout.Window;
using BreakoutGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Breakout.Util
{
    internal class HighScoreManager
    {
        public static readonly string filename = "data\\highscores.scoremap";
        public static List<Scoring> cachedHighscores = new List<Scoring>();

        public static SaveManager highscoresSave = new SaveManager(filename, "ddvb_SAVE-user:score", (line) =>
        {
            string[] lineS = line.ToString().Split(':');

            cachedHighscores.Add(new Scoring()
            {
                score = int.Parse(lineS[1]),
                user = lineS[0]
            });
        });
        

        public static void AddNewScore(string name, int score)
        {
            highscoresSave.addLine($"{name}:{score}");
            highscoresSave.write();
            Fetch();
        }

        public static void Sort()
        {
            cachedHighscores.Sort((Scoring x, Scoring y) =>
            {
                int i = x.score.CompareTo(y.score);
                return i == 0 ? 0 : i == -1 ? 1 : -1;
            });
        }

        public static void Fetch()
        {
            cachedHighscores.Clear();
            highscoresSave.Fetch();
        }


        public struct Scoring
        {
            public int score;
            public string user;
        }

    }
}
