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
        public static readonly string filename = "saves\\highscores.save";
        public static SaveManager highscoresSave = new SaveManager(filename);

        public static List<int> cachedHighscores = new List<int>();

        public static void addNewScore(int score)
        {
            highscoresSave.read();

            highscoresSave.addLine(score.ToString());
            foreach (var s in highscoresSave.lines)
            {
                cachedHighscores.Add(int.Parse(s.ToString()));
            }

            highscoresSave.write();
        }

        public static void sort()
        {

            int tempIndex = 0;
            int lastIndex = 0;

            while (!isSorted())
            {

                for (int i = 0; i < cachedHighscores.Count; i++)
                {
                    if (cachedHighscores[i] < lastIndex)
                    {
                        tempIndex = cachedHighscores[i];
                        cachedHighscores[i] = lastIndex;
                        lastIndex = tempIndex;
                    }
                }
            }
        }

        public static bool isSorted()
        {
            bool sorted = true;

            int lastIndex = 0;
            int thisIndex = 0;

            for(int i = 0; i < cachedHighscores.Count;  i++)
            {
                thisIndex = cachedHighscores[i];
                
                if (thisIndex < lastIndex)
                {
                    sorted = false;
                    break;
                }

                lastIndex = thisIndex;
            }

            return sorted;
        }

        internal static void Fetch()
        {
            highscoresSave.read();

            foreach (var s in highscoresSave.lines)
            {
                cachedHighscores.Add(int.Parse(s.ToString()));
            }

            highscoresSave.write();
        }
    }
}
