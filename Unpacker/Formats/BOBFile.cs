using System;
using System.Collections.Generic;
using System.IO;

// also used in DOS version of Turrican II
namespace Kelloggs.Formats
{
    class BOBFile
    {
        public byte[] Palette;
        public List<BOBFrame> Frames;
        private DATFileEntry source;

        public string Error { get; private set; } = "";

        public BOBFile(DATFileEntry source)
        {
            this.source = source;

            try
            {
                parse();
            }
            catch(Exception e)
            {
                Error = e.Message;
            }

        }

        private void parse()
        { 
            using (var f = new BinaryReader(new MemoryStream(source.Data)))
            {
                /*
                 * turrican specific
                if(BitConverter.ToInt32(data, 0) == 13107520) // detect NUM.BOB
                {
                    Frames = new List<BOBFrame>();
                    for(int i = 0; i < 2; ++i)
                    {
                        int width = f.ReadInt16();
                        int height = f.ReadInt16();
                        int drawProgramSize = f.ReadInt16();
                        int pixelDataSize = f.ReadInt16();
                        var pixelData = f.ReadBytes(pixelDataSize);
                        var drawProgram = f.ReadBytes(drawProgramSize);

                        Frames.Add(new BOBFrame { Width = width, Height = height, DrawProgram = drawProgram, PixelData = pixelData });
                    }
                    return;
                }
                */

                Palette = f.ReadBytes(256 * 3);
                int one = f.ReadInt16(); // "1"
                if(one != 1)
                    throw new Exception("unsupported file type");
                int numFrames = f.ReadInt16();
                f.BaseStream.Position += 1;

                Frames = new List<BOBFrame>(numFrames);

                for(int i = 0; i < numFrames; ++i)
                {
                    int settings = f.ReadInt32();
                    int width = f.ReadInt16() + 1;
                    int height = f.ReadInt16() + 1;
                    int drawProgramSize = f.ReadInt16();
                    int pixelDataSize = f.ReadInt16();

                    int zero = f.ReadInt32(); // always "0"
                    if(zero != 0)
                        throw new Exception("format error");

                    var drawProgram = f.ReadBytes(drawProgramSize);
                    var pixelData = f.ReadBytes(pixelDataSize);

                    Frames.Add(new BOBFrame
                    {
                        DrawProgram = drawProgram,
                        PixelData = pixelData,
                        Settings = settings,
                        Width = width,
                        Height = height
                    });
                }

                // 16 zerobytes trail each bob file
            }
        }
    }

    public class BOBFrame
    {
        /// <summary>
        /// a x86 program to draw the sprite, consisting of unrolled loops and pokes to the EGA card
        /// </summary>
        public byte[] DrawProgram;
        /// <summary>
        /// data that is accessed by the draw program
        /// </summary>
        public byte[] PixelData;
        /// <summary>
        /// yet unknown purpose
        /// </summary>
        public int Settings;

        public int Width;
        public int Height;
    }
}
