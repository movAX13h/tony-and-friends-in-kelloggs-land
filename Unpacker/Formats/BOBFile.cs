using Kelloggs.Tool;
using System;
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
                // does not happen with original assets of the game
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
                    // HEADER
                    // byte     desc
                    // ------------------------------------------
                    // 0, 1     unknown
                    // 2, 3     unknown
                    // 4, 5     unknown
                    // 6, 7     maximum width (confirm?)
                    // 8, 9     maximum height (confirm?)
                    // 10,11    length of the pointers segment
                    // 12,13    padding/unknown

                    var header = f.ReadBytes(14); // unknown (irrelevant)

                    int width = BitConverter.ToInt16(header, 6);
                    int height = BitConverter.ToInt16(header, 8);
                    int pointersSegmentLength = BitConverter.ToInt16(header, 10);

                    var pointersData = f.ReadBytes(pointersSegmentLength);
                    
                    int lastInstruction = BitConverter.ToInt16(pointersData, pointersData.Length - 2);

                    var nextFramePosition = f.BaseStream.Position + lastInstruction;

                    var execSegment = f.ReadBytes((int)(nextFramePosition - f.BaseStream.Position));

                    //if (source.Filename == "KROKO.BOB") File.WriteAllBytes(source.Filename + "-exec.com", execSegment);

                    if(parseExecutable(execSegment, 0, out CopyInstruction[] copyInstructions))
                    {
                        var bmp = new Bitmap(width, height);
                        {
                            int stride = 84;
                            foreach(var copyInstruction in copyInstructions)
                            {
                                for(int i = 0; i < copyInstruction.Data.Length; ++i)
                                {
                                    int p = copyInstruction.Offset + i;
                                    int x = (p % stride) * 4 + copyInstruction.EGAPage;
                                    int y = p / stride;
                                    bmp.SetPixel(x, y, palette.Colors[copyInstruction.Data[i] - 0x80]);
                                }
                            }

                            Elements.Add(bmp);
                        }
                    }

                    ++frameIndex;
                }

                if (f.BaseStream.Position != f.BaseStream.Length)
                    throw new Exception($"missed {f.BaseStream.Length - f.BaseStream.Position} extra bytes at end of file");
            }

        }

        struct CopyInstruction
        {
            public int EGAPage;
            public int Offset;
            public byte[] Data;
        }

        // Extracts copy instructions from executable segments.
        static bool parseExecutable(byte[] data, int startAddress, out CopyInstruction[] result)
        {
            result = null;

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
                            return false;
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
                            return false;
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
                            return false;
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

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant }, EGAPage = egaPage });
                        }
                        else if (opcEx == 0x44)
                        {
                            // mov byte ptr [si+AA], BBh
                            offset = data[pc + 2];
                            constant = data[pc + 3];
                            pc += 4;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant }, EGAPage = egaPage });
                        }
                        else
                            return false;
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

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant, (byte)(constant >> 8) }, EGAPage = egaPage });
                        }
                        else if (opcEx == 0x84)
                        {
                            // mov word ptr [si+AAAA], BBBBh
                            offset = data[pc + 2];
                            offset += data[pc + 3] * 256;
                            constant = data[pc + 4];
                            constant += data[pc + 5] * 256;
                            pc += 6;

                            copyInstructions.Add(new CopyInstruction { Offset = offset, Data = new byte[] { (byte)constant, (byte)(constant >> 8) }, EGAPage = egaPage });
                        }
                        else
                            return false;
                        break;
                    default:
                        return false;
                }
            }

            result = copyInstructions.ToArray();
            return true;
        }

    }

}
