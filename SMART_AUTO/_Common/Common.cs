using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Threading.Tasks;

namespace SMART_AUTO
{
    public static class Common
    {
        #region Private Methods

        private static TimeSpan _defaultTimeSpan = new TimeSpan(0, 0, 30);

        #endregion

        #region Public Methods

        public static string WebBrowser { get; set; }
        public static TimeSpan DriverTimeout { get { return _defaultTimeSpan; } set { _defaultTimeSpan = value; } }
        public static IWebDriver CurrentDriver { get; set; }
        public static int OSbit { get; set; }
        public static string currentReportLocation { get; set; }
        public static string currentTestSuite { get; set; }
        public static string currentTestScenario { get; set; }
        public static string scenarioNumberForSS { get; set; }

        public static string ApplicationURL { get; set; }
        public static string ErrorMethod { get; set; }

        public static string DirectoryPath { get; set; }

        #endregion
    }
}
