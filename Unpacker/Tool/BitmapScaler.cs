using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Kelloggs.Tool
{
    static class BitmapScaler
    {
        public static Bitmap PixelScale(Bitmap source, int scale)
        {
            if (scale < 0) throw new Exception("Scale must not be negative!");

            Bitmap bmp = new Bitmap(source.Width * scale, source.Height * scale);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                gfx.PixelOffsetMode = PixelOffsetMode.Half;
                gfx.DrawImage(source, new Rectangle(Point.Empty, bmp.Size));
            }

            return bmp;
        }
    }
}
