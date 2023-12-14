using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AventStack.ExtentReports.Model;
using OpenQA.Selenium;
using AventStack.ExtentReports.Reporter.Config;
using AventStack.ExtentReports.MarkupUtils;
using NUnit.Framework.Interfaces;

namespace DotNetSelenium
{
    public class Reporting
    {
        //Local Fields
        private ExtentReports extent;
        private ExtentSparkReporter spark;
        private ExtentTest testCase;
        private string baseReportFolder;
        private string fullReportFolder;
        public IWebDriver driver;
        private Status status;


        //Properties
        public ExtentReports Extent { get { return this.extent; } }
        public ExtentSparkReporter Spark { get { return this.spark; } }

        //public ExtentTest TestCase { get { return this.testCase; } }
        public string BaseReportFolder { get { return this.baseReportFolder; } }
        public string FullReportFolder { get { return this.fullReportFolder; } }

        public Reporting (ExtentTest Test)
        {
            this.testCase = Test;
        }

        public void InitDriver(IWebDriver driver) 
        {
            this.driver = driver;
        }

        public void SetupReport() 
        {
            extent = new ExtentReports();
            extent.AddSystemInfo("Project", TestContext.Parameters["ProjectName"]);
            extent.AddSystemInfo("Browser", TestContext.Parameters["BrowserName"]);
            extent.AddSystemInfo("Env", TestContext.Parameters["TestEnvironment"]);
            extent.AddSystemInfo("Executed By", Environment.UserName.ToString());
            extent.AddSystemInfo("Executed Machine", Environment.MachineName.ToString());
            extent.AddSystemInfo("Execution Time", DateTime.Now.ToString("MM/ddy/yyy hh:mm:ss"));
            spark = new ExtentSparkReporter(CreateReportFolder());
            spark.Config.Theme = Theme.Dark;
            spark.Config.ReportName = TestContext.Parameters["BrowserName"];
            spark.Config.DocumentTitle = TestContext.Parameters["BrowserName"];
            spark.Config.Encoding = "utf-8";
            extent.AttachReporter(spark);
        }

        private string CreateReportFolder()
        {
            //Get Result Path
            string userID = $"_{Environment.UserName.ToString()}";
            baseReportFolder = TestContext.Parameters["ReportLocation"] + DateTime.Now.ToString("MMddyyyy_hhmmss") + userID + @"\";
            fullReportFolder = BaseReportFolder + TestContext.Parameters["ProjectName"] + @"\" + TestContext.Parameters["ReportName"] + ".html";
            FolderSetup();
            return FullReportFolder;
        }

        private void FolderSetup()
        {
            try
            {
                string[] strSubFolders = new string[2] { "ScreenShots", TestContext.Parameters["ProjectName"] };
                bool blFExist = System.IO.Directory.Exists(BaseReportFolder);
                if (!blFExist)
                {
                    System.IO.Directory.CreateDirectory(BaseReportFolder);
                }
                else
                    blFExist = false;

                foreach (string strFolder in strSubFolders)
                {
                    blFExist = System.IO.Directory.Exists(BaseReportFolder + strFolder);

                    if (!blFExist)
                    {
                        System.IO.Directory.CreateDirectory(BaseReportFolder + strFolder);
                    }
                }
            }
            catch { }
        }

        public Media TakeScreenshot()
        {
            string ssName = $"SC_Image_{DateTime.Now.ToString("MMddyyyy_hhmmss")}.png";
            string strFileLocation = $@"{BaseReportFolder}Screenshots\{ssName}";
            Screenshot screenshot = (driver as ITakesScreenshot).GetScreenshot();
            screenshot.SaveAsFile(strFileLocation);
            var ss = MediaEntityBuilder.CreateScreenCaptureFromPath(strFileLocation).Build();
            return ss;
        }

        public ExtentTest CreateTestCase(string testCase) 
        {
            return extent.CreateTest(testCase);
        }

        public void Log(ExtentTest testCase, string description, string status, string addCategory = "", bool screenshot = false)
        {
            Media tempSS = null;
            if (screenshot) 
            {
                tempSS = TakeScreenshot();
            }

            switch (status.ToUpper().Trim()) 
            {
                case "PASS":
                    testCase.Pass(description, tempSS).AssignCategory(addCategory);
                    break;
                case "FAIL":
                    testCase.Fail(description, tempSS).AssignCategory(addCategory);
                    break;
                case "INFO":
                    testCase.Info(description, tempSS).AssignCategory(addCategory);
                    break;
                case "WARNING":
                    testCase.Pass(description, tempSS).AssignCategory(addCategory);
                    break;
            }
            var x = testCase.Status;
            var objStatus = TestContext.CurrentContext.Result.Outcome.Status;
        }


    }
}
