using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace ChannelChange
{
    public partial class Form1 : Form
    {
        private WebDriver browser;
        private string networkIPDirectory;
        private string username;
        private string password;

        public Form1()
        {
            InitializeComponent();
            browser = new WebDriver();
            MakeIPsFolder();
            UpdateComboBox(); // Adds the txt files from the 'ips' to the combo box
        }

        public Form1(string username, string password, string networkIPDirectory) : this()
        {
            Console.WriteLine("Network directory: " + networkIPDirectory);
            this.networkIPDirectory = networkIPDirectory;
            this.username = username;
            this.password = password;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string ipAddress = comboBoxIPAddress.Text;
            string channel = textBoxChannel.Text;
            int ipsTried = 0;
            int ipsFailed = 0;
            List<string> failedIps = new List<string>();

            // Checking to see if we're using a single IP Address 
            if (comboBoxIPAddress.Text.Contains('.'))
            {
                browser.initFirefox();
                IWebDriver driver = browser.Driver;
                browser.GoToWebsite(username, password, ipAddress);
                ++ipsTried;
                browser.ChangeChannelUrl(channel);
                browser.ApplyChange();
                if (!browser.CheckStatus())
                {
                    ++ipsFailed;
                    failedIps.Add(ipAddress);
                }
                browser.QuitBrowser();
                InformFailedIps(ipsTried, ipsFailed, failedIps);
            }
            // Or if we're using a range supplied from a .txt file
            else if (!comboBoxIPAddress.Text.Contains('.') && comboBoxIPAddress.Items.Contains(comboBoxIPAddress.Text))
            {
                browser.initFirefox();
                IWebDriver driver = browser.Driver;

                string selectedFile;
                string[] ipRange;
                string[] allLocations = Directory.GetFiles(networkIPDirectory);

                if (checkBoxSetDefault.Checked)
                {
                    selectedFile = networkIPDirectory + @"\Default.txt";
                    ipRange = File.ReadAllLines(selectedFile); // every line in a provided .txt file
                    foreach (string ip in ipRange)
                    {
                        ipAddress = ip.Split(',')[0].ToString().Replace(" ", "");
                        // Try to pull a default channel from the .txt file
                        // if none is provided we're going to set the channel to 22
                        try
                        {
                            channel = ip.Split(',')[1].ToString().Replace(" ", "");
                        }
                        catch
                        {
                            Console.WriteLine("No default channel was found for this IP address in the provided .txt file");
                            channel = "22";
                        }
                        Console.WriteLine("IP Address: " + ipAddress + " Channel: " + channel);
                        browser.GoToWebsite(username, password, ipAddress);
                        browser.ChangeChannelUrl(channel);
                        browser.ApplyChange();
                    }
                }
                else if (!checkBoxSetDefault.Checked)
                {
                    selectedFile = networkIPDirectory + @"\" + comboBoxIPAddress.Text + ".txt";
                    ipRange = File.ReadAllLines(selectedFile); // every line in a provided .txt file
                    foreach (string ip in ipRange)
                    {
                        Console.WriteLine("IP Address: " + ipAddress + " Channel: " + channel);
                        browser.GoToWebsite(username, password, ip);
                        browser.ChangeChannelUrl(channel);
                        browser.ApplyChange();
                    }
                }

                browser.QuitBrowser();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser.QuitBrowser();

            string directory = @"use \\dr-main\departments\BlackBox\ips /delete";
            Process cmd = new Process();
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.FileName = "net.exe";
            cmd.StartInfo.Arguments = directory;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.WaitForExit();

            Application.Exit();
        }

        private void UpdateComboBox()
        {
            string ipFileLocation = @"\\dr-main\departments\BlackBox\ips";
            string[] files = Directory.GetFiles(ipFileLocation);
            foreach (string file in files)
            {
                comboBoxIPAddress.Items.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        private void MakeIPsFolder()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory().ToString() + @"\ips\"))
            {
                Directory.CreateDirectory(@"ips");
                File.WriteAllText(Directory.GetCurrentDirectory().ToString() + @"\ips\Example.txt", "Enter all IP Addresses seperated by a newline" + Environment.NewLine + Environment.NewLine + "0.0.0.0" + Environment.NewLine + "1.1.1.1");
            }
        }

        private void InformFailedIps(int ipsTried, int ipsFailed, List<string> failedIps)
        {
            string inform;
            if (failedIps.Count > 0)
            {
                string allFailed = "";
                foreach (string item in failedIps)
                {
                    allFailed += item + ", ";
                }
                inform = String.Format("IPs Tried: {0}\nIPs Failed: {1}\nFailed IPs: {2}", ipsTried, ipsFailed, allFailed);
            }
            else
            {
                inform = String.Format("IPs Tried: {0}\nIPs Failed: {1}", ipsTried, ipsFailed);
            }
            
            MessageBox.Show(inform);
        }

        // If checkbox is checked we're setting all the IPs to their default channels
        // Otherwise we're setting the IP to whatever channel is being provided
        private void checkBoxSetDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSetDefault.Checked)
            {
                textBoxChannel.Enabled = false;
                textBoxChannel.Text = "";
                comboBoxIPAddress.Enabled = false;
                comboBoxIPAddress.SelectedItem = "Default";
            }
            else if (!checkBoxSetDefault.Checked)
            {
                textBoxChannel.Enabled = true;
                comboBoxIPAddress.Enabled = true;
            }
        }
    }
}
