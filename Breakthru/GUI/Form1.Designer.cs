namespace GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Player1Select = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Player2Select = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Player1Select
            // 
            this.Player1Select.FormattingEnabled = true;
            this.Player1Select.Location = new System.Drawing.Point(843, 231);
            this.Player1Select.Name = "Player1Select";
            this.Player1Select.Size = new System.Drawing.Size(90, 28);
            this.Player1Select.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(839, 302);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(8, 28);
            this.comboBox1.TabIndex = 1;
            // 
            // Player2Select
            // 
            this.Player2Select.FormattingEnabled = true;
            this.Player2Select.Location = new System.Drawing.Point(843, 265);
            this.Player2Select.Name = "Player2Select";
            this.Player2Select.Size = new System.Drawing.Size(90, 28);
            this.Player2Select.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 541);
            this.Controls.Add(this.Player2Select);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Player1Select);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox Player1Select;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox Player2Select;
    }
}

