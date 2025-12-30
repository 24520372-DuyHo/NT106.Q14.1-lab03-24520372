using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bai1_lab3
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            Server f = new Server();
            f.Show();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            Client f = new Client();
            f.Show();
        }
    }
}
