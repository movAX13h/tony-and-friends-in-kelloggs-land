using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kelloggs.Formats
{
    // this is the only container format
    class DATFile
    {
        public Dictionary<string, DATFileEntry> Entries { get; private set; }
        public string Log { get; private set; }

        public bool Ready { get; private set; } = false;
        private string path;
        private string exportDirectory;

        public DATFile(string path)
        {
            this.path = path;

            exportDirectory = Path.Combine(Path.GetDirectoryName(path), "unpacked");
            Entries = new Dictionary<string, DATFileEntry>();
        }

        public bool Parse()
        {
            if (!File.Exists(path))
            {
                Log = "File not found.";
                return false;
            }

            try
            {
                parse();
                return true;
            }
            catch(Exception e)
            {
                Log += "Exception: " + e.Message + Environment.NewLine;
                return false;
            }
        }

        private void parse()
        { 
            byte[] data = File.ReadAllBytes(path);
            int numEntries = BitConverter.ToUInt16(data, data.Length - 4) + 1;
            int offset = BitConverter.ToInt32(data, data.Length - 8);
            int indexOffset = offset;
            DATFileEntry[] files = new DATFileEntry[numEntries];

            Log = "Num entries: " + numEntries.ToString() + Environment.NewLine + "Index address: " + offset.ToString() + Environment.NewLine;

            for (int i = 0; i < numEntries; i++)
            {
                DATFileEntry entry = new DATFileEntry();

                int nameLen = BitConverter.ToUInt16(data, offset);
                offset += 2;
                entry.Filename = Encoding.UTF8.GetString(data, offset, nameLen); //BitConverter.ToString(data, offset, nameLen);
                offset += nameLen;
                entry.Offset = BitConverter.ToInt32(data, offset);
                offset += 4;
                files[i] = entry;
                Log += entry.ToString() + Environment.NewLine;
                entry.ExportPath = Path.Combine(exportDirectory, entry.Filename);
            }

            for (int i = 0; i < numEntries; i++)
            {
                DATFileEntry file = files[i];
                int length = (i < numEntries - 1 ? files[i + 1].Offset : indexOffset) - file.Offset;
                file.Data = data.SubArray(file.Offset, length);
                Entries.Add(file.Filename, file);
            }

            Ready = true;
        }

        public bool ExportAll()
        {
            bool allOk = true;

            foreach(DATFileEntry entry in Entries.Values)
            {
                if (!entry.SaveToDisk()) allOk = false;
            }

            return allOk;
        }

    }
}
