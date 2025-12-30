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

namespace bai4_lab3
{
    public partial class Server : Form
    {
        private Socket serverSocket;
        private List<Socket> clients = new List<Socket>();
        private Dictionary<int, string> seatStatus = new Dictionary<int, string>();
        private Thread updateThread;
        public Server()
        {
            InitializeComponent();
            btnListen.Click += new EventHandler(btnListen_Click);

            for (int i = 1; i <= 25; i++)
            {
                seatStatus[i] = "";
            }
            UpdateSeatStatus();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, int.Parse(txtPort.Text)));
                serverSocket.Listen(10);
                Thread listenThread = new Thread(AcceptClients);
                listenThread.Start();

                btnListen.Enabled = false;

                MessageBox.Show("Server is listening for connections!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                updateThread = new Thread(SendSeatUpdates);
                updateThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AcceptClients()
        {
            while (true)
            {
                try
                {
                    Socket client = serverSocket.Accept();
                    clients.Add(client);
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                    UpdateConnectionCount();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void HandleClient(Socket client)
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesReceived = client.Receive(buffer);
                    string request = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                    ProcessClientRequest(client, request);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    clients.Remove(client);
                    client.Close();
                    UpdateConnectionCount();
                    break;
                }
            }
        }

        private void ProcessClientRequest(Socket client, string request)
        {
            try
            {
                string[] parts = request.Split(',');
                string clientName = parts[0];
                int seatNumber = int.Parse(parts[1]);

                seatStatus[seatNumber] = clientName;

                if (InvokeRequired)
                {
                    Invoke(new Action(() => UpdateSeatStatus()));
                }
                else
                {
                    UpdateSeatStatus();
                }

                byte[] data = Encoding.ASCII.GetBytes("booked");
                client.Send(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing client request: " + ex.Message);
            }
        }

        private delegate void UpdateConnectionCountDelegate(int connectionCount);
        private void UpdateConnectionCount(int connectionCount)
        {
            Number_of_connections.Text = connectionCount.ToString();
        }
        private void UpdateConnectionCount()
        {
            if (Number_of_connections.InvokeRequired)
            {
                Number_of_connections.Invoke(new UpdateConnectionCountDelegate(UpdateConnectionCount), clients.Count);
            }
            else
            {
                Number_of_connections.Text = clients.Count.ToString();
            }
        }

        private void UpdateSeatStatus()
        {
            for (int i = 1; i <= 25; i++)
            {
                Button btn = this.Controls.Find($"btnSeat{i}", true).FirstOrDefault() as Button;
                if (btn != null)
                {
                    if (seatStatus[i] != "")
                    {
                        btn.BackColor = Color.Gray;
                        btn.Enabled = false;
                        btn.Text = $"{i} ({seatStatus[i]})";
                    }
                    else
                    {
                        btn.BackColor = Color.White;
                        btn.Enabled = true;
                        btn.Text = i.ToString();
                    }
                }
            }

            Number_of_seats_selected.Text = seatStatus.Where(x => x.Value != "").Count().ToString();
            Number_of_empty_seats.Text = seatStatus.Where(x => x.Value == "").Count().ToString();
        }

        private void SendSeatUpdates()
        {
            while (true)
            {
                foreach (Socket client in clients)
                {
                    try
                    {
                        string seatUpdate = "";
                        for (int i = 1; i <= 25; i++)
                        {
                            if (seatStatus[i] != "")
                            {
                                seatUpdate += $"{i},{seatStatus[i]};";
                            }
                        }

                        byte[] data = Encoding.ASCII.GetBytes(seatUpdate);
                        client.Send(data);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error sending seat updates: " + ex.Message);
                        clients.Remove(client);
                    }
                }

                Thread.Sleep(1000); 
            }
        }

        private void btnSeat1_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat2_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat3_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat4_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat5_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat6_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat7_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat8_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat9_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat10_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat11_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat12_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat13_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat14_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat15_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat16_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat17_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat18_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat19_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat20_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat21_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat22_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat23_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat24_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }
        private void btnSeat25_Click(object sender, EventArgs e) { SeatButtonClick(sender, e); }

        private void SeatButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int seatNumber = int.Parse(clickedButton.Text);
            if (seatStatus[seatNumber] != "")
            {
                MessageBox.Show("This seat is already booked!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

            }
        }
    }
}
