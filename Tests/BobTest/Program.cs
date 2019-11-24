using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobTest
{
    class Program
    {
        // Find first occurance of a byte pattern in byte array:
        static int Locate(byte[] ba, byte[] pattern, int start = 0)
        {
            if(start < 0)
                throw new Exception("Parameter start must not be negative.");
            for(int i = start; i + pattern.Length <= ba.Length; ++i)
            {
                bool isMatch = true;
                for(int j = 0; j < pattern.Length; ++j)
                {
                    if(ba[i + j] != pattern[j])
                    {
                        isMatch = false;
                        break;
                    }
                }
                if(isMatch)
                    return i;
            }
            return -1;
        }
        
        // Using these patterns we can serach for the executable sections in the files:
        // "push si"
        static readonly byte[] startPattern = new byte[] { 0x56, 0x50 };
        // "pop si"
        // "retf"
        static readonly byte[] endPattern = new byte[] { 0x5E, 0xCB };

        // List all instruction segments contained in the file.
        // In theory, there should be 1 segment per frame.
        static void ListProgramLocations(byte[] data)
        {
            int at = 0;
            int numFrames = 0;

            while(true)
            {
                int start = Locate(data, startPattern, at);
                if(start == -1)
                    break;
                at = start;
                int end = Locate(data, endPattern, at) + 2;
                at = end;

                ++numFrames;

                Console.WriteLine("#" + numFrames + "   instructions in $" + start.ToString("X") + " - $" + end.ToString("X") + "   ($" + (end - start).ToString("X") + " bytes)");
            }
        }

        class CopyInstruction
        {
            public int egaPage;
            public int Offset;
            public byte[] Data;
        }

        // Extracts copy instructions from executable segments.
        static CopyInstruction[] ParseExecutable(byte[] data, int startAddress)
        {
            List<CopyInstruction> copyInstructions = new List<CopyInstruction>();

            int pc = startAddress;
            byte opcEx;
            int offset;
            int constant;
            bool returned = false;
            int egaPage = -1;
            while(!returned)
            {
                switch(data[pc])
                {
                    case 0x03:
                        opcEx = data[pc + 1];
                        if(opcEx == 0xF1)
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
                        if(opcEx == 0xC0)
                        {
                            // rol al, 1
                            pc += 2;
                        }
                        else
                            throw new Exception();
                        break;
                    case 0x8A:
                        opcEx = data[pc + 1];
                        if(opcEx == 0xCC)
                        {
                            // mov cl, ah
                            pc += 2;
                        }
                        else if(opcEx == 0xCB)
                        {
                            // mov cl, bl
                            pc += 2;
                        }
                        else if(opcEx == 0xCF)
                        {
                            // mov cl, bh
                            pc += 2;
                        }
                        else
                            throw new Exception();
                        break;
                    case 0xC6:
                        opcEx = data[pc + 1];
                        if(opcEx == 0x84)
                        {
                            // mov byte ptr [si+AAAA], BBh
                            offset = data[pc + 2];
                            offset += data[pc + 3] * 256;
                            constant = data[pc + 4];
                            pc += 5;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant }, egaPage = egaPage });
                        }
                        else if(opcEx == 0x44)
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
                        if(opcEx == 0x44)
                        {
                            // mov word ptr [si+AA], BBBBh
                            offset = data[pc + 2];
                            constant = data[pc + 3];
                            constant += data[pc + 4] * 256;
                            pc += 5;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant, (byte)(constant >> 8) }, egaPage = egaPage });
                        }
                        else if(opcEx == 0x84)
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

        static void ListHeadersAndTables(byte[] data)
        {
            ListProgramLocations(data);

            int frameIndex = 0;

            var rnd = new Random(123456);
            Color[] colors = new Color[256];
            var lines = File.ReadAllLines(@"..\..\tony.pal.txt");
            for(int i = 0; i < 256; ++i)
            {
                //colors[i] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                var subs = lines[i].Split(' ');
                colors[i] = Color.FromArgb(byte.Parse(subs[0]), byte.Parse(subs[1]), byte.Parse(subs[2]));
            }

            List<Bitmap> elements = new List<Bitmap>();

            using(var f = new BinaryReader(new MemoryStream(data)))
            {
                while(f.BaseStream.Position < f.BaseStream.Length)
                {
                    Console.Write("@" + f.BaseStream.Position.ToString("X") + "\t");

                    var header = f.ReadBytes(14);

                    Console.WriteLine(BitConverter.ToString(header));

                    int dataSegmentLength = BitConverter.ToInt16(header, 0xA);

                    var dataSegment = f.ReadBytes(dataSegmentLength);

                    for(int i = 0; i * 2 < dataSegmentLength; ++i)
                    {
                        Console.Write(BitConverter.ToInt16(dataSegment, i * 2).ToString("X") + " ");
                    }
                    Console.WriteLine();

                    int end = Locate(data, endPattern, (int)f.BaseStream.Position) + 2;

                    var execSegment = f.ReadBytes(end - (int)f.BaseStream.Position);


                    //File.WriteAllBytes("CURRENT.COM", execSegment);

                    var cmds = ParseExecutable(execSegment, 0);

                    var bmp = new Bitmap(40, 60);
                    {
                        int stride = 84;
                        foreach(var cmd in cmds)
                        {
                            for(int i = 0; i < cmd.Data.Length; ++i)
                            {
                                int p = cmd.Offset + i;
                                int x = (p % stride) * 4 + cmd.egaPage;
                                int y = p / stride;
                                bmp.SetPixel(x, y, colors[cmd.Data[i] - 0x80]);
                            }
                        }

                        elements.Add(bmp);
                        //bmp.Save("frame" + frameIndex + ".png");
                    }

                    ++frameIndex;
                }

                if(f.BaseStream.Position == f.BaseStream.Length)
                    Console.WriteLine("OK!");
                else
                    Console.WriteLine("END NOT REACHED!");
            }


            MakeSheet(elements).Save("sheet.png");
        }

        private static Bitmap MakeSheet(List<Bitmap> elements)
        {
            int x = 0, y = 0;
            int width = 0, height = 0;
            foreach(var element in elements)
            {
                x += element.Width;
                if(x > 400)
                {
                    x = 0;
                    y += element.Height;
                }
                width = Math.Max(width, x + element.Width);
                height = Math.Max(height, y + element.Height);
            }
            var bmp = new Bitmap(width, height);
            using(var gfx = Graphics.FromImage(bmp))
            {
                x = 0;
                y = 0;
                foreach(var element in elements)
                {
                    gfx.DrawImage(element, x, y);
                    x += element.Width;
                    if(x > 400)
                    {
                        x = 0;
                        y += element.Height;
                    }
                }
            }
            return bmp;
        }

        static void Main(string[] args)
        {
            var data = File.ReadAllBytes(args[0]);

            ListHeadersAndTables(data);
        }
    }
}
