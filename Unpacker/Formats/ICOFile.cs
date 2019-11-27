using Kelloggs.Tool;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kelloggs.Formats
{
    class ICOFile
    {
        private DATFileEntry source;
        private Palette palette;
        public string Error { get; private set; } = "";
        public bool Ready { get { return Error == ""; } }

        public Bitmap[] Bitmaps { get; private set; }

        public ICOFile(DATFileEntry source, Palette palette)
        {
            this.source = source;
            this.palette = palette;

            try
            {
                parse();
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }

        private void parse()
        {
            // pixel order (EGA):
            //
            // 0   4   8  12   1   5   9  13   2   6  10  14   3   7  11  15
            // 16  20  24  28  17  21  25  29  18  22  26  30  19  23  27  31
            // ...
            int numTilesPerBlockPic = (source.Data.Length - 1) / 256;
            Bitmaps = new Bitmap[numTilesPerBlockPic];
            int ptr = 0;

            int minIndex = 255;

            for (int i = 0; i < numTilesPerBlockPic; ++i)
            {
                var bmp = new Bitmap(16, 16);
                for (int y = 0; y < 16; ++y)
                {
                    for (int page = 0; page < 4; ++page)
                    {
                        for (int x = 0; x < 4; ++x)
                        {
                            int k = source.Data[ptr++];
                            bmp.SetPixel(x * 4 + page, y, palette.Colors[k - 160 + 32]);
                            if (k < minIndex) minIndex = k;
                        }
                    }
                }
                Bitmaps[i] = bmp;
            }

            Console.WriteLine("minimum color index: " + minIndex);
        }
    }
}
