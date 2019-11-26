using System.Drawing;

namespace Kelloggs.Formats
{
    static class ICOPainter
    {
        /// <summary>
        /// arrange the array of tiles into a single bitmap
        /// </summary>
        public static Bitmap TileSetFromBitmaps(Bitmap[] bitmaps)
        {
            int num = bitmaps.Length;
            int w = 8;
            int h = (num / w) + 1;

            Bitmap bmp = new Bitmap(16 * w, 16 * h);
            using (var gfx = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < num; i++)
                {
                    int x = i % w;
                    int y = i / w;

                    gfx.DrawImage(bitmaps[i], 16 * x, 16 * y, 16, 16);
                }
            }

            return bmp;
        }
    }
}
