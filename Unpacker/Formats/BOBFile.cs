﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

// a very similar format is used in the DOS version of Turrican II
namespace Kelloggs.Formats
{
    class BOBFile
    {
        // Using these patterns we can serach for the executable sections in the files:
        // "push si"
        static readonly byte[] startPattern = new byte[] { 0x56, 0x50 };

        // "pop si"
        // "retf"
        static readonly byte[] endPattern = new byte[] { 0x5E, 0xCB };

        public string Error { get; private set; } = "";

        private DATFileEntry source;
        private Palette palette;

        public List<Bitmap> Elements { get; private set; } 
        
        public BOBFile(DATFileEntry source, Palette palette)
        {
            this.source = source;
            this.palette = palette;

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
            int frameIndex = 0;

            Elements = new List<Bitmap>();

            using (var f = new BinaryReader(new MemoryStream(source.Data)))
            {
                while (f.BaseStream.Position < f.BaseStream.Length)
                {
                    //Console.Write("@" + f.BaseStream.Position.ToString("X") + "\t");

                    var header = f.ReadBytes(14);

                    //Console.WriteLine(BitConverter.ToString(header));

                    int dataSegmentLength = BitConverter.ToInt16(header, 0xA);

                    var dataSegment = f.ReadBytes(dataSegmentLength);

                    for (int i = 0; i * 2 < dataSegmentLength; ++i)
                    {
                        Console.Write(BitConverter.ToInt16(dataSegment, i * 2).ToString("X") + " ");
                    }
                    //Console.WriteLine();

                    int end = source.Data.Locate(endPattern, (int)f.BaseStream.Position) + 2;

                    var execSegment = f.ReadBytes(end - (int)f.BaseStream.Position);


                    //File.WriteAllBytes("CURRENT.COM", execSegment);

                    var cmds = parseExecutable(execSegment, 0);

                    var bmp = new Bitmap(40, 60);
                    {
                        int stride = 84;
                        foreach (var cmd in cmds)
                        {
                            for (int i = 0; i < cmd.Data.Length; ++i)
                            {
                                int p = cmd.Offset + i;
                                int x = (p % stride) * 4 + cmd.egaPage;
                                int y = p / stride;
                                bmp.SetPixel(x, y, palette.Colors[cmd.Data[i] - 0x80]);
                            }
                        }

                        Elements.Add(bmp);
                        //bmp.Save("frame" + frameIndex + ".png");
                    }

                    ++frameIndex;
                }

                if (f.BaseStream.Position != f.BaseStream.Length) throw new Exception($"missed {f.BaseStream.Length - f.BaseStream.Position} extra bytes at end of file");
            }

        }

        struct CopyInstruction
        {
            public int egaPage;
            public int Offset;
            public byte[] Data;
        }

        // Extracts copy instructions from executable segments.
        static CopyInstruction[] parseExecutable(byte[] data, int startAddress)
        {
            List<CopyInstruction> copyInstructions = new List<CopyInstruction>();

            int pc = startAddress;
            byte opcEx;
            int offset;
            int constant;
            bool returned = false;
            int egaPage = -1;
            while (!returned)
            {
                switch (data[pc])
                {
                    case 0x03:
                        opcEx = data[pc + 1];
                        if (opcEx == 0xF1)
                        {
                            // add si, cx
                            pc += 2;
                        }
                        else
                            throw new Exception();
                        break;
                    // retf
                    case 0xCB:
                        returned = true;
                        ++pc;
                        break;
                    // push si
                    case 0x56:
                        ++pc;
                        break;
                    // push ax
                    case 0x58:
                        ++pc;
                        break;
                    // pop si
                    case 0x5E:
                        ++pc;
                        break;
                    // push ax
                    case 0x50:
                        ++pc;
                        break;
                    // out dx, al
                    case 0xEE:
                        ++egaPage;
                        egaPage &= 3;
                        ++pc;
                        break;
                    case 0xD0:
                        opcEx = data[pc + 1];
                        if (opcEx == 0xC0)
                        {
                            // rol al, 1
                            pc += 2;
                        }
                        else
                            throw new Exception();
                        break;
                    case 0x8A:
                        opcEx = data[pc + 1];
                        if (opcEx == 0xCC)
                        {
                            // mov cl, ah
                            pc += 2;
                        }
                        else if (opcEx == 0xCB)
                        {
                            // mov cl, bl
                            pc += 2;
                        }
                        else if (opcEx == 0xCF)
                        {
                            // mov cl, bh
                            pc += 2;
                        }
                        else
                            throw new Exception();
                        break;
                    case 0xC6:
                        opcEx = data[pc + 1];
                        if (opcEx == 0x84)
                        {
                            // mov byte ptr [si+AAAA], BBh
                            offset = data[pc + 2];
                            offset += data[pc + 3] * 256;
                            constant = data[pc + 4];
                            pc += 5;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant }, egaPage = egaPage });
                        }
                        else if (opcEx == 0x44)
                        {
                            // mov byte ptr [si+AA], BBh
                            offset = data[pc + 2];
                            constant = data[pc + 3];
                            pc += 4;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant }, egaPage = egaPage });
                        }
                        else
                            throw new Exception();
                        break;
                    case 0xC7:
                        opcEx = data[pc + 1];
                        if (opcEx == 0x44)
                        {
                            // mov word ptr [si+AA], BBBBh
                            offset = data[pc + 2];
                            constant = data[pc + 3];
                            constant += data[pc + 4] * 256;
                            pc += 5;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant, (byte)(constant >> 8) }, egaPage = egaPage });
                        }
                        else if (opcEx == 0x84)
                        {
                            // mov word ptr [si+AAAA], BBBBh
                            offset = data[pc + 2];
                            offset += data[pc + 3] * 256;
                            constant = data[pc + 4];
                            constant += data[pc + 5] * 256;
                            pc += 6;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant, (byte)(constant >> 8) }, egaPage = egaPage });
                        }
                        else
                            throw new Exception();
                        break;
                    default:
                        throw new Exception();
                }
            }

            return copyInstructions.ToArray();
        }

    }

}
