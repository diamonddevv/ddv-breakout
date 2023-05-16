using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Breakout.Util.HighScoreManager;

namespace Breakout.Util
{
    internal class SaveManager
    {
        private string filename;
        private string formatVersionId;
        public List<string> lines;
        private readonly Action<string> fetchFunction;
        private bool wasCorrupt = false;

        public SaveManager(string filename, string formatVersionId, Action<string> FetchFunction) {
            this.filename = filename;
            this.formatVersionId = formatVersionId;
            fetchFunction = FetchFunction;
            this.lines = new List<string>();
        }

        public void addLine(string s)
        {
            lines.Add(s);
        }

        public void removeLine(string s)
        {
            lines.Remove(s);
        }

        public void write()
        {
            SSKVPFManager.PrepareFileManagement(filename);
            File.WriteAllText(filename, string.Empty);

            StreamWriter write = new StreamWriter(filename);

            write.WriteLine($"saveFormatVersionIdentifier:{formatVersionId}|wasCorrupt:{wasCorrupt}");
            write.WriteLine();

            foreach (var line in lines)
            {
                write.WriteLine(line);
            }

            write.Close();
        }

        public void read()
        {
            lines.Clear();

            if (!SSKVPFManager.PrepareFileManagement(filename))
            {
                StreamReader read = new StreamReader(filename);
                
                read.ReadLine();
                read.ReadLine(); // ignore save format version

                while (true)
                {
                    string line = read.ReadLine();
                    if (line == null) break;
                    lines.Add(line);
                }

                read.Close();
            }
        }

        public Dictionary<string, string>? ReadMetadata()
        {
            StreamReader read = new StreamReader(filename);
            var s = read.ReadLine().Split('|');
            read.Close();

            var d = new Dictionary<string, string>();
            foreach (var t in s)
            {
                var q = t.Split(':');
                d.Add(q[0], q[1]);
            }

            return d;
        }

        public void Fetch()
        {
            read();

            bool reset = false;

            foreach (var s in highscoresSave.lines)
            {

                try
                {
                    fetchFunction.Invoke(s);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Save file invalid (the format was probably updated) - Exception Details: {e.Message}");
                    Console.WriteLine("The file will deleted, as it is considered corrupt.");
                    reset = true;
                    break;
                }

            }

            if (reset) Reset(true);

            CollapseDuplicates();

            write();
        }

        public bool ReadWasCorrupted()
        {
            var s = ReadMetadata();
            if (s.TryGetValue("wasCorrupt", out string sb))
            {
                return bool.Parse(sb);
            }
            return false;
        }
        public string ReadFormatVersionId()
        {
            var s = ReadMetadata();
            if (s.TryGetValue("saveFormatVersionIdentifier", out string ss))
            {
                return ss;
            }
            return "null";
        }

        public string GetExpectedFormatVersionId()
        {
            return formatVersionId;
        }

        public void Reset(bool corrupted)
        {
            lines.Clear();
            if (corrupted)
            {
                this.wasCorrupt = true;
            }
            write();
        }

        

        public void CollapseDuplicates()
        {
            List<string> ss = new List<string>();
            foreach (var s in lines)
            {
                if (!ss.Contains(s))
                {
                    ss.Add(s);
                }
            }

            lines = ss;
            write();
        }
    }
}
