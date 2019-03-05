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
    public class SummaryByCategory
    {
        #region Private Variables

        private IWebDriver summaryByCategory;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public SummaryByCategory(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.summaryByCategory = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.summaryByCategory; }
            set { this.summaryByCategory = value; }
        }

        /// <summary>
        /// Verify Summary By Category screen
        /// </summary>
        /// <returns></returns>
        public SummaryByCategory verifySummaryByCategoryScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='main-view']/div"), "Annual Summary By Category screen not present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "filter-menu"), "'Filter Bar' not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul/li"), "Filter Fields not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul//.//cft-field-editor-timeframe-calendar"), "'Date Range' Filter not found or match.");
            IList<IWebElement> fieldsCollection = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li"));
            string[] filterLabels = { "Media", "Product", "Category" };
            string[] filterIDs = { "media", "AdvertiserProduct", "category" };
            for (int i = 0; i < filterLabels.Length; i++)
                Assert.AreEqual(true, fieldsCollection[i].GetAttribute("id").ToLower().Contains(filterIDs[i].ToLower()), "'" + filterLabels[i] + "' Filter not found or match.");

            Results.WriteStatus(test, "Pass", "Verified, Report Screen Details on Page.");
            return new SummaryByCategory(driver, test);
        }

        /// <summary>
        /// Verify Filter bar Section on screen
        /// </summary>
        /// <returns></returns>
        public SummaryByCategory verifyFilterBarSectionOnScreen()
        {
            if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and contains(@class,'prev-button')]"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@id='side-menu-button' and contains(@class,'prev-button disabled')]"), "'Previous' Arrow not Disable.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button']"), "'Next' Arrow not Enable.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Filter Bar Section on Screen.");
            return new SummaryByCategory(driver, test);
        }

        #region Filter Section

        /// <summary>
        /// Click on Date Filter field and select option
        /// </summary>
        /// <param name="optionName">Option Name to select</param>
        /// <returns></returns>
        public PromoDashboard clickOnDateFilterFieldAndSelectOption(string optionName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[contains(@id,'internal_timeframe')]/a"));
            driver._clickByJavaScriptExecutor("//*[contains(@id,'internal_timeframe')]/a");
            Thread.Sleep(500);

            IList<IWebElement> optionsCollections = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//ul/li[1]//.//ul[contains(@class,'insert-ranges')]/li"));
            for (int j = 0; j < optionsCollections.Count; j++)
            {
                if (optionsCollections[j].Text.Contains(optionName))
                {
                    optionsCollections[j].Click();
                    Thread.Sleep(500);
                    driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20);
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Clicked, 'Date Filter Field and Selected 'Custom Range' Option.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify From and To Date Selection Section on screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyFromAndToDateSelectionSectionOnScreen()
        {
            Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='calendar first left']")).Displayed, "From Month section not present.");
            Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='calendar second right']")).Displayed, "To Month section not present.");

            Assert.True(driver._isElementPresent("xpath", "//div[@class='calendar first left']//.//select[@class='form-control yearselect']"), "Year Dropdown on from section not present.");
            Assert.True(driver._isElementPresent("xpath", "//div[@class='calendar second right']//.//select[@class='form-control yearselect']"), "Year Dropdown on from section not present.");

            Results.WriteStatus(test, "Pass", "Verified, 'From' and 'To' Month section on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Filter section with checkbox and select option
        /// </summary>
        /// <param name="filterName">Filter name to select</param>
        /// <returns></returns>
        public String verifyFilterSectionWithCheckboxAndSelectOption(string filterName)
        {
            IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='ag-body-viewport']/div/div"));
            for (int i = 0; i < filterLists.Count; i++)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='ag-body-viewport']/div/div[" + (i + 1) + "]/div/span")));
                //IWebElement element = driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label/input"));
                //Assert.AreEqual(true, element.GetAttribute("type").Contains("checkbox"), "[" + i + "] Record Checkbox not Present on '" + filterName + "' Filter List section.");
            }

            string optionName = driver._getText("xpath", "//li[@class='dropdown open']//.//div[@class='ag-body-viewport']/div/div[1]/div/span");
            //filterLists[1].Click(); Thread.Sleep(2000);
            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='ag-body-viewport']/div/div[1]/div/span");
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading')]");
            driver._waitForElementToBeHidden("xpath", "//p[@class='lead' and contains(text(),'Loading')]");
            Results.WriteStatus(test, "Pass", "Verified Filter section with checkbox and Selected '" + optionName + "' Option from Option.");
            return optionName;
        }

        #endregion

        #region Pivot Grid

        /// <summary>
        /// Verify Report Screen Details
        /// </summary>
        /// <returns></returns>
        public SummaryByCategory verifyReportScreenDetails()
        {
            string[] menuIcons = { "User", "Files", "Help", "Search" };
            Home homePage = new Home(driver, test);
            homePage.verifyMenusIconButtonsOnTopOfScreen(menuIcons);
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");
            Assert.AreEqual(true, driver._isElementPresent("id", "filter-menu"), "'Filter Bar' not Present on Screen.");

            IList<IWebElement> buttons = driver.FindElements(By.XPath("//div[contains(@class,'btn-group btn-grid-actions')]//.//button"));
            string[] buttonNames = { "Export Grid", "Schedule", "View Selected", "Reset Selected", "Pivot Options" };
            string[] buttonStatus = { null, "true", "true", "true", null };
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
            Results.WriteStatus(test, "Pass", "Verified, Report Screen Details on Page.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customCompRankPoP')]"), "'Company Ranking Period over Period' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customCompRankYoY')]"), "'Company Ranking Year over Year' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customDivRankPoP')]"), "'Division Ranking Period over Period' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customDivRankYoY')]"), "'Division Ranking Year over Year' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customCompSOV')]"), "'Company Share of Spend' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customDivSOV')]"), "'Division Share of Spend' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customMediaMixPie')]"), "'Media Share Summary' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customLeadCompMediaMix')]"), "'Leading Company Share of Media' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-spend-customCompBarFull')]"), "'Company by Spend' Chart Section not present.");
            Results.WriteStatus(test, "Pass", "Verified, All Chart of Screen.");

            return new SummaryByCategory(driver, test);
        }

        #region Company Ranking Period over Period Chart

        /// <summary>
        /// Verify Company Ranking Period over Period Chart
        /// </summary>
        /// <returns></returns>
        public SummaryByCategory verifyCompanyRankingPeriodOverPeriodChart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompRankPoP-0']"), "'Company Ranking Period over Period' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompRankPoP-0']/div[@class='highcharts-container']"), "'Company Ranking Period over Period' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='chart-spend-customCompRankPoP-0']//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");
            Assert.AreEqual("true", driver.FindElement(By.XPath("//chart-export[@target='chart-spend-customCompRankPoP-0']//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompRankPoP-0']//.//*[name()='svg']/*[name()='text' and @id='TimePeriodChartTimeframe']"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompRankPoP-0']//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Company Ranking Period over Period' Chart on Screen.");
            return new SummaryByCategory(driver, test);
        }

        /// <summary>
        /// Click Icon Button on screen for chart
        /// </summary>
        /// <param name="chartName">Chart Name to verify</param>
        /// <param name="iconName">Icon Name to click</param>
        /// <returns></returns>
        public SummaryByCategory clickIconButtonOnScreenForChart(string chartName, string iconName)
        {
            string divName = "chart-spend-customCompRankPoP-0";

            switch (chartName)
            {
                case "Company Ranking Year over Year":
                    {
                        divName = "chart-spend-customCompRankYoY-1";
                        break;
                    }

                case "Division Ranking Period over Period":
                    {
                        divName = "chart-spend-customDivRankPoP-0";
                        break;
                    }

                case "Division Ranking Year over Year":
                    {
                        divName = "chart-spend-customDivRankYoY-1";
                        break;
                    }

                case "Company Share of Spend":
                    {
                        divName = "chart-spend-customCompSOV-0";
                        break;
                    }

                case "Division Share of Spend":
                    {
                        divName = "chart-spend-customDivSOV-1";
                        break;
                    }

                case "Media Share Summary":
                    {
                        divName = "chart-spend-customMediaMixPie-0";
                        break;
                    }

                case "Leading Company Share of Media":
                    {
                        divName = "chart-spend-customLeadCompMediaMix-1";
                        break;
                    }

                case "Company by Spend":
                    {
                        divName = "chart-spend-customCompBarFull-0";
                        break;
                    }
            }

            if (iconName.Equals("Schedule"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='" + divName + "']//.//div[@class='export-button']/cft-scheduled-export-popover//.//i"), "'" + iconName + "' Icon not present for Chart.");
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver._findElement("xpath", "//chart-export[@target='" + divName + "']//.//div[@class='export-button']/cft-scheduled-export-popover//.//i"));
                ((IJavaScriptExecutor)driver).ExecuteScript("javascript:window.scrollBy(0,-150)");
                driver._clickByJavaScriptExecutor("//chart-export[@target='" + divName + "']//.//div[@class='export-button']/cft-scheduled-export-popover//.//i");
                Thread.Sleep(5000);
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='" + divName + "']//.//div[@class='export-button']/button[@uib-tooltip='" + iconName + "']"), "'" + iconName + "' Icon not present for Chart.");
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver._findElement("xpath", "//chart-export[@target='" + divName + "']//.//div[@class='export-button']/button[@uib-tooltip='" + iconName + "']"));
                ((IJavaScriptExecutor)driver).ExecuteScript("javascript:window.scrollBy(0,-150)");
                driver._clickByJavaScriptExecutor("//chart-export[@target='" + divName + "']//.//div[@class='export-button']/button[@uib-tooltip='" + iconName + "']");
                Thread.Sleep(3000);
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='export-button open']"), "Export Popup window not open.");
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + iconName + "' Button Icon on screen for '" + chartName + "' Chart.");
            return new SummaryByCategory(driver, test);
        }

        /// <summary>
        /// Verify Download popup window and click on option
        /// </summary>
        /// <param name="optionName">Option Name for click</param>
        /// <returns></returns>
        public SummaryByCategory verifyDownloadPopupWindowAndClickOnOption(string optionName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='export-button open']/ul[contains(@class,'dropdown-menu-form')]/li"), "Download Options not present.");
            IList<IWebElement> optionsList = driver.FindElements(By.XPath("//div[@class='export-button open']/ul[contains(@class,'dropdown-menu-form')]/li"));
            for (int i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i].Text.Contains(optionName))
                {
                    optionsList[i].Click();
                    Thread.Sleep(8000);
                    driver._waitForElementToBeHidden("xpath", "//button/i[contains(@class,'fa-spinner-add-active')]");
                    break;
                }
            }

            driver._waitForElementToBeHidden("xpath", "//button/i[contains(@class,'fa-spinner-add-active')]");
            Results.WriteStatus(test, "Pass", "Verified, Download popup window and Clicked '" + optionName + "' Option from List.");
            return new SummaryByCategory(driver, test);
        }

        /// <summary>
        /// verify file downloaded or not on screen
        /// </summary>
        /// <param name="fileName">file name to verify</param>
        /// <param name="FileType">file type</param>
        /// <returns></returns>
        public SummaryByCategory verifyFileDownloadedOrNotOnScreen(string fileName, string FileType)
        {
            bool Exist = false;
            string FilePath = "";
            string Path = ExtentManager.ResultsDir;
            string[] filePaths = Directory.GetFiles(Path, FileType);

            foreach (string filePath in filePaths)
            {
                FileInfo ThisFile = new FileInfo(filePath);
                if (filePath.Contains(fileName + "-" + DateTime.Today.ToString("yyyyMMdd")))
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

            Assert.AreEqual(true, Exist, "'" + fileName + "' " + FileType.Replace("*", "").ToUpper() + "' File Not Exported Properly.");
            Results.WriteStatus(test, "Pass", "Verified, <b>'" + FileType.Replace("*", "").ToUpper() + "'</b> File Exported Properly for '" + fileName + "' Report File.");
            return new SummaryByCategory(driver, test);
        }

        /// <summary>
        /// Verify Legend to click and verify for chart
        /// </summary>
        /// <param name="chartName">Chart Name for Legend</param>
        /// <returns></returns>
        public SummaryByCategory verifyLegendToClickAndVerifyLegendColor(string chartName)
        {
            string divName = "chart-spend-customCompRankPoP-0";

            switch (chartName)
            {
                case "Company Ranking Year over Year":
                    {
                        divName = "chart-spend-customCompRankYoY-1";
                        break;
                    }

                case "Division Ranking Period over Period":
                    {
                        divName = "chart-spend-customDivRankPoP-0";
                        break;
                    }

                case "Division Ranking Year over Year":
                    {
                        divName = "chart-spend-customDivRankYoY-1";
                        break;
                    }

                case "Company Share of Spend":
                    {
                        divName = "chart-spend-customCompSOV-0";
                        break;
                    }

                case "Division Share of Spend":
                    {
                        divName = "chart-spend-customDivSOV-1";
                        break;
                    }

                case "Media Share Summary":
                    {
                        divName = "chart-spend-customMediaMixPie-0";
                        break;
                    }

                case "Leading Company Share of Media":
                    {
                        divName = "chart-spend-customLeadCompMediaMix-1";
                        break;
                    }

                case "Company by Spend":
                    {
                        divName = "chart-spend-customCompBarFull-0";
                        break;
                    }
            }

            IList<IWebElement> legendLists = driver.FindElements(By.XPath("//div[@id='" + divName + "']/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-legend')]//.//*[name()='g' and contains(@class,'highcharts-legend-item')]/*[name()='text']"));
            Random rand = new Random();
            int x = rand.Next(0, legendLists.Count);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver._findElement("xpath", "//div[@id='" + divName + "']/div/*[name()='svg']//.//*[name()='text' and contains(@class,'highcharts-title')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("javascript:window.scrollBy(0,-150)");
            Thread.Sleep(2000); legendLists[x].Click(); Thread.Sleep(3000);
            Assert.AreEqual(true, legendLists[x].GetCssValue("color").Contains("rgba(147, 162, 173, 1)"), "Grey Color not match for disable legend.");
            Results.WriteStatus(test, "Pass", "Verified, Legend to Click and Verified Grey Color for Legend.");
            return new SummaryByCategory(driver, test);
        }

        #endregion

        #region Company Share of Spend Chart

        /// <summary>
        /// Verify Company Share of Spend Chart
        /// </summary>
        /// <returns></returns>
        public SummaryByCategory verifyCompanyShareOfSpendChart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompSOV-0']"), "'Company Ranking Period over Period' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompSOV-0']/div[@class='highcharts-container']"), "'Company Ranking Period over Period' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='chart-spend-customCompSOV-0']//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");
            Assert.AreEqual("true", driver.FindElement(By.XPath("//chart-export[@target='chart-spend-customCompSOV-0']//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompSOV-0']//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Company Share of Spend' Chart on Screen.");
            return new SummaryByCategory(driver, test);
        }

        #endregion

        #region Leading Company Share of Media Chart

        /// <summary>
        /// Verify Leading Company Share of Media Chart
        /// </summary>
        /// <returns></returns>
        public SummaryByCategory verifyLeadingCompanyShareOfMediaChart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customLeadCompMediaMix-1']"), "'Leading Company Share of Media' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customLeadCompMediaMix-1']/div[@class='highcharts-container']"), "'Leading Company Share of Media' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='chart-spend-customLeadCompMediaMix-1']//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");
            Assert.AreEqual("true", driver.FindElement(By.XPath("//chart-export[@target='chart-spend-customLeadCompMediaMix-1']//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customLeadCompMediaMix-1']//.//*[name()='svg']/*[name()='text' and @id='SubmediaChartTimeframe']"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customLeadCompMediaMix-1']//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Leading Company Share of Media' Chart on Screen.");
            return new SummaryByCategory(driver, test);
        }

        #endregion

        #region Company by Spend Chart

        /// <summary>
        /// Verify Company by Spend Chart
        /// </summary>
        /// <returns></returns>
        public SummaryByCategory verifyCompanyBySpendChart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompBarFull-0']"), "'Company by Spend' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompBarFull-0']/div[@class='highcharts-container']"), "'Company by Spend' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='chart-spend-customCompBarFull-0']//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");
            Assert.AreEqual("true", driver.FindElement(By.XPath("//chart-export[@target='chart-spend-customCompBarFull-0']//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompBarFull-0']//.//*[name()='svg']/*[name()='text' and @id='UnknownChartTimeframe']"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-customCompBarFull-0']//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Company by Spend' Chart on Screen.");
            return new SummaryByCategory(driver, test);
        }

        #endregion

        #endregion

        #endregion
    }
}
