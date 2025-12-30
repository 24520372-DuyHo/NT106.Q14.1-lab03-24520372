namespace bai5_lab3
{
    partial class Client
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtDish = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRandomUser = new System.Windows.Forms.Button();
            this.btnRandomAll = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(20, 20);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(120, 23);
            this.txtIP.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(160, 20);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(70, 23);
            this.txtPort.Text = "9000";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(250, 18);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 27);
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(20, 60);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(150, 23);
            // 
            // txtDish
            // 
            this.txtDish.Location = new System.Drawing.Point(190, 60);
            this.txtDish.Name = "txtDish";
            this.txtDish.Size = new System.Drawing.Size(160, 23);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(370, 58);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 27);
            this.btnAdd.Text = "Thêm món";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRandomUser
            // 
            this.btnRandomUser.Location = new System.Drawing.Point(20, 100);
            this.btnRandomUser.Name = "btnRandomUser";
            this.btnRandomUser.Size = new System.Drawing.Size(160, 27);
            this.btnRandomUser.Text = "Ngẫu nhiên (Cá nhân)";
            this.btnRandomUser.UseVisualStyleBackColor = true;
            this.btnRandomUser.Click += new System.EventHandler(this.btnRandomUser_Click);
            // 
            // btnRandomAll
            // 
            this.btnRandomAll.Location = new System.Drawing.Point(200, 100);
            this.btnRandomAll.Name = "btnRandomAll";
            this.btnRandomAll.Size = new System.Drawing.Size(160, 27);
            this.btnRandomAll.Text = "Ngẫu nhiên (Cộng đồng)";
            this.btnRandomAll.UseVisualStyleBackColor = true;
            this.btnRandomAll.Click += new System.EventHandler(this.btnRandomAll_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(20, 140);
            this.txtLog.Multiline = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(450, 220);
            this.txtLog.ReadOnly = true;
            // 
            // Client
            // 
            this.ClientSize = new System.Drawing.Size(500, 380);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtDish);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRandomUser);
            this.Controls.Add(this.btnRandomAll);
            this.Controls.Add(this.txtLog);
            this.Name = "Client";
            this.Text = "Client - Hôm nay ăn gì?";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtDish;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRandomUser;
        private System.Windows.Forms.Button btnRandomAll;
        private System.Windows.Forms.TextBox txtLog;
    }
}
