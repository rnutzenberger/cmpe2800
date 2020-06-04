namespace RNutzenbergerICA3
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
            this._btnOpen = new System.Windows.Forms.Button();
            this._lsbStats = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // _btnOpen
            // 
            this._btnOpen.Location = new System.Drawing.Point(18, 263);
            this._btnOpen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._btnOpen.Name = "_btnOpen";
            this._btnOpen.Size = new System.Drawing.Size(464, 35);
            this._btnOpen.TabIndex = 0;
            this._btnOpen.Text = "Open";
            this._btnOpen.UseVisualStyleBackColor = true;
            // 
            // _lsbStats
            // 
            this._lsbStats.FormattingEnabled = true;
            this._lsbStats.ItemHeight = 20;
            this._lsbStats.Location = new System.Drawing.Point(18, 12);
            this._lsbStats.Name = "_lsbStats";
            this._lsbStats.Size = new System.Drawing.Size(464, 244);
            this._lsbStats.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 309);
            this.Controls.Add(this._lsbStats);
            this.Controls.Add(this._btnOpen);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _btnOpen;
        private System.Windows.Forms.ListBox _lsbStats;
    }
}

