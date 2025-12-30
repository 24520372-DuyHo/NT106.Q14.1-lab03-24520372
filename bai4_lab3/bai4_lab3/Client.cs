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
using System.Xml.Linq;

namespace bai4_lab3
{
    public partial class Client : Form
    {
        private Socket clientSocket;
        private int selectedSeat = 0;
        public Client()
        {
            InitializeComponent();
            btnComfirm.Click += new EventHandler(btnComfirm_Click);
            btnConnect.Click += new EventHandler(btnConnect_Click);
            btnSeat1.Click += new EventHandler(btnSeat1_Click);
            btnSeat2.Click += new EventHandler(btnSeat2_Click);
            btnSeat3.Click += new EventHandler(btnSeat3_Click);
            btnSeat4.Click += new EventHandler(btnSeat4_Click);
            btnSeat5.Click += new EventHandler(btnSeat5_Click);
            btnSeat6.Click += new EventHandler(btnSeat6_Click);
            btnSeat7.Click += new EventHandler(btnSeat7_Click);
            btnSeat8.Click += new EventHandler(btnSeat8_Click);
            btnSeat9.Click += new EventHandler(btnSeat9_Click);
            btnSeat10.Click += new EventHandler(btnSeat10_Click);
            btnSeat11.Click += new EventHandler(btnSeat11_Click);
            btnSeat12.Click += new EventHandler(btnSeat12_Click);
            btnSeat13.Click += new EventHandler(btnSeat13_Click);
            btnSeat14.Click += new EventHandler(btnSeat14_Click);
            btnSeat15.Click += new EventHandler(btnSeat15_Click);
            btnSeat16.Click += new EventHandler(btnSeat16_Click);
            btnSeat17.Click += new EventHandler(btnSeat17_Click);
            btnSeat18.Click += new EventHandler(btnSeat18_Click);
            btnSeat19.Click += new EventHandler(btnSeat19_Click);
            btnSeat20.Click += new EventHandler(btnSeat20_Click);
            btnSeat21.Click += new EventHandler(btnSeat21_Click);
            btnSeat22.Click += new EventHandler(btnSeat22_Click);
            btnSeat23.Click += new EventHandler(btnSeat23_Click);
            btnSeat24.Click += new EventHandler(btnSeat24_Click);
            btnSeat25.Click += new EventHandler(btnSeat25_Click);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse("127.0.0.1"); 
                IPEndPoint remoteEndPoint = new IPEndPoint(ip, int.Parse(txtPort.Text));
                clientSocket.Connect(remoteEndPoint);

                EnableSeatButtons();
                btnConnect.Enabled = false;
                MessageBox.Show("Connected to server!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Thread updateThread = new Thread(ReceiveSeatUpdates);
                updateThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableSeatButtons()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button && control.Name.StartsWith("btnSeat"))
                {
                    control.Enabled = true;
                }
            }
        }

        private void SeatButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int seatNumber = int.Parse(clickedButton.Text);
            if (selectedSeat != 0)
            {
                ResetSelectedSeat();
            }

            selectedSeat = seatNumber;
            clickedButton.BackColor = Color.LightSkyBlue;
        }

        private void ResetSelectedSeat()
        {
            int selectedSeatInt = 0;

            foreach (Control control in this.Controls)
            {
                if (control is Button && control.Name.StartsWith("btnSeat") &&
                    int.TryParse(control.Text, out selectedSeatInt) && selectedSeatInt == selectedSeat)
                {
                    control.BackColor = Color.White;
                    break;
                }
            }
            selectedSeat = 0;
        }


        private void btnComfirm_Click(object sender, EventArgs e)
        {
            if (selectedSeat == 0)
            {
                MessageBox.Show("Please select a seat.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string bookingRequest = $"{txtName.Text},{selectedSeat}";
                byte[] data = Encoding.ASCII.GetBytes(bookingRequest);
                clientSocket.Send(data);

                int selectedSeatInt = 0;
                foreach (Control control in this.Controls)
                {
                    if (control is Button && control.Name.StartsWith("btnSeat") &&
                        int.TryParse(control.Text, out selectedSeatInt) && selectedSeatInt == selectedSeat)
                    {
                        control.BackColor = Color.Gray;
                        control.Enabled = false;
                        break;
                    }
                }

                MessageBox.Show("Seat booked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                selectedSeat = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Booking Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveSeatUpdates()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesReceived = clientSocket.Receive(buffer);
                    string seatUpdates = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

                    if (!string.IsNullOrEmpty(seatUpdates))
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() => UpdateSeats(seatUpdates)));
                        }
                        else
                        {
                            UpdateSeats(seatUpdates);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error receiving seat updates: " + ex.Message);
                    if (clientSocket != null)
                    {
                        clientSocket.Close();
                    }
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => btnComfirm.Enabled = false));
                    }
                    else
                    {
                        btnComfirm.Enabled = false;
                    }
                    break;
                }
            }
        }

        private void UpdateSeats(string seatUpdates)
        {
            string[] seatInfos = seatUpdates.Split(';');

            foreach (string seatInfo in seatInfos)
            {
                if (!string.IsNullOrEmpty(seatInfo))
                {
                    string[] parts = seatInfo.Split(',');
                    int seatNumber = 0;
                    string clientName = "";

                    if (int.TryParse(parts[0], out seatNumber) && parts.Length > 1)
                    {
                        clientName = parts[1];

                        Button btn = this.Controls.Find($"btnSeat{seatNumber}", true).FirstOrDefault() as Button;
                        if (btn != null)
                        {
                            btn.BackColor = Color.Gray;
                            btn.Enabled = false;
                            btn.Text = $"{seatNumber}";

                        }
                    }
                }
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
    }
}
