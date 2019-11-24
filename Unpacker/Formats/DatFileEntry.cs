using System;
using System.Collections.Generic;
using System.IO;

namespace Kelloggs.Formats
{
    internal class DATFileEntry
    {
        public static Dictionary<string, string> TypeNames = new Dictionary<string, string> {
            { ".ARE", "Area" },
            { ".MAP", "Game Map" },
            { ".BOB", "Sprite" },
            { ".ICO", "Small Image" },
            { ".PCC", "Image" },
            { ".TFX", "TFMX Music" },
            { ".SAM", "TFMX Samples" }
        };

        public string Filename;
        public string ExportPath;
        public int Offset;
        public byte[] Data;

        public string Type
        {
            get
            {
                return Path.GetExtension(Filename).Substring(1);
            }
        }

        public string TypeName
        {
            get
            {
                string name = Path.GetExtension(Filename);
                if (TypeNames.ContainsKey(name)) name = TypeNames[name];
                return name;
            }
        }

        public string Note = "";

        public bool SaveToDisk()
        {
            try
            {
                string dir = Path.GetDirectoryName(ExportPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllBytes(ExportPath, Data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Filename + " @ " + Offset.ToString();
        }
    }
}
