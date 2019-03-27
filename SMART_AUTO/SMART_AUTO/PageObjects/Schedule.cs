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
    public class Schedule
    {
        #region Private Variables

        private IWebDriver schedule;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public Schedule(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.schedule = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.schedule; }
            set { this.schedule = value; }
        }

        /// <summary>
        /// Verify Report Screen Details
        /// </summary>
        /// <returns></returns>
        public Schedule verifyReportScreenDetails()
        {
            string[] menuIcons = { "User", "Files", "Help", "Search" };
            verifyMenuIconOnTopOfScreen(menuIcons);
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");

            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]") || driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No results found')]"))
            {
                PromoDashboard promoDashboard = new PromoDashboard(driver, test);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 6 Months");
            }

            IList<IWebElement> buttons = driver.FindElements(By.XPath("//div[contains(@class,'btn-group btn-grid-actions')]//.//button"));
            string[] buttonNames = { "Export Grid", "Schedule", "View Selected", "Reset Selected", "Export All", "View Selected", "Reset Selected", "Field Options" };
            string[] buttonStatus = { null, "true", "true", "true", null, "true", "true", null };
            int cnt = 0;

            for (int i = 0; i < buttons.Count; i++)
                for (int j = 0; j < buttonNames.Length; j++)
                    if (buttons[i].Text.Contains(buttonNames[j]) == true)
                    {
                        if (buttonNames[j] != "Schedule")
                            Assert.AreEqual(buttons[i].GetAttribute("disabled"), buttonStatus[j], "'" + buttonNames[j] + "' Button not Enable or Disable on Screen.");
                        cnt++;
                        break;
                    }

            Assert.AreEqual(cnt, buttonNames.Length, "Button Name not found on Screen.");
            Results.WriteStatus(test, "Pass", "Verified, Buttons on screen.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']"), "'Pivot Grid' not Present on screen.");
            verifyPaginationForGridSection();
            verifyThumbnailSectionOnScreen();

            Results.WriteStatus(test, "Pass", "Verified, Report Screen Details.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify Records on Reports Screen
        /// </summary>
        /// <returns></returns>
        public Schedule verifyRecordsOnReportScreen()
        {
            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]") || driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No results found')]"))
            {
                PromoDashboard promoDashboard = new PromoDashboard(driver, test);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 6 Months");
            }

            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify Menu Icons on Top of Screen
        /// </summary>
        /// <param name="iconsName">Menu Icon Names to Verify</param>
        /// <returns></returns>
        public Schedule verifyMenuIconOnTopOfScreen(string[] iconsName = null)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='baseexpand']"), "'Navigation Menu' Icon not Present on Page.");

            if (iconsName != null)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right menuItem']"), "'Menu Icons' not Present on top of Screen.");
                IList<IWebElement> menuCollections = driver._findElements("xpath", "//div[@class='pull-right menuItem']");
                foreach (IWebElement menus in menuCollections)
                    Assert.AreEqual(iconsName[menuCollections.IndexOf(menus)], menus.Text, "'" + menus.Text + "' Menu Icon not Present on Top.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Menu Icons on Top of Screen.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify Pagination for Grid Section
        /// </summary>
        /// <returns></returns>
        public Schedule verifyPaginationForGridSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']"), "Pagination Section not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-first page-item disabled']"), "First Icon Default Disable not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-prev page-item disabled']"), "Previous Icon Default Disable not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-page page-item active']"), "Actice First Page not Present.");
            if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-next page-item']") == false)
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-next page-item disabled']"), "Next Icon not Present for Grid.");
            if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-last page-item']") == false)
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-last page-item disabled']"), "Last Icon not Present for Grid.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'10')]"), "Item Per Page '10' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'25')]"), "Item Per Page '25' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'50')]"), "Item Per Page '50' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'100')]"), "Item Per Page '100' not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Pagination for Grid section.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify Thumbnail Section on Screen
        /// </summary>
        /// <returns></returns>
        public Schedule verifyThumbnailSectionOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']"), "Ad Thumbnail not Present for Table View Screen.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//img"), "Ad Image not Present on Section.");
            IWebElement image = driver._findElement("xpath", "//div[@class='aditem aditem-long']//.//img");
            bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
            Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Thumbnail Section.");

            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//div[@class='detail-view-content']"), "Detail View section not Present on Ad Image Section.");

            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'View Ad')]"), "View Ad Icon not Present on Ad Image.");
            if (driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Markets')]"))
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Markets')]"), "Markets Icon not Present on Ad Image.");
            else
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Occurrences')]"), "Occurrences Icon not Present on Ad Image.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Details')]"), "Details Icon not Present on Ad Image.");

            Results.WriteStatus(test, "Pass", "Verified, Thumbnail Section on Screen.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Create New Search Or Click Saved Search To Apply Search On Screen
        /// </summary>
        /// <returns></returns>
        public String createNewSearchOrClickSavedSearchToApplySearchOnScreen(bool applySavedSearch = true)
        {
            string scheduleSearchName = "";
            IList<IWebElement> menus = driver.FindElements(By.XPath("//ul[@class='nav nav-tabs modal-tabs']/li"));
            bool avail = false;
            for (int i = 0; i < menus.Count; i++)
                if (menus[i].Text.Contains("Saved Searches") == true)
                {
                    if (menus[i].GetAttribute("class") != "disabled")
                    {
                        avail = true;
                        driver._clickByJavaScriptExecutor("//ul[@class='nav nav-tabs modal-tabs']/li[" + (i + 1) + "]/a");
                        Results.WriteStatus(test, "Pass", "Clicked, Saved Searches Button on Screen.");
                        break;
                    }
                }

            if (avail == false)
            {
                clickButtonOnScreen("Save As");

                Assert.IsTrue(driver._waitForElement("xpath", "//input[contains(@placeholder,'What would you like to call your search') and @type='text']", 20), "'What would you like to call your search?' textarea not Present.");
                driver._clickByJavaScriptExecutor("//input[contains(@placeholder,'What would you like to call your search') and @type='text']");
                scheduleSearchName = "Test" + driver._randomString(4, true);
                driver._type("xpath", "//input[contains(@placeholder,'What would you like to call your search') and @type='text']", scheduleSearchName);
                Results.WriteStatus(test, "Pass", "Entered Save As Search Report Name on Screen.");

                clickButtonOnScreen("Save!");
                if (applySavedSearch)
                    clickButtonOnScreen("Apply Search");
                else
                    createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");
            }
            else
            {
                if (applySavedSearch)
                {
                    IList<IWebElement> savedSearches = driver.FindElements(By.XPath("//button[contains(@class,'btn-block custom-btn-default') and contains(text(),'Apply Search')]"));
                    scheduleSearchName = driver.FindElement(By.XPath("//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;
                    savedSearches[0].Click();
                    Thread.Sleep(2000);
                    Results.WriteStatus(test, "Pass", "Clicked Saved Seached and Apply Saved Sareches.");
                    driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");
                }
            }

            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading')]");
            Thread.Sleep(2000);
            return scheduleSearchName;
        }

        /// <summary>
        /// Verify Tooltip message on filter section
        /// </summary>
        /// <param name="message">Tooltip Message to Verify</param>
        /// <returns></returns>
        public Schedule verifyTooltipMessageOrClickButtonOnScreen(string buttonName, string message, bool clickButton = false)
        {
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");
            Thread.Sleep(2000);

            IList<IWebElement> buttons = driver.FindElements(By.XPath("//div[@class='btn-group btn-grid-actions']//.//button"));
            for (int i = 0; i < buttons.Count; i++)
                if (buttons[i].Text.Contains(buttonName) == true)
                {
                    driver.MouseHoverByJavaScript(buttons[i]);
                    if (clickButton)
                    {
                        buttons[i].Click();
                        Results.WriteStatus(test, "Pass", "Clicked on '" + buttonName + "' Button on Screen.");
                    }
                    break;
                }

            if (clickButton == false)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='tooltip-inner']"), "Tooltip not Present on Screen.");
                Assert.AreEqual(message, driver._getText("xpath", "//div[@class='tooltip-inner']"), "'" + message + "' Tooltip message not match.");
                Results.WriteStatus(test, "Pass", "Verified, '" + message + "' Tooltip message for '" + buttonName + "' Button on Screen.");
            }
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify Schedule Window
        /// </summary>
        /// <param name="searchName">Search Name to Verify</param>
        /// <returns></returns>
        public Schedule verifyScheduleWindow(string searchName)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='popover-content popover-body']", 20), "Schedule Window not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@class='form-control' and @placeholder='" + searchName + "']"), "'" + searchName + "' Search Name Default not display.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='fa fa-check text-success form-control-feedback']"), "'" + searchName + "' Search Name Feedback in Green Right color not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default dropdown-toggle']"), "Schedule Dropdown not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//button[@class='btn btn-default dropdown-toggle']").Contains("Daily"), "Schedule Dropdown Default 'Daily' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row']/span[contains(text(),'" + searchName + "')]"), "'" + searchName + " will be delivered every day.' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Create Scheduled Export')]"), "'Create Scheduled Export' Button not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Schedule Window on screen.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Click Schedule Dropdown and Verify Lists
        /// </summary>
        /// <returns></returns>
        public Schedule clickScheduleDropdownAndVerifyListsOrClick(string scheduleOption = "")
        {
            driver._clickByJavaScriptExecutor("//div[@class='btn-group']/button[@class='btn btn-default dropdown-toggle']");

            IList<IWebElement> lists = driver.FindElements(By.XPath("//div[@class='btn-group open']/ul/li/a"));
            string[] listNames = { "Daily", "Weekly", "Monthly" };
            for (int i = 0; i < listNames.Length; i++)
                if (scheduleOption != "")
                {
                    if (scheduleOption.Equals(lists[i].Text))
                    {
                        lists[i].Click();
                        Thread.Sleep(500);
                        break;
                    }
                }
                else
                    Assert.AreEqual(lists[i].Text, listNames[i], "'" + lists[i].Text + "' Option not Present on Schedule Dropdown.");

            Results.WriteStatus(test, "Pass", "Clicked, Schedule Dropdown and Verified Lists.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify All Days label or Select on Schedule window
        /// </summary>
        /// <param name="day">Day Name to Select</param>
        /// <returns></returns>
        public Schedule verifyAllDaysLabelOrSelectOnScheduleWindow(string day)
        {
            IList<IWebElement> dayLists = driver.FindElements(By.XPath("//div[@class='btn-group btn-group-no-padding']/button"));
            string[] dayTitles = { "S", "M", "T", "W", "T", "F", "S" };
            for (int i = 0; i < dayTitles.Length; i++)
                if (day != "")
                {
                    dayLists[i].Click();
                    Thread.Sleep(500);
                    Results.WriteStatus(test, "Pass", "Selected, '" + day + "' Day On Schedule Window.");
                    break;
                }
                else
                    Assert.AreEqual(dayLists[i].Text, dayTitles[i], "'" + dayLists[i].Text + "' Day Option not Present on Schedule Dropdown.");

            Results.WriteStatus(test, "Pass", "Verified, All Days label on Window.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify Monthly section on schedule window
        /// </summary>
        /// <returns></returns>
        public Schedule verifyMonthlySectionOnScheduleWindow()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row clickable-row']"), "");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='input-group CFT-spinner']/input"), "Day input text area not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row']/span[contains(text(),'will be delivered every')]"), " 'will be delivered every' not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Monthly Section on Schedule window.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Verify Message for Month on Schedule window
        /// </summary>
        /// <param name="message">verify message</param>
        /// <returns></returns>
        public Schedule verifyMessageForTheMonthOnScheduleWindow(string message)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='row']/span[contains(text(),'" + message + "')]", 10), "Invalid Message not Present.");
            Results.WriteStatus(test, "Pass", "Verified, Message for the Month on Schedule  window.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// Enter Day on Monthly section on Schedule window
        /// </summary>
        /// <returns></returns>
        public String enterDayInMonthlySectionOnScheduleWindow(string dayValue)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='input-group CFT-spinner']/input"), "Day input text area not present.");
            string day = driver._randomString(1, true);
            if (dayValue.Equals("") == false)
                day = dayValue;
            driver._clickByJavaScriptExecutor("//div[@class='input-group CFT-spinner']/input");
            driver._type("xpath", "//div[@class='input-group CFT-spinner']/input", day);
            Thread.Sleep(1000);

            Results.WriteStatus(test, "Pass", "Entered, '" + "" + "' Day in Monthly section on Schedule window.");
            return day;
        }

        /// <summary>
        /// Click Button on Screen
        /// </summary>
        /// <param name="buttonName">Button Name to Click</param>
        /// <returns></returns>
        public Schedule clickButtonOnScreen(string buttonName)
        {
            if (buttonName.Equals("Create Scheduled Export"))
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//button[@class='btn btn-default' and contains(text(),'Create Scheduled Export')]", 20), "'Create Scheduled Export' Button not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Create Scheduled Export')]");
            }

            if (buttonName.Equals("Save As"))
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//button[@class='btn btn-default' and contains(text(),'Save As')]", 20), "'Save As...' Button not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Save As')]");
            }

            if (buttonName.Equals("Save!"))
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//button[@class='btn btn-success' and contains(text(),'Save!')]", 20), "'Save!' Button not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-success' and contains(text(),'Save!')]");
            }

            if (buttonName.Equals("Apply Search"))
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//button[@class='btn btn-primary' and contains(text(),'Apply Search')]", 20), "'Apply Search' Button not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-primary' and contains(text(),'Apply Search')]");
            }

            Thread.Sleep(2000);
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on screen.");
            return new Schedule(driver, test);
        }

        /// <summary>
        /// verify Schedule Message on Screen
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        public Schedule verifyScheduleMessageOnScreen(string message)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-scheduled-export-popover-form//.//span"), "Successfully created a scheduled export for Message not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//cft-scheduled-export-popover-form//.//span").Contains(message), "'" + message + "' message not match.");
            Results.WriteStatus(test, "Pass", "Verified, '" + message + "' Message on Screen.");
            return new Schedule(driver, test);
        }

        #endregion
    }
}
