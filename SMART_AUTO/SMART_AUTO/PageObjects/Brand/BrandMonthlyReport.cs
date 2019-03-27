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
    public class BrandMonthlyReport
    {
        #region Private Variables

        private IWebDriver brandMonthlyReport;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public BrandMonthlyReport(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.brandMonthlyReport = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.brandMonthlyReport; }
            set { this.brandMonthlyReport = value; }
        }

        /// <summary>
        /// Verify Brand Monthly Report screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyBrandMonthlyReportScreen(bool resetField = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='main-view']/div"), "Brand Monthly Report screen not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='summary-panel-wrapper']"), "Summary Panel not present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='chart-rows-wrapper fade-in']"), "Chart section not present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='domain-carousel']"), "Carousel section not present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='carousel-filters']"), "Carousel filter section not present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-actions-wrapper']"), "Grid not present on screen.");
            if (resetField)
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li[@class='disabled']"), "'Reset All' Button Default not Disable.");
            Results.WriteStatus(test, "Pass", "Verified, Report Screen Details on Page.");
            return new BrandMonthlyReport(driver, test);
        }

        #region Filter Section

        /// <summary>
        /// Verify filter bar section on screen
        /// </summary>
        /// <param name="fields">to verify Fields Options</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyFilterBarSectionOnScreen(bool fields = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("id", "filter-menu"), "'Filter Bar' not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul/li"), "Filter Fields not Present.");

            IList<IWebElement> fieldsCollection = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li"));
            string[] filterLabels = { "Last Month", "All Media Types", "All Advertiser Products", "All Categories", "All Markets" };
            string[] filterIDs = { "timeframe", "mediaName", "advertiserProduct", "category", "dmaName" };
            if (fields)
            {
                for (int i = 0; i < filterLabels.Length; i++)
                    Assert.AreEqual(true, fieldsCollection[i].GetAttribute("id").ToLower().Contains(filterIDs[i].ToLower()), "'" + filterLabels[i] + "' not found or match.");
            }

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li[@class='disabled']"), "'Reset All' Button Default not Disable.");
            Results.WriteStatus(test, "Pass", "Verified, Filter Bar Section Category Summary on Screen.");

            PromoDashboard promoDashboard = new PromoDashboard(driver, test);
            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]"))
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 3 Months");

            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify From and To Month Section on screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyFromAndToMonthSectionOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_month"), "'Month' tab button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_quarter"), "'Quarter' tab button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_year"), "'Year' tab button not Present.");

            Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='calendar first left']")).Displayed, "From Month section not present.");
            Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='calendar second right']")).Displayed, "To Month section not present.");

            Assert.True(driver._isElementPresent("xpath", "//div[@class='calendar first left']//.//select[@class='form-control yearselect']"), "Year Dropdown on from section not present.");
            Assert.True(driver._isElementPresent("xpath", "//div[@class='calendar second right']//.//select[@class='form-control yearselect']"), "Year Dropdown on from section not present.");

            Results.WriteStatus(test, "Pass", "Verified, 'From' and 'To' Month section on Screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Filter section with checkbox and select option
        /// </summary>
        /// <param name="filterName">Filter Name to click</param>
        /// <returns></returns>
        public String verifyFilterSectionWithCheckboxAndSelectOption(string filterName)
        {
            IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li"));
            for (int i = 0; i < filterLists.Count; i++)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label")));
                IWebElement element = driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label/input"));
                if (element.GetAttribute("class").Contains("not-empty"))
                    filterLists[i].Click();
                Assert.AreEqual(true, element.GetAttribute("type").Contains("checkbox"), "[" + i + "] Record Checkbox not Present on '" + filterName + "' Filter List section.");
            }

            string optionName = filterLists[1].Text;
            filterLists[1].Click(); Thread.Sleep(2000);
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading')]");
            driver._waitForElementToBeHidden("xpath", "//p[@class='lead' and contains(text(),'Loading')]");
            Results.WriteStatus(test, "Pass", "Verified Filter section with checkbox and Selected '" + filterLists[1].Text + "' Option from Option.");
            return optionName;
        }

        /// <summary>
        /// Verify Selected Records on Carousel section
        /// </summary>
        /// <param name="mediaType">Media Type to verify</param>
        /// <returns></returns>
        public BrandMonthlyReport verifySelectedRecordsOnCarouselSection(string mediaType)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='carousel-wrapper domain-item-carousel']"));
            driver._scrollintoViewElement("xpath", "//div[@class='carousel-wrapper domain-item-carousel']");

            for (int i = 0; i < 3; i++)
            {
                IList<IWebElement> carouselItems = driver.FindElements(By.XPath("//div[@class='carousel-wrapper domain-item-carousel']//.//div[contains(@class,'active')]/div[@ng-repeat='domainItem in itemSet']"));
                for (int j = 0; j < carouselItems.Count; j++)
                {
                    IWebElement mediaName = carouselItems[j].FindElement(By.XPath(".//div[@class='row aditem-header-row']/div[2]"));
                    Assert.AreEqual(mediaType, mediaName.Text, "'" + mediaName.Text + "' Media Type not present on Carousel.");
                }

                if (driver._isElementPresent("xpath", "//div[@class='carousel-wrapper domain-item-carousel']//.//a[@class='right carousel-control']") == true)
                    driver._clickByJavaScriptExecutor("//div[@class='carousel-wrapper domain-item-carousel']//.//a[@class='right carousel-control']");
            }

            Results.WriteStatus(test, "Pass", "Verified, Selected Record on Carousel section.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Clear Keyword from search textbox
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport clearKeywordFromSearchTextBox()
        {
            if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//button[contains(@class,'CFT-textbox-inner-button')]"))
            {
                driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//button[contains(@class,'CFT-textbox-inner-button')]");
                Results.WriteStatus(test, "Pass", "Cleared, Keyword from Search Textbox.");
            }

            return new BrandMonthlyReport(driver, test);
        }

        #endregion

        #region Advertiser Rankings Period over Period

        /// <summary>
        /// Verify Advertiser Rankings Period over Period Section on screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-advertiserActivity-0-0']"), "'Advertiser Rankings Period over Period' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-advertiserActivity-0-0']/div[@class='highcharts-container']"), "'Advertiser Rankings Period over Period' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='chart-spend-advertiserActivity-0-0']//.//div[@class='export-button']/button[@uib-tooltip='Expand']"), "Expand Icon not present for Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='chart-spend-advertiserActivity-0-0']//.//div[@class='export-button']/button[@uib-tooltip='Tabular']"), "Tabular Icon not present for Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='chart-spend-advertiserActivity-0-0']//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");

            Assert.AreEqual("true", driver.FindElement(By.XPath("//chart-export[@target='chart-spend-advertiserActivity-0-0']//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-advertiserActivity-0-0']//.//*[name()='svg']/*[name()='text' and @id='advertiserActivityChartTimeframe']"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='chart-spend-advertiserActivity-0-0']//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Advertiser Rankings Period over Period' Section on Screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click Icon Button on screen for chart
        /// </summary>
        /// <param name="chartName">Chart Name to verify</param>
        /// <param name="iconName">Icon Name to click</param>
        /// <returns></returns>
        public BrandMonthlyReport clickIconButtonOnScreenForChart(string chartName, string iconName)
        {
            string divName = "chart-spend-advertiserActivity-0-0";
            if (chartName.Equals("Advertiser Rankings Period over Period"))
                divName = "chart-spend-advertiserActivity-0-0";
            if (chartName.Equals("Advertiser Rankings Period over Period Full Screen"))
                divName = "selected-chart";

            if (iconName.Equals("Schedule"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='" + divName + "']//.//div[@class='export-button']/cft-scheduled-export-popover//.//i"), "'" + iconName + "' Icon not present for Chart.");
                driver._clickByJavaScriptExecutor("//chart-export[@target='" + divName + "']//.//div[@class='export-button']/cft-scheduled-export-popover//.//i");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='" + divName + "']//.//div[@class='export-button']/button[@uib-tooltip='" + iconName + "']"), "'" + iconName + "' Icon not present for Chart.");
                driver._clickByJavaScriptExecutor("//chart-export[@target='" + divName + "']//.//div[@class='export-button']/button[@uib-tooltip='" + iconName + "']");
            }

            Thread.Sleep(3000);
            Results.WriteStatus(test, "Pass", "Clicked, '" + iconName + "' Button Icon on screen for '" + chartName + "' Chart.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Full screen of Advertiser Rankings Period over Period Section on screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyFullScreenOfAdvertiserRankingsPeriodOverPeriodChart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']"), "'Advertiser Rankings Period over Period' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']/div[@class='highcharts-container']"), "'Advertiser Rankings Period over Period' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='selected-chart']//.//div[@class='export-button']/button[@uib-tooltip='Go Back']"), "'Go Back' Icon not present for Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='selected-chart']//.//div[@class='export-button']/button[@uib-tooltip='Tabular']"), "Tabular Icon not present for Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='selected-chart']//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");

            Assert.AreEqual("true", driver.FindElement(By.XPath("//chart-export[@target='selected-chart']//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']//.//*[name()='svg']/*[name()='text' and @id='advertiserActivityChartTimeframe']"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Advertiser Rankings Period over Period' Chart on Full screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Tabular View Section on screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyTabularViewSectionOnScreen()
        {
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Tabular Data')]");
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading details')]");
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Preview')]");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='main-view']//.//div[contains(@class,'tabular-details-wrapper')]"), "Tabular View not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@config='tabularCtrl.tabularView']"), "Tabular View Data not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@config='tabularCtrl.domainView']"), "Domain View Data not present.");

            IList<IWebElement> GridButtons = driver.FindElements(By.XPath("//div[@config='tabularCtrl.tabularView']//.//div[@class='btn-group btn-grid-actions']/button"));
            string[] Buttons = { "Download Grid", "View Selected", "Reset Selected" };
            int cnt = 0;
            for (int i = 0; i < GridButtons.Count; i++)
                for (int j = 0; j < Buttons.Length; j++)
                    if (GridButtons[i].Text.Contains(Buttons[j]))
                        cnt++;

            Assert.AreEqual(Buttons.Length, cnt, "Grid Buttons collections not match.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[contains(@ng-click,'tabularPivotCtrl.toggleToolPanel')]"), "Tabular Options Button not found.");

            IList<IWebElement> actionCollection = driver.FindElements(By.XPath("//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions']/button"));
            string[] buttonNames = { "Export All", "View Selected", "Reset Selected", "Field Options" };
            for (int i = 0; i < actionCollection.Count; i++)
                Assert.IsTrue(actionCollection[i].Text.Contains(buttonNames[i]), "'" + buttonNames[i] + "' Button not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Tabular View Section on screen.");
            return new BrandMonthlyReport(driver, test);
        }

        #endregion

        /// <summary>
        /// Click Button from tabular view screen
        /// </summary>
        /// <param name="buttonName">Button Name to click</param>
        /// <returns></returns>
        public BrandMonthlyReport clickButtonFromTabularViewScreen(string buttonName)
        {
            IList<IWebElement> GridButtons = driver.FindElements(By.XPath("//div[@config='tabularCtrl.tabularView']//.//div[@class='btn-group btn-grid-actions']/button"));
            if (buttonName.Equals(""))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[contains(@ng-click,'tabularPivotCtrl.toggleToolPanel')]"), "Tabular Options Button not found.");
                driver._clickByJavaScriptExecutor("//button[contains(@ng-click,'tabularPivotCtrl.toggleToolPanel')]");
            }
            else
                for (int i = 0; i < GridButtons.Count; i++)
                {
                    if (GridButtons[i].Text.Contains(buttonName) == true)
                    {
                        GridButtons[i].Click();
                        break;
                    }
                }

            Thread.Sleep(500);
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on from Tabular View Screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Exporting Grid Process to complete
        /// </summary>
        /// <returns></returns>1
        public BrandMonthlyReport verifyExportingGridProcessToComplete()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@ng-show='tabularPivotCtrl.isExporting']"), "Exporting Processing Button not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@ng-show='tabularPivotCtrl.isExporting']/i[@class='fa fa-spinner fa-spin']"), "Spin Icon for Exporting not present.");
            driver._waitForElementToBeHidden("xpath", "//button[@ng-show='tabularPivotCtrl.isExporting']/i[@class='fa fa-spinner fa-spin']");
            Thread.Sleep(4000);
            Results.WriteStatus(test, "Pass", "Verified, Exporting Grid Process to complete.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify File downloaded or not on screen
        /// </summary>
        /// <param name="fileName">Filename to verify</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyExcelFileDownloadedOrNotOnScreen(string fileName)
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
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Schedule window on screen
        /// </summary>
        /// <param name="searchName">Search Name to verify</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyScheduleWindow(string searchName)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='popover-content popover-body']", 20), "Schedule Window not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@class='form-control' and @placeholder='" + searchName + "']"), "'" + searchName + "' Search Name Default not display.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='fa fa-check text-success form-control-feedback']"), "'" + searchName + "' Search Name Feedback in Green Right color not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default dropdown-toggle']"), "Schedule Dropdown not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row']/span[contains(text(),'" + searchName + "')]"), "'" + searchName + " will be delivered every day.' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Create Scheduled Export')]"), "'Create Scheduled Export' Button not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Schedule Window on screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Download popup window and click on option
        /// </summary>
        /// <param name="optionName">OptionName for click</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyDownloadPopupWindowAndClickOnOption(string optionName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='export-button open']/ul[contains(@class,'dropdown-menu-form')]/li"), "Download Options not present.");
            IList<IWebElement> optionsList = driver.FindElements(By.XPath("//div[@class='export-button open']/ul[contains(@class,'dropdown-menu-form')]/li"));
            for (int i = 0; i < optionsList.Count; i++)
            {
                if (optionsList[i].Text.Contains(optionName))
                {
                    optionsList[i].Click();
                    driver._waitForElementToBeHidden("xpath", "//button/i[contains(@class,'fa-spinner-add')]");
                    Thread.Sleep(2000);
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Download popup window and Clicked '" + optionName + "' Option from List.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify File Downloaded Or Not for Ad Sharing And Exclusivity Screen
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="FileType"></param>
        /// <returns></returns>
        public BrandMonthlyReport verifyFileDownloadedOrNotOnScreen(string fileName, string FileType)
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
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Legend to click and verify for chart
        /// </summary>
        /// <param name="chartName">Chart Name for Legend</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyLegendToClickAndVerifyForChart(string chartName)
        {
            string divName = "chart-spend-advertiserActivity-0-0";
            if (chartName.Equals("Advertiser Rankings Period over Period"))
                divName = "chart-spend-advertiserActivity-0-0";
            if (chartName.Equals("Share of Spend"))
                divName = "selected-chart";

            IList<IWebElement> legendLists = driver.FindElements(By.XPath("//div[@id='" + divName + "']/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-legend')]//.//*[name()='g' and contains(@class,'highcharts-legend-item')]/*[name()='text']"));
            Random rand = new Random();
            int x = rand.Next(0, legendLists.Count);
            legendLists[x].Click(); Thread.Sleep(2000);
            Assert.AreEqual(true, legendLists[x].GetCssValue("color").Contains("rgba(147, 162, 173, 1)"), "Grey Color not match for disable legend.");
            Results.WriteStatus(test, "Pass", "Verified, Legend to Click and Verified Grey Color for Legend.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Product Thumbnail in product carousel
        /// </summary>
        /// <param name="thumbnail">Thumbnail view verify</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyProductThumbnailInProductCarousel(bool thumbnail = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/a[@class='left carousel-control']"), "Previous Page Arrow not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "domain-carousel"), "Product Thumbnails not Present.");
            if (driver._isElementPresent("xpath", "//*[@id='domain-carousel']/ol[@class='carousel-indicators']"))
            {
                IList<IWebElement> productCounts = driver.FindElements(By.XPath("//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div"));
                Assert.AreEqual(productCounts.Count, 6, "6 Product Thumbnails not available on Carousel");

                if (thumbnail)
                    for (int i = 0; i < productCounts.Count; i++)
                    {
                        IWebElement image = driver._findElement("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//img");
                        bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
                        Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Carousel Section.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row aditem-header-row']/div"), "Retailer Name not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row aditem-header-row']/div[2]"), "Media Type not Present for [" + (i + 1) + "] Record.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[2]/div"), "First Run Label not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[2]/div[2]"), "First Run value not Present for [" + (i + 1) + "] Record.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[3]/div"), "Last Run Label not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[3]/div[2]"), "Last Run value not Present for [" + (i + 1) + "] Record.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']//.//button[text()='View Ad']"), "'View Ad' Button not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']//.//button[text()='Markets']"), "'Markets' Button not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']//.//button[text()='Details']"), "'Details' Button not Present for [" + (i + 1) + "] Record.");
                    }
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/a[@class='right carousel-control']"), "Next Page Arrow not Present.");
            }

            Results.WriteStatus(test, "Pass", "Verify Product Thumbnail for Product Carousel.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click Button lonk for product on carousel section
        /// </summary>
        /// <param name="linkName">Link Name for Clicking</param>
        /// <returns></returns>
        public BrandMonthlyReport clickButtonLinkForProductOnCarouselSection(string linkName)
        {
            IList<IWebElement> productCounts = driver.FindElements(By.XPath("//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div"));
            Random rand = new Random();
            int x = rand.Next(0, productCounts.Count);
            if (linkName == "Ad Image")
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (x + 1) + "]//.//img"), "'" + linkName + "' Button not Present for [" + (x + 1) + "] Record.");
                driver._clickByJavaScriptExecutor("//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (x + 1) + "]//.//img");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (x + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']/div/button[text()='" + linkName + "']"), "'" + linkName + "' Button not Present for [" + (x + 1) + "] Record.");
                driver._clickByJavaScriptExecutor("//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (x + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']/div/button[text()='" + linkName + "']");
            }
            Thread.Sleep(1000);
            Results.WriteStatus(test, "Pass", "Clicked, '" + linkName + "' Button link for Product on Carousel section.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Product Detail Popup Window on Dashboard Page
        /// </summary>
        /// <param name="tabsName">TabNames to Veriy on Window</param>
        /// <param name="defaultView">Default Selected Tab Name</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyProductDetailPopupWindowOnDashboardPage(string[] tabNames, string defaultView, bool enable = true)
        {
            if (enable)
            {
                driver._waitForElementToBeHidden("xpath", "//i[@class='fa fa-fw fa-spinner fa-spin']");
                Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='modal-content']", 20), "Product Details Popup Window not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='panel-heading modal-header-inner']/span"), "Header not prensent for popup window.");

                IList<IWebElement> tabCollections = driver.FindElements(By.XPath("//div[@class='panel-heading modal-header-inner']/ul/li"));
                int cnt = 0; bool avail = false;

                for (int i = 0; i < tabNames.Length; i++)
                    for (int j = 0; j < tabCollections.Count; j++)
                    {
                        if (tabCollections[j].Text == defaultView)
                        {
                            Assert.AreEqual("pull-right active", tabCollections[j].GetAttribute("class"), "'" + defaultView + "' Tab Defult not Selected."); avail = true;
                        }

                        if (tabNames[i] == tabCollections[j].Text)
                        {
                            cnt++; break;
                        }
                    }

                Assert.AreEqual(true, avail, "'" + defaultView + "' Tab Default not Selected.");
                Assert.AreEqual(tabNames.Length, cnt, "Tab Collections not match on Popup Window.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='panel-heading modal-header-inner']/button[@class='pull-right modal-close' and text()='×']"), "Close Icon not Present on Popup Window.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Close')]"), "'Close' Button not Present on Popup Window.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-primary']"), "'Export' Button not Present on Popup Window.");

                Results.WriteStatus(test, "Pass", "Verified, Product Detail popup Window on Dashboard Page.");
            }
            else
            {
                Assert.AreEqual(false, driver._isElementPresent("xpath", "//div[@class='modal-content']"), "Product Details Popup Window Present.");
                Results.WriteStatus(test, "Pass", "Verified, Product Detail popup Window close on Dashboard Page.");
            }

            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify View Ad Screen on Popup Window
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyViewAdScreenOnPopupWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='row aditem-image-row']", 20), "Pages of Ad block not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='creative-thumbnail-container']/div/div/div"), "List of Ad Block Pages not found.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='creative-thumbnail-container']/div/div/div//.//div[contains(@class,'active')]"), "Current Selected Page not Display on Screen.");

            IWebElement image = driver._findElement("xpath", "//*[@id='creative-thumbnail-container']/div/div/div//.//div[contains(@class,'active')]/img");
            bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
            Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Popup Window.");

            IList<IWebElement> totalPages = driver.FindElements(By.XPath("//*[@id='creative-thumbnail-container']/div/div/div"));
            for (int i = 0; i < totalPages.Count; i++)
            {
                IWebElement pages = driver._findElement("xpath", "//*[@id='creative-thumbnail-container']/div/div/div[" + (i + 1) + "]//.//img");
                bool pageLoaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", pages));
                Assert.AreEqual(true, pageLoaded, "'(" + image.GetAttribute("src") + ")' Page Image Not Load on Popup Window.");
            }

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Close')]"), "'Close' Button not present on window.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Export')]"), "'Export' Button not present on window.");

            Results.WriteStatus(test, "Pass", "Verified, View Ad Screen on Popup window.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click button on popup window
        /// </summary>
        /// <param name="buttonName">Button name to click</param>
        /// <returns></returns>
        public BrandMonthlyReport clickButtonOnPopupWindow(string buttonName)
        {
            switch (buttonName)
            {
                case "Close":
                    {
                        Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Close')]"), "'Close' Button not Present on Popup Window.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Close')]");
                        break;
                    }

                case "Export":
                    {
                        Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Export')]"), "'Export' Button not Present on Popup Window.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-primary' and contains(text(),'Export')]");
                        break;
                    }

                case "Download Grid":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='btn-group']//.//i[@class='fa fa-cloud-download']"), "'Download Grid' Button not Present on Window.");
                        driver._clickByJavaScriptExecutor("//div[@class='btn-group']//.//i[@class='fa fa-cloud-download']");
                        break;
                    }

                case "Grid Options":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='btn-group']//.//i[@class='fa fa-gear']"), "'Grid Options' Button not Present on Window.");
                        driver._clickByJavaScriptExecutor("//div[@class='btn-group']//.//i[@class='fa fa-gear']");
                        break;
                    }
            }

            Thread.Sleep(500);
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on Popup Window.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Markets Tab on Popup Window
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyMarketsTabOnPopupWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='modal-content']//.//div[@id='borderLayout_eRootPanel']//.//div[@class='ag-body-container']/div", 20), "Grid Proper not Loading.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='modal-content']//.//div[@id='borderLayout_eRootPanel']//.//div[@class='ag-header-container']"), "Grid Header not present on window.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='btn-group']//.//i[@class='fa fa-cloud-download']"), "'Download Grid' Button not Present on Window.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='btn-group']//.//i[@class='fa fa-gear']"), "'Grid Options' Button not Present on Window.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Close')]"), "'Close' Button not present on window.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Export')]"), "'Export' Button not present on window.");

            Results.WriteStatus(test, "Pass", "Verified, Stores Screen on popup window.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click on Grid header to verify sorting functionality
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport clickOnGridHeaderToVerifySortingFunctionality()
        {
            IList<IWebElement> headerCollections = driver.FindElements(By.XPath("//div[@class='modal-content']//.//div[@id='borderLayout_eRootPanel']//.//div[@class='ag-header-container']/div/div"));
            Random rand = new Random();
            int x = rand.Next(0, headerCollections.Count);
            string colID = headerCollections[x].GetAttribute("col-id");
            string headerName = headerCollections[x].Text;
            headerCollections[x].Click();

            Assert.AreEqual(true, headerCollections[x].FindElement(By.XPath(".//div[contains(@class,'ag-header-cell-sorted-desc')]")).Displayed, "'" + headerName + "' Column Header not sorted in Desc properly.");
            Assert.AreEqual(true, headerCollections[x].FindElement(By.XPath(".//span[@ref='eSortDesc' and @class='ag-header-icon ag-sort-descending-icon']")).Displayed, "'" + headerName + "' Column Header not sorted in Desc properly.");
            Results.WriteStatus(test, "Pass", "Clicked, '" + headerName + "' Column header and verified, Row sorted in Descending order.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify More Details Screen on Popup Window
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyMoreDetailsScreenOnPopupWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='aditem aditem-thumbnail-view aditem-modal']", 20), "Image Section not Present on Details Section.");
            IWebElement image = driver._findElement("xpath", "//div[@class='aditem aditem-thumbnail-view aditem-modal']//.//img");
            bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
            Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Details Section.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='aditem aditem-detail-view aditem-detail-view-modal']/div[@class=' detail-view-content']"), "Detail View Content not Present.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='panel-heading modal-header-inner']/button[@class='pull-right modal-close' and text()='×']"), "Close Icon not Present on Popup Window.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Close')]"), "'Close' Button not Present on Popup Window.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Export')]"), "'Export' Button not Present on Popup Window.");

            Results.WriteStatus(test, "Pass", "Verified, More Details Screen on popup window.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Navigation Arrow for Carousel
        /// </summary>
        /// <param name="pageArrow">Page Arrow to Verify</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyNavigationArrowForCarousel(string pageArrow, bool clickAndVerify)
        {
            string ArrowLocation = "left";
            if (pageArrow.Equals("Next"))
                ArrowLocation = "right";
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/a[@class='" + ArrowLocation + " carousel-control']"), "" + pageArrow + " Page Arrow not Present.");
            driver.MouseHoverUsingElement("xpath", "//*[@id='domain-carousel']/a[@class='" + ArrowLocation + " carousel-control']/span");
            Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//*[@id='domain-carousel']/a[@class='" + ArrowLocation + " carousel-control']/span")).GetCssValue("color"), "'" + pageArrow + "' Arrow not Highlighted with Blue color.");
            Results.WriteStatus(test, "Pass", "Verified, '" + ArrowLocation + "' Navigation Arrow for Carousel.");

            if (clickAndVerify)
            {
                IList<IWebElement> indicators = driver.FindElements(By.XPath("//*[@id='domain-carousel']/ol[@class='carousel-indicators']/li"));
                for (int i = 0; i < indicators.Count; i++)
                {
                    if (indicators[i].GetAttribute("class").Contains("active"))
                    {
                        driver._clickByJavaScriptExecutor("//*[@id='domain-carousel']/a[@class='" + ArrowLocation + " carousel-control']/span");
                        Thread.Sleep(500);
                        Assert.AreEqual("active", indicators[i + 1].GetAttribute("class"), "Next Carousel Product not display.");
                        break;
                    }
                }
                Results.WriteStatus(test, "Pass", "Clicked, Next Arrow and Verified next carousel product on screen.");
            }

            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click Carousel radio option and verify product
        /// </summary>
        /// <param name="radioOption">Radio option</param>
        /// <returns></returns>
        public BrandMonthlyReport clickCarouselRadioOptionAndVerifyProduct(string radioOption)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='carousel-filters']/label"));
            IList<IWebElement> options = driver.FindElements(By.XPath("//div[@class='carousel-filters']/label"));
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].Text.Contains(radioOption))
                {
                    driver._clickByJavaScriptExecutor("//div[@class='carousel-filters']/label[" + (i + 1) + "]/span");
                    Thread.Sleep(500);
                    Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='carousel-filters']/label[" + (i + 1) + "]/span")).GetCssValue("color").Contains("rgba(0, 74, 82, 1)"), "'" + radioOption + "' Radion option not selected.");
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + radioOption + "' Radio option and Verified product.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Drag and Drop Field from Field Options section
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport dragAndDropFieldFromFieldOptionsSection()
        {
            IList<IWebElement> fieldsCollections = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']"));
            IWebElement fromElement;
            IWebElement toElement;
            for (int i = 0; i < fieldsCollections.Count; i++)
            {
                IWebElement headerName = fieldsCollections[i].FindElement(By.XPath(".//div[@class='row view-customizer-header']"));
                if (headerName.Text.Contains("Hidden Fields"))
                {
                    fromElement = fieldsCollections[i].FindElement(By.XPath(".//div[@class='row view-customizer-content']//.//tbody/tr[1]"));

                    for (int j = 0; j < fieldsCollections.Count; j++)
                    {
                        IWebElement headerNameNew = fieldsCollections[j].FindElement(By.XPath(".//div[@class='row view-customizer-header']"));
                        if (headerNameNew.Text.Contains("Visible Fields"))
                        {
                            toElement = fieldsCollections[j].FindElement(By.XPath(".//div[@class='row view-customizer-content']//.//tbody/tr[1]"));
                            Actions action = new Actions(driver);
                            action.ClickAndHold(fromElement).MoveToElement(toElement).Release(toElement).Build().Perform();
                            break;
                        }
                    }
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Drag and Drop field from Field Options section.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click Reset Fields button from Visible Fields section
        /// </summary>
        /// <param name="buttonName">Button Name to Click</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyAndClickButtonFromFieldOptionsSection(string buttonName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button"), "'" + buttonName + "' Button not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button").Contains(buttonName), "'" + buttonName + "' not match.");
            Assert.AreEqual(null, driver._getAttributeValue("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button", "disabled"), "'" + buttonName + "' Button not Enable.");
            driver._clickByJavaScriptExecutor("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button");
            Thread.Sleep(500);
            Assert.AreEqual("true", driver._getAttributeValue("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button", "disabled"), "'" + buttonName + "' Button not Disable.");
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' button from Visible Fields section.");
            return new BrandMonthlyReport(driver, test);
        }

        #region View Actions Section

        /// <summary>
        /// Click View Button icon to Verify options and click
        /// </summary>
        /// <param name="optionName">Option Name for Click</param>
        /// <returns></returns>
        public BrandMonthlyReport clickViewButtonIconToVerifyOptionsAndClick(string optionName = "")
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button"), "'View' Button not Present on Section.");
            driver._scrollintoViewElement("xpath", "//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button/i");
            driver._clickByJavaScriptExecutor("//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button/i");

            IList<IWebElement> options = driver.FindElements(By.XPath("//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/ul/li"));
            string[] optionNames = { "Details", "Table", "Thumbnail" };
            int cnt = 0;
            bool avail = false;
            for (int i = 0; i < options.Count; i++)
            {
                for (int j = 0; j < optionNames.Length; j++)
                    if (options[i].Text.Contains(optionNames[j]))
                    {
                        if (options[i].Text.Contains(optionName))
                            avail = true;
                        cnt++;
                        break;
                    }
            }

            Assert.AreEqual(cnt, optionNames.Length, "Views Options not match on List.");
            if (optionName != "")
                Assert.AreEqual(true, avail, "'" + optionName + "' Options not match on List.");
            Results.WriteStatus(test, "Pass", "Clicked, Details View Button and Verified Options.");

            if (optionName != "")
            {
                driver._clickByJavaScriptExecutor("//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/ul/li/label[contains(text(),'" + optionName + "')]");
                Thread.Sleep(1000);
                driver._scrollintoViewElement("xpath", "//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button");
                Assert.AreEqual(true, driver._getText("xpath", "//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button").Contains(optionName), "'" + optionName + "' Option not selected.");
                Results.WriteStatus(test, "Pass", "Clicked, '" + optionName + "' Option from List.");
            }

            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Table View Sectipn on screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyTableViewSectionOnScreen()
        {
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading')]");
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ref='centerRow']", 10), "Grid not Present for Table View Screen.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']"), "Ad Thumbnail not Present for Table View Screen.");
            Results.WriteStatus(test, "Pass", "Verified, Table View section on Screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Grid Section for Table View
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyGridSectionForTableView()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//.//div[@class='ag-header-container']/div[@class='ag-header-row']"), "Column Header not Present on Grid.");
            IList<IWebElement> gridRecords = driver.FindElements(By.XPath("//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div"));
            for (int i = 0; i < gridRecords.Count; i++)
                if (driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[" + (i + 1) + "]//.//span[@class='ag-selection-checkbox']"))
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[" + (i + 1) + "]//.//span[@class='ag-selection-checkbox']"), "Checkbox not Present on Grid for [" + (i + 1) + "] Record number.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']"), "Pagination Section not Present.");
            Results.WriteStatus(test, "Pass", "Verified, Grid Section for Table View.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Thumbnail Section on Screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyThumbnailSectionOnScreen()
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
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Thumbnail View Section on Screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyThumbnailViewSectionOnScreen()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='grid-wrapper fade-in CFT-view-results']", 10), "Ad Blocks not Present on screen.");
            IList<IWebElement> adBlockCollections = driver.FindElements(By.XPath("//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-thumbnail-view']"));
            for (int i = 0; i < adBlockCollections.Count; i++)
            {
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//div[@class='row checkbox checkbox-header']"), "Header not Present for Record " + (i + 1) + " on Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//span[@class='checkbox-header-lead-text']"), "Retailer Name not Present for Record " + (i + 1) + " on Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//span[@class='checkbox-header-subtext']"), "Number of Stores not Present for Record " + (i + 1) + " on Ad Block.");

                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//img[contains(@class,'aditem-image-layout')]"), "Ad Image not Present for Record " + (i + 1) + " on Ad Block.");

                driver.MouseHoverUsingElement("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']");
                Thread.Sleep(300);
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'View Ad')]"), "'View Ad' Button not Present for Record " + (i + 1) + " on Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Markets')]"), "'Markets' Button not Present for Record " + (i + 1) + " on Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Details')]"), "'Details' Button not Present for Record " + (i + 1) + " on Ad Block.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Details Thumbnail section on Screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Details View Section on Screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyDetailsViewSectionOnScreen()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='grid-wrapper fade-in CFT-view-results']", 10), "Ad Blocks not Present on screen.");
            IList<IWebElement> adBlockCollections = driver.FindElements(By.XPath("//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-detail-view']"));
            for (int i = 0; i < adBlockCollections.Count; i++)
            {
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[@class='row checkbox checkbox-header']"), "Header not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//span[@class='checkbox-header-lead-text']"), "Retailer Name not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//span[@class='checkbox-header-subtext']"), "Number of Stores not Present for Ad Block.");

                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//img[contains(@class,'aditem-image-layout')]"), "Ad Image not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'col-wrapper-details')]"), "Ad Details not Present beside Ad Image.");

                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'View Ad')]"), "'View Ad' Button not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Markets')]"), "'Markets' Button not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Details')]"), "'Details' Button not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Download')]"), "'Download' Button not Present for Ad Block.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Details View section on Screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Pagination Panel for Table view Section
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyPaginationPanelForViewSection(string viewName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']"), "Pagination Section not Present.");

            if (viewName.Equals("Table View"))
            {
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
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-first disabled']"), "First Icon Default Disable not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-prev disabled']"), "Previous Icon Default Disable not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-page active']"), "Actice First Page not Present.");
                if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-next']") == false)
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-next disabled']"), "Next Icon not Present for Grid.");
                if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-last']") == false)
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-last disabled']"), "Last Icon not Present for Grid.");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[text()='10']"), "Item Per Page '10' not Present.");
                if (viewName == "Table View")
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[text()='25']"), "Item Per Page '25' not Present.");
                else
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[text()='20']"), "Item Per Page '20' not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[text()='50']"), "Item Per Page '50' not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[text()='100']"), "Item Per Page '100' not Present.");
            }
            Results.WriteStatus(test, "Pass", "Verified, Pagination Panel for '" + viewName + "' Section.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click Button on Thumbnails Section
        /// </summary>
        /// <param name="buttonName">Button name to click</param>
        /// <returns></returns>
        public BrandMonthlyReport clickButtonOnViewSection(string buttonName, string viewName)
        {
            if (viewName.Equals("Table View"))
            {
                if (buttonName.Equals("Ad Image"))
                {
                    Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//img"), "Ad Image not Present on Section.");
                    driver._clickByJavaScriptExecutor("//div[@class='aditem aditem-long']//.//img");
                }

                if (buttonName.Equals("View Ad") || buttonName.Equals("Markets") || buttonName.Equals("Details"))
                {
                    Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'" + buttonName + "')]"), "'" + buttonName + "' Icon not Present on Ad Image.");
                    driver._clickByJavaScriptExecutor("//div[@class='aditem aditem-long']//.//button[contains(text(),'" + buttonName + "')]");
                }
            }
            string viewState = "";
            if (viewName.Equals("Details View"))
                viewState = "detail";

            if (viewName.Equals("Thumbnail View"))
                viewState = "thumbnail";

            if (viewName.Equals("Details View") || viewName.Equals("Thumbnail View"))
            {
                if (buttonName.Equals("Ad Image"))
                {
                    driver.MouseHoverUsingElement("xpath", "//*[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//img[contains(@class,'aditem-image-layout')]");
                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//img[contains(@class,'aditem-image-layout')]"), "Ad Image not Present on Section.");
                    driver._clickByJavaScriptExecutor("//*[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//img[contains(@class,'aditem-image-layout')]");
                }

                if (buttonName.Equals("View Ad") || buttonName.Equals("Markets") || buttonName.Equals("Details") || buttonName.Equals("Download"))
                {
                    driver.MouseHoverUsingElement("xpath", "//*[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'" + buttonName + "')]");
                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'" + buttonName + "')]"), "'" + buttonName + "' Icon not Present on " + viewName + " Section.");
                    driver._clickByJavaScriptExecutor("//*[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'" + buttonName + "')]");
                }
            }

            Thread.Sleep(1000);
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' on '" + viewName + "' Section.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click Page Number and Icon from Grid
        /// </summary>
        /// <param name="pageIcon">Page Icon for Click</param>
        /// <returns></returns>
        public BrandMonthlyReport clickPageNumberAndIconFromGrid(string pageIcon = "Page Number")
        {
            driver._waitForElementToBeHidden("xpath", "//*[@id='overlay']/div/div/span[contains(text(),'Loading')]");
            if (pageIcon.Equals("Page Number"))
                if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-page']"))
                {
                    string pageNumer = driver._getText("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-page']/a");
                    driver._clickByJavaScriptExecutor("//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-page']/a");
                    Thread.Sleep(500);
                    driver._waitForElementToBeHidden("xpath", "//*[@id='overlay']/div/div/span[contains(text(),'Loading')]");
                    Assert.AreEqual(pageNumer, driver._getText("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-page active']"), "'" + pageNumer + "' Page not Active.");
                    Results.WriteStatus(test, "Pass", "Clicked, '" + pageNumer + "' Page Number from Grid and Verified.");
                }
                else
                    Results.WriteStatus(test, "Pass", "More Pages not avaible on Grid.");

            if (pageIcon.ToLower().Equals("first") || pageIcon.ToLower().Equals("prev") || pageIcon.ToLower().Equals("next") || pageIcon.ToLower().Equals("last"))
            {
                if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-" + pageIcon.ToLower() + "']/a") == true)
                {
                    driver._clickByJavaScriptExecutor("//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-" + pageIcon.ToLower() + "']/a");
                    Thread.Sleep(500);
                    driver._waitForElementToBeHidden("xpath", "//*[@id='overlay']/div/div/span[contains(text(),'Loading')]");
                    if (pageIcon.ToLower().Equals("first") || pageIcon.ToLower().Equals("last"))
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-" + pageIcon.ToLower() + " disabled']"), "'" + pageIcon + "' Page Icon not Active.");
                    Results.WriteStatus(test, "Pass", "Clicked, '" + pageIcon + "' Page Icon from Grid.");
                }
                else
                    Results.WriteStatus(test, "Pass", "" + pageIcon + " Icon Already Disable.");
            }

            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Button Disable or Not on Screen
        /// </summary>
        /// <param name="buttonName">Button Name to verify</param>
        /// <param name="Disabled">to verify button Disable state</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyButtonDisableOrNotOnScreen(string buttonName, bool Disabled = true)
        {
            bool avail = false;
            if (buttonName.Equals("View Option"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button"), "'View' Button not Present on Section.");
                if (Disabled)
                    Assert.AreEqual(driver._getAttributeValue("xpath", "//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button", "disabled"), "true", "'" + buttonName + "' Button not Disable.");
                else
                    Assert.AreEqual(driver._getAttributeValue("xpath", "//*[@id='affixViewActions']//.//div[contains(@class,'btn-group btn-grid-actions dropdown')]/button", "disabled"), null, "'" + buttonName + "' Button not Enabled.");
                avail = true;
            }
            else
            {
                IList<IWebElement> buttonCollections = driver.FindElements(By.XPath("//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions']/button"));
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
            }

            Assert.AreEqual(true, avail, "'" + buttonName + "' Button not Present to verify.");
            Results.WriteStatus(test, "Pass", "Verified, '" + buttonName + "' Button on screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Select Record from Grid Panel on screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport selectRecordFromGridPanelOnScreen()
        {
            if (driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-detail-view']") == true)
            {
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-detail-view']//.//div[@class='row checkbox checkbox-header']/label"), "Header not Present for First Ad Block.");
                driver._scrollintoViewElement("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-detail-view']//.//div[@class='row checkbox checkbox-header']/label");
                driver._clickByJavaScriptExecutor("//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-detail-view']//.//div[@class='row checkbox checkbox-header']/label");
            }
            else
                if (driver._isElementPresent("xpath", "//div[@id='borderLayout_eGridPanel']//.//div[@class='ag-header-container']/div[@class='ag-header-row']") == true)
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='borderLayout_eGridPanel']//.//div[@class='ag-pinned-left-cols-container']/div[1]//.//span[@class='ag-selection-checkbox']"), "Checkbox not Present on Grid for First Record number.");
                    driver._scrollintoViewElement("xpath", "//div[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[1]//.//span[@class='ag-selection-checkbox']");
                    driver._clickByJavaScriptExecutor("//div[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[1]//.//span[@class='ag-selection-checkbox']/span[contains(@class,'fa-square')]");
                }
                else
                    if (driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-thumbnail-view']") == true)
                    {
                        Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-thumbnail-view']//.//div[@class='row checkbox checkbox-header']"), "Header not Present for First Record on Ad Block.");
                        driver._scrollintoViewElement("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-thumbnail-view']//.//div[@class='row checkbox checkbox-header']");
                        driver._clickByJavaScriptExecutor("//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-thumbnail-view']//.//div[@class='row checkbox checkbox-header']");
                    }

            Thread.Sleep(1000);
            Results.WriteStatus(test, "Pass", "Selected Record from View Section");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Click Button on View Action Section
        /// </summary>
        /// <param name="buttonName">Button Name for click</param>
        /// <returns></returns>
        public BrandMonthlyReport clickButtonOnViewActionSection(string buttonName)
        {
            IList<IWebElement> actionCollection = driver.FindElements(By.XPath("//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions']/button"));
            for (int i = 0; i < actionCollection.Count; i++)
            {
                if (actionCollection[i].Text.Contains(buttonName))
                {
                    Assert.AreEqual(actionCollection[i].GetAttribute("disabled"), null, "'" + buttonName + "' Button not Enabled.");
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", actionCollection[i]);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", actionCollection[i]);
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on View Actions Section.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify View Selected Button Checked or not on Screen
        /// </summary>
        /// <param name="Checked"></param>
        /// <returns></returns>
        public BrandMonthlyReport verifyViewSelectedButtonCheckedOrNotOnScreen(bool Checked = false)
        {
            IList<IWebElement> buttonCollections = driver.FindElements(By.XPath("//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions']/button"));
            bool avail = false;
            for (int i = 0; i < buttonCollections.Count; i++)
            {
                if (buttonCollections[i].Text.Contains("View Selected"))
                {
                    if (Checked)
                        Assert.AreEqual("fa fa-check-square", driver._getAttributeValue("xpath", "//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions'][" + (i + 1) + "]/button/i", "class"), "'View Selected' Checkbox is not Checked.");
                    else
                        Assert.AreEqual(true, driver._getAttributeValue("xpath", "//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions'][" + (i + 1) + "]/button/i", "class").Contains("checkbox-unchecked"), "'View Selected' Checkbox is not UnChecked.");
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'View Selected' Button not Present to verify.");
            Results.WriteStatus(test, "Pass", "Verified, 'View Selected' Button Checked or not on Screen.");
            return new BrandMonthlyReport(driver, test);
        }

        #endregion

        #region Export All Functionality

        /// <summary>
        /// Verify Exort All Section on Dashboard Screen
        /// </summary>
        /// <returns></returns>
        public BrandMonthlyReport verifyExportAllSectionOnDashboardScreen()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']", 20), "Export All Section not Present on screen.");

            IList<IWebElement> sections = driver.FindElements(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div"));
            string[] sectionNames = { "Data Reports", "Power Point Reports", "Asset Downloads" };
            for (int i = 0; i < sections.Count; i++)
                Assert.AreEqual(sectionNames[i], driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='row view-customizer-header']"), "'" + sectionNames + "' Header not Present or match.");

            Results.WriteStatus(test, "Pass", "Verified, Export All section on Dashboard screen.");
            return new BrandMonthlyReport(driver, test);
        }

        /// <summary>
        /// Verify Export All section in Detail on screen
        /// </summary>
        /// <param name="sectionName">Section Name</param>
        /// <returns></returns>
        public String verifyOrClickExportAllSectionInDetailOnScreen(string sectionName, string clickingButtonName = "Nothing", string ReportTitle = "Random")
        {
            IList<IWebElement> sections = driver.FindElements(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div"));
            string reportName = "";
            for (int i = 0; i < sections.Count; i++)
            {
                if (sectionName.Contains(driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='row view-customizer-header']")) == true)
                {
                    IList<IWebElement> lists = driver.FindElements(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div"));

                    if (clickingButtonName.Equals("Download"))
                    {
                        if (ReportTitle != "Random")
                        {
                            for (int j = 0; j < lists.Count; j++)
                                if (lists[j].Text.Contains(ReportTitle))
                                {
                                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/div/button"), "Download Button not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                    reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[1]/div[2]");

                                    driver._clickByJavaScriptExecutor("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/div/button/i[contains(@class,'download')]");
                                    Thread.Sleep(10000);
                                    driver._waitForElementToBeHidden("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/div/button/i[contains(@class,'spinner')]");
                                    Assert.IsTrue(driver._waitForElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/div/button/i[contains(@class,'download')]", 20), "Download icon not display after click download button.");
                                    Thread.Sleep(10000);
                                    //Assert.AreEqual(true, driver.FindElement(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/div/button/i[contains(@class,'download')]")).Displayed, "Download icon not display after click download button.");
                                    Results.WriteStatus(test, "Pass", "Clicked, '" + clickingButtonName + "' Button for '" + reportName + "' Report for '" + sectionName + "' Section.");
                                    break;
                                }
                        }
                        else
                        {
                            Random rand = new Random();
                            int x = rand.Next(0, lists.Count);
                            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button"), "Download Button not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                            reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]");

                            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'download')]"), "Download Option not Present for '" + sectionName + "'.");
                            driver._clickByJavaScriptExecutor("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'download')]");
                            Thread.Sleep(10000);
                            driver._waitForElementToBeHidden("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'spinner')]");
                            Assert.IsTrue(driver._waitForElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'download')]", 20), "Download icon not display after click download button.");
                            Thread.Sleep(10000);
                            //Assert.AreEqual(true, driver.FindElement(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'download')]")).Displayed, "Download icon not display after click download button.");
                            Results.WriteStatus(test, "Pass", "Clicked, '" + clickingButtonName + "' Button for '" + reportName + "' Report for '" + sectionName + "' Section.");
                            break;
                        }
                    }
                    else
                    {
                        if (clickingButtonName.Equals("Schedule"))
                        {
                            if (ReportTitle != "Random")
                            {
                                for (int j = 0; j < lists.Count; j++)
                                    if (lists[j].Text.Contains(ReportTitle))
                                    {
                                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/cft-scheduled-export-popover"), "Schedule Icon not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                        reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[1]/div[2]");

                                        driver._clickByJavaScriptExecutor("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/cft-scheduled-export-popover//.//button");
                                        Thread.Sleep(500);
                                        Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='popover-content popover-body']", 20), "Schedule Popup Window not display after clicked on Schedule button.");
                                        Results.WriteStatus(test, "Pass", "Clicked, '" + clickingButtonName + "' Button for '" + reportName + "' Report for '" + sectionName + "' Section.");
                                        break;
                                    }
                            }
                            else
                            {
                                Random rand = new Random();
                                int x = rand.Next(0, lists.Count);
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/cft-scheduled-export-popover"), "Schedule Icon not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]");

                                driver._clickByJavaScriptExecutor("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/cft-scheduled-export-popover//.//button");
                                Thread.Sleep(500);
                                Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='popover-content popover-body']", 20), "Schedule Popup Window not display after clicked on Schedule button.");
                                Results.WriteStatus(test, "Pass", "Clicked, '" + clickingButtonName + "' Button for '" + reportName + "' Report for '" + sectionName + "' Section.");
                                break;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < lists.Count; j++)
                            {
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[1]/div[contains(@class,'preview-icon icon')]"), "Reports Icon not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/div/button"), "Download Button not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (j + 1) + "]/div[2]/cft-scheduled-export-popover"), "Schedule Icon not Present for Record number [" + (i + 1) + "] for '" + sectionName + "'.");
                            }

                            Results.WriteStatus(test, "Pass", "Verified, '" + sectionName + "' Section in Detail on Screen.");
                            break;
                        }
                    }
                }
            }

            return reportName;
        }

        /// <summary>
        /// Verify File Downloaded or not for Records on screen
        /// </summary>
        /// <param name="fileName">File Name to verify</param>
        /// <param name="FileType">file extension types</param>
        /// <returns></returns>
        public BrandMonthlyReport verifyFileDownloadedOrNotForRecordsOnScreen(string fileName, string FileType)
        {
            bool Exist = false;
            string FilePath = "";
            string Path = ExtentManager.ResultsDir;
            string[] filePaths = Directory.GetFiles(Path, FileType);

            foreach (string filePath in filePaths)
            {
                FileInfo ThisFile = new FileInfo(filePath);
                if (filePath.Contains(fileName) && filePath.Contains(DateTime.Today.ToString("yyyyMMdd")))
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

            Assert.AreEqual(true, Exist, "'" + FileType.Replace("*", "").ToUpper() + "'" + fileName + "' File Not Exported Properly.");
            Results.WriteStatus(test, "Pass", "Verified, <b>'" + FileType.Replace("*", "").ToUpper() + "'</b> File Exported Properly for '" + fileName + "' Report File.");
            return new BrandMonthlyReport(driver, test);
        }

        #endregion

        #endregion
    }
}
