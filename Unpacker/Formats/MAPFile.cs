﻿using Kelloggs.Tool;
using System;
using System.Drawing;
using System.IO;

namespace Kelloggs.Formats
{
    class MAPFile
    {
        public class MapCell
        {
            public int Tile = 0;
            public int Type = 0;
        }

        private const string Signature = "TLE1";
        public Bitmap Bitmap { get; private set; }
        private DATFileEntry source;
        public string Error { get; private set; } = "";
        public bool Ready { get { return Error == ""; } }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public MapCell[,] Cells { get; private set; }

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
            Cells = new MapCell[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ushort value = reader.ReadUInt16();
                    Cells[x, y] = new MapCell() { Tile = value & 0b0000_0001_1111_1111 , Type = value >> 9 };
                }
            }

            if (reader.BaseStream.Position != reader.BaseStream.Length) throw new Exception($"missed {reader.BaseStream.Length - reader.BaseStream.Position} extra bytes at end of file");
        }
    }
}
