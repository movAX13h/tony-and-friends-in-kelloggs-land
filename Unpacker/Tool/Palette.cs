﻿using Kelloggs.Properties;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Kelloggs.Tool
{
    class Palette
    {
        private static Palette defaultPalette;
        public static Palette Default
        {
            get
            {
                // cache so that we don't need to parse the resource string everytime
                if (defaultPalette != null) return defaultPalette;

                // load default VGA palette from embedded resource text
                Palette p = new Palette();
                p.Name = "VGA default";
                string[] lines = Resources.VGAPalette.Split('\n');

                for(int i = 0; i < lines.Length; i++)
                {
                    string[] comp = Regex.Split(lines[i].Trim(), @"\s+");
                    Color col = Color.FromArgb(255, byte.Parse(comp[0]), byte.Parse(comp[1]), byte.Parse(comp[2]));
                    p.Colors[i] = col;
                }

                defaultPalette = p;
                return p;
            }
        }

        public static Bitmap ToBitmap(Palette palette)
        {
            return ToBitmap(palette.Colors);
        }

        public static Bitmap ToBitmap(Color[] colors)
        {
            int w = 8, h = 32;
            Bitmap bmp = new Bitmap(w, h);
            
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int i = x + y * w;
                    bmp.SetPixel(x, y, colors[i]);                    
                }
            }

            return bmp;
        }


        public Color[] Colors;
        public string Name = "unnamed";

        public Palette()
        {
            Colors = new Color[256];            
        }

        public Palette(Color[] colors)
        {
            Colors = colors;
        }

        public Palette Clone()
        {
            Palette pal = new Palette();
            for(int i = 0; i < Colors.Length; i++)
            {
                pal.Colors[i] = Colors[i];
            }
            return pal;
        }

    }
}
