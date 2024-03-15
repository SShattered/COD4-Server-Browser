using COD4_Server_Browser.Properties;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;

namespace COD4_Server_Browser
{
    public partial class MainForm : Form
    {
        private readonly byte[] request = { 0xff, 0xff, 0xff, 0xff, 0x67, 0x65, 0x74, 0x73,
                                    0x65, 0x72, 0x76, 0x65, 0x72, 0x73, 0x20, 0x32,
                                    0x31, 0x20, 0x66, 0x75, 0x6c, 0x6c, 0x20, 0x65,
                                    0x6d, 0x70, 0x74, 0x79, 0x20, 0x00 };

        private readonly byte[] infoRequest = { 0xff, 0xff, 0xff, 0xff, 0x54, 0x53, 0x6f, 0x75,
                                    0x72, 0x63, 0x65, 0x20, 0x45, 0x6e, 0x67, 0x69,
                                    0x6e, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79,
                                    0x20, 0x78, 0x78, 0x78 };

        private readonly Image image;
        private Dictionary<string, Image> mapImages;

        object LOCK = new object();
        private readonly List<ServerInfo> serverInfos;

        public MainForm()
        {
            InitializeComponent();

            serverInfos = new List<ServerInfo>();
            image = Image.FromFile(@"Image1.png");
            mapImages = new Dictionary<string, Image>();
            var files = Directory.GetFiles(@"maps\");
            for (int i = 0; i < files.Length; i++)
            {
                mapImages.Add(Path.GetFileNameWithoutExtension(files[i]), Image.FromFile(files[i]));
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            var xolo = Dns.GetHostEntry("cod4master.cod4x.ovh", AddressFamily.InterNetwork);
            tcpClient.Connect(xolo.AddressList[0].ToString(), 20810);
            Task.Run(() =>
            {
                Listen(new BinaryReader(tcpClient.GetStream()));
            });
            BinaryWriter bw = new BinaryWriter(tcpClient.GetStream());
            bw.Write(request);
        }

        void Listen(BinaryReader br)
        {
            List<byte> data = new List<byte>();
            byte[] dem = { 0x5c, 0x00, 0x00, 0x00, 0x00 };
            while (true)
            {
                byte b = br.ReadByte();
                data.Add(b);

                if (data.Count > 4)
                {
                    if (data[data.Count - 1] == 'F' && data[data.Count - 2] == 'O' && data[data.Count - 3] == 'E')
                    {
                        var subList = data.GetRange(27, (data.Count - 31));
                        var dataArray = subList.ToArray();
                        var splitArrays = dataArray.separate(dem);

                        for (int i = 0; i < splitArrays.Length; i++)
                        {
                            byte[] temp = splitArrays[i];
                            if (temp[0] == 0x05) continue;
                            try
                            {
                                int port = (temp[5] << 8) | temp[6];
                                string ip = temp[1].ToString() + "." + temp[2].ToString() + "." +
                                            temp[3].ToString() + "." + temp[4].ToString();

                                //WriteText($"{ip}:{port}");

                                Task.Factory.StartNew(() =>
                                {
                                    try
                                    {
                                        UdpClient client = new UdpClient(ip, port);
                                        client.Send(infoRequest, infoRequest.Length);
                                        client.Client.ReceiveTimeout = 500;
                                        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);
                                        var recv = client.Receive(ref ipep);
                                        //var ascii = Encoding.ASCII.GetString(recv);
                                        string mapName = GetServerDetails(1, recv);
                                        string serverName = GetServerDetails(0, recv);

                                        lock (LOCK)
                                        {
                                            Invoke((MethodInvoker)delegate
                                            {
                                                exListBox1.Items.Add(
                                                    new exListBoxItem(0, serverName, mapName + $" {ip}:{port.ToString()}",
                                                    GetMapImage(mapName)));
                                                serverInfos.Add(new ServerInfo
                                                {
                                                    ServerName = serverName,
                                                    IP = ip,
                                                    Port = port
                                                });
                                            });
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        /*if (ex is TimeoutException)
                                            WriteText("Server Timeout");*/
                                    }
                                }, TaskCreationOptions.LongRunning);

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        break;
                    }
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private Image GetMapImage(string map)
        {
            if (mapImages.ContainsKey(map))
                return mapImages[map];
            else
                return image;
        }

        private string GetServerDetails(int start, byte[] bytes)
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

        private void btnSetPath_Click(object sender, EventArgs e)
        {
            var result = ofd.ShowDialog(this);
            if (result == DialogResult.OK)
            {

            }
        }

        private void btnLaunch_Click(object sender, EventArgs e)
        {
            var proc = new Process();
            proc.StartInfo.FileName = ofd.FileName;
            proc.StartInfo.Arguments = $"+connect {serverInfos[exListBox1.SelectedIndex].IP}:{serverInfos[exListBox1.SelectedIndex].Port}";
            proc.Start();   
        }
    }

    struct ServerInfo
    {
        public string ServerName;
        public string ServerVersion;
        public string IP;
        public int Port;
        public IPEndPoint endPoint;
    }
}