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

namespace bai2_lab3
{
    public partial class Form1 : Form
    {
        private TcpListener listener;
        private Thread listenThread;
        private bool isListening = false;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void StartListening()
        {
            try
            {
                int port = int.Parse(txtPort.Text);
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();

                isListening = true;
                btnListen.Text = "Stop";
                AddLog($"✅ Đang lắng nghe tại cổng {port}...");

                listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                AddLog("❌ Lỗi: " + ex.Message);
            }
        }

        private void StopListening()
        {
            isListening = false;
            listener?.Stop();
            btnListen.Text = "Listen";
            AddLog("🛑 Đã dừng lắng nghe.");
        }

        private void ListenForClients()
        {
            while (isListening)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    AddLog("📩 Client mới kết nối!");
                    Thread t = new Thread(HandleClient);
                    t.IsBackground = true;
                    t.Start(client);
                }
                catch
                {
                    if (!isListening) break;
                }
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream ns = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytes = ns.Read(buffer, 0, buffer.Length);
                    if (bytes == 0) break;
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                    AddLog("💬 Nhận: " + msg.Trim());
                }
            }
            catch { }

            AddLog("❌ Client ngắt kết nối.");
            client.Close();
        }

        private void AddLog(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (!isListening)
                StartListening();
            else
                StopListening();
        }
    }
}
