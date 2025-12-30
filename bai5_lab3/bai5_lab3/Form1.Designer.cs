namespace bai5_lab3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.server = new System.Windows.Forms.Button();
            this.client = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // server
            // 
            this.server.Location = new System.Drawing.Point(47, 41);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(102, 38);
            this.server.TabIndex = 0;
            this.server.Text = "Server";
            this.server.UseVisualStyleBackColor = true;
            this.server.Click += new System.EventHandler(this.server_Click);
            // 
            // client
            // 
            this.client.Location = new System.Drawing.Point(202, 41);
            this.client.Name = "client";
            this.client.Size = new System.Drawing.Size(99, 38);
            this.client.TabIndex = 1;
            this.client.Text = "Client";
            this.client.UseVisualStyleBackColor = true;
            this.client.Click += new System.EventHandler(this.client_Click);
            // 
            // Bai5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 111);
            this.Controls.Add(this.client);
            this.Controls.Add(this.server);
            this.Name = "Bai5";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button server;
        private System.Windows.Forms.Button client;
    }
}