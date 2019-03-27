using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Configuration;
using System.Data;
using AventStack.ExtentReports;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;
using OpenQA.Selenium.Interactions;

namespace SMART_AUTO
{
    public class BrandCanada_Screen
    {
        #region Private Variables

        private IWebDriver brandCanada_Screen;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public BrandCanada_Screen(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.brandCanada_Screen = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.brandCanada_Screen; }
            set { this.brandCanada_Screen = value; }
        }

        /// <summary>
        /// Verify Promo Dashboard Screen
        /// </summary>
        /// <returns></returns>
        public BrandCanada_Screen verifyBrandCanadaScreen()
        {
            string[] menuIcons = { "User", "Files", "Help", "Search" };
            Schedule schedule = new Schedule(driver, test);
            schedule.verifyMenuIconOnTopOfScreen(menuIcons);

            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");

            Assert.AreEqual(true, driver._isElementPresent("id", "filter-menu"), "'Filter Bar' not Present on Screen.");
            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]") || driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No results found')]"))
            {
                PromoDashboard promoDashboard = new PromoDashboard(driver, test);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 6 Months");
            }

            Results.WriteStatus(test, "Pass", "Verified, Brand Canada Screen.");
            return new BrandCanada_Screen(driver, test);
        }

        #endregion
    }
}
