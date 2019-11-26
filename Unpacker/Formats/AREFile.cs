using System;

namespace Kelloggs.Formats
{
    class AREFile
    {
        private DATFileEntry source;
        public string Error { get; private set; } = "";
        public bool Ready { get { return Error == ""; } }

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

        }
    }
}
