using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using System.IO;
using System.Configuration;
using AventStack.ExtentReports.Reporter;

namespace SMART_AUTO
{
    internal class ExtentManager
    {
        public static String ReportTime = System.DateTime.Now.ToString("yyyy-dd-MM.hh.mm.ss"); // Report Time
        public static string FolderName = "_SMARTAuto_" + ReportTime; // Report Folder Name
        public static string DirectoryBase = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName).FullName; // Current Repository Path
        public static string ResultsDir = Path.Combine(DirectoryBase + ConfigurationManager.AppSettings["HTMLResultsLocation"], FolderName); // Combine 'Reporsitory Path' With 'Reports File Path'

        public static DirectoryInfo x = System.IO.Directory.CreateDirectory(ResultsDir); // Create Directory

        public static String ResultsFileName = Path.Combine(ResultsDir, FolderName + ".html"); // Extent Report HTML File Path
        public static ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(ResultsFileName);
        public static ExtentReports Instance = new ExtentReports();
    }
}
