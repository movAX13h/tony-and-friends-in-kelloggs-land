using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Kelloggs.Formats
{
    internal class DATFileEntry
    {
        public static Dictionary<string, string> TypeNames = new Dictionary<string, string> {
            { ".ARE", "Area" },
            { ".MAP", "Game Map" },
            { ".BOB", "Sprite" },
            { ".ICO", "Sprite Set" },
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

        public Color TypeColor
        {
            get
            {
                switch(Type)
                {
                    case "MAP": return Color.Yellow;
                    case "ARE": return Color.LightYellow;
                    case "BOB": return Color.LawnGreen;
                    case "ICO": return Color.Turquoise;
                    case "PCC": return Color.LightGreen;
                    default: return Color.White;
                }
            }
        }

        public string Note = "";

        public string SaveToDisk()
        {
            try
            {
                string dir = Path.GetDirectoryName(ExportPath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllBytes(ExportPath, Data);
                return ExportPath;
            }
            catch
            {
                return "";
            }
        }

        public override string ToString()
        {
            return Filename + " @ " + Offset.ToString();
        }
    }
}
