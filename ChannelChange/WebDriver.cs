using System;
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
            Console.Write(website);

            driver.Navigate().GoToUrl(website);

            // Checking to see if there's any alerts that need to be dismissed before
            // we continue on to the next page
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();
                // Giving a 2 second wait here because otherwise we try and fill in our form
                // to quickly after dismissing the popup
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
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                IWebElement channelSubscriptionURL = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("href")));
                channelSubscriptionURL.Clear();
                channelSubscriptionURL.SendKeys(@"https://blackbox.tmcc.edu/channel/" + channel);
            }
            catch
            {
                Console.Write("Could not find element");
            }
        }
    }
}
