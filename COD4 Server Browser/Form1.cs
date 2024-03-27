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
            Task.Run(() =>
            {
                var result = ServerCalls.GetAllServerDetails();
                result.Wait();
                Invoke(delegate ()
                {
                    for(int i = 0; i < result.Result.Count; i++)
                    {
                        var item = result.Result[i];
                        exListBox1.Items.Add(new exListBoxItem(
                            i,
                            item.ServerName,
                            $"{item.MapName}, {item.IP}:{item.Port}",
                            mapImages.ContainsKey(item.MapName) ? mapImages[item.MapName] : image));
                    }
                });
            });
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
}