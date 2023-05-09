using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Util
{
    internal class SaveManager
    {
        private string filename;
        public List<string> lines;

        public SaveManager(string filename) {
            this.filename = filename;
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
                
                while (true)
                {
                    string line = read.ReadLine();
                    if (line == null) break;
                    lines.Add(line);
                }

                read.Close();
            }
        }
    }
}
