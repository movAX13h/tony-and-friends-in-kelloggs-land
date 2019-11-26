using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kelloggs.Formats
{
    static class MAPPainter
    {
        public static Bitmap Paint(MAPFile map, ICOFile ico, int scale) // doing scaling here because we might need debug text rendering which does not look good when scaled
        {
            var tiles = ico.Bitmaps;
            Bitmap bmp = new Bitmap(map.Width * 16 * scale, map.Height * 16 * scale);
            SolidBrush brush = new SolidBrush(Color.Red);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.PixelOffsetMode = PixelOffsetMode.Half;

                for (int y = 0; y < map.Height; y++)
                {
                    for (int x = 0; x < map.Width; x++)
                    {
                        MAPFile.MapCell cell = map.Cells[x, y];
                        
                        int px = x * 16 * scale;
                        int py = y * 16 * scale;

                        //int c = Math.Min(255, 255 * v / 65536); // 2 bytes per cell
                        //brush.Color = Palette.Default.Colors[c]; //Color.FromArgb(255, c, c, c);
                        //gfx.FillRectangle(brush, x, y, 1, 1);
                        
                        if (cell.Tile < tiles.Length) gfx.DrawImage(tiles[cell.Tile], px, py, 16 * scale, 16 * scale);
                        else
                        {
                            gfx.FillRectangle(Brushes.Red, px, py, 16 * scale, 16 * scale);
                            
                        }
                        //gfx.DrawString(cell.Type.ToString("X"), SystemFonts.SmallCaptionFont, Brushes.White, px, py);
                    }
                }
            }
         
            return bmp;
        }
    }
}
