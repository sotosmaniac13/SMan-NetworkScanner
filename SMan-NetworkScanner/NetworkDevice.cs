using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMan_NetworkScanner
{
    public class NetworkDevice
    {
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string MacAddress { get; set; }
    }
}
