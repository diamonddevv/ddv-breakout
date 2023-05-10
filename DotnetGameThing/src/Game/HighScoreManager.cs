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
        public static SaveManager highscoresSave = new SaveManager(filename, "ddvb_SAVE-user:score");

        public static List<Scoring> cachedHighscores = new List<Scoring>();

        public static void addNewScore(string name, int score)
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
            highscoresSave.read();

            bool reset = false;

            foreach (var s in highscoresSave.lines)
            {
                string[] line = s.ToString().Split(':');

                try
                {
                    cachedHighscores.Add(new Scoring()
                    {
                        score = int.Parse(line[1]),
                        user = line[0]
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Save invalid (the format was probably updated) - Exception Details: {e.Message}");
                    Console.WriteLine("The save will deleted, as it is considered corrupt.");
                    reset = true;
                    break;
                }

            }

            if (reset) highscoresSave.Reset(true);

            highscoresSave.CollapseDuplicates();

            highscoresSave.write();
        }


        public struct Scoring
        {
            public int score;
            public string user;
        }

    }
}
