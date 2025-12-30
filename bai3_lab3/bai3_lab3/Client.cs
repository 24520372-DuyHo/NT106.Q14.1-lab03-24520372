using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai3_lab3
{
    public partial class Client : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        public Client()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(IPAddress.Parse("127.0.0.1"), 8080);
                stream = client.GetStream();
                MessageBox.Show("Connected to server");
                btnConnect.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to connect: " + ex.Message);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (stream == null)
                {
                    MessageBox.Show("Ban chua ket noi");
                    return;
                }
                string message = tbMessage.Text;
                byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                stream.Write(data, 0, data.Length);
                MessageBox.Show("Gui thanh cong");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gui khong thanh cong: " + ex.Message);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (client != null)
                {
                    client.Close();
                    MessageBox.Show("Disconnected");
                }
                btnConnect.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Disconnected khong thanh cong: " + ex.Message);
            }
        }
    }
}
