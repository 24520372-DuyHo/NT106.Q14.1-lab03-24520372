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

namespace bai6_lab3
{
    public partial class Server : Form
    {
        private TcpListener listener;
        private Thread listenThread;
        private Dictionary<string, TcpClient> clients = new Dictionary<string, TcpClient>();
        public Server()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            btnListen.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            listenThread = new Thread(StartServer);
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                IPEndPoint ip = (IPEndPoint)client.Client.RemoteEndPoint;
                Log($"New client connected from {ip.Address}:{ip.Port}");

                Thread t = new Thread(() => HandleClient(client));
                t.IsBackground = true;
                t.Start();
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] buffer = new byte[4096];
            string username = "";

            try
            {
                int bytes = ns.Read(buffer, 0, buffer.Length);
                username = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
                lock (clients)
                {
                    clients[username] = client;

                }


                while (true)
                {
                    bytes = ns.Read(buffer, 0, buffer.Length);
                    if (bytes == 0) break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();


                    if (msg.StartsWith("/to "))
                    {
                        int sep = msg.IndexOf(':');
                        if (sep > 0)
                        {
                            string target = msg.Substring(4, sep - 4).Trim();
                            string content = msg.Substring(sep + 1).Trim();
                            SendPrivate(username, target, content);
                        }
                    }
                    else
                    {
                        Log($"{username}: {msg}");
                        Broadcast($"{username}: {msg}", username);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"{username} disconnected. ({ex.Message})");
            }
            finally
            {
                lock (clients)
                {
                    if (username != "" && clients.ContainsKey(username))
                        clients.Remove(username);
                }

                Broadcast($"{username} left the room.", username);
                client.Close();
            }
        }

        private void SendPrivate(string from, string to, string msg)
        {
            lock (clients)
            {
                if (clients.ContainsKey(to))
                {
                    var ns = clients[to].GetStream();
                    byte[] data = Encoding.UTF8.GetBytes($"[Private from {from}]: {msg}");
                    ns.Write(data, 0, data.Length);
                    Log($"[Private] {from} to {to}: {msg}");
                }
                else
                {
                    if (clients.ContainsKey(from))
                    {
                        byte[] data = Encoding.UTF8.GetBytes($"User {to} not found.");
                        clients[from].GetStream().Write(data, 0, data.Length);
                    }
                }
            }
        }


        private void Broadcast(string msg, string sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            lock (clients)
            {
                foreach (var kv in clients)
                {
                    if (kv.Key != sender)
                    {
                        try
                        {
                            kv.Value.GetStream().Write(data, 0, data.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void Log(string text)
        {
            this.Invoke(new Action(() =>
            {
                rtbDisplay.AppendText(text + Environment.NewLine);
            }));
        }
    }
}
