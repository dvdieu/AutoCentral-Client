using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCentral
{
    public class ConfirmDTO
    {
        public string server { get; set; }
        public string account { get; set; }
        public string value { get; set; }
        public string command { get; set; }

        public string gameid { get; set; }
    }
}
