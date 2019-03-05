using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.IO;
using SMART_AUTO;
using AventStack.ExtentReports;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace SMART_AUTO
{
    public static class Logging
    {
        public static void LogStop(IWebDriver driver, ExtentTest test, Exception e, MethodBase methodBase, string scenarioTC)
        {
            StackTrace trace = new StackTrace(e);
            string getlatest = "";
            string GetMethoName = "";

            foreach (StackFrame frame in trace.GetFrames())
            {
                if (frame.GetMethod() == methodBase)
                {
                    break;
                }
                getlatest = frame.GetMethod().ToString();
                GetMethoName = frame.GetMethod().Name;
            }
            Common.ErrorMethod = GetMethoName;

            string screenshotName = scenarioTC + ".png";
            string screenshotPath = Path.GetFullPath(Common.currentReportLocation) + "\\" + screenshotName;
            try
            {
                driver._takeScreenshot(screenshotPath);
            }
            catch
            {
                Results.WriteStatus(test, "Stop", "<br><b> ERROR Step : <font color='red'> Browser Not Load Properly or May be Closed. Screenshot Not Generated. </font></b><br>" + "<br><b>ERROR Message:</b><br>" + e.Message + "<br><br><b>StackTrace:</b><br> " + e.StackTrace + ".<br>");
                throw;
            }
            Results.WriteStatus(test, "Fail", "<br><b>ERROR Method : <font color='red'> " + Common.ErrorMethod + "</font></b><br>" + "<br><b>ERROR Message:</b><br>" + e.Message + "<br><br><b>StackTrace:</b><br> " + e.StackTrace + "<br><a> Screenshot of the Page where the execution Stopped.<a>", screenshotName);

            driver.Quit();
        }
    }
}
