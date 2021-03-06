﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;
using System.Threading;
using System.Web;

namespace ChannelChange
{
    class WebDriver
    {
        private IWebDriver driver;
        public IWebDriver Driver
        {
            get
            {
                return driver;
            }
            set { }
        }

        public WebDriver()
        {
            
        }

        // Launches the Firefox browser with the geckodriver.exe
        // Since we dont want to have a ton of driver instances
        // we're making sure to close any previous instances before launching a new one.
        public void initFirefox()
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
            service.HideCommandPromptWindow = true; // This CMD window is launched by the driver and gives debugging / status info about current driver
            FirefoxOptions options = new FirefoxOptions();
            service.FirefoxBinaryPath = "Firefox/firefox.exe";

            try
            {
                driver.Quit();
            }
            catch
            {
                Console.WriteLine("No driver to close");
            }
            finally
            {
                driver = new FirefoxDriver(service, options, TimeSpan.FromSeconds(120));
            }
        }

        // Launches the Chrome browser with the chromedriver.exe
        // Since we dont want to have a ton of driver instances
        // we're making sure to close any previous instances before launching a new one.
        public void initChrome()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory());
            service.HideCommandPromptWindow = true; // This CMD window is launched by the driver and gives debugging / status info about current driver
            ChromeOptions options = new ChromeOptions();

            try
            {
                driver.Quit();
            }
            catch
            {
                Console.WriteLine("No driver to close");
            }
            finally
            {
                driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(120));
            }
        }

        // Would like to make this a little more extensible in the future
        // to ensure the browser is being closed regardless of popups or whatever
        public void QuitBrowser()
        {
            try
            {
                driver.Quit();
            }
            catch
            {
                Console.WriteLine("Could not close driver");
            }
        }

        // Will navigate to the channel manager for the IP Address provided
        // Signs in via URL authentication
        // We could sign in by passing our credentials to the pop up box that appears when navigating to the page
        // but its more cumbersome for not a lot of extra benefit
        public void GoToWebsite(string username, string password, string ipAddress)
        {
            string website = String.Format("http://{0}:{1}@{2}/tools/channel_manager?tab=channel_settings", username, password, ipAddress);

            // Ensuring that special characters are being handled in our passwords
            // There are some special characters that the URL can't handle
            // such as '#'
            foreach (char letter in password)
            {
                if (!Char.IsLetterOrDigit(letter))
                {
                    string unicodeChar = Convert.ToByte(letter).ToString("x2");
                    website = website.Replace(letter.ToString(), "%" + unicodeChar);
                }
            }
            Console.WriteLine(website);

            driver.Navigate().GoToUrl(website);

            // Checking to see if there's any alerts that need to be dismissed before
            // we continue on to the next page
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();
                // Giving a 2 second wait here because otherwise we try and fill in our form
                // too quickly after dismissing the popup
                Thread.Sleep(2000);
            }
            catch
            {
                Console.WriteLine("No alert to dismiss");
            }
        }

        // Changes the channel URL based on the given input
        // URL template is https://blackbox.tmcc.edu/channel/<CHANNEL_NUMBER>
        public void ChangeChannelUrl(string channel)
        {
            try
            {
                Console.WriteLine("\nFinding Element on page " + driver.Url);
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement channelSubscriptionURL = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("href")));
                channelSubscriptionURL.Clear();
                channelSubscriptionURL.SendKeys(@"https://blackbox.tmcc.edu/channel/" + channel);
            }
            catch
            {
                Console.Write("Could not find channel URL element");
            }
        }

        // Hitting the apply button on the webpage
        public void ApplyChange()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                IWebElement applyButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//input[@type='submit']")));
                applyButton.Click();
            }
            catch
            {
                Console.WriteLine("Could not find apply button");
            }
        }

        // Checks the status of the changes after they've been applied
        // Returns false if 5 successful status boxes have not been found on page
        public bool CheckStatus()
        {
            bool status = false;

            // There's an alert that pops up if a change could not be applied that I want to dismiss
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch
            {
                Console.WriteLine("No alert to dismiss");
            }

            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement statusChecks = wait.Until(ExpectedConditions.ElementExists(By.ClassName("success_local_message")));
                var elements = driver.FindElements(By.ClassName("success_local_message"));

                Console.WriteLine(elements.Count);

                if (elements.Count != 5)
                {
                    status = false;
                }
                else if (elements.Count == 5)
                {
                    status = true;
                }
            }
            catch
            {
                Console.WriteLine("Could not find success messages");
            }

            return status;
        }
    }
}
