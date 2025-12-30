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

namespace bai1_lab3
{
    public partial class Server : Form
    {
        private UdpClient udpServer;
        private Thread listenThread;
        public Server()
        {
            InitializeComponent();
            lvMessage.Columns.Add("Tin nhắn nhận được", -2);
        }

        private void StartListening()
        {
            //lắng nghe từ bất kỳ nguồn IP nào
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                try
                {
                    byte[] receiveBytes = udpServer.Receive(ref remoteEP);

                    string receivedData = Encoding.UTF8.GetString(receiveBytes);

                    string message = remoteEP.Address.ToString() + ":" + remoteEP.Port + " - " + receivedData;

                    UpdateMessages(message);
                }
                catch 
                {
                    break;
                }
            }
        }

        private void UpdateMessages(string message)
        {
            if (lvMessage.InvokeRequired)
            {
                lvMessage.Invoke(new MethodInvoker(delegate { UpdateMessages(message); }));
            }
            else
            {
                lvMessage.Items.Add(new ListViewItem(message));
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            int port;
            if (!int.TryParse(textBoxPort.Text, out port) || port < 1 || port > 65535)
            {
                MessageBox.Show("Vui lòng nhập một số hiệu cổng hợp lệ (1-65535).");
                return;
            }

            try
            {
                udpServer = new UdpClient(port);


                listenThread = new Thread(new ThreadStart(StartListening));
                listenThread.IsBackground = true;
                listenThread.Start();

                
                btnListen.Enabled = false;
                textBoxPort.Enabled = false;
                UpdateMessages("Server đang lắng nghe trên port " + port);
            }
            catch (SocketException)
            {
                MessageBox.Show("Lỗi: Port " + port + " đã được sử dụng.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
