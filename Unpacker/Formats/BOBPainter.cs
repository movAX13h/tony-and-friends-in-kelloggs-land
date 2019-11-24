using System;
using System.Drawing;

namespace Kelloggs.Formats
{
    static class BOBPainter
    {
        public static Bitmap MakeSheet(BOBFile bob)
        {
            int x = 0, y = 0;
            int width = 0, height = 0;
            foreach (var element in bob.Elements)
            {
                x += element.Width;
                if (x > 400)
                {
                    x = 0;
                    y += element.Height;
                }
                width = Math.Max(width, x + element.Width);
                height = Math.Max(height, y + element.Height);
            }
            var bmp = new Bitmap(width, height);
            using (var gfx = Graphics.FromImage(bmp))
            {
                x = 0;
                y = 0;
                foreach (var element in bob.Elements)
                {
                    gfx.DrawImage(element, x, y);
                    x += element.Width;
                    if (x > 400)
                    {
                        x = 0;
                        y += element.Height;
                    }
                }
            }
            return bmp;
        }
    }
}
