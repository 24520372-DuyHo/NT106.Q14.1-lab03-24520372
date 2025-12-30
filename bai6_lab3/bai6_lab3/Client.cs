using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace bai6_lab3
{
    public partial class Client : Form
    {
        private TcpClient client;
        private NetworkStream ns;
        private Thread listenThread;
        private string username = "";
        public Client()
        {
            InitializeComponent();
        }

        private void lab3_bai6_client_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                username = tbName.Text.Trim();
                if (username == "") { MessageBox.Show("vui long nhap ten cua ban"); return; }

                client = new TcpClient("127.0.0.1", 8080);
                ns = client.GetStream();

                byte[] nameData = Encoding.UTF8.GetBytes(username);
                ns.Write(nameData, 0, nameData.Length);

                listenThread = new Thread(ListenFromServer);
                listenThread.IsBackground = true;
                listenThread.Start();
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string msg = tbMessage.Text.Trim();
            if (msg == "") return;
            SendMessage(msg);
            tbMessage.Clear();
        }
        private void ListenFromServer()
        {
            byte[] buffer = new byte[4096];
            while (true)
            {
                try
                {
                    int bytes = ns.Read(buffer, 0, buffer.Length);
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                    if (msg.StartsWith("USERS|"))
                    {
                        string data = msg.Substring(6);
                        this.Invoke(new Action(() =>
                        {
                            rtbLog.Clear();
                            foreach (var name in data.Split('\n'))
                            {
                                if (!string.IsNullOrWhiteSpace(name))
                                    rtbLog.AppendText(name + Environment.NewLine);
                            }
                        }));
                        continue;
                    }
                    Log(msg);
                }
                catch
                {
                    Log("you left the group");
                    break;
                }
            }
        }
        private void SendMessage(string msg)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                ns.Write(data, 0, data.Length);
                Log("Me: " + msg);
            }
            catch (Exception ex)
            {
                Log("Send failed: " + ex.Message);
            }
        }


        private void Log(string msg)
        {
            this.Invoke(new Action(() =>
            {
                rtbDisplay.AppendText(msg + "\n");
            }));
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                client?.Close();
                listenThread?.Abort();
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                rtbLog.Clear();
            }
            catch { }
        }
    }
}
