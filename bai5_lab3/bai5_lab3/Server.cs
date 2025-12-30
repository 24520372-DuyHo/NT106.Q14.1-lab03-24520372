using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace bai5_lab3
{
    public partial class Server : Form
    {
        TcpListener listener;
        Thread listenThread;
        List<TcpClient> clients = new List<TcpClient>();
        SQLiteConnection conn;
        bool isRunning = false;
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
                StartServer();
            else
                StopServer();
        }

        private void StartServer()
        {
            try
            {
                int port = int.Parse(txtPort.Text.Trim());
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                isRunning = true;
                btnStart.Text = "Stop Server";
                lblStatus.Text = $"Server đang chạy tại cổng {port}";
                AddLog("✅ Server bắt đầu lắng nghe...");

                // Kết nối SQLite
                conn = new SQLiteConnection("Data Source=data.db;Version=3;");
                conn.Open();
                new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS MonAn (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    TenNguoiDung TEXT,
                    TenMon TEXT,
                    NgayThem TEXT
                );", conn).ExecuteNonQuery();

                listenThread = new Thread(ListenClients);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                AddLog("❌ Lỗi: " + ex.Message);
            }
        }

        private void StopServer()
        {
            isRunning = false;
            listener?.Stop();
            conn?.Close();
            btnStart.Text = "Start Server";
            lblStatus.Text = "Server đã dừng";
            AddLog("🛑 Server đã dừng.");
        }

        private void ListenClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    clients.Add(client);
                    AddLog("📩 Client mới kết nối!");
                    Thread t = new Thread(() => HandleClient(client));
                    t.IsBackground = true;
                    t.Start();
                }
                catch { }
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] buffer = new byte[1024];

            while (isRunning)
            {
                try
                {
                    int bytes = ns.Read(buffer, 0, buffer.Length);
                    if (bytes == 0) break;
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
                    AddLog("📥 Nhận: " + msg);
                    string reply = ProcessMessage(msg);
                    byte[] data = Encoding.UTF8.GetBytes(reply);
                    ns.Write(data, 0, data.Length);
                }
                catch { break; }
            }

            AddLog("❌ Client ngắt kết nối.");
            clients.Remove(client);
            client.Close();
        }

        private string ProcessMessage(string msg)
        {
            try
            {
                string[] parts = msg.Split('|');
                string cmd = parts[0];
                if (cmd == "ADD")
                {
                    string user = parts[1];
                    string dish = parts[2];
                    var sql = "INSERT INTO MonAn (TenNguoiDung, TenMon, NgayThem) VALUES (@u,@m,datetime('now'))";
                    var cmdSql = new SQLiteCommand(sql, conn);
                    cmdSql.Parameters.AddWithValue("@u", user);
                    cmdSql.Parameters.AddWithValue("@m", dish);
                    cmdSql.ExecuteNonQuery();
                    return $"✅ Đã thêm món '{dish}' cho {user}";
                }
                else if (cmd == "RANDOM_USER")
                {
                    string user = parts[1];
                    string sql = "SELECT TenMon FROM MonAn WHERE TenNguoiDung=@u ORDER BY RANDOM() LIMIT 1";
                    var cmdSql = new SQLiteCommand(sql, conn);
                    cmdSql.Parameters.AddWithValue("@u", user);
                    var result = cmdSql.ExecuteScalar();
                    return result != null ? $"🎲 {user} nên ăn: {result}" : "Không có món nào!";
                }
                else if (cmd == "RANDOM_ALL")
                {
                    string sql = "SELECT TenMon FROM MonAn ORDER BY RANDOM() LIMIT 1";
                    var result = new SQLiteCommand(sql, conn).ExecuteScalar();
                    return result != null ? $"🎲 Cộng đồng gợi ý: {result}" : "Chưa có dữ liệu!";
                }
                else return "❓ Lệnh không hợp lệ!";
            }
            catch (Exception ex)
            {
                return "❌ Lỗi xử lý: " + ex.Message;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn == null)
                {
                    MessageBox.Show("⚠️ Server chưa khởi động!");
                    return;
                }

                var confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa toàn bộ dữ liệu món ăn?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    new SQLiteCommand("DELETE FROM MonAn;", conn).ExecuteNonQuery();
                    AddLog("🧹 Đã xóa toàn bộ dữ liệu trong bảng MonAn!");
                    MessageBox.Show("✅ Dữ liệu đã được xóa!");
                }
            }
            catch (Exception ex)
            {
                AddLog("❌ Lỗi khi xóa dữ liệu: " + ex.Message);
                MessageBox.Show("Lỗi khi xóa: " + ex.Message);
            }
        }

        private void AddLog(string msg)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }
    }
}
