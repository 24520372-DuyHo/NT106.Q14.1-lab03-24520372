using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai1_lab3
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            UdpClient udpClient = new UdpClient();

            string serverIP = textBoxIP.Text;
            int serverPort;

            if (!int.TryParse(textBoxPort.Text, out serverPort) || serverPort < 1 || serverPort > 65535)
            {
                MessageBox.Show("Vui lòng nhập một số hiệu cổng hợp lệ (1-65535).");
                return;
            }

            IPAddress ipAddress;
            if (!IPAddress.TryParse(serverIP, out ipAddress))
            {
                MessageBox.Show("Vui lòng nhập một địa chỉ IP hợp lệ.");
                return;
            }

            string message = textBoxMessage.Text;
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Vui lòng nhập tin nhắn.");
                return;
            }

            try
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(message);

                udpClient.Send(sendBytes, sendBytes.Length, serverIP, serverPort);

                textBoxMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi: " + ex.Message);
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}
