using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ChannelChange
{
    public partial class LogonForm : Form
    {
        private string username;
        private string password;
        private string directory;

        public string Username
        {
            get
            {
                return username;
            }
            set { }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set { }
        }
        public string Directory
        {
            get
            {
                return directory;
            }
            set { }
        }

        public LogonForm()
        {
            InitializeComponent();
        }

        private void buttonCredentials_Click(object sender, EventArgs e)
        {
            try
            {
                username = textBoxUsername.Text;
                password = textBoxPassword.Text;
                directory = @"\\tmccadmn\cloud\departments\BlackBox\ips";
                string directoryWithAuth = String.Format(@"use {2} /user:tmccadmn.tmcc.edu\{0} {1}", username, password, directory);

                // Manually mapping our blackbox drive just in case user doesn't have it mapped
                Process cmd = new Process();
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.FileName = "net.exe";
                cmd.StartInfo.Arguments = directoryWithAuth;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                cmd.WaitForExit();

                Form1 mainForm = new Form1(username, password, directory);
                mainForm.Show();
                this.Hide();
                // Process.Start("net.exe", @"use \\dr-main\departments\BlackBox\ips /delete").WaitForExit();
            }
            catch (Exception error)
            {
                // If an error is caught, we're going to remove the connection we tried to use
                // because if we don't it sometimes won't let us add it again with the correct credentials
                string directory = @"use \\tmccadmn\cloud\departments\BlackBox\ips /delete";
                Process cmd = new Process();
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.FileName = "net.exe";
                cmd.StartInfo.Arguments = directory;
                cmd.StartInfo.UseShellExecute = false;
                cmd.Start();
                cmd.WaitForExit();

                Console.WriteLine(error.Message);
                MessageBox.Show("Error logging in");
            }
        }
    }
}
