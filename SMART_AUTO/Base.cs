using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using AventStack.ExtentReports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SMART_AUTO
{
    public class Base
    {
        #region Private Variables

        private InternetExplorerOptions _ie;
        private ChromeOptions _chrome;
        private DesiredCapabilities _cap;
        private IWebDriver _driver;

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
            Common.DirectoryPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName).FullName;
            Common.WebBrowser = Bname.ToLower();
            string driverDir = Common.DirectoryPath + ConfigurationManager.AppSettings["DriverDir"];
            string driverPath = "";
            string downloadFilepath = ExtentManager.ResultsDir;

            switch (Bname.ToLower())
            {
                case "firefox":
                    FirefoxOptions _options = new FirefoxOptions();
                    _options.SetPreference("browser.download.dir", downloadFilepath);
                    _options.SetPreference("browser.download.folderList", 2);
                    _options.SetPreference("browser.helperApps.neverAsk.saveToDisk",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;,application/pdf,image/png,image/jpg,image/jpeg,application/x-msexcel,application/excel,application/x-excel,application/vnd.ms-excel,application/zip," +
                        "application/vnd.ms-powerpoint,application/vnd.openxmlformats-officedocument.presentationml.presentation");

                    _options.SetPreference("browser.download.manager.showWhenStarting", false);
                    _options.SetPreference("pdfjs.disabled", true);
                    _driver = new FirefoxDriver(_options);
                    break;
                case "iexplore":
                case "ie":
                    //_cap = DesiredCapabilities.InternetExplorer();
                    //_cap.SetCapability(CapabilityType.AcceptSslCertificates, true);
                    _ie = new InternetExplorerOptions();
                    _ie.IgnoreZoomLevel = true;
                    _ie.EnableNativeEvents = true;
                    _ie.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    driverPath = driverDir + "\\IE";
                    _driver = new InternetExplorerDriver(driverPath, _ie, TimeSpan.FromSeconds(120));
                    break;

                case "chrome":
                    driverPath = driverDir + "\\Chrome";
                    //_cap = DesiredCapabilities.Chrome();
                    //_cap.SetCapability(CapabilityType.AcceptSslCertificates, true);
                    _chrome = new ChromeOptions();
                    _chrome.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                    _chrome.AddUserProfilePreference("download.default_directory", downloadFilepath);
                    _chrome.AddArguments("test-type");
                    _chrome.AddArguments("chrome.switches", "--disable-extensions");
                    _chrome.AddArguments("disable-infobars");
                    _chrome.AddUserProfilePreference("credentials_enable_service", false);
                    _chrome.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);

                    _driver = new ChromeDriver(driverPath, _chrome, TimeSpan.FromSeconds(120));
                    break;
            }

            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Manage().Window.Maximize();

            return _driver;
        }

        #endregion
    }
}