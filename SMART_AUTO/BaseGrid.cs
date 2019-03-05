using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMART_AUTO
{
    public class BaseGrid
    {
        #region Private Variables
        private IWebDriver _driver;
        private DesiredCapabilities capabilities = new DesiredCapabilities();

        protected ExtentReports extent;
        protected ExtentTest test;
        #endregion

        #region Public Methods

        [OneTimeSetUp]
        public void FixtureInit()
        {
            extent = ExtentManager.Instance;

            string environment = ConfigurationManager.AppSettings["ApplicationURL"];
            Common.ApplicationURL = environment;
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }
        }

        public static IEnumerable<String> BrowserToRun()
        {
            //String[] browsers = { "firefox", "chrome", "ie" };
            IList<String> browsers = ConfigurationManager.AppSettings["Browsers"].Split(',').ToList();

            foreach (String b in browsers)
            {
                yield return b;
            }
        }

        public void starttest(string str, params string[] Category)
        {
            test = extent.CreateTest(str).AssignCategory(Category);
        }

        public IWebDriver StartBrowser(String Bname)
        {
            Common.WebBrowser = Bname;
            DesiredCapabilities capability = new DesiredCapabilities();

            switch (Bname)
            {
                case "firefox":
                    //capabilities = DesiredCapabilities.Firefox();
                    //capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                    _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capabilities); //address of the GRID hub
                    break;
                case "iexplore":
                case "ie":
                    //capabilities = DesiredCapabilities.InternetExplorer();
                    //capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                    _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capabilities);
                    break;
                case "chrome":
                    //capabilities = DesiredCapabilities.Chrome();
                    //capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                    _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capabilities);
                    break;
                case "edge":
                    //capabilities = DesiredCapabilities.Edge();
                    //capabilities.SetCapability(CapabilityType.Platform, new Platform(PlatformType.Windows));
                    _driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), capabilities);
                    break;
            }
            _driver.Manage().Window.Maximize();

            return _driver;
        }

        #endregion
    }
}
