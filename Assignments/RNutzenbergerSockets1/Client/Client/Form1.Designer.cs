namespace Client
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
            this._lblStatus = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this._lblGuess = new System.Windows.Forms.Label();
            this._btnGuess = new System.Windows.Forms.Button();
            this._btnConnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // _lblStatus
            // 
            this._lblStatus.AutoSize = true;
            this._lblStatus.Location = new System.Drawing.Point(437, 28);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(168, 20);
            this._lblStatus.TabIndex = 0;
            this._lblStatus.Text = "Press Connect to Play!";
            // 
            // trackBar1
            // 
            this.trackBar1.Enabled = false;
            this.trackBar1.Location = new System.Drawing.Point(12, 69);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(1010, 69);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 1;
            // 
            // _lblGuess
            // 
            this._lblGuess.AutoSize = true;
            this._lblGuess.Location = new System.Drawing.Point(12, 118);
            this._lblGuess.Name = "_lblGuess";
            this._lblGuess.Size = new System.Drawing.Size(18, 20);
            this._lblGuess.TabIndex = 2;
            this._lblGuess.Text = "1";
            // 
            // _btnGuess
            // 
            this._btnGuess.Enabled = false;
            this._btnGuess.Location = new System.Drawing.Point(890, 165);
            this._btnGuess.Name = "_btnGuess";
            this._btnGuess.Size = new System.Drawing.Size(132, 43);
            this._btnGuess.TabIndex = 4;
            this._btnGuess.Text = "Guess!";
            this._btnGuess.UseVisualStyleBackColor = true;
            // 
            // _btnConnect
            // 
            this._btnConnect.Location = new System.Drawing.Point(16, 165);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.Size = new System.Drawing.Size(132, 43);
            this._btnConnect.TabIndex = 5;
            this._btnConnect.Text = "Connect";
            this._btnConnect.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 222);
            this.Controls.Add(this._btnConnect);
            this.Controls.Add(this._btnGuess);
            this.Controls.Add(this._lblGuess);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this._lblStatus);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label _lblGuess;
        private System.Windows.Forms.Button _btnGuess;
        private System.Windows.Forms.Button _btnConnect;
    }
}

