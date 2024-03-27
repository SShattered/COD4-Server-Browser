using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace COD4_Server_Browser
{
    public struct ServerInfo
    {
        public string ServerName;
        public string ServerVersion;
        public string IP;
        public string MapName;
        public int Port;
        public IPEndPoint endPoint;
    }
}
