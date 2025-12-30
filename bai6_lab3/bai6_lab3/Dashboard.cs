using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bai6_lab3;

namespace bai6_lab3
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            Server Server = new Server();
            Server.Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Client Client = new Client();
            Client.Show();
        }
    }
}
