using System;
using System.Drawing;
using System.IO;

namespace Kelloggs.Formats
{
    class MAPFile
    {
        private const string Signature = "TLE1";
        public Bitmap Bitmap { get; private set; }
        private DATFileEntry source;
        public string Error { get; private set; } = "";
        public bool Ready { get { return Error == ""; } }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int[,] Cells { get; private set; }

        public MAPFile(DATFileEntry source)
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
            BinaryReaderBigEndian reader = new BinaryReaderBigEndian(new MemoryStream(source.Data));
            if (string.Join("", reader.ReadChars(Signature.Length)) != Signature) throw new Exception("signature not found");

            Width = reader.ReadInt16();
            Height = reader.ReadInt16();

            if (Width <= 0 || Height <= 0) throw new Exception($"invalid dimensions encountered: width={Width}, height={Height}");

            int unknown = reader.ReadInt16();
            if (unknown != 9) throw new Exception($"invalid unknown constant: {unknown}");

            int num = Width * Height;
            Cells = new int[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int value = reader.ReadInt16();
                    Cells[x, y] = value;
                }
            }

        }
    }
}
