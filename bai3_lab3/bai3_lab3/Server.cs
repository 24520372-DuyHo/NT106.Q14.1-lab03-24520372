using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai3_lab3
{
    public partial class Server : Form
    {
        private TcpListener listener;
        private Thread listenThread;
        public Server()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (listenThread != null && listenThread.IsAlive)
            {
                rtbMessage.AppendText("Server dang chay san");
                return;
            }
            rtbMessage.AppendText("server started\n");
            btnListen.Enabled = false;
            listenThread = new Thread(startListening);
            listenThread.IsBackground = true;
            listenThread.Start();
        }
        private void startListening()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 8080);
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                string infoClient = client.Client.RemoteEndPoint.ToString();
                rtbMessage.AppendText("Connection accepted from " + infoClient + "\n");
                NetworkStream ns = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = ns.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    rtbMessage.AppendText("From client with love: " + data + "\n");
                }
                ns.Close();
                client.Close();
                listener.Stop();
                rtbMessage.AppendText("Disconnected....\n");
                btnListen.Enabled = true;

            }
            catch (Exception ex)
            {
                rtbMessage.AppendText("Error: " + ex.Message + "\n");
            }
        }
    }
}
