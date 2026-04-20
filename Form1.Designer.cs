namespace frmcocuni
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
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            grbdetect = new GroupBox();
            lbldetect = new Label();
            grbdetail = new GroupBox();
            btnremove1 = new Button();
            btnremove = new Button();
            txtdetail = new TextBox();
            groupBox3 = new GroupBox();
            pictureBox1 = new PictureBox();
            lblos = new Label();
            grbdetect.SuspendLayout();
            grbdetail.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            // 
            // grbdetect
            // 
            grbdetect.Controls.Add(lbldetect);
            grbdetect.Location = new Point(3, 2);
            grbdetect.Name = "grbdetect";
            grbdetect.Size = new Size(315, 47);
            grbdetect.TabIndex = 0;
            grbdetect.TabStop = false;
            grbdetect.Text = "Detect:";
            // 
            // lbldetect
            // 
            lbldetect.AutoSize = true;
            lbldetect.Location = new Point(50, 19);
            lbldetect.Name = "lbldetect";
            lbldetect.Size = new Size(38, 15);
            lbldetect.TabIndex = 0;
            lbldetect.Text = "label1";
            // 
            // grbdetail
            // 
            grbdetail.Controls.Add(btnremove1);
            grbdetail.Controls.Add(btnremove);
            grbdetail.Controls.Add(txtdetail);
            grbdetail.Location = new Point(3, 55);
            grbdetail.Name = "grbdetail";
            grbdetail.Size = new Size(315, 171);
            grbdetail.TabIndex = 1;
            grbdetail.TabStop = false;
            grbdetail.Text = "Detail:";
            // 
            // btnremove1
            // 
            btnremove1.Enabled = false;
            btnremove1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnremove1.Location = new Point(230, 82);
            btnremove1.Name = "btnremove1";
            btnremove1.Size = new Size(75, 23);
            btnremove1.TabIndex = 4;
            btnremove1.Text = "Remove";
            btnremove1.UseVisualStyleBackColor = true;
            btnremove1.Click += BtnRemove1_Click;
            // 
            // btnremove
            // 
            btnremove.Enabled = false;
            btnremove.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnremove.Location = new Point(13, 82);
            btnremove.Name = "btnremove";
            btnremove.Size = new Size(75, 23);
            btnremove.TabIndex = 3;
            btnremove.Text = "Uninstall";
            btnremove.UseVisualStyleBackColor = true;
            btnremove.Click += BtnRemove_Click;
            // 
            // txtdetail
            // 
            txtdetail.Location = new Point(10, 41);
            txtdetail.Name = "txtdetail";
            txtdetail.ReadOnly = true;
            txtdetail.Size = new Size(295, 23);
            txtdetail.TabIndex = 2;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(pictureBox1);
            groupBox3.Controls.Add(lblos);
            groupBox3.Location = new Point(3, 232);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(315, 47);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "OS:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.images;
            pictureBox1.Location = new Point(269, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 26);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // lblos
            // 
            lblos.AutoSize = true;
            lblos.Location = new Point(6, 25);
            lblos.Name = "lblos";
            lblos.Size = new Size(13, 15);
            lblos.TabIndex = 2;
            lblos.Text = "1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 281);
            Controls.Add(groupBox3);
            Controls.Add(grbdetail);
            Controls.Add(grbdetect);
            MaximizeBox = false;
            MaximumSize = new Size(336, 320);
            MinimizeBox = false;
            MinimumSize = new Size(336, 320);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            grbdetect.ResumeLayout(false);
            grbdetect.PerformLayout();
            grbdetail.ResumeLayout(false);
            grbdetail.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public System.Windows.Forms.Timer timer1;
        private GroupBox grbdetect;
        private Label lbldetect;
        private GroupBox grbdetail;
        private Button btnremove1;
        private Button btnremove;
        private TextBox txtdetail;
        private GroupBox groupBox3;
        private Label lblos;
        private PictureBox pictureBox1;
    }
}
