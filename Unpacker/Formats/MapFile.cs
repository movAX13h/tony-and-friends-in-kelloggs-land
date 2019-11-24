using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kelloggs.Formats
{
    class MapFile
    {
        private const string Signature = "TLE1";
        public Bitmap Bitmap { get; private set; }
        private DATFileEntry source;

        public MapFile(DATFileEntry source)
        {
            this.source = source;
            if (parse())
            {

            }
        }

        private bool parse()
        {


            return false;
        }
    }
}
