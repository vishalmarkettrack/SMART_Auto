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
    public class BrandDashboard
    {
        #region Private Variables

        private IWebDriver brandDashboard;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public BrandDashboard(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.brandDashboard = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.brandDashboard; }
            set { this.brandDashboard = value; }
        }

        /// <summary>
        /// Verify Brand Monthly Report screen
        /// </summary>
        /// <returns></returns>
        public BrandDashboard verifyBrandDashboardScreen()
        {
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading')]");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='main-view']/div"), "Brand Monthly Report screen not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='domain-carousel']"), "Carousel section not present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='carousel-filters']"), "Carousel filter section not present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-actions-wrapper']"), "Grid not present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li[@class='disabled']"), "'Reset All' Button Default not Disable.");
            Results.WriteStatus(test, "Pass", "Verified, Report Screen Details on Page.");
            return new BrandDashboard(driver, test);
        }

        #region Count of Creatives Running by Advertiser and Media Type

        /// <summary>
        /// Verify Count of Creatives Running by Advertiser and Media Type Chart
        /// </summary>
        /// <returns></returns>
        public BrandDashboard verifyCountOfCreativesRunningByAdvertiserAndMediaTypeChart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]"), "'Count of Creatives Running by Advertiser and Media Type' Chart not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]/div[@class='highcharts-container']"), "'Count of Creatives Running by Advertiser and Media Type' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[contains(@target,'chart-creative-advertiserMediaMix')]//.//div[@class='export-button']/button[@uib-tooltip='Expand']"), "Expand Icon not present for Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[contains(@target,'chart-creative-advertiserMediaMix')]//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[contains(@target,'chart-creative-advertiserMediaMix')]//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]"), "Schedule Icon button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//*[name()='svg']/*[name()='text' and @id='MediaTypeChartTimeframe']"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Count of Creatives Running by Advertiser and Media Type' Chart on Screen.");
            return new BrandDashboard(driver, test);
        }

        /// <summary>
        /// Click Icon Button on screen for chart
        /// </summary>
        /// <param name="chartName">Chart Name to verify</param>
        /// <param name="iconName">Icon Name to click</param>
        /// <returns></returns>
        public BrandDashboard clickIconButtonOnScreenForChart(string chartName, string iconName)
        {
            string divName = "chart-creative-advertiserMediaMix";
            if (chartName.Equals("Count of Creatives Running by Competitor"))
                divName = "chart-creative-shareOfVoice";
            if (chartName.Equals("Full Screen"))
                divName = "selected-chart";
            bool avail = false;

            if (iconName.Equals("Schedule"))
            {
                Assert.AreEqual(null, driver.FindElement(By.XPath("//chart-export[contains(@target,'" + divName + "')]//.//div[@class='export-button']/cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button is Disable.");
                driver._clickByJavaScriptExecutor("//chart-export[contains(@target,'" + divName + "')]//.//div[@class='export-button']/cft-scheduled-export-popover//.//i");
                avail = true;
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[contains(@target,'" + divName + "')]//.//div[@class='export-button']/button[@uib-tooltip='" + iconName + "']"), "'" + iconName + "' Icon not present for Chart.");
                driver._clickByJavaScriptExecutor("//chart-export[contains(@target,'" + divName + "')]//.//div[@class='export-button']/button[@uib-tooltip='" + iconName + "']");
                avail = true;
            }

            Thread.Sleep(3000);
            Assert.AreEqual(true, avail, "'" + iconName + "' Icon not present for Chart.");
            Results.WriteStatus(test, "Pass", "Clicked, '" + iconName + "' Button Icon on screen for '" + chartName + "' Chart.");
            return new BrandDashboard(driver, test);
        }

        /// <summary>
        /// Verify Full screen of Count of Creatives Running by Advertiser and Media Type Chart on screen
        /// </summary>
        /// <returns></returns>
        public BrandDashboard verifyFullScreenOf_CountOfCreativesRunningByAdvertiserAndMediaType_Chart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']"), "'Count of Creatives Running by Advertiser and Media Type' Chart Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']/div[@class='highcharts-container']"), "'Count of Creatives Running by Advertiser and Media Type' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='selected-chart']//.//div[@class='export-button']/button[@uib-tooltip='Go Back']"), "'Go Back' Icon not present for Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[@target='selected-chart']//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "'Download' Icon not present for Chart.");

            Assert.AreEqual("true", driver.FindElement(By.XPath("//chart-export[@target='selected-chart']//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]")).GetAttribute("disabled"), "Schedule Icon button not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']//.//*[name()='svg']/*[name()='text' and @id='MediaTypeChartTimeframe']"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='selected-chart']//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, 'Advertiser Rankings Period over Period' Chart on Full screen.");
            return new BrandDashboard(driver, test);
        }

        #endregion

        #region Count of Creatives Running by Competitor

        /// <summary>
        /// Click on Pie chart and verify drill down level
        /// </summary>
        /// <param name="chartName"></param>
        /// <returns></returns>
        public BrandDashboard clickOnPieChartAndVerifyDrillDownLevel(string chartId)
        {
            IList<IWebElement> chartSeries = driver.FindElements(By.XPath("//div[contains(@id,'" + chartId + "')]/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-series-group')]//.//*[contains(@class,'highcharts-tracker')]/*[name()='path']"));
            IList<IWebElement> legendLists = driver.FindElements(By.XPath("//div[contains(@id,'" + chartId + "')]/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-legend')]//.//*[contains(@class,'highcharts')]"));
            Random rand = new Random(); int x = rand.Next(0, chartSeries.Count);
            string chartDrillPoint = legendLists[x].Text;
            chartSeries[x].Click(); Thread.Sleep(5000);
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'" + chartId + "')]/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-button')]"), "Back to Prvious View link not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[contains(@id,'" + chartId + "')]/div/*[name()='svg']/*[name()='text' and contains(@class,'highcharts-subtitle')]").Contains(chartDrillPoint), "'" + chartDrillPoint + "' Subtitle not present.");

            Results.WriteStatus(test, "Pass", "Clicked on Pie chart and Verified drill down level.");
            return new BrandDashboard(driver, test);
        }

        #endregion

        /// <summary>
        /// Verify Chart Details on screen
        /// </summary>
        /// <returns></returns>
        public String verifyChartDetailsOnScreem(string chartName)
        {
            Assert.AreEqual(true, driver._isElementPresent("id", "key-metrics-creative-carousel"), "Charts Not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='key-metrics-creative-carousel']//.//ol/li"), "Carousel Slider not present.");

            bool avail = false;
            string chartId = "";

            IList<IWebElement> sliderLists = driver.FindElements(By.XPath("//div[@id='key-metrics-creative-carousel']//.//ol/li"));
            for (int i = 0; i < sliderLists.Count; i++)
            {
                IList<IWebElement> chartCollections = driver.FindElements(By.XPath("//div[@id='key-metrics-creative-carousel']//.//div[@class='item text-center active']//.//*[name()='svg']/*[name()='text' and @class='highcharts-title']"));
                for (int j = 0; j < chartCollections.Count; j++)
                {
                    if (chartCollections[j].Text.Contains(chartName))
                    {
                        avail = true;
                        chartId = driver._getAttributeValue("xpath", "//div[@id='key-metrics-creative-carousel']//.//div[@class='item text-center active']/div[" + (j + 1) + "]//.//div[contains(@class,'chart-highchart-wrapper')]", "id");
                        break;
                    }
                }
                if (avail)
                    break;
                else
                    if (sliderLists.Count == i + 1)
                        break;
                    else
                        driver._clickByJavaScriptExecutor("//div[@id='key-metrics-creative-carousel']//.//ol/li[" + (i + 1) + "]");
            }

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'" + chartId + "')]"), "'" + chartName + "' Chart not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'" + chartId + "')]/div[@class='highcharts-container']"), "'" + chartName + "' Chart not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[contains(@target,'" + chartId + "')]//.//div[@class='export-button']/button[@uib-tooltip='Expand']"), "Expand Icon not present for Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[contains(@target,'" + chartId + "')]//.//div[@class='export-button']/button[@uib-tooltip='Download']"), "Download Icon not present for Chart.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//chart-export[contains(@target,'" + chartId + "')]//.//cft-scheduled-export-popover//.//button[contains(@class,'nested-btn')]"), "Schedule Icon button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'" + chartId + "')]//.//*[name()='svg']/*[name()='text' and contains(@id,'Timeframe')]"), "Date Range not present on Chart.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'" + chartId + "')]//.//*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), "'Numerator' website link not present on Chart.");

            Results.WriteStatus(test, "Pass", "Verified, '" + chartName + "' Chart on Screen.");
            return chartId;
        }

        /// <summary>
        /// Verify Legend to click and Verify for chat
        /// </summary>
        /// <param name="chartName"></param>
        /// <returns></returns>
        public BrandDashboard verifyLegendToClickAndVerifyForChart(string chartName)
        {
            string divName = "chart-spend-advertiserMediaMix";
            if (chartName.Equals("Count of Creatives Running by Advertiser and Media Type"))
                divName = "chart-creative-advertiserMediaMix";

            IList<IWebElement> legendLists = driver.FindElements(By.XPath("//div[contains(@id,'" + divName + "')]/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-legend')]//.//*[contains(@class,'highcharts')]"));
            Random rand = new Random();
            int x = rand.Next(0, legendLists.Count);
            legendLists[x].Click(); Thread.Sleep(2000);
            IList<IWebElement> elment = legendLists[x]._findElementsWithinElement("xpath", ".//*[name()='rect']");
            Assert.AreEqual("#93A2AD", elment[0].GetAttribute("fill"), "'Grey Color not match for disable legend.");
            Results.WriteStatus(test, "Pass", "Verified, Legend to Click and Verified Grey Color for Legend.");
            return new BrandDashboard(driver, test);
        }

        /// <summary>
        /// Verify Download popup window and click on option
        /// </summary>
        /// <param name="optionName">OptionName for click</param>
        /// <returns></returns>
        public BrandDashboard verifyDownloadPopupWindowAndClickOnOption(string optionName)
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
            return new BrandDashboard(driver, test);
        }

        /// <summary>
        /// Verify File Downloaded Or Not for Chart
        /// </summary>
        /// <param name="fileName">File Name to verify</param>
        /// <param name="FileType">File Extension to verify</param>
        /// <returns></returns>
        public BrandDashboard verifyFileDownloadedOrNotOnScreen(string fileName, string FileType)
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
            return new BrandDashboard(driver, test);
        }

        /// <summary>
        /// Verify Schedule window on screen
        /// </summary>
        /// <param name="searchName">Search Name to verify</param>
        /// <returns></returns>
        public BrandDashboard verifyScheduleWindow(string searchName)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='popover-content popover-body']"), "Schedule Window not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@class='form-control' and @placeholder='" + searchName + "']"), "'" + searchName + "' Search Name Default not display.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='fa fa-check text-success form-control-feedback']"), "'" + searchName + "' Search Name Feedback in Green Right color not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default dropdown-toggle']"), "Schedule Dropdown not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row']/span[contains(text(),'" + searchName + "')]"), "'" + searchName + " will be delivered every day.' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Create Scheduled Export')]"), "'Create Scheduled Export' Button not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Schedule Window on screen.");
            return new BrandDashboard(driver, test);
        }

        /// <summary>
        /// Hover Mouse on Bar chart and get tooltip records to verify after Deselecting legend.
        /// </summary>
        /// <param name="chartName">Chart Name to get and verify tooltip records</param>
        /// <returns></returns>
        public BrandDashboard hoverMouseOnBarChartAndGetTheTooltipRecords(string chartName)
        {
            string divName = "chart-spend-advertiserMediaMix";
            if (chartName.Equals("Count of Creatives Running by Advertiser and Media Type"))
                divName = "chart-creative-advertiserMediaMix";

            string title1, title2, tableHead1, tableHead2, tableBody1, tableBody2;
            IList<IWebElement> legendLists = driver.FindElements(By.XPath("//div[contains(@id,'" + divName + "')]/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-legend')]//.//*[contains(@class,'highcharts')]"));
            IList<IWebElement> chartSeries = driver.FindElements(By.XPath("//div[contains(@id,'" + divName + "')]/div/*[name()='svg']/*[name()='g' and contains(@class,'highcharts-series-group')]//.//*[contains(@class,'highcharts-tracker')]"));
            Actions action = new Actions(driver);
            action.MoveToElement(chartSeries[chartSeries.Count - 1]).Build().Perform();
            action.MoveByOffset(1, 1).Perform(); Thread.Sleep(3000);

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]"));
            title1 = driver._getText("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]//.//h5");
            tableHead1 = driver._getText("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]//.//table[@class='table highchart-tooltip-table']/thead");
            tableBody1 = driver._getText("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]//.//table[@class='table highchart-tooltip-table']/tbody");

            legendLists[1].Click(); Thread.Sleep(2000);

            action.MoveToElement(chartSeries[chartSeries.Count - 1]).Build().Perform();
            action.MoveByOffset(1, 1).Perform(); Thread.Sleep(2000);

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]"));
            title2 = driver._getText("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]//.//h5");
            tableHead2 = driver._getText("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]//.//table[@class='table highchart-tooltip-table']/thead");
            tableBody2 = driver._getText("xpath", "//div[contains(@id,'chart-creative-advertiserMediaMix')]//.//div[contains(@class,'highcharts-tooltip')]//.//table[@class='table highchart-tooltip-table']/tbody");

            Assert.AreEqual(title1, title2, "Title of tooltip not same.");
            Assert.AreEqual(tableHead1, tableHead2, "Tooltip Header Title not same.");
            Assert.AreNotEqual(tableBody1, tableBody2, "Tooltip Records remain same.");

            Results.WriteStatus(test, "Pass", "Mouse hover on '" + chartName + "' Chart and Verified tooltip after deselecting legend.");
            return new BrandDashboard(driver, test);
        }

        #endregion
    }
}