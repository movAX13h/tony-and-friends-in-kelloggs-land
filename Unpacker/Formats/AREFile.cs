using System;
using System.Collections.Generic;
using System.IO;

namespace Kelloggs.Formats
{
    class AREFile
    {
        private DATFileEntry source;
        public string Error { get; private set; } = "";
        public bool Ready { get { return Error == ""; } }

        public List<ushort> Section1 { get; private set; }

        public AREFile(DATFileEntry source)
        {
            this.source = source;

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
            BinaryReader reader = new BinaryReader(new MemoryStream(source.Data));

            for(int i = 0; i < 176; i++)
            {
                ushort n = reader.ReadUInt16();
                Section1.Add(n);
            }
        }
    }
}
