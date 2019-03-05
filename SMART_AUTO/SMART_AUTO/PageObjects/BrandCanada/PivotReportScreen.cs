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
    public class PivotReportScreen
    {
        #region Private Variables

        private IWebDriver pivotReportScreen;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public PivotReportScreen(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.pivotReportScreen = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.pivotReportScreen; }
            set { this.pivotReportScreen = value; }
        }

        /// <summary>
        /// Verify Promo Dashboard Screen
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyReportScreenDetails()
        {
            string[] menuIcons = { "User", "Files", "Help", "Search" };
            Schedule schedule = new Schedule(driver, test);
            schedule.verifyMenuIconOnTopOfScreen(menuIcons);

            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");

            Assert.AreEqual(true, driver._isElementPresent("id", "filter-menu"), "'Filter Bar' not Present on Screen.");
            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]") || driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No results found')]"))
            {
                PromoDashboard promoDashboard = new PromoDashboard(driver, test);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last Month");
            }

            IList<IWebElement> buttons = driver.FindElements(By.XPath("//div[contains(@class,'btn-group btn-grid-actions')]//.//button"));
            string[] buttonNames = { "Export Grid", "Schedule", "View Selected", "Reset Selected", "Pivot Options", "Export All", "View Selected", "Reset Selected", "Field Options" };
            string[] buttonStatus = { null, "true", "true", "true", null, null, "true", "true", null };
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
            schedule.verifyPaginationForGridSection();
            schedule.verifyThumbnailSectionOnScreen();

            Results.WriteStatus(test, "Pass", "Verified, Report Screen Details on Page.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click Button on Pivot screen
        /// </summary>
        /// <param name="buttonName"></param>
        /// <returns></returns>
        public PivotReportScreen clickButtonOnPivotScreen(string buttonName)
        {
            IList<IWebElement> buttons = driver.FindElements(By.XPath("//div[contains(@class,'btn-group btn-grid-actions')]//.//button"));
            bool avail = false;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Text.Contains(buttonName))
                {
                    buttons[i].Click();
                    Thread.Sleep(1000);
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + buttonName + "' Button not present.");
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on Screen.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Exporting Grid process to complete
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyExportingGridProcessToComplete()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@ng-show='pivotCtrl.isExporting']"), "Exporting Processing Button not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@ng-show='pivotCtrl.isExporting']/i[@class='fa fa-spinner fa-spin']"), "Spin Icon for Exporting not present.");
            driver._waitForElementToBeHidden("xpath", "//button[@ng-show='pivotCtrl.isExporting']/i[@class='fa fa-spinner fa-spin']");
            Thread.Sleep(4000);
            Results.WriteStatus(test, "Pass", "Verified, Exporting Grid Process to complete.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Pivot options section on screen
        /// </summary>
        /// <param name="avail">Verify Section present or not</param>
        /// <returns></returns>
        public PivotReportScreen verifyPivotOptionsSectionOnScreen(bool avail = true, string fieldName = "None", string[] titles = null)
        {
            if (avail)
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='pivot-options']", 20), "Pivot Options Section not Present on screen.");

                IList<IWebElement> fields = driver.FindElements(By.XPath("//div[contains(@class,'CFT-view-customizer')]//.//div[@class='CFT-view-customizer-section']"));
                string[] fieldsHeader = { "Pivot Fields", "Metrics", "Formatting", "Other Options" };
                for (int i = 0; i < fields.Count; i++)
                {
                    IList<IWebElement> headerName = fields[i]._findElementsWithinElement("xpath", ".//div[@class='row view-customizer-header']");
                    Assert.AreEqual(true, headerName[0].Text.Contains(fieldsHeader[i]), "'" + fieldsHeader[i] + "' Header not match with '" + headerName[0].Text + "' Header.");
                    if (fieldsHeader[i] == fieldName)
                    {
                        IList<IWebElement> lists = fields[i]._findElementsWithinElement("xpath", ".//div[@class='list-group-item']");
                        int cnt = 0;
                        if (titles != null)
                        {
                            for (int j = 0; j < lists.Count; j++)
                                for (int t = 0; t < titles.Length; t++)
                                    if (lists[i].Text.Contains(titles[t]))
                                    {
                                        cnt++;
                                        break;
                                    }

                            Assert.AreEqual(cnt, titles.Length, "Pivot Fields options not match.");
                        }
                    }
                }
            }
            else
                Assert.AreEqual(false, driver._isElementPresent("xpath", "//div[@class='pivot-options']"), "Pivot Options Section Present on screen.");

            Results.WriteStatus(test, "Pass", "Verified, Fields options section on Dashboard screen.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Pivot fields header on Pivot Grid
        /// </summary>
        /// <param name="headers">Header Names to verify</param>
        /// <returns></returns>
        public PivotReportScreen verifyPivotFieldsHeaderOnPivotGrid(string[] headers, bool random)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));

            if (random == true)
            {
                for (int i = 0; i < pivotFieldHeaders.Count; i++)
                    Assert.AreEqual(true, pivotFieldHeaders[i].Text.Contains(headers[i]), "'" + headers[i] + "' Header not match with '" + pivotFieldHeaders[i].Text + "' Grid Header.");
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < pivotFieldHeaders.Count; i++)
                    for (int j = 0; j < headers.Length; j++)
                    {
                        if (pivotFieldHeaders[i].Text.Contains(headers[j]))
                        {
                            cnt++;
                            break;
                        }
                    }

                Assert.AreEqual(cnt, headers.Length, "Checked Header not present with Grid Records.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Pivot fields Header on Pivot Grid Records.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Column Present or not on pivot grid
        /// </summary>
        /// <param name="columnName">Column Name to verify</param>
        /// <param name="present">Column Present or not on grid</param>
        /// <returns></returns>
        public PivotReportScreen verifyColumnPresentOrNotOnPivotGrid(string columnName, bool present)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            bool avail = false;

            for (int i = 0; i < pivotFieldHeaders.Count; i++)
                if (pivotFieldHeaders[i].Text.Contains(columnName))
                    avail = true;

            Assert.AreEqual(avail, present, "Verified, '" + columnName + "' Column on Pivot Grid.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Formatting options fields on Pivot Options Section
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyFormattingOptionsFieldsOnPivotOptionsSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@ng-repeat,'pivotOptionsCtrl.options.formatters')]//.//div[@class='list-group-item']"), "Pivot Fields option not present.");
            IList<IWebElement> formatOptions = driver.FindElements(By.XPath("//div[contains(@ng-repeat,'pivotOptionsCtrl.options.formatters')]//.//div[@class='list-group-item']"));
            string[] optionTitle = { "Spend in Dollars ($)", "Spend in Thousands $(000)" };
            for (int i = 0; i < formatOptions.Count; i++)
            {
                Assert.AreEqual(true, formatOptions[i].Text.Contains(optionTitle[i]), "'" + optionTitle[i] + "' Option not present on section.");
                IWebElement radioOption = formatOptions[i].FindElement(By.XPath(".//input[@name='spend_formatters']"));
                if (optionTitle[i].Equals("Spend in Dollars ($)"))
                    Assert.AreEqual("true", radioOption.GetAttribute("checked"), "'" + optionTitle[i] + "' Radio Option not checked.");
                else
                    Assert.AreEqual(null, radioOption.GetAttribute("checked"), "'" + optionTitle[i] + "' Radio Option not checked.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Formatting options fields on Pivot Options Section.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Other options Section on Pivot Options
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyOtherOptionsSectionOnPivotOptions()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'list-group-other-options')]"), "Other Options Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='list-group-item pivot-options-rank-on']"), "'Rank on' Row not present.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[@class='list-group-item pivot-options-rank-on']").Contains("Rank on"), "'Rank on' Row not present.");
            IWebElement RankOnCheckbox = driver.FindElement(By.XPath("//div[@class='list-group-item pivot-options-rank-on']//.//input[contains(@ng-click,'pivotOptionsCtrl.toggleRankOn')]"));
            Assert.AreEqual(null, RankOnCheckbox.GetAttribute("checked"), "'Rank on' Checkbox is Disable.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'list-group-other-options')]/div[@class='list-group-item']"), "Show Summary Totals Option not present.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[contains(@class,'list-group-other-options')]/div[@class='list-group-item']").Contains("Show Summary Totals"), "Show Summary Totals Option not present.");
            IWebElement summaryTotals = driver.FindElement(By.XPath("//div[contains(@class,'list-group-other-options')]/div[@class='list-group-item']//.//input[contains(@ng-click,'pivotOptionsCtrl.toggleSummary')]"));
            Assert.AreEqual(null, summaryTotals.GetAttribute("checked"), "'Rank on' Checkbox is Disable.");

            Results.WriteStatus(test, "Pass", "Verified, Other Options section on Pivot Options.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Metrics Section on Pivot Options
        /// </summary>
        /// <param name="optionNames">Option Names</param>
        /// <returns></returns>
        public PivotReportScreen verifyMetricsSectionOnPivotOptions(string[] optionNames)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@ng-hide,'missingMetrics')]//.//div[@class='list-group-item']"), "Pivot Fields option not present.");
            IList<IWebElement> metricsCollections = driver.FindElements(By.XPath("//div[contains(@ng-hide,'missingMetrics')]//.//div[@class='list-group-item']"));
            int cnt = 0;

            if (optionNames == null)
            {
                for (int i = 0; i < metricsCollections.Count; i++)
                    Assert.AreEqual(true, metricsCollections[i].FindElement(By.XPath(".//span")).Displayed, "Label with checkbox not present on section.");
            }
            else
            {
                for (int i = 0; i < metricsCollections.Count; i++)
                    for (int j = 0; j < optionNames.Length; j++)
                        if (metricsCollections[i].Text.Contains(optionNames[j]))
                        {
                            cnt++;
                            break;
                        }
                Assert.AreEqual(cnt, optionNames.Length, "Metrics Options not present on List.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Metrics section on Pivot Options.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Checked or UnChecked option from Other Options Section
        /// </summary>
        /// <param name="optionName">Option Name to Checked or UnChecked</param>
        /// <param name="unChecked">UnChecked Option</param>
        /// <returns></returns>
        public PivotReportScreen checkedOUnCheckedOptionFromOtherOptionsSection(string optionName, bool unChecked)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[contains(@class,'list-group-other-options')]/div[contains(@class,'list-group-item')]"), "Other Options not present.");
            IList<IWebElement> otherOptions = driver.FindElements(By.XPath("//div[contains(@class,'list-group-other-options')]/div[contains(@class,'list-group-item')]"));

            for (int i = 0; i < otherOptions.Count; i++)
            {
                if (otherOptions[i].Text.Contains(optionName))
                {
                    IList<IWebElement> input = otherOptions[i]._findElementsWithinElement("xpath", ".//input[@type='checkbox']");
                    if (unChecked)
                    {
                        if (input[0].GetAttribute("checked") == "true")
                        {
                            IWebElement element = otherOptions[i].FindElement(By.XPath(".//label/span"));
                            element.Click();
                            Thread.Sleep(5000);
                            Results.WriteStatus(test, "Pass", "Unchecked, '" + optionName + "' Field from Other Options section.");
                        }
                        else
                            Results.WriteStatus(test, "Pass", "Already Unchecked, '" + optionName + "' Field on Other Options section.");
                    }
                    else
                    {
                        if (input[0].GetAttribute("checked") == null)
                        {
                            IWebElement element = otherOptions[i].FindElement(By.XPath(".//label/span"));
                            element.Click();
                            Thread.Sleep(5000);
                            Results.WriteStatus(test, "Pass", "Checked, '" + optionName + "' Field from Other Options section.");
                        }
                        else
                            Results.WriteStatus(test, "Pass", "Already Checked, '" + optionName + "' Field on Other Options section.");
                    }
                    break;
                }
            }
            driver._waitForElement("xpath", "//span[@class='ag-overlay-loading-center']");
            driver._waitForElementToBeHidden("xpath", "//span[@class='ag-overlay-loading-center']");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Checked or UnChecked option from Metrics Section
        /// </summary>
        /// <param name="optionName">Option Name to Select</param>
        /// <param name="unChecked">Unchecked Option</param>
        /// <returns></returns>
        public PivotReportScreen checkedOUnCheckedOptionFromMetricsSection(string optionName, bool unChecked)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@ng-hide,'missingMetrics')]//.//div[@class='list-group-item']"), "Metrics Options not present.");
            IList<IWebElement> metricsLists = driver.FindElements(By.XPath("//div[contains(@ng-hide,'missingMetrics')]//.//div[@class='list-group-item']"));

            if (optionName.Equals("All"))
            {
                for (int i = 0; i < metricsLists.Count; i++)
                {
                    IList<IWebElement> input = metricsLists[i]._findElementsWithinElement("xpath", ".//input[@type='checkbox']");
                    if (unChecked)
                    {
                        if (input[0].GetAttribute("checked") == "true")
                        {
                            IWebElement element = metricsLists[i].FindElement(By.XPath(".//label/span"));
                            element.Click();
                            Thread.Sleep(2000);
                        }
                    }
                    else
                    {
                        if (input[0].GetAttribute("checked") == null)
                        {
                            IWebElement element = metricsLists[i].FindElement(By.XPath(".//label/span"));
                            element.Click();
                            Thread.Sleep(2000);
                        }
                    }
                }
                Results.WriteStatus(test, "Pass", "Checked or Unchecked All Options from Metrics List.");
            }
            else
                for (int i = 0; i < metricsLists.Count; i++)
                {
                    if (metricsLists[i].Text.Contains(optionName))
                    {
                        IList<IWebElement> input = metricsLists[i]._findElementsWithinElement("xpath", ".//input[@type='checkbox']");
                        if (unChecked)
                        {
                            if (input[0].GetAttribute("checked") == "true")
                            {
                                IWebElement element = metricsLists[i].FindElement(By.XPath(".//label/span"));
                                element.Click();
                                Thread.Sleep(5000);
                                Results.WriteStatus(test, "Pass", "Unchecked, '" + optionName + "' Field from Metrics Options section.");
                            }
                            else
                                Results.WriteStatus(test, "Pass", "Already Unchecked, '" + optionName + "' Field on Metrics Options section.");
                        }
                        else
                        {
                            if (input[0].GetAttribute("checked") == null)
                            {
                                IWebElement element = metricsLists[i].FindElement(By.XPath(".//label/span"));
                                element.Click();
                                Thread.Sleep(5000);
                                Results.WriteStatus(test, "Pass", "Checked, '" + optionName + "' Field from Metrics Options section.");
                            }
                            else
                                Results.WriteStatus(test, "Pass", "Already Checked, '" + optionName + "' Field on Metrics Options section.");
                        }
                        break;
                    }
                }

            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click Rank on Dropdown and Select option from List
        /// </summary>
        /// <param name="optionName">Option Name to Select</param>
        /// <returns></returns>
        public PivotReportScreen clickRankOnDropdownAndSelectOptionFromList(string optionName)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[contains(@class,'ui-select-match')]"), "'Rank on' Dropdown not present.");
            if (driver._isElementPresent("xpath", "//div[contains(@class,'ui-select-match ng-hide')]") == false)
            {
                driver._clickByJavaScriptExecutor("//div[contains(@class,'ui-select-match')]//.//span[contains(text(),'Please select one')]");
                Thread.Sleep(2000);
                Results.WriteStatus(test, "Pass", "Clicked Rank on Dropdown from Section.");
            }

            IList<IWebElement> optionCollections = driver.FindElements(By.XPath("//li[@class='ui-select-choices-group']/div[contains(@class,'ui-select-choices-row')]"));
            if (optionName == "Random")
            {
                Random rand = new Random();
                int x = rand.Next(0, optionCollections.Count);
                optionCollections[x].Click();
                Thread.Sleep(5000);
                Results.WriteStatus(test, "Pass", "Selected '" + optionName + "' Option from Dropdown List.");
            }
            else
            {
                for (int i = 0; i < optionCollections.Count; i++)
                {
                    if (optionCollections[i].Text.Contains(optionName))
                    {
                        optionCollections[i].Click();
                        Thread.Sleep(5000);
                        Results.WriteStatus(test, "Pass", "Selected '" + optionName + "' Option from Dropdown List.");
                        break;
                    }
                }
            }
            driver._waitForElement("xpath", "//span[@class='ag-overlay-loading-center']");
            driver._waitForElementToBeHidden("xpath", "//span[@class='ag-overlay-loading-center']");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Select Option from Formatting section
        /// </summary>
        /// <param name="optionName">Option name to Select</param>
        /// <returns></returns>
        public PivotReportScreen selectOptionFromFormattingSection(string optionName)
        {
            IList<IWebElement> formatOptions = driver.FindElements(By.XPath("//div[contains(@ng-repeat,'pivotOptionsCtrl.options.formatters')]//.//div[@class='list-group-item']"));
            for (int i = 0; i < formatOptions.Count; i++)
            {
                if (formatOptions[i].Text.Contains(optionName) == true)
                {
                    IWebElement radioOptionName = formatOptions[i].FindElement(By.XPath(".//label/span"));
                    radioOptionName.Click();
                    Thread.Sleep(3000);
                    IWebElement radioOption = formatOptions[i].FindElement(By.XPath(".//input[@name='spend_formatters']"));
                    Assert.AreEqual("true", radioOption.GetAttribute("checked"), "'" + optionName + "' Radio option not checked.");
                    break;
                }
            }

            driver._waitForElement("xpath", "//span[@class='ag-overlay-loading-center']");
            driver._waitForElementToBeHidden("xpath", "//span[@class='ag-overlay-loading-center']");
            Results.WriteStatus(test, "Pass", "Selected, '" + optionName + "' Radio option from section.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Pivot Grid Data with Proper format
        /// </summary>
        /// <param name="spendInDollar">Verify Data in Dollar format</param>
        /// <returns></returns>
        public PivotReportScreen verifyPivotGridDataWithProperFormat(bool spendInDollar)
        {
            IList<IWebElement> headerCollections = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            string[] colIDs = new string[5];
            int cnt = 0;
            for (int i = 0; i < headerCollections.Count; i++)
            {
                if (headerCollections[i].Text.Contains("Spend CP"))
                {
                    colIDs[cnt] = headerCollections[i].GetAttribute("col-id");
                    cnt++;
                    if (cnt == 5)
                        break;
                }
            }

            IList<IWebElement> gridRowCollections = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div"));
            for (int i = 0; i < gridRowCollections.Count; i++)
            {
                IList<IWebElement> gridColCollections = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div"));
                for (int j = 0; j < gridColCollections.Count; j++)
                {
                    if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[" + (j + 1) + "]"))
                    {
                        IWebElement attValue = driver.FindElement(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[" + (j + 1) + "]"));
                        for (int c = 0; c < colIDs.Length; c++)
                            if (attValue.GetAttribute("col-id").Equals(colIDs[c]))
                                if (attValue.Text != "")
                                    if (attValue.Text != "$0")
                                    {
                                        if (spendInDollar)
                                            Assert.AreEqual(false, attValue.Text.Contains("."), "'" + attValue.Text + "' Value not display in Dollar.");
                                        else
                                            Assert.AreEqual(true, attValue.Text.Contains("."), "'" + attValue.Text + "' Value not display in Thousands $.");
                                    }
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Pivot Grid Data with Proper format on screen.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Checked or unChecked Pivot fields from Options
        /// </summary>
        /// <param name="fieldName">Field Name to verify</param>
        /// <param name="unChecked">Unchecked Checkbox form list</param>
        /// <returns></returns>
        public PivotReportScreen checkedOrUnCheckedPivotFieldsFromOptions(string fieldName, bool unChecked)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'list-group-pivot')]//.//div[@class='list-group-item']"), "Pivot Fields option not present.");
            IList<IWebElement> pivotFields = driver.FindElements(By.XPath("//div[contains(@class,'list-group-pivot')]//.//div[@class='list-group-item']"));

            if (fieldName.Equals("All Fields"))
            {
                for (int i = 0; i < pivotFields.Count; i++)
                {
                    IList<IWebElement> input = pivotFields[i]._findElementsWithinElement("xpath", ".//input[@type='checkbox']");
                    if (unChecked)
                    {
                        if (input[0].GetAttribute("checked") == "true")
                        {
                            IWebElement element = pivotFields[i].FindElement(By.XPath(".//label/span"));
                            element.Click();
                            Thread.Sleep(2000);
                        }
                    }
                    else
                    {
                        if (input[0].GetAttribute("checked") == null)
                        {
                            IWebElement element = pivotFields[i].FindElement(By.XPath(".//label/span"));
                            element.Click();
                            Thread.Sleep(2000);
                        }
                    }
                }

                Results.WriteStatus(test, "Pass", "Checked or Unchecked, All Fields from Pivot Fields section.");
            }
            else
                for (int i = 0; i < pivotFields.Count; i++)
                {
                    if (pivotFields[i].Text.Contains(fieldName))
                    {
                        IList<IWebElement> input = pivotFields[i]._findElementsWithinElement("xpath", ".//input[@type='checkbox']");
                        if (unChecked)
                        {
                            if (input[0].GetAttribute("checked") == "true")
                            {
                                IWebElement element = pivotFields[i].FindElement(By.XPath(".//label/span"));
                                element.Click();
                                Thread.Sleep(10000);
                                Results.WriteStatus(test, "Pass", "Unchecked, '" + fieldName + "' Field from Pivot Fields section.");
                            }
                            else
                                Results.WriteStatus(test, "Pass", "Already Unchecked, '" + fieldName + "' Field on Pivot Fields section.");
                        }
                        else
                        {
                            if (input[0].GetAttribute("checked") == null)
                            {
                                IWebElement element = pivotFields[i].FindElement(By.XPath(".//label/span"));
                                element.Click();
                                Thread.Sleep(10000);
                                Results.WriteStatus(test, "Pass", "Checked, '" + fieldName + "' Field from Pivot Fields section.");
                            }
                            else
                                Results.WriteStatus(test, "Pass", "Already Checked, '" + fieldName + "' Field on Pivot Fields section.");
                        }
                        break;
                    }
                }

            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Drag and Drop field from Pivot Fields section
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen dragAndDropFieldFromPivotFieldsSection()
        {
            IList<IWebElement> pivotFields = driver.FindElements(By.XPath("//div[contains(@class,'list-group-pivot')]//.//div[@class='list-group-item']"));
            bool avail = false;
            IWebElement fromElement;
            IWebElement toElement;
            int cnt = 0;
            for (int i = 0; i < pivotFields.Count; i++)
            {
                IList<IWebElement> checkBox = pivotFields[i]._findElementsWithinElement("xpath", ".//input[@type='checkbox']");
                if (avail == false)
                    cnt = i;

                fromElement = pivotFields[cnt].FindElement(By.XPath(".//div[@class='icon-no-label']/i[@class='fa fa-arrows']"));
                if (checkBox[0].GetAttribute("checked") == "true")
                {
                    if (avail)
                    {
                        toElement = pivotFields[i].FindElement(By.XPath(".//div[@class='icon-no-label']/i[@class='fa fa-arrows']"));
                        Actions action = new Actions(driver);
                        action.ClickAndHold(fromElement).MoveToElement(toElement).Release(toElement).Build().Perform();
                        break;
                    }
                    avail = true;
                }
            }

            Results.WriteStatus(test, "Pass", "Drag and Drop Field from Pivot Fields section.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify button Disable or not on screen
        /// </summary>
        /// <param name="buttonName">Button Name to Verify</param>
        /// <param name="Disabled">Button Disable or Not</param>
        /// <returns></returns>
        public PivotReportScreen verifyButtonDisableOrNotOnScreen(string buttonName, bool Disabled = true)
        {
            bool avail = false;
            IList<IWebElement> buttonCollections = driver.FindElements(By.XPath("//div[@class='btn-group btn-grid-actions']/button"));
            for (int i = 0; i < buttonCollections.Count; i++)
            {
                if (buttonCollections[i].Text.Contains(buttonName))
                {
                    if (Disabled)
                        Assert.AreEqual(buttonCollections[i].GetAttribute("disabled"), "true", "'" + buttonName + "' Button not Disable.");
                    else
                        Assert.AreEqual(buttonCollections[i].GetAttribute("disabled"), null, "'" + buttonName + "' Button not Enabled.");
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + buttonName + "' Button not Present to verify.");
            Results.WriteStatus(test, "Pass", "Verified, '" + buttonName + "' Button on screen.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click button and verify button Checked or not
        /// </summary>
        /// <param name="buttonName">Button name to verify</param>
        /// <param name="CheckBoxSelected">Checkbox Selected or Not</param>
        /// <returns></returns>
        public PivotReportScreen clickButtonAndVerifyButtonCheckedOrNot(string buttonName, bool CheckBoxSelected)
        {
            bool avail = false;
            IList<IWebElement> buttonCollections = driver.FindElements(By.XPath("//div[@class='btn-group btn-grid-actions']/button"));
            for (int i = 0; i < buttonCollections.Count; i++)
            {
                if (buttonCollections[i].Text.Contains(buttonName))
                {
                    Assert.AreEqual(buttonCollections[i].GetAttribute("disabled"), null, "'" + buttonName + "' Button not Enabled.");
                    buttonCollections[i].Click();

                    if (CheckBoxSelected)
                        Assert.AreEqual(true, buttonCollections[i].FindElement(By.XPath(".//i[@class='fa fa-check-square']")).Displayed, "'" + buttonName + "' Button not Checked.");
                    else
                        Assert.AreEqual(true, buttonCollections[i].FindElement(By.XPath(".//i[@class='fa fa-square text-checkbox-unchecked']")).Displayed, "'" + buttonName + "' Button not Checked.");

                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + buttonName + "' Button not Present to verify.");
            Results.WriteStatus(test, "Pass", "Verified, '" + buttonName + "' Button on screen.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Select Metrics from section
        /// </summary>
        /// <param name="metricsName">Metrocs Name to Select</param>
        /// <returns></returns>
        public PivotReportScreen selectMetricsFromSection(string metricsName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@ng-hide,'missingMetrics')]//.//div[@class='list-group-item']"), "Pivot Fields option not present.");
            IList<IWebElement> metricsCollections = driver.FindElements(By.XPath("//div[contains(@ng-hide,'missingMetrics')]//.//div[@class='list-group-item']"));
            bool avail = false;
            for (int i = 0; i < metricsCollections.Count; i++)
            {
                if (metricsCollections[i].Text.Contains(metricsName))
                {
                    IWebElement checkBoxOption = metricsCollections[i].FindElement(By.XPath(".//label/input[@type='checkbox']"));
                    if (checkBoxOption.GetAttribute("checked") == "true")
                        Results.WriteStatus(test, "Pass", "'" + metricsName + "' Metrics Alreasy Checked from section.");
                    else
                    {
                        metricsCollections[i].FindElement(By.XPath(".//label/span")).Click();
                        Results.WriteStatus(test, "Pass", "Selected, '" + metricsName + "' Metrics from section.");
                    }
                    avail = true;
                    break;
                }
            }
            driver._waitForElement("xpath", "//span[@class='ag-overlay-loading-center']");
            driver._waitForElementToBeHidden("xpath", "//span[@class='ag-overlay-loading-center']");
            Assert.AreEqual(true, avail, "Metrics Options not present on List.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Total Summary Section below Grid on screen
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyTotalSummarySectionBelowGridOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-floating-bottom']"), "Total Summary should not be display bewlow the Pivot grid.");
            Results.WriteStatus(test, "Pass", "Verified, Total Summary section below Pivot Grid on screen.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Pivot Grid screen
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyPivotGridScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agClosed']/i[@class='fa fa-minus-square-o']"), "Total Header with Minus Sign not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//div[@ref='agContainer']/span[text()='Total']"), "Total Label not match.");

            string[] pivotHeades = { "Class", "Company" };
            verifyPivotFieldsHeaderOnPivotGrid(pivotHeades, false);

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//div[@class='ag-header-row']//.//span[text()='Spend CP']"), "'Spend CP' Label not present.");
            Results.WriteStatus(test, "Pass", "Verified, Pivot Grid screen.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click Plus / Minus Button Icon of Total Icon Header from Pivot Grid
        /// </summary>
        /// <param name="clickMinus">Click Minus Icon to Collapsed column</param>
        /// <returns></returns>
        public PivotReportScreen clickPlus_MinusButtoIconOfTotalHeaderFromPivotGrid(bool clickMinus)
        {
            if (clickMinus)
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agClosed' and contains(@class,'ag-hidden')]/i[@class='fa fa-minus-square-o']") == false)
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agClosed']/i[@class='fa fa-minus-square-o']"), "Total Header with Minus Sign not present.");
                    driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agClosed']/i[@class='fa fa-minus-square-o']");
                    Thread.Sleep(3000);
                }
                Results.WriteStatus(test, "Pass", "Clicked, Minus Button Icon of Total Header from Pivot Grid.");
            }
            else
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agOpened' and @class='ag-header-icon ag-header-expand-icon ag-header-expand-icon-expanded']/i[@class='fa fa-plus-square-o']") == true)
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agOpened']/i[@class='fa fa-plus-square-o']"), "Total Header with Plus Sign not present.");
                    driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agOpened']/i[@class='fa fa-plus-square-o']");
                    Thread.Sleep(3000);
                }
                Results.WriteStatus(test, "Pass", "Clicked, Plus Button Icon of Total Header from Pivot Grid.");
            }

            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Plus / Minus icon button on Grid
        /// </summary>
        /// <param name="visible"></param>
        /// <returns></returns>
        public PivotReportScreen verifyPlus_MinusIconButtonOnGrid(bool visible)
        {
            Assert.AreEqual(visible, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agClosed' and contains(@class,'ag-hidden')]/i[@class='fa fa-minus-square-o']"), "Minus Icon button not present.");
            Assert.AreEqual(visible, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']//.//span[@ref='agOpened' and contains(@class,'ag-hidden')]/i[@class='fa fa-plus-square-o']"), "Plus Icon button not present.");
            Results.WriteStatus(test, "Pass", "Verified, Plus / Minus icon Button on Grid.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click Column Header to Sort Data from Pivot Grid
        /// </summary>
        /// <param name="columnName">Column Name to Click</param>
        /// <param name="descendingOrder">To Sort Column in Descending order</param>
        /// <returns></returns>
        public PivotReportScreen clickColumnHeaderToSortDataFromPivotGrid(string columnName, bool descendingOrder)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            string ColId = "";

            for (int i = 0; i < pivotFieldHeaders.Count; i++)
                if (pivotFieldHeaders[i].Text.Contains(columnName))
                {
                    pivotFieldHeaders[i].FindElement(By.XPath(".//span[@ref='eText']")).Click();
                    Thread.Sleep(1000);

                    if (descendingOrder)
                    {
                        if (pivotFieldHeaders[i].FindElement(By.XPath(".//span[@ref='eSortDesc' and @class='ag-header-icon ag-sort-descending-icon']")).Displayed == false)
                        {
                            pivotFieldHeaders[i].FindElement(By.XPath(".//span[@ref='eText']")).Click();
                            Thread.Sleep(3000);
                        }
                        Assert.AreEqual(true, pivotFieldHeaders[i].FindElement(By.XPath(".//span[@ref='eSortDesc' and @class='ag-header-icon ag-sort-descending-icon']")).Displayed, "'" + columnName + "' Column not in Descending order.");
                    }
                    else
                    {
                        if (pivotFieldHeaders[i].FindElement(By.XPath(".//span[@ref='eSortAsc' and @class='ag-header-icon ag-sort-ascending-icon ag-hidden']")).Displayed == false)
                        {
                            pivotFieldHeaders[i].FindElement(By.XPath(".//span[@ref='eText']")).Click();
                            Thread.Sleep(3000);
                        }
                        Assert.AreEqual(true, pivotFieldHeaders[i].FindElement(By.XPath(".//span[@ref='eSortAsc' and @class='ag-header-icon ag-sort-ascending-icon']")).Displayed, "'" + columnName + "' Column not in Ascending order.");
                    }
                    ColId = pivotFieldHeaders[i].GetAttribute("col-id");
                    break;
                }

            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div"));
            IList<string> all = new List<string>(1);
            int count = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']") == true)
                {
                    IWebElement webElementBody = driver._findElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']/div[@col-id='" + ColId + "']");
                    all.Add(webElementBody.Text.Replace(",", "").Replace("*,", ""));
                    count = count + 1;
                }
            }

            for (int i = 0; i < count - 1; i++)
            {
                if (descendingOrder)
                    Assert.GreaterOrEqual(all[i], all[(i + 1)]);
                else
                    Assert.LessOrEqual(all[i], all[(i + 1)]);
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + columnName + "' Header to Sort Data from Pivot Grid and Verified Sorted Data.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click Filter icon and verify Section
        /// </summary>
        /// <param name="headerName">Header name to click and verify section</param>
        /// <returns></returns>
        public PivotReportScreen clickFilterIconAndVerifySection(string headerName)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));

            for (int i = 0; i < pivotFieldHeaders.Count; i++)
            {
                if (pivotFieldHeaders[i].Text.Contains(headerName))
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')][" + (i + 1) + "]//.//span[@ref='eMenu']/i"), "'" + headerName + "' Filter Icon not Present.");
                    driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')][" + (i + 1) + "]//.//span[@ref='eMenu']/i");
                    Thread.Sleep(500);
                    Assert.IsTrue(driver._isElementPresent("id", "tabBody"), "'" + headerName + "' Section not Present.");
                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='ag-mini-filter']/input"), "'" + headerName + "' Textbox not Present.");

                    Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='ag-filter-header-container']/label/span[contains(text(),'Select All')]"), "'" + headerName + "' Select All Checkbox not Present.");
                    Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='ag-filter-header-container']//.//div[contains(@id,'selectAll')]/i")).GetCssValue("color").Contains("0, 74, 82"), "'Select All' Option Default not Selected.");

                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='richList']"), "'" + headerName + "' Fields Values not Present.");
                    IList<IWebElement> filterValues = driver.FindElements(By.XPath("//*[@id='richList']/div/div/div"));
                    for (int j = 0; j < filterValues.Count; j++)
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='richList']/div/div/div[" + (j + 1) + "]/label/div[@class='ag-filter-checkbox']"), "Values Option Default not Selected.");

                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + headerName + "' Filter Icon and Verified Section.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Select Option from filter bar section
        /// </summary>
        /// <param name="optionName">Option Name to Select</param>
        /// <param name="unChecked">Uncheck Option from list</param>
        /// <returns></returns>
        public String selectOptionFromFilterBarSection(string optionName, bool unChecked = false)
        {
            string fieldValue = "Select All";
            if (optionName.Equals("Select All"))
            {
                if (driver.FindElement(By.XPath("//div[@class='ag-filter-header-container']//.//div[@id='selectAll']/i")).GetCssValue("color").Contains("0, 74, 82") == unChecked)
                {
                    driver._clickByJavaScriptExecutor("//div[@class='ag-filter-header-container']//.//div[@id='selectAll']/i");
                    Thread.Sleep(500);
                }
            }

            if (optionName.Equals("Random"))
            {
                IList<IWebElement> filterValues = driver.FindElements(By.XPath("//*[@id='richList']/div/div/div"));
                Random rand = new Random();
                int x = rand.Next(0, filterValues.Count);
                fieldValue = filterValues[x].Text;
                filterValues[x].Click();
            }

            Results.WriteStatus(test, "Pass", "Selected, '" + fieldValue + "' Option from Filter bar Section.");
            return fieldValue;
        }

        /// <summary>
        /// Enter and Verify keyword into Filter search textarea
        /// </summary>
        /// <param name="size">Length of string to enter</param>
        /// <returns></returns>
        public PivotReportScreen enterAndVerifyKeywordInToFilterSearchTextbox(int size = 0)
        {
            IList<IWebElement> filterValues = driver.FindElements(By.XPath("//*[@id='richList']/div/div/div"));
            Random rand = new Random();
            int x = rand.Next(0, filterValues.Count);
            string searchTextbox = filterValues[x].Text;
            if (size == 0)
                driver._type("xpath", "//*[@id='ag-mini-filter']/input", searchTextbox);
            else
                driver._type("xpath", "//*[@id='ag-mini-filter']/input", searchTextbox.Substring(0, 5));
            Thread.Sleep(500);

            filterValues = driver.FindElements(By.XPath("//*[@id='richList']/div/div/div"));
            for (int j = 0; j < filterValues.Count; j++)
                Assert.AreEqual(true, filterValues[j].Text.Contains(searchTextbox), "'" + searchTextbox + "' Keyword not Present.");

            Results.WriteStatus(test, "Pass", "Entered, '" + searchTextbox + "' Keyword in Filter Search textbox and Verified Filter Record.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify font color of search value on filter section
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyFontColorOfSearchValueOnFilterSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='ag-mini-filter']/input"), "Search textarea not present.");
            Assert.AreNotEqual("", driver._getValue("xpath", "//*[@id='ag-mini-filter']/input"), "Search textarea should not be Blank to verify font color.");
            Assert.True(driver.FindElement(By.XPath("//*[@id='ag-mini-filter']/input")).GetCssValue("color").Contains("rgba(0, 74, 82, 1)"), "'Navy Blue' color not match for Search Textarea.");

            Results.WriteStatus(test, "Pass", "Verified, font color of Search value on Filter section.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Clear Search Textbox on Filter section
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen clearSearchTextboxOnFilterSection()
        {
            IWebElement toClear = driver.FindElement(By.XPath("//*[@id='ag-mini-filter']/input"));
            toClear.SendKeys(Keys.Control + "a");
            toClear.SendKeys(Keys.Delete);
            Thread.Sleep(500);
            Results.WriteStatus(test, "Pass", "Cleared, Search textbox on Filter Section.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click Column header filter icon and verify filter section
        /// </summary>
        /// <param name="headerName">Header Name to click</param>
        /// <returns></returns>
        public String clickColumnHeaderFilterIconAndVerifyFilterSection(string headerName, bool defaultOption = true)
        {
            IList<IWebElement> headerCollections = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            string columnId = "";
            for (int i = 0; i < headerCollections.Count; i++)
            {
                if (headerCollections[i].Text.Contains(headerName))
                {
                    columnId = headerCollections[i].GetAttribute("col-id");
                    IWebElement barOption = headerCollections[i].FindElement(By.XPath(".//span[@ref='eMenu']/i"));
                    barOption.Click();
                    break;
                }
            }

            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='ag-filter']"), "Filter Section not Open.");
            Assert.AreEqual(true, driver._isElementPresent("id", "filterType"), "Filter Dropdown List not Present.");
            if (defaultOption)
            {
                IWebElement element = driver._findElement("id", "filterType");
                var selectedItemText = (string)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].options[arguments[0].selectedIndex].text;", element);
                Assert.AreEqual("Equals", selectedItemText, "Default 'Equals' not Selected.");

                Assert.AreEqual(true, driver._isElementPresent("id", "filterText"), "'Filter...' text area not Present.");
                Assert.AreEqual("Filter...", driver._getAttributeValue("id", "filterText", "placeholder"), "'Filter...' text area Placeholder not match.");
            }

            Assert.AreEqual(true, driver._isElementPresent("id", "filterType"), "'Filter' Option not Present.");
            driver._click("id", "filterType");
            Thread.Sleep(2000);
            IList<IWebElement> filterConditions = driver._findElements("xpath", "//select[@id='filterType']/option");
            string[] filterTitle = { "Equals", "Not equal", "Less than", "Less than or equals", "Greater than", "Greater than or equals", "In range" };

            for (int i = 0; i < filterConditions.Count; i++)
                Assert.AreEqual(filterConditions[i].Text, filterTitle[i], "'" + filterTitle[i] + "' Option not present.");

            Results.WriteStatus(test, "Pass", "Clicked '" + headerName + "' Column Filter Icon and Verified Section.");
            return columnId;
        }

        /// <summary>
        /// Select Condition from Filter Option
        /// </summary>
        /// <param name="condition">condition to Select</param>
        /// <returns></returns>
        public PivotReportScreen selectConditionFromFilterOption(string columnId, string condition)
        {
            if (driver._isElementPresent("id", "filterType") == false)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i"));
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i");
                Results.WriteStatus(test, "Pass", "Clicked Column Header to open Filter popup window.");
            }

            IList<IWebElement> filterConditions = driver._findElements("xpath", "//select[@id='filterType']/option");
            IWebElement toClear = driver.FindElement(By.XPath("//*[@id='filterType']/option"));

            for (int i = 0; i < filterConditions.Count; i++)
                if (filterConditions[i].Text == condition)
                {
                    filterConditions[i].Click();
                    Thread.Sleep(1000);
                    break;
                }

            var selectedText = (string)((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].options[arguments[0].selectedIndex].text;", driver._findElement("id", "filterType"));
            Assert.AreEqual(selectedText, condition, "Filter Condition not Selected Properly.");
            Results.WriteStatus(test, "Pass", "Selected '" + condition + "' Condition from Filter Option.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Enter or Clear Value from filter text area
        /// </summary>
        /// <param name="columnId">Column id to get grid value</param>
        /// <param name="clearText">Clear Filter text</param>
        /// <returns></returns>
        public String enterOrClearValueFromFilterTextArea(string columnId, bool clearText, string enterValue = "")
        {
            if (driver._isElementPresent("id", "filterText") == false)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i"));
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i");
                Results.WriteStatus(test, "Pass", "Clicked Column Header to open Filter popup window.");
            }

            Assert.AreEqual(true, driver._isElementPresent("id", "filterText"), "'Filter' Text area not Present.");
            if (clearText)
            {
                IWebElement toClear = driver.FindElement(By.Id("filterText"));
                toClear.SendKeys(Keys.Control + "a");
                toClear.SendKeys(Keys.Delete);
                Thread.Sleep(1000);

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]"), "Header not present.");
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]");
                Results.WriteStatus(test, "Pass", "Cleared Filter Value From Filter Text area.");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[1]/div[@col-id='" + columnId + "']"), "Grid Record Value not present for Column.");
                bool blank = false;
                if (enterValue == "")
                {
                    blank = true;
                    enterValue = driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[1]/div[@col-id='" + columnId + "']").Trim().Replace("$", "").Replace(",", "");
                }
                driver._type("id", "filterText", enterValue);
                Thread.Sleep(1000);

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]"), "Header not present.");
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]");

                Assert.AreEqual(blank, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//div[@class='table-filter-applied']"), "Yellow Filter Icon not present.");
                Results.WriteStatus(test, "Pass", "Entered, '" + enterValue + "' Value In Filter Text area.");
            }

            return enterValue;
        }

        /// <summary>
        /// Enter Text in Filter input area and verify filter icon not display
        /// </summary>
        /// <param name="columnId">Column Id to click and verify Value</param>
        /// <param name="enterValue">Value to Enter</param>
        /// <returns></returns>
        public String enterTextInFilterInputAreaAndVerifyFilterIconNotDisplay(string columnId, string enterValue)
        {
            if (driver._isElementPresent("id", "filterText") == false)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i"));
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i");
                Results.WriteStatus(test, "Pass", "Clicked Column Header to open Filter popup window.");
            }

            Assert.AreEqual(true, driver._isElementPresent("id", "filterText"), "'Filter' Text area not Present.");
            driver._type("id", "filterText", enterValue);
            Thread.Sleep(1000);

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]"), "Header not present.");
            driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]");

            Assert.AreEqual(false, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//div[@class='table-filter-applied']"), "Yellow Filter Icon not present.");
            Results.WriteStatus(test, "Pass", "Entered, '" + enterValue + "' Value In Filter Text area and Verified Filtered Yellow Icon not Display.");
            return enterValue;
        }

        /// <summary>
        /// Enter or clear In Range value on filter Textarea
        /// </summary>
        /// <param name="columnId">Column id to get column record</param>
        /// <param name="clearText">To Clear entered text in filter area</param>
        /// <returns></returns>
        public String enterOrClearInRangeValueOnFilterTextArea(string columnId, bool clearText)
        {
            if (driver._isElementPresent("id", "filterText") == false)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i"));
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//span[@ref='eMenu']/i");
                Results.WriteStatus(test, "Pass", "Clicked Column Header to open Filter popup window.");
            }

            Assert.AreEqual(true, driver._isElementPresent("id", "filterText"), "From 'Filter' Text area not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "filterToText"), "To 'Filter' Text area not Present.");
            string enterValue = "";

            if (clearText)
            {
                IWebElement fromClear = driver.FindElement(By.Id("filterText"));
                fromClear.SendKeys(Keys.Control + "a");
                fromClear.SendKeys(Keys.Delete);
                Thread.Sleep(1000);
                IWebElement toClear = driver.FindElement(By.Id("filterToText"));
                toClear.SendKeys(Keys.Control + "a");
                toClear.SendKeys(Keys.Delete);
                Thread.Sleep(1000);

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]"), "Header not present.");
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]");

                Results.WriteStatus(test, "Pass", "Cleared Filter Value From Filter Text area.");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[1]/div[@col-id='" + columnId + "']"), "Grid Record Value not present for Column.");
                enterValue = driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[1]/div[@col-id='" + columnId + "']").Trim().Replace("$", "").Replace(",", "");
                driver._type("id", "filterText", enterValue);
                Thread.Sleep(1000);

                string uppwerLimit = Convert.ToString((Convert.ToInt32(enterValue) + 50));
                driver._type("id", "filterToText", uppwerLimit);
                Thread.Sleep(1000);

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]"), "Header not present.");
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'group-cell-with-group')][1]");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable') and @col-id='" + columnId + "']//.//div[@class='table-filter-applied']"), "Yellow Filter Icon not present.");
                Results.WriteStatus(test, "Pass", "Entered, '" + enterValue + "' Value In Filter Text area.");
            }

            return enterValue;
        }

        /// <summary>
        /// Verify Filtered Value on Grid for column
        /// </summary>
        /// <param name="columnId">Column Id to verify Value</param>
        /// <param name="filterValue">Filter value to verify with grid data</param>
        /// <param name="filterType">filter type to perform action</param>
        /// <returns></returns>
        public PivotReportScreen verifyFilteredValueOnGridForColumn(string columnId, string filterValue, string filterType)
        {
            if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div") == true)
            {
                IList<IWebElement> gridCollection = driver._findElements("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div");
                string textname = "";
                decimal var1 = 0;
                decimal var2 = 0;

                for (int i = 0; i < gridCollection.Count; i++)
                {
                    if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[@col-id='" + columnId + "']"))
                    {
                        var1 = Convert.ToDecimal(driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[@col-id='" + columnId + "']").Replace("$", "").Replace(",", ""));
                        var2 = Convert.ToDecimal(filterValue);

                        if (filterType.Equals("Contains"))
                            Assert.AreEqual(true, driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[@col-id='" + columnId + "']").ToLower().Replace("$", "").Replace(",", "").Contains(filterValue.ToLower()), "Filter Label Column Value not match.");

                        if (filterType.Equals("Equals"))
                            Assert.AreEqual(var1, var2, "Filter Label Column Value not match.");

                        if (filterType.Equals("Not Equals"))
                            Assert.AreNotEqual(var1, var2, "Filter Label Column Value matched.");

                        if (filterType.Equals("Less than"))
                            Assert.Less(var1, var2, "Filter Label Column Value not match.");

                        if (filterType.Equals("Less than or equals"))
                            Assert.LessOrEqual(var1, var2, "Filter Label Column Value not match.");

                        if (filterType.Equals("Greater than"))
                            Assert.Greater(var1, var2, "Filter Label Column Value not match.");

                        if (filterType.Equals("Greater than or equals"))
                            Assert.GreaterOrEqual(var1, var2, "Filter Label Column Value not match.");

                        if (filterType.Equals("Starts with"))
                            Assert.AreEqual(filterValue.Substring(0, filterValue.Length).ToLower(), driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[@col-id='" + columnId + "']").Substring(0, filterValue.Length).ToLower().Replace("$", "").Replace(",", ""), "Filter Label Column Value not Start With Expected.");

                        if (filterType.Equals("Ends with"))
                        {
                            textname = driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[@col-id='" + columnId + "']").Replace("$", "").Replace(",", "");
                            Assert.AreEqual(true, filterValue.EndsWith(textname), "Filter Label Column Value not End With Expected.");
                        }
                    }
                }
                Results.WriteStatus(test, "Pass", "'" + filterValue + "' Filtered Value '" + filterType + "' With Grid Value for Column.");
            }
            else
            {
                Results.WriteStatus(test, "Pass", "No Records found to Verify Records.");
            }

            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Filtered In Range Value with Grid value for column
        /// </summary>
        /// <param name="columnId">Column id of the Header</param>
        /// <param name="lowerLimit">Lower limit of the Range section</param>
        /// <returns></returns>
        public PivotReportScreen verifyFilteredInRangeValueWithGridValueForColumn(string columnId, string lowerLimit)
        {
            Decimal uppwerLimit = Convert.ToDecimal(lowerLimit) + 50;
            if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div") == true)
            {
                IList<IWebElement> gridCollection = driver._findElements("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div");
                for (int i = 0; i < gridCollection.Count; i++)
                {
                    if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[@col-id='" + columnId + "']"))
                    {
                        decimal gridValue = Convert.ToDecimal(driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[@col-id='" + columnId + "']").Replace("$", "").Replace(",", ""));
                        decimal var1 = uppwerLimit;
                        decimal var2 = Convert.ToDecimal(lowerLimit);
                        Assert.LessOrEqual(gridValue, var1, "Less Value with Expected Filter not matched.");
                        Assert.GreaterOrEqual(gridValue, var2, "Greater Value with Expected Filter not matched.");
                    }
                }
                Results.WriteStatus(test, "Pass", "Verified, Filtered Value Between with Grid Value For Column.");
            }
            else
                Results.WriteStatus(test, "Pass", "No Records found to Verify Records.");

            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Select Record from Pivot Grid
        /// </summary>
        /// <param name="headerName">Column Header Name</param>
        /// <param name="selectRecords">How many records select from grid</param>
        /// <returns></returns>
        public PivotReportScreen selectRecordsFromPivotGrid(string headerName, int selectRecords = 1)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            string colId = "";
            for (int i = 0; i < pivotFieldHeaders.Count; i++)
                if (pivotFieldHeaders[i].Text.Contains(headerName))
                {
                    colId = pivotFieldHeaders[i].GetAttribute("col-id");
                    break;
                }

            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div"));
            int count = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']") == true)
                {
                    if (driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']", "class").Contains("tabular-cell-selected") == false)
                    {
                        driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']");
                        count++;
                        if (count == selectRecords)
                            break;
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, Records of '" + headerName + "' Column from Pivot Grid");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// unSelect Records from Pivot Grid
        /// </summary>
        /// <param name="headerName">Header name to verify</param>
        /// <param name="selectRecords">How many Records to verify</param>
        /// <returns></returns>
        public PivotReportScreen unSelectRecordsFromPivotGrid(string headerName, int selectRecords = 1)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-header']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            string colId = "";
            for (int i = 0; i < pivotFieldHeaders.Count; i++)
                if (pivotFieldHeaders[i].Text.Contains(headerName))
                {
                    colId = pivotFieldHeaders[i].GetAttribute("col-id");
                    break;
                }

            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div"));
            int count = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']") == true)
                {
                    if (driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']", "class").Contains("tabular-cell-selected") == true)
                    {
                        driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']");
                        count++;
                        Assert.AreEqual(false, driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']", "class").Contains("tabular-cell-selected"), "Column not unChecked.");
                        if (count == selectRecords)
                            break;
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, Records of '" + headerName + "' Column from Pivot Grid");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Select Each column record and verify removed previous option from pivot grid
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen selectEachColumnRecordAndVerifyRemovedPreviousOptionFromPivotGrid()
        {
            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div"));
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='1']"), "First Row not Present on Pivot Grid.");

            IList<IWebElement> columns = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='1']/div"));
            for (int j = 0; j < columns.Count; j++)
            {
                driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='1']/div[" + (j + 1) + "]");
                Assert.AreEqual(true, driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='1']/div[" + (j + 1) + "]", "class").Contains("tabular-cell-selected"), "(" + (j + 1) + ") Column on (1) Row not selected.");
                if (j != 0)
                    Assert.AreEqual(false, driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='1']/div[" + j + "]", "class").Contains("tabular-cell-selected"), "(" + j + ") Column on (1) Row selected not Removed.");
            }

            Results.WriteStatus(test, "Pass", "Selected, Each columns Record and Verified Previously Selected Record Removed on Pivot Grid.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Select Records from Pivot view Reports Grid
        /// </summary>
        /// <param name="headerName"></param>
        /// <param name="selectRecords"></param>
        /// <returns></returns>
        public PivotReportScreen selectRecordsFromPivotViewReportGrid(string headerName, int selectRecords = 1)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            string colId = "";
            for (int i = 0; i < pivotFieldHeaders.Count; i++)
                if (pivotFieldHeaders[i].Text.Contains(headerName))
                {
                    colId = pivotFieldHeaders[i].GetAttribute("col-id");
                    break;
                }

            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div"));
            int count = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']") == true)
                {
                    if (driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']", "class").Contains("tabular-cell-selected") == false)
                    {
                        driver._clickByJavaScriptExecutor("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']");
                        Assert.AreEqual(true, driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']", "class").Contains("tabular-cell-selected"), "(" + (i + 1) + ") Record not Selected.");
                        count++;
                        if (count == selectRecords)
                            break;
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, Records of '" + headerName + "' Column from Pivot Grid");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Column Selected Or not on Grid
        /// </summary>
        /// <param name="headerName">Header Name to Verify</param>
        /// <param name="selectRecords"></param>
        /// <returns></returns>
        public PivotReportScreen verifyColumnSelectedOrNotOnGrid(string headerName, bool columnSelected, int selectRecords = 1)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"), "Cell Headers not present.");
            IList<IWebElement> pivotFieldHeaders = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-header-container']/div[@class='ag-header-row']/div[contains(@class,'ag-header-cell ag-header-cell-sortable')]"));
            string colId = "";
            for (int i = 0; i < pivotFieldHeaders.Count; i++)
                if (pivotFieldHeaders[i].Text.Contains(headerName))
                {
                    colId = pivotFieldHeaders[i].GetAttribute("col-id");
                    break;
                }

            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div"));
            int count = 0;
            for (int i = 0; i < rows.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']") == true)
                {
                    if (columnSelected)
                        Assert.AreEqual(true, driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']", "class").Contains("tabular-cell-selected"), "(" + (i + 1) + ") Record not Selected.");
                    else
                        Assert.AreEqual(false, driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + colId + "']", "class").Contains("tabular-cell-selected"), "(" + (i + 1) + ") Record not Selected.");

                    count++;
                    if (count == selectRecords)
                        break;
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, Records of '" + headerName + "' Column from Pivot Grid");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Disable Pivot View Report Grid Records
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyDisablePivotViewReportGridRecords()
        {
            IList<IWebElement> rowsCollections = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div"));
            for (int i = 0; i < rowsCollections.Count; i++)
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']") == true)
                {
                    IList<IWebElement> columnCollections = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div"));
                    for (int j = 0; j < columnCollections.Count; j++)
                        if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[" + (j + 1) + "]") == true)
                        {
                            string columnValue = driver._getText("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[" + (j + 1) + "]");
                            if (columnValue == "$0.00" || columnValue == "$0" || columnValue == "NA")
                                Assert.AreEqual(false, driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']/div[" + (j + 1) + "]", "class").Contains("cell-selectable"), "'" + columnValue + "' Column selectable.");
                        }
                }

            Results.WriteStatus(test, "Pass", "Verified, Disable Pivot View Grid Records.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Scroll and Verify All Records from pivot grid
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen scrollAndVerifyAllRecordsFromPivotGrid()
        {
            IList<IWebElement> rowsCollections = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div"));
            for (int i = 0; i < 100; i++)
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']") == true)
                    driver._scrollintoViewElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[@row-index='" + i + "']");

            Results.WriteStatus(test, "Pass", "Scroll and Verified Records from pivot Grid.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Mouse hover on Record and verify color
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen mouseHoverOnPivotFieldGridRecordAndVerifyColor()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[1]"), "Rows not Present on Pivot Grid.");
            string rowIndex = driver._getAttributeValue("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[1]", "row-index");
            IList<IWebElement> columns = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + rowIndex + "']/div"));

            for (int i = 0; i < columns.Count; i++)
            {
                driver.MouseHoverUsingElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + rowIndex + "']/div[" + (i + 1) + "]");
                //Assert.AreEqual("rgba(214, 220, 224, 1)", driver.FindElement(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + rowIndex + "']/div[" + (i + 1) + "]")).GetCssValue("background-color"), "Grey Color not match on Grid Record after mouse hover.");
                Assert.AreEqual("rgba(224, 233, 234, 1)", driver.FindElement(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-pinned-left-cols-container']/div[@row-index='" + rowIndex + "']/div[" + (i + 1) + "]")).GetCssValue("background-color"), "Grey Color not match on Grid Record after mouse hover.");
            }

            Results.WriteStatus(test, "Pass", "Mouse hover on Records and Verified Grey Color on Grid Value.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Mouse hover on non selectable value from pivot grid
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen mouseHoverOnNonSelectableValueFromPivotGrid()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div"), "Records Row not Present on Pivot Grid.");
            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div"));

            for (int i = 0; i < rows.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]"))
                {
                    IList<IWebElement> columns = rows[i].FindElements(By.XPath(".//div"));
                    for (int j = 0; j < columns.Count; j++)
                    {
                        if (columns[j].Text == "$0" || columns[j].Text == "$0.00")
                        {
                            driver.MouseHoverUsingElement("xpath", "//div[@ag-grid='pivotCtrl.gridOptions']//.//div[@class='ag-body-viewport']/div[@class='ag-body-container']/div[" + (i + 1) + "]/div[" + (j + 1) + "]");
                            Assert.AreEqual(false, columns[j].GetAttribute("class").Contains("cell-selectable"), "Value is Zero but selectable.");
                        }
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Mouse hover on Non-Selectable Value from Pivot Grid.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Total Summary Section below side of pivot grid
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyTotalSummarySectionBelowSideOfPivotGrid(bool totalSummary)
        {
            Assert.AreEqual(totalSummary, driver._isElementPresent("xpath", "//div[@class='ag-pinned-left-floating-bottom']"), "Total Summary Section not Present below pivot grid.");
            Assert.AreEqual(totalSummary, driver._isElementPresent("xpath", "//div[@class='ag-floating-bottom-viewport']"), "Total Summary not present of Pivot grid Records.");
            Results.WriteStatus(test, "Pass", "Verified, Total Summary section below side of Pivot grid.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify File Downloaded or not on screen
        /// </summary>
        /// <param name="fileName">Filename to verify</param>
        /// <returns></returns>
        public PivotReportScreen verifyFileDownloadedOrNotOnScreen(string fileName)
        {
            bool Exist = false;
            string FilePath = "";
            string Path = ExtentManager.ResultsDir;
            string[] filePaths = Directory.GetFiles(Path, "*.xlsx");

            foreach (string filePath in filePaths)
            {
                FileInfo ThisFile = new FileInfo(filePath);
                if (filePath.Contains(fileName))
                {
                    ThisFile = new FileInfo(filePath);
                    FilePath = filePath;
                    if (ThisFile.LastWriteTime.ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                        ThisFile.LastWriteTime.AddMinutes(1).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                        ThisFile.LastWriteTime.AddMinutes(2).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                        ThisFile.LastWriteTime.AddMinutes(3).ToShortTimeString() == DateTime.Now.ToShortTimeString() ||
                        ThisFile.LastWriteTime.AddMinutes(4).ToShortTimeString() == DateTime.Now.ToShortTimeString())
                    {
                        Exist = true;
                        File.Delete(FilePath);
                        break;
                    }
                }
            }

            Assert.AreEqual(true, Exist, "'" + fileName + "' File Not Exported Properly.");
            Results.WriteStatus(test, "Pass", "Verified, <b>'*.xlsx'</b> File Exported Properly for '" + fileName + "' Report File.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Verify Grid section below pivot grid
        /// </summary>
        /// <returns></returns>
        public PivotReportScreen verifyGridSectionBelowPivotGrid()
        {
            if (driver._isElementPresent("xpath", "//div[@ng-if='view.groupId']//.//*[@id='borderLayout_eGridPanel']//.//div[@class='ag-header-container']/div[@class='ag-header-row']"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ng-if='view.groupId']//.//*[@id='borderLayout_eGridPanel']//.//div[@class='ag-header-container']/div[@class='ag-header-row']"), "Column Header not Present on Grid.");
                IList<IWebElement> gridRecords = driver.FindElements(By.XPath("//div[@ng-if='view.groupId']//.//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div"));
                for (int i = 0; i < gridRecords.Count; i++)
                    if (driver._isElementPresent("xpath", "//div[@ng-if='view.groupId']//.//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[" + (i + 1) + "]//.//span[@class='ag-selection-checkbox']"))
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ng-if='view.groupId']//.//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[" + (i + 1) + "]//.//span[@class='ag-selection-checkbox']"), "Checkbox not Present on Grid for [" + (i + 1) + "] Record number.");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']"), "Ad Thumbnail not Present for Table View Screen.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//img"), "Ad Image not Present on Section.");
                IWebElement image = driver._findElement("xpath", "//div[@class='aditem aditem-long']//.//img");
                bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
                Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Thumbnail Section.");

                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//div[@class='detail-view-content']"), "Detail View section not Present on Ad Image Section.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'View Ad')]"), "View Ad Icon not Present on Ad Image.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Markets')]"), "Markets Icon not Present on Ad Image.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Details')]"), "Details Icon not Present on Ad Image.");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ng-if='view.groupId']//.//div[@domain-item-data='domainItemGroupCtrl.domainItemData']"), "Column Header not Present on Grid.");
                IList<IWebElement> detailRecords = driver.FindElements(By.XPath("//div[@ng-if='view.groupId']//.//div[contains(@ng-repeat,'item in domainItemDetailsCtrl')]"));
                for (int i = 0; i < detailRecords.Count; i++)
                {
                    Assert.AreEqual(true, detailRecords[i].FindElement(By.XPath(".//div[@class='row checkbox checkbox-header']")), "Header with Checkbox not present.");
                    Assert.AreEqual(true, detailRecords[i].FindElement(By.XPath(".//button[contains(text(),'View Ad')]")), "'View Ad' Option not present for Record.");
                    Assert.AreEqual(true, detailRecords[i].FindElement(By.XPath(".//button[contains(text(),'Markets')]")), "'Markets' Option not present for Record.");
                    Assert.AreEqual(true, detailRecords[i].FindElement(By.XPath(".//button[contains(text(),'Details')]")), "'Details' Option not present for Record.");
                    Assert.AreEqual(true, detailRecords[i].FindElement(By.XPath(".//button[contains(text(),'Download')]")), "'Download' Option not present for Record.");
                }
            }

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']"), "Pagination Section not Present.");
            Results.WriteStatus(test, "Pass", "Verified, Grid Section for Table View.");
            return new PivotReportScreen(driver, test);
        }

        /// <summary>
        /// Click button from View and verify popup window
        /// </summary>
        /// <param name="buttonName">Button Name to Click verify tab to open</param>
        /// <returns></returns>
        public PivotReportScreen clickButtonFromViewAndVerifyPoupWindow(string buttonName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[contains(text(),'" + buttonName + "')]"), "'" + buttonName + "' Button with Icon not present.");
            driver._clickByJavaScriptExecutor("//button[contains(text(),'" + buttonName + "')]");
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button from View.");

            Assert.AreEqual(true, driver._waitForElement("xpath", "//div[@class='modal-content']"), "Product popup window not present.");
            driver._waitForElementToBeHidden("xpath", "//div[@class='loading-domainitem modal-tab-content']");
            IList<IWebElement> images = driver.FindElements(By.XPath("//div[@class='modal-content']//.//img"));
            for (int i = 0; i < images.Count; i++)
            {
                bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", images[i]));
                Assert.AreEqual(true, loaded, "'(" + images[i].GetAttribute("src") + ")' Image Not Load on Thumbnail Section.");
            }

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='pull-right modal-close' and text()='×']"), "Close(x) Icon not present on popup window.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//ul[@role='navigation']/li[@class='pull-right active']/a[text()='" + buttonName + "']"), "'" + buttonName + "' Tab not Default open.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Close')]"), "Close Button not present on popup window.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Export')]"), "Export Button not present on popup window.");

            Results.WriteStatus(test, "Pass", "Verified, Product popup window.");
            return new PivotReportScreen(driver, test);
        }

        #endregion
    }
}
