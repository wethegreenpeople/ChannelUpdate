namespace ChannelChange
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
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.labelIPAddress = new System.Windows.Forms.Label();
            this.labelChannel = new System.Windows.Forms.Label();
            this.textBoxChannel = new System.Windows.Forms.TextBox();
            this.comboBoxIPAddress = new System.Windows.Forms.ComboBox();
            this.checkBoxSetDefault = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(60, 147);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 0;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // labelIPAddress
            // 
            this.labelIPAddress.AutoSize = true;
            this.labelIPAddress.Location = new System.Drawing.Point(12, 11);
            this.labelIPAddress.Name = "labelIPAddress";
            this.labelIPAddress.Size = new System.Drawing.Size(58, 13);
            this.labelIPAddress.TabIndex = 6;
            this.labelIPAddress.Text = "IP Address";
            // 
            // labelChannel
            // 
            this.labelChannel.AutoSize = true;
            this.labelChannel.Location = new System.Drawing.Point(12, 52);
            this.labelChannel.Name = "labelChannel";
            this.labelChannel.Size = new System.Drawing.Size(46, 13);
            this.labelChannel.TabIndex = 8;
            this.labelChannel.Text = "Channel";
            // 
            // textBoxChannel
            // 
            this.textBoxChannel.Location = new System.Drawing.Point(11, 68);
            this.textBoxChannel.Name = "textBoxChannel";
            this.textBoxChannel.Size = new System.Drawing.Size(172, 20);
            this.textBoxChannel.TabIndex = 7;
            // 
            // comboBoxIPAddress
            // 
            this.comboBoxIPAddress.FormattingEnabled = true;
            this.comboBoxIPAddress.Location = new System.Drawing.Point(12, 27);
            this.comboBoxIPAddress.Name = "comboBoxIPAddress";
            this.comboBoxIPAddress.Size = new System.Drawing.Size(172, 21);
            this.comboBoxIPAddress.TabIndex = 6;
            // 
            // checkBoxSetDefault
            // 
            this.checkBoxSetDefault.AutoSize = true;
            this.checkBoxSetDefault.Location = new System.Drawing.Point(11, 94);
            this.checkBoxSetDefault.Name = "checkBoxSetDefault";
            this.checkBoxSetDefault.Size = new System.Drawing.Size(126, 17);
            this.checkBoxSetDefault.TabIndex = 10;
            this.checkBoxSetDefault.Text = "Set Default Channels";
            this.checkBoxSetDefault.UseVisualStyleBackColor = true;
            this.checkBoxSetDefault.CheckedChanged += new System.EventHandler(this.checkBoxSetDefault_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(196, 182);
            this.Controls.Add(this.checkBoxSetDefault);
            this.Controls.Add(this.comboBoxIPAddress);
            this.Controls.Add(this.labelChannel);
            this.Controls.Add(this.textBoxChannel);
            this.Controls.Add(this.labelIPAddress);
            this.Controls.Add(this.buttonUpdate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Label labelIPAddress;
        private System.Windows.Forms.Label labelChannel;
        private System.Windows.Forms.TextBox textBoxChannel;
        private System.Windows.Forms.ComboBox comboBoxIPAddress;
        private System.Windows.Forms.CheckBox checkBoxSetDefault;
    }
}

