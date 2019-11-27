using System.Collections.Generic;
using System.IO;

namespace Kelloggs.Tool
{
    class Notes
    {
        public Dictionary<string, string> Entries = new Dictionary<string, string>();

        public Notes(string path)
        {
            if (!File.Exists(path)) return;

            string all = File.ReadAllText(path);
            string[] lines = all.Split('\n');

            foreach(string line in lines)
            {
                if (line.StartsWith("#")) continue;
                string[] parts = line.Split(new char[] { ':' }, 2);
                string key = parts[0].Trim();
                Entries[key] = parts[1].Trim();
            }
        }

        public string GetNote(string key)
        {
            if (Entries.ContainsKey(key)) return Entries[key];
            return "";
        }
    }
}
