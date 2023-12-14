using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;

namespace DotNetSelenium
{
    public class Browser
    {
        //Local Fields
        private IWebDriver driver;
        private WebDriverWait wait;


        //Local Properties
        public IWebDriver Driver { get { return this.driver; } }
        public WebDriverWait Wait { get { return this.Wait; } }



        /// <summary>
        /// Starts a new intance of a driver.
        /// </summary>
        /// <param name="browserName">Provide a browser like Chrome, Edge or Firefox</param>
        /// <param name="preferredLanguaje">Specify the preferred languaje like us, es, gb, etc.</param>
        public void StartBrowser(string browserName, string preferredLanguaje = "")
        {
            //Select Correct Driver
            switch (browserName.ToUpper())
            {
                case "FIREFOX":
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                    FirefoxOptions optionfirefox = new FirefoxOptions();
                    optionfirefox.AddArgument("no-sandbox");
                    optionfirefox.AddArgument("start-maximized");
                    if (preferredLanguaje != "") { optionfirefox.AddArgument($"--lang={preferredLanguaje}"); }

                    driver = new FirefoxDriver(optionfirefox);
                    break;
                case "EDGE":
                    new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                    EdgeOptions optionedge = new EdgeOptions();
                    optionedge.AddArgument("no-sandbox");
                    optionedge.AddArgument("start-maximized");
                    if (preferredLanguaje != "") { optionedge.AddArgument($"--lang={preferredLanguaje}"); }
                    driver = new EdgeDriver(optionedge);
                    break;
                case "CHROME":
                default:
                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                    ChromeOptions optionchrome = new ChromeOptions();
                    optionchrome.AddArgument("no-sandbox");
                    optionchrome.AddArgument("start-maximized");
                    if (preferredLanguaje != "") { optionchrome.AddArgument($"--lang={preferredLanguaje}"); }
                    driver = new ChromeDriver(optionchrome);
                    break;
            }
            //Driver Setup
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);  //time before throwing an exception
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10); //to wait for a page to load completely before throwing an error
            driver.Manage().Window.Maximize();
            driver.Manage().Cookies.DeleteAllCookies();
            //Init WebDriverWait for JS calls
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Goes to URL given
        /// </summary>
        /// <param name="URL"></param>
        public void NavigateTo(string URL) { driver.Url = URL; }

        /// <summary>
        /// Gets current page title
        /// </summary>
        /// <returns></returns>
        public string GetTile() { return driver.Title; }

        /// <summary>
        /// Gets current url
        /// </summary>
        /// <returns></returns>
        public string GetUrl() { return driver.Url; }

        //Closing and quit driver instance
        public void CloseBrowser()
        {
            try { driver.Close(); driver.Quit(); }
            catch { }
        }

    }
}
