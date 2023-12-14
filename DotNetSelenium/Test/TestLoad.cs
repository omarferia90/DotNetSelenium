using AventStack.ExtentReports.Reporter.Config;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports.Model;

namespace DotNetSelenium.Test
{
    public class TestLoad
    {

        //Declare Fields
        public Browser browser;
        public Reporting report;
        private IWebDriver driver;
        public ExtentTest TestCase;


        [OneTimeSetUp]
        public void BeforeClass()
        {
            browser = new Browser();
            report = new Reporting(TestCase);
            report.SetupReport();
        }

        public void SetUpTC(string testCaseName)
        {
            TestCase = report.CreateTestCase(testCaseName);
            browser.StartBrowser(TestContext.Parameters["BrowserName"]);
            this.driver = browser.Driver;
            report.InitDriver(this.driver);
        }

        [Test]
        public void Test1()
        {
            SetUpTC("Test1");
            browser.NavigateTo("https://demoqa.com/");
            report.Log(TestCase, "test1", "Pass", "Test1",true);
            report.Log(TestCase, "test1", "Fail");
            report.Log(TestCase, "test1", "Info", "Test1.1",true);
        }

        [Test]
        public void Test2()
        {
            SetUpTC("Test2");
            browser.NavigateTo("https://demoqa.com/");
            report.Log(TestCase, "test1", "Pass", "", true);
            report.Log(TestCase, "test1", "Fail");
            report.Log(TestCase, "test1", "Info", "", true);
        }

        [Test]
        public void Test3()
        {
            SetUpTC("Test2");
            browser.NavigateTo("https://demoqa.com/");
            report.Log(TestCase, "test1", "Fail");

        }

        [TearDown]
        public void CloseTC()
        {
            browser.CloseBrowser();
            report.Extent.Flush();
        }

        [OneTimeTearDown]
        public void AfterClass()
        {
            report.Extent.Flush();
        }




    }
}
