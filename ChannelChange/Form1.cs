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

namespace ChannelChange
{
    public partial class Form1 : Form
    {
        private WebDriver browser;

        public Form1()
        {
            InitializeComponent();
            browser = new WebDriver();
            MakeIPsFolder();
            UpdateComboBox(); // Adds the txt files from the 'ips' to the combo box
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string ipAddress = comboBoxIPAddress.Text;
            string channel = textBoxChannel.Text;

            // Checking to see if we're using a single IP Address 
            if (comboBoxIPAddress.Text.Contains('.'))
            {
                browser.initFirefox();
                IWebDriver driver = browser.Driver;
                browser.GoToWebsite(username, password, ipAddress);
                browser.ChangeChannelUrl(channel);
            }
            // Or if we're using a range supplied from a .txt file
            else if (!comboBoxIPAddress.Text.Contains('.') && comboBoxIPAddress.Items.Contains(comboBoxIPAddress.Text))
            {
                browser.initFirefox();
                IWebDriver driver = browser.Driver;

                string selectedFile = Directory.GetCurrentDirectory().ToString() + @"\ips\" + comboBoxIPAddress.Text + ".txt";
                string[] ipRange = File.ReadAllLines(selectedFile);

                foreach (string ip in ipRange)
                {
                    browser.GoToWebsite(username, password, ip);
                    browser.ChangeChannelUrl(channel);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            browser.QuitBrowser();
        }

        private void UpdateComboBox()
        {
            string ipFileLocation = Directory.GetCurrentDirectory().ToString() + @"\ips";
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
    }
}
