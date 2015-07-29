using System.ComponentModel;
using System.Windows.Forms;

namespace MH4U_Cipher_App
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.RegionBox = new System.Windows.Forms.ComboBox();
            this.dlcrad = new System.Windows.Forms.RadioButton();
            this.saverad = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Decrypt";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(12, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Encrypt";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // RegionBox
            // 
            this.RegionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RegionBox.FormattingEnabled = true;
            this.RegionBox.Location = new System.Drawing.Point(60, 35);
            this.RegionBox.Name = "RegionBox";
            this.RegionBox.Size = new System.Drawing.Size(98, 21);
            this.RegionBox.TabIndex = 2;
            // 
            // dlcrad
            // 
            this.dlcrad.AutoSize = true;
            this.dlcrad.Checked = true;
            this.dlcrad.Location = new System.Drawing.Point(12, 12);
            this.dlcrad.Name = "dlcrad";
            this.dlcrad.Size = new System.Drawing.Size(46, 17);
            this.dlcrad.TabIndex = 3;
            this.dlcrad.TabStop = true;
            this.dlcrad.Text = "DLC";
            this.dlcrad.UseVisualStyleBackColor = true;
            // 
            // saverad
            // 
            this.saverad.AutoSize = true;
            this.saverad.Location = new System.Drawing.Point(85, 12);
            this.saverad.Name = "saverad";
            this.saverad.Size = new System.Drawing.Size(73, 17);
            this.saverad.TabIndex = 4;
            this.saverad.Text = "SaveData";
            this.saverad.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Region:";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(170, 122);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saverad);
            this.Controls.Add(this.dlcrad);
            this.Controls.Add(this.RegionBox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "MH4U Cipher - V1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private Button button2;
        internal ComboBox RegionBox;
        private RadioButton dlcrad;
        private RadioButton saverad;
        private Label label1;
    }
}

