namespace Client
{
    partial class ConnectDialog
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
            this._txbPort = new System.Windows.Forms.TextBox();
            this._btnConnect = new System.Windows.Forms.Button();
            this._pbar = new System.Windows.Forms.ProgressBar();
            this._txbAddr = new System.Windows.Forms.TextBox();
            this._lblAddr = new System.Windows.Forms.Label();
            this._lblPort = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _txbPort
            // 
            this._txbPort.Location = new System.Drawing.Point(12, 112);
            this._txbPort.Name = "_txbPort";
            this._txbPort.ReadOnly = true;
            this._txbPort.Size = new System.Drawing.Size(316, 26);
            this._txbPort.TabIndex = 1;
            this._txbPort.Text = "1666";
            this._txbPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _btnConnect
            // 
            this._btnConnect.Location = new System.Drawing.Point(113, 166);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.Size = new System.Drawing.Size(94, 32);
            this._btnConnect.TabIndex = 2;
            this._btnConnect.Text = "Connect";
            this._btnConnect.UseVisualStyleBackColor = true;
            // 
            // _pbar
            // 
            this._pbar.Location = new System.Drawing.Point(12, 229);
            this._pbar.Name = "_pbar";
            this._pbar.Size = new System.Drawing.Size(316, 39);
            this._pbar.TabIndex = 3;
            // 
            // _txbAddr
            // 
            this._txbAddr.Location = new System.Drawing.Point(12, 38);
            this._txbAddr.Name = "_txbAddr";
            this._txbAddr.Size = new System.Drawing.Size(316, 26);
            this._txbAddr.TabIndex = 4;
            this._txbAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _lblAddr
            // 
            this._lblAddr.AutoSize = true;
            this._lblAddr.Location = new System.Drawing.Point(8, 15);
            this._lblAddr.Name = "_lblAddr";
            this._lblAddr.Size = new System.Drawing.Size(68, 20);
            this._lblAddr.TabIndex = 5;
            this._lblAddr.Text = "Address";
            // 
            // _lblPort
            // 
            this._lblPort.AutoSize = true;
            this._lblPort.Location = new System.Drawing.Point(8, 89);
            this._lblPort.Name = "_lblPort";
            this._lblPort.Size = new System.Drawing.Size(38, 20);
            this._lblPort.TabIndex = 6;
            this._lblPort.Text = "Port";
            // 
            // ConnectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 280);
            this.Controls.Add(this._lblPort);
            this.Controls.Add(this._lblAddr);
            this.Controls.Add(this._txbAddr);
            this.Controls.Add(this._pbar);
            this.Controls.Add(this._btnConnect);
            this.Controls.Add(this._txbPort);
            this.Name = "ConnectDialog";
            this.Text = "ConnectDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _txbPort;
        private System.Windows.Forms.Button _btnConnect;
        private System.Windows.Forms.ProgressBar _pbar;
        private System.Windows.Forms.TextBox _txbAddr;
        private System.Windows.Forms.Label _lblAddr;
        private System.Windows.Forms.Label _lblPort;
    }
}