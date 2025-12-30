using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai5_lab3
{
    public partial class Client : Form
    {
        TcpClient client;
        NetworkStream ns;
        bool isConnected = false;
        public Client()
        {
            InitializeComponent();

            txtUser.Text = "Tên người dùng";
            txtUser.ForeColor = Color.Gray;
            txtUser.GotFocus += (s, e) =>
            {
                if (txtUser.Text == "Tên người dùng")
                {
                    txtUser.Text = "";
                    txtUser.ForeColor = Color.Black;
                }
            };
            txtUser.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtUser.Text))
                {
                    txtUser.Text = "Tên người dùng";
                    txtUser.ForeColor = Color.Gray;
                }
            };

            txtDish.Text = "Tên món ăn";
            txtDish.ForeColor = Color.Gray;
            txtDish.GotFocus += (s, e) =>
            {
                if (txtDish.Text == "Tên món ăn")
                {
                    txtDish.Text = "";
                    txtDish.ForeColor = Color.Black;
                }
            };
            txtDish.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtDish.Text))
                {
                    txtDish.Text = "Tên món ăn";
                    txtDish.ForeColor = Color.Gray;
                }
            };
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!isConnected)
                ConnectServer();
            else
                Disconnect();
        }

        private void ConnectServer()
        {
            try
            {
                client = new TcpClient(txtIP.Text, int.Parse(txtPort.Text));
                ns = client.GetStream();
                isConnected = true;
                btnConnect.Text = "Disconnect";
                AddLog("✅ Kết nối server thành công!");
            }
            catch (Exception ex)
            {
                AddLog("❌ Lỗi kết nối: " + ex.Message);
            }
        }

        private void Disconnect()
        {
            ns?.Close();
            client?.Close();
            isConnected = false;
            btnConnect.Text = "Connect";
            AddLog("🔌 Đã ngắt kết nối server.");
        }

        private void SendMessage(string msg)
        {
            if (!isConnected)
            {
                AddLog("⚠️ Vui lòng kết nối đến server trước!");
                return;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(msg);
                ns.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytes = ns.Read(buffer, 0, buffer.Length);
                string reply = Encoding.UTF8.GetString(buffer, 0, bytes);
                AddLog("📩 Server: " + reply);
            }
            catch (Exception ex)
            {
                AddLog("❌ Lỗi khi gửi tin: " + ex.Message);
                Disconnect();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "Tên người dùng" || txtDish.Text == "Tên món ăn")
            {
                MessageBox.Show("⚠️ Vui lòng nhập tên người dùng và món ăn!");
                return;
            }
            SendMessage($"ADD|{txtUser.Text}|{txtDish.Text}");
        }

        private void btnRandomUser_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "Tên người dùng")
            {
                MessageBox.Show("⚠️ Vui lòng nhập tên người dùng!");
                return;
            }
            SendMessage($"RANDOM_USER|{txtUser.Text}");
        }

        private void btnRandomAll_Click(object sender, EventArgs e)
        {
            SendMessage("RANDOM_ALL");
        }

        private void AddLog(string msg)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }
    }
}
