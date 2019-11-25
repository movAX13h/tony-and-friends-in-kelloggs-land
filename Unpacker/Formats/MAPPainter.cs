using Kelloggs.Tool;
using System;
using System.Drawing;

namespace Kelloggs.Formats
{
    static class MAPPainter
    {
        public static Bitmap Paint(MAPFile map)
        {
            Bitmap bmp = new Bitmap(map.Width, map.Height);
            Graphics gfx = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.Red);

            for(int y = 0; y < map.Height; y++)
            {
                for(int x = 0; x < map.Width; x++)
                {
                    int v = map.Cells[x, y];
                    int c = Math.Min(255, 255 * v / 65536); // 2 bytes per cell
                    brush.Color = Palette.Default.Colors[c]; //Color.FromArgb(255, c, c, c);
                    gfx.FillRectangle(brush, x, y, 1, 1);
                }
            }

            gfx.Dispose();
            return bmp;
        }
    }
}
