using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;

namespace COD4_Server_Browser
{
    public class ServerCalls
    {
        private static string MASTER_URL = "cod4master.cod4x.ovh";
        private static int MASTER_PORT = 20810;

        private static readonly byte[] request = { 0xff, 0xff, 0xff, 0xff, 0x67, 0x65, 0x74, 0x73,
                                    0x65, 0x72, 0x76, 0x65, 0x72, 0x73, 0x20, 0x32,
                                    0x31, 0x20, 0x66, 0x75, 0x6c, 0x6c, 0x20, 0x65,
                                    0x6d, 0x70, 0x74, 0x79, 0x20, 0x00 };

        private static readonly byte[] response = { 0xff, 0xff, 0xff, 0xff, 0x54, 0x53, 0x6f, 0x75,
                                    0x72, 0x63, 0x65, 0x20, 0x45, 0x6e, 0x67, 0x69,
                                    0x6e, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79,
                                    0x20, 0x78, 0x78, 0x78 };

        private static readonly byte[] dem = { 0x5c, 0x00, 0x00, 0x00, 0x00 };

        static ServerCalls()
        {
            _instance = new ServerCalls();
        }

        private static ServerCalls _instance;
        public static ServerCalls Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServerCalls();
                return _instance;
            }
        }

        /// <summary>
        /// Returns the IPV4 of master server
        /// </summary>
        /// <returns></returns>
        private static string GetMasterIPV4()
        {
            return Dns.GetHostEntry(MASTER_URL, AddressFamily.InterNetwork).AddressList[0].ToString();
        }

        /// <summary>
        /// This function sends the request to the master-server
        /// master-server responses by a list contains the IP4/6:PORT
        /// returns List<byte>
        /// </summary>
        private static List<byte> GetMasterServerList()
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(GetMasterIPV4(), MASTER_PORT);
            List<byte> data = new List<byte>();
            BinaryReader br = new BinaryReader(tcpClient.GetStream());
            BinaryWriter bw = new BinaryWriter(tcpClient.GetStream());
            bw.Write(request);
            while (true)
            {
                byte b = br.ReadByte();
                data.Add(b);

                if (data.Count > 4)
                    if (data[data.Count - 1] == 'F' && data[data.Count - 2] == 'O' && data[data.Count - 3] == 'E')
                        break;
            }
            return data;
        }

        /// <summary>
        /// Decode and return a list of ip:port
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, int>> DecodeServerList(List<byte> data)
        {
            List<KeyValuePair<string, int>> ipPortList = new List<KeyValuePair<string, int>>();

            var subList = data.GetRange(27, (data.Count - 31));
            var dataArray = subList.ToArray();
            var splitArrays = dataArray.separate(dem);

            for (int i = 0; i < splitArrays.Length; i++)
            {
                byte[] temp = splitArrays[i];
                if (temp[0] == 0x05) continue;
                int port = (temp[5] << 8) | temp[6];
                string ip = temp[1].ToString() + "." + temp[2].ToString() + "." +
                            temp[3].ToString() + "." + temp[4].ToString();
                ipPortList.Add(new KeyValuePair<string, int>(ip, port));
            }
            return ipPortList;
        }

        private async static Task<List<ServerInfo>> GetServersDetails(List<KeyValuePair<string, int>> keyValues)
        {
            var servers = new List<ServerInfo>();
            var taskList = new List<Task>();
            foreach (var record in keyValues)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        UdpClient client = new UdpClient(record.Key, record.Value);
                        client.Send(response, response.Length);
                        client.Client.ReceiveTimeout = 500;
                        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(record.Key), record.Value);
                        var recv = client.Receive(ref ipep);
                        //var ascii = Encoding.ASCII.GetString(recv);
                        string mapName = GetServerDetails(1, recv);
                        string serverName = GetServerDetails(0, recv);

                        servers.Add(new ServerInfo
                        {
                            ServerName = serverName,
                            IP = record.Key,
                            Port = record.Value,
                            MapName = mapName
                        });
                    }
                    catch (Exception ex)
                    {
                        WriteLog(ex.Message);
                    }
                }, TaskCreationOptions.None);
                taskList.Add(task);
            }
            await Task.WhenAll(taskList);
            return servers;
        }

        public async static Task<List<ServerInfo>> GetAllServerDetails()
        {
            var bytes = GetMasterServerList();
            var keyValuePairs = DecodeServerList(bytes);
            return await GetServersDetails(keyValuePairs);
        }

        private static string GetServerDetails(int start, byte[] bytes)
        {
            List<int> list = new List<int>();
            list.Add(6);
            for (int i = 6; i < bytes.Length; i++)
                if (bytes[i] == 0) list.Add(i + 1);

            Debug.WriteLine("");

            for (int i = list[start]; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                    return Encoding.ASCII.GetString(bytes.ToList().GetRange(list[start], i - list[start]).ToArray());
            }
            return "";
        }

        private static object _LOCK = new object();
        private static void WriteLog(string msg)
        {
            try
            {
                lock (_LOCK)
                {
                    File.AppendAllText(@"log.txt", msg);
                }
            }
            catch (Exception ex)
            {
                //Probably never going to happend xD
                File.AppendAllText(@"log2.txt", ex.Message);
            }
        }
    }
}
