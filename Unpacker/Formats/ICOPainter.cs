using System.Collections.Generic;
using System.Drawing;
using Kelloggs.Tool;

namespace Kelloggs.Formats
{
    static class ICOPainter
    {
        public static Bitmap[] ICOToBitmaps(byte[] blockPic, Palette palette)
        {
            //      pixel order (EGA):
            //
            //       0   4   8  12   1   5   9  13   2   6  10  14   3   7  11  15
            //      16  20  24  28  17  21  25  29  18  22  26  30  19  23  27  31
            //      ...

            int numTilesPerBlockPic = (blockPic.Length - 1) / 256;
            var bitmaps = new List<Bitmap>();
            int ptr = 0;

            for (int i = 0; i < numTilesPerBlockPic; ++i)
            {
                var bmp = new Bitmap(16, 16);
                for (int y = 0; y < 16; ++y)
                {
                    for (int page = 0; page < 4; ++page)
                    {
                        for (int x = 0; x < 4; ++x)
                        {
                            int k = blockPic[ptr++];
                            bmp.SetPixel(x * 4 + page, y, palette.Colors[k]);
                        }
                    }
                }
                bitmaps.Add(bmp);
            }
            return bitmaps.ToArray();
        }

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
