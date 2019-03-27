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

namespace SMART_AUTO
{
    public class Search
    {
        #region Private Variables

        private IWebDriver search;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public Search(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.search = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.search; }
            set { this.search = value; }
        }

        /// <summary>
        /// Verify My Search screen
        /// </summary>
        /// <param name="accountName">Account Name to verify Search Fields</param>
        /// <returns></returns>
        public Search verifyMySearchScreen(string accountName = "QA Testing - Brand")
        {
            Assert.True(driver._waitForElement("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'my_search')]", 20), "'Edit Search' Button not Present.");
            Assert.IsTrue(driver._getText("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'my_search')]").Contains("Edit Search"), "'Edit Search' Button Label not match.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'saved_search')]"), "'Saved Searches' Button not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'saved_search')]").Contains("Saved Searches"), "'Saved Searches' Button Label not match.");

            Assert.AreEqual(true, driver._isElementPresent("id", "CftSearchMenu"), "Search Menu Section not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Basic']/div[@id='heading-Basic']"), "'Basic Fields' Tab not Present.");
            Assert.IsTrue(driver._getText("xpath", "//div[@menu-name='Basic']/div[@id='heading-Basic']").Contains("Basic Fields"), "'Basic Fields' Tab Label not match.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Other']/div[@id='heading-Other']"), "'Other Fields' Tab not Present.");
            Assert.IsTrue(driver._getText("xpath", "//div[@menu-name='Other']/div[@id='heading-Other']").Contains("Other Fields"), "'Other Fields' Tab Label not match.");

            if (accountName.Equals("QA Testing - Brand"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Basic']/div[@role='tabpanel']/button/span[text() = 'Date Range']"), "'Date Range' Menu Item not Present under Basic Fields Tab.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-timeframe-calendar/div[@class='list-group-item CFT-search-field']"), "'Date Range' Section not Present.");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Basic']/div[@role='tabpanel']/button/span[text() = 'Media']"), "'Media' Menu Item not Present under Basic Fields Tab.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'occurrence_media')]"), "'Media' Section not Present.");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Basic']/div[@role='tabpanel']/button/span[text() = 'Market']"), "'Market' Menu Item not Present under Basic Fields Tab.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-ct_occurrence_dmaName']"), "'Market' Section not Present.");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Other']/div[@role='tabpanel']/button/span[text() = 'Ad Status']"), "'Ad Status' Menu Item not Present under Basic Fields Tab.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Other']/div[@role='tabpanel']/button/span[text() = 'Advertiser Product']"), "'Advertiser Product' Menu Item not Present under Basic Fields Tab.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@menu-name='Other']/div[@role='tabpanel']/button/span[text() = 'Category']"), "'Category' Menu Item not Present under Basic Fields Tab.");
            }

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@id,'chart-creative-mysearch')]"), "'Creatives' Chart not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and text() = 'Cancel']"), "'Cancel' Button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and text() = 'Apply Search']"), "'Apply Search' Button not Present.");

            Results.WriteStatus(test, "Pass", "Verified, My Search Screen successfully.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Saved Searches Button on screen
        /// </summary>
        /// <returns></returns>
        public Search verifySavedSearchesButtonOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'saved_search')]"), "'Saved Searches' Button not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'saved_search')]").Contains("Saved Searches"), "'Saved Searches' Button Label not match.");

            if (driver._getAttributeValue("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//li[contains(@ng-class,'SavedSearchTab')]", "class") == null)
                Results.WriteStatus(test, "Pass", "Verified, No Saved Searches Records available so 'Saved Searches' Button is Disable.");
            else
                Results.WriteStatus(test, "Pass", "Verified, Saved Searches Records available so 'Saved Searches' Button is Enable.");

            return new Search(driver, test);
        }

        /// <summary>
        /// Select Date Range option from Section
        /// </summary>
        /// <param name="dateRange">Date Range to Select</param>
        /// <returns></returns>
        public Search selectDateRangeOptionFromSection(string dateRange = "Random")
        {
            IList<IWebElement> dateRangeCollections = driver.FindElements(By.XPath("//cft-field-editor-timeframe-calendar//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div/div[1]//.//ul/li"));
            bool avail = false;

            if (dateRange.Equals("Random"))
            {
                Random rand = new Random();
                for (int i = 0; i < 6; i++)
                {
                    int x = rand.Next(3, dateRangeCollections.Count);
                    dateRange = dateRangeCollections[x].Text;
                    driver._clickByJavaScriptExecutor("//cft-field-editor-timeframe-calendar//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div/div[1]//.//ul/li[" + (x + 1) + "]/span");
                    Thread.Sleep(500);
                    if (driver._getText("xpath", "//*[@id='CftSearchSummary']/div[1]").Contains("No Data Found") == false)
                        break;
                }
                Results.WriteStatus(test, "Pass", "Selected, '" + dateRange + "' Date Range from Section.");
            }
            else
            {
                for (int i = 0; i < dateRangeCollections.Count; i++)
                {
                    if (dateRangeCollections[i].Text == dateRange)
                    {
                        driver._clickByJavaScriptExecutor("//cft-field-editor-timeframe-calendar//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div/div[1]//.//ul/li[" + (i + 1) + "]/span");
                        avail = true; Thread.Sleep(2000);
                        break;
                    }
                }

                Assert.AreEqual(true, avail, "'" + dateRange + "' Date Range not Present.");
                Results.WriteStatus(test, "Pass", "Selected, '" + dateRange + "' Date Range from Section.");
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Select Media checkbox option from  section
        /// </summary>
        /// <param name="mediaName">Media name to Select</param>
        /// <returns></returns>
        public Search selectMediaCheckboxOptionFromSection(string mediaName = "Random")
        {
            IList<IWebElement> mediaCollections = driver.FindElements(By.XPath("//div[contains(@id,'occurrence_mediaName')]//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div"));
            bool avail = false;

            if (mediaName.Equals("Random"))
            {
                Random rand = new Random();
                int x = rand.Next(0, mediaCollections.Count);
                mediaName = mediaCollections[x].Text;
                driver._clickByJavaScriptExecutor("//div[contains(@id,'occurrence_mediaName')]//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div[" + (x + 1) + "]/label");
                Results.WriteStatus(test, "Pass", "Selected, '" + mediaName + "' Date Range from Section.");
            }
            else
            {
                for (int i = 0; i < mediaCollections.Count; i++)
                {
                    if (mediaCollections[i].Text == mediaName)
                    {
                        driver._clickByJavaScriptExecutor("//div[contains(@id,'occurrence_mediaName')]//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div[" + (i + 1) + "]/label");
                        avail = true; Thread.Sleep(2000);
                        break;
                    }
                }

                Assert.AreEqual(true, avail, "'" + mediaName + "' Media not Present.");
                Results.WriteStatus(test, "Pass", "Selected, '" + mediaName + "' Media from Section.");
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Selected Data Range or Select Different Date Range
        /// </summary>
        /// <param name="verifyDataRange">Verify Selected Data Range</param>
        /// <returns></returns>
        public String verifySelectedDateRangeORSelectDifferentDateRange(bool verifyDataRange)
        {
            IList<IWebElement> dateRangeCollections = driver.FindElements(By.XPath("//cft-field-editor-timeframe-calendar//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div/div[1]//.//ul/li"));
            string dateRange = "";

            for (int i = 0; i < dateRangeCollections.Count; i++)
            {
                if (driver.FindElement(By.XPath("//cft-field-editor-timeframe-calendar//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div/div[1]//.//ul/li[" + (i + 1) + "]/span")).GetCssValue("color").Contains("0, 74, 82") == verifyDataRange)
                {
                    dateRange = driver._getText("xpath", "//cft-field-editor-timeframe-calendar//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div/div[1]//.//ul/li[" + (i + 1) + "]/span");
                    Results.WriteStatus(test, "Pass", "Verified, '" + dateRange + "' Date Range on screen");
                    if (verifyDataRange == false)
                    {
                        driver._clickByJavaScriptExecutor("//cft-field-editor-timeframe-calendar//.//div[@class='CFT-search-list-group-field CFT-search-list-group-field-child']/div/div/div[1]//.//ul/li[" + (i + 1) + "]/span");
                        Results.WriteStatus(test, "Pass", "Selected '" + dateRange + "' Date Range on screen");
                    }
                    break;
                }
            }

            return dateRange;
        }

        /// <summary>
        /// Verify Saved Searches Section on Screen 
        /// </summary>
        /// <returns></returns>
        public Search verifySavedSearchesSectionOnScreen(bool searchCard = true)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='list-group-item cftSearchField']", 20), "'Saved Searched' Section not Present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-search-list-group-field']//.//input[contains(@placeholder,'Filter Saved Searches By Name')]"), "'Filter Saved Searches By Name' textarea not present.");

            IList<IWebElement> buttons = driver.FindElements(By.XPath("//div[@class='CFT-search-list-group-field']//.//button"));
            string[] buttonNames = { "Applied Search", "Default Search" };
            for (int i = 0; i < buttons.Count; i++)
                Assert.AreEqual(true, buttons[i].Text.Contains(buttonNames[i]), "'" + buttonNames[i] + "' Button not Present.");

            string searchTitle = "";
            if (searchCard)
            {
                IList<IWebElement> savedSearches = driver.FindElements(By.XPath("//cft-saved-search-list-item"));
                for (int l = 0; l < savedSearches.Count; l++)
                {
                    Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                    searchTitle = savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;
                    Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-subtext']")).Displayed, "Created Date of '" + searchTitle + "' Saved Search not Present.");

                    IList<IWebElement> searchFields = savedSearches[l]._findElementsWithinElement("xpath", ".//table[@class='table table-details-content']//.//tr");
                    string[] fieldNames = { "Status", "Date Range" };
                    int cnt = 0;
                    for (int f = 0; f < searchFields.Count; f++)
                    {
                        if (searchFields[f].FindElement(By.XPath(".//th")).Text.Contains(fieldNames[0]) == true || searchFields[f].FindElement(By.XPath(".//th")).Text.Contains(fieldNames[1]) == true)
                            cnt++;

                        if (searchFields[f].FindElement(By.XPath(".//th")).Text == "Scheduled Exports")
                        {
                            IList<IWebElement> exports = searchFields[f].FindElements(By.XPath(".//button[contains(@class,'nested-btn-default')]"));
                            for (int e = 0; e < exports.Count; e++)
                            {
                                driver.MouseHoverByJavaScript(exports[e]);
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='tooltip-inner']"), "Tooltip not Present on screen.");
                                Assert.AreEqual("Schedule", driver._getText("xpath", "//div[@class='tooltip-inner']"), "'Schedule' Tooltip message not match.");
                            }
                        }
                    }
                    Assert.AreEqual(cnt, fieldNames.Length, "'Status' or 'Date Range' not present for '" + searchTitle + "' Saved Search.");

                    IList<IWebElement> buttonLists = savedSearches[l]._findElementsWithinElement("xpath", ".//button[@class='btn btn-default btn-block custom-btn-default']");
                    string[] buttonsTitle = { "Delete", "Edit Search", "Make Default", "Apply Search" };
                    for (int b = 0; b < buttonLists.Count; b++)
                        Assert.AreEqual(true, buttonLists[b].Text.Contains(buttonsTitle[b]), "'" + buttonsTitle[b] + "' Button not Present for '" + searchTitle + "' Saved Search.");
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Saved Searches Section on Screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Enter Value in Filter Saved searches input area on screen
        /// </summary>
        /// <param name="searchValueName">Search Value</param>
        /// <returns></returns>
        public Search enterValueInFilterSavedSearchedInputAreaOnScreen(string searchValueName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-search-list-group-field']//.//input[contains(@placeholder,'Filter Saved Searches By Name')]"), "'Filter Saved Searches By Name' textarea not present.");
            driver._type("xpath", "//div[@class='CFT-search-list-group-field']//.//input[contains(@placeholder,'Filter Saved Searches By Name')]", searchValueName);
            Thread.Sleep(2000);
            Results.WriteStatus(test, "Pass", "Entered, '" + searchValueName + "' Value in Filter Saved Searches input area on screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Click Schedule from Schedule Export and perform Action
        /// </summary>
        /// <param name="action">Action to Perform</param>
        /// <returns></returns>
        public Search clickScheduleFromScheduleExportAndPerformAction(string action)
        {
            string exportScheduleName = "";
            IList<IWebElement> element = driver.FindElements(By.XPath("//button[contains(@class,'nested-btn-default')]"));

            if (element.Count == 0)
                Results.WriteStatus(test, "Pass", "Schedule not Present on Schedule Export Section.");
            else
            {
                exportScheduleName = element[0].Text;
                element[0].Click();
                Results.WriteStatus(test, "Pass", "Clicked, Schedule From Schedule Exports Section.");

                if (action.Equals("Update"))
                    clickButtonFromScheduleWindow("Update");

                if (action.Equals("Delete"))
                    clickButtonFromScheduleWindow("Delete");
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Click Button from Schedule Window
        /// </summary>
        /// <param name="buttonName">Button Name to click</param>
        /// <returns></returns>
        public Search clickButtonFromScheduleWindow(string buttonName)
        {
            if (buttonName.Equals("Update"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'" + buttonName + "')]"), "'" + buttonName + "' Button not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'" + buttonName + "')]");
                Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button from Schedule Window.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[contains(text(),'Successfully updated a scheduled export for')]"), "Schedule Updated message not Present.");
            }

            if (buttonName.Equals("Delete"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'" + buttonName + "')]"), "'" + buttonName + "' Button not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-primary' and contains(text(),'" + buttonName + "')]");
                Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button from Schedule Window.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[contains(text(),'Successfully deleted scheduled export!')]"), "Schedule Deleted message not Present.");
            }

            Results.WriteStatus(test, "Pass", "Verified, tooltip message for '" + buttonName + "' Action on Screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Schedule Window 
        /// </summary>
        /// <returns></returns>
        public Search verifyScheduleWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='popover-content popover-body']", 20), "Schedule Window not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@class='form-control']"), "Search Name Default not display.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='fa fa-check text-success form-control-feedback']"), "Search Name Feedback in Green Right color not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default dropdown-toggle']"), "Schedule Dropdown not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Update')]"), "'Update' Button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Delete')]"), "'Delete' Button not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Schedule Window on screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Checked or UnChecked Fixed Date Range from Search Section
        /// </summary>
        /// <param name="unChecked">UnChecked Date Range</param>
        /// <returns></returns>
        public Search checkedOrUnCheckedFixedDateRangeFromSearchScreen(bool Checked)
        {
            if (driver._isElementPresent("xpath", "//div[@class='CFT-textbox has-success']/input[@name='startDate' and @class='form-control ng-untouched ng-pristine']") == Checked)
                driver._clickByJavaScriptExecutor("//cft-field-editor-timeframe-calendar/div[@class='list-group-item CFT-search-field']//.//form/div[@class='checkbox']/label/span");

            Assert.AreEqual(Checked, driver._isElementPresent("xpath", "//div[@class='CFT-textbox has-success']/input[@name='startDate' and contains(@class,'ng-valid')]"), "'Start Date' Textarea not Enable or Disable.");
            Assert.AreEqual(Checked, driver._isElementPresent("xpath", "//div[@class='CFT-textbox has-success']/input[@name='endDate' and contains(@class,'ng-valid')]"), "'End Date' Textarea not Enable or Disable.");
            Results.WriteStatus(test, "Pass", "Checked / UnChecked Fixed Date Range from Seaech Screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Media Field section on Screen
        /// </summary>
        /// <returns></returns>
        public Search verifyMediaFieldSectionOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']"), "Media Fields section not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//input[@type='text' and contains(@placeholder,'Filter Media Types')]"), "Search Area not Present for Media.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button/span[text()='Select Displayed']"), "'Select Displayed' Button not Present for Media.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]"), "'Exclude' Button not Present for Media.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//div[contains(@id,'multi-ag-grid')]"), "Media Value tree view not Present for Media.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'loadMore')]"), "'Load More' Button not Present for Media.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'deselectAll')]"), "'Clear Selected' Button not Present for Media.");

            Results.WriteStatus(test, "Pass", "Verified, Media Field Section on Screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Select Records from Right section and verify into Selected Section
        /// </summary>
        /// <returns></returns>
        public Search selectRecordsFromRightSectionAndVerifyIntoSelectedSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media']"), "Media Value tree view not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media-selected']"), "Selected Media Value tree view not present.");

            string mediaTitle = "";

            IList<IWebElement> totalMedia = driver.FindElements(By.XPath("//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media']//.//div[@class='ag-body-container']/div"));
            Random rand = new Random();
            int x = rand.Next(0, totalMedia.Count);
            mediaTitle = totalMedia[x].Text;
            if (mediaTitle.Contains("("))
                mediaTitle = mediaTitle.Substring(0, mediaTitle.IndexOf("("));
            totalMedia[x].Click();
            Thread.Sleep(1000);
            Results.WriteStatus(test, "Pass", "Selected Media from Right section.");

            IList<IWebElement> selectedMedia = driver.FindElements(By.XPath("//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media-selected']//.//div[@class='ag-body-container']/div"));
            bool avail = false;
            for (int i = 0; i < selectedMedia.Count; i++)
            {
                if (mediaTitle.Contains(selectedMedia[i].Text))
                {
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + mediaTitle + "' Media not available on Selected section.");
            Results.WriteStatus(test, "Pass", "Verified, '" + mediaTitle + "' Media on Selceted section.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Mouse hover on button to verify effect
        /// </summary>
        /// <param name="buttonName">Button name to verify Effect</param>
        /// <returns></returns>
        public Search mouseHoverOnButtonToVerifyEffect(string buttonName = "Select Displayed")
        {
            if (buttonName.Equals("Select Displayed"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button/span[text()='Select Displayed']"), "" + buttonName + " Button not Present.");
                driver.MouseHoverUsingElement("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button/span[text()='Select Displayed']");
                Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'selectAll')]")).GetCssValue("color"), "'" + buttonName + "' Button not Highlighted with Blue color.");
            }
            if (buttonName.Equals("Exclude"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]"), "" + buttonName + " Button not Present.");
                driver.MouseHoverUsingElement("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]");
                Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]")).GetCssValue("color"), "'" + buttonName + "' Button not Highlighted with Blue color.");
            }
            if (buttonName.Equals("Coop Selected"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setViewMode')]"), "" + buttonName + " Button not Present.");
                driver.MouseHoverUsingElement("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setViewMode')]");
                Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setViewMode')]")).GetCssValue("color"), "'" + buttonName + "' Button not Highlighted with Blue color.");
            }
            if (buttonName.Equals("Coop Select Displayed"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'selectAll')]"), "" + buttonName + " Button not Present.");
                driver.MouseHoverUsingElement("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'selectAll')]");
                Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'selectAll')]")).GetCssValue("color"), "'" + buttonName + "' Button not Highlighted with Blue color.");
            }
            if (buttonName.Equals("Coop Exclude"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'Excluded')]"), "" + buttonName + " Button not Present.");
                driver.MouseHoverUsingElement("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'Excluded')]");
                Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'Excluded')]")).GetCssValue("color"), "'" + buttonName + "' Button not Highlighted with Blue color.");
            }

            Results.WriteStatus(test, "Pass", "Mouse Hover on '" + buttonName + "' Button and verified Effects.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Click Button on Search screen
        /// </summary>
        /// <param name="buttonName">Button Name to Click</param>
        /// <returns></returns>
        public Search clickButtonOnSearchScreen(string buttonName)
        {
            switch (buttonName)
            {
                case "Select Displayed":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button/span[text()='Select Displayed']"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_media']//.//button/span[text()='Select Displayed']");
                        break;
                    }

                case "Exclude":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]");
                        break;
                    }

                case "Coop Selected":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setViewMode')]"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setViewMode')]");
                        break;
                    }

                case "Coop Clear Selected":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'deselectAll')]"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'deselectAll')]");
                        break;
                    }

                case "Coop Cancel":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setFilterTerm')]"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setFilterTerm')]");
                        break;
                    }

                case "Coop Select Displayed":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button/span[text()='Select Displayed']"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button/span[text()='Select Displayed']");
                        break;
                    }

                case "Coop Exclude":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'Excluded')]"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'Excluded')]");
                        break;
                    }

                case "Coop Browse":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setBrowseViewMode')]"), "" + buttonName + " Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'setBrowseViewMode')]");
                        break;
                    }

                case "Apply Search":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Apply Search')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-primary' and contains(text(),'Apply Search')]");
                        Thread.Sleep(2000);
                        driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");
                        break;
                    }

                case "Save As":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Save As')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Save As')]");
                        break;
                    }

                case "Reset":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Reset')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Reset')]");
                        break;
                    }

                case "Save!":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-success' and contains(text(),'Save!')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-success' and contains(text(),'Save!')]");
                        break;
                    }

                case "Cancel":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Cancel')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Cancel')]");
                        break;
                    }

                case "Reset Changes":
                case "Clear Search":
                    {
                        if (driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Reset Changes')]"))
                        {
                            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Reset Changes')]"), "'" + buttonName + "' Button not Present.");
                            driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Reset Changes')]");
                        }
                        else
                        {
                            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Clear Search')]"), "'" + buttonName + "' Button not Present.");
                            driver._clickByJavaScriptExecutor("//button[@class='btn btn-primary' and contains(text(),'Clear Search')]");
                        }
                        break;
                    }

                case "Default Search":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-search-list-group-field']//.//button[contains(text(),'Default Search')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@class='CFT-search-list-group-field']//.//button[contains(text(),'Default Search')]");
                        break;
                    }

                case "Applied Search":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-search-list-group-field']//.//button[contains(text(),'Applied Search')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//div[@class='CFT-search-list-group-field']//.//button[contains(text(),'Applied Search')]");
                        break;
                    }

                case "Overwrite":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Overwrite')]"), "'" + buttonName + "' Button not Present.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-default' and contains(text(),'Overwrite')]");
                        break;
                    }

                case "Edit Search":
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'my_search')]"), "'" + buttonName + "' Button not present.");
                        driver._clickByJavaScriptExecutor("//ul[@class='nav nav-tabs modal-tabs']//.//a[contains(@ng-click,'my_search')]");
                        break;
                    }
            }

            Thread.Sleep(2000);
            Results.WriteStatus(test, "Pass", "Clicked '" + buttonName + "' Button on Search screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Button Disable or not on Screen
        /// </summary>
        /// <param name="buttonName">ButtonName to verify</param>
        /// <param name="Disabled">Verify Button Disable</param>
        /// <returns></returns>
        public Search verifyButtonDisableOrNotOnScreen(string buttonName, bool Disabled = true)
        {
            if (Disabled)
            {
                Assert.AreEqual(driver._getAttributeValue("xpath", "//button[@class='btn btn-default' and contains(text(),'" + buttonName + "')]", "disabled"), ("true"), "'" + buttonName + "' Button not Disabled.");
                Results.WriteStatus(test, "Pass", "Verified, '" + buttonName + "' Button Disabled on screen.");
            }
            else
            {
                Assert.AreEqual(driver._getAttributeValue("xpath", "//button[@class='btn btn-default' and contains(text(),'" + buttonName + "')]", "disabled"), null, "'" + buttonName + "' Button not Disabled.");
                Results.WriteStatus(test, "Pass", "Verified, '" + buttonName + "' Button Enabled on screen.");
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Enter Search Value on Search Screen
        /// </summary>
        /// <returns></returns>
        public String enterSearchValueOnSearchScreen()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//input[contains(@placeholder,'What would you like to call your search') and @type='text']", 20), "'What would you like to call your search?' textarea not Present.");
            driver._clickByJavaScriptExecutor("//input[contains(@placeholder,'What would you like to call your search') and @type='text']");
            string scheduleSearchName = "Test" + driver._randomString(4, true);
            driver._type("xpath", "//input[contains(@placeholder,'What would you like to call your search') and @type='text']", scheduleSearchName);
            Thread.Sleep(1000);
            Results.WriteStatus(test, "Pass", "Entered Save As Search Report Name on Screen.");
            return scheduleSearchName;
        }

        /// <summary>
        /// Verify Selectd Records on Select Displayed section
        /// </summary>
        /// <returns></returns>
        public Search verifySelectedRecordsOnSelectDisplayedSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media']"), "Media Value tree view not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media-selected']"), "Selected Media Value tree view not present.");

            IList<IWebElement> totalMedia = driver.FindElements(By.XPath("//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media']//.//div[@class='ag-body-container']/div"));
            IList<IWebElement> selectedMedia = driver.FindElements(By.XPath("//div[@id='field-brand-n_occurrence_media']//.//div[@id='multi-ag-grid-brand-n_occurrence_media-selected']//.//div[@class='ag-body-container']/div"));

            for (int i = 0; i < selectedMedia.Count; i++)
                Assert.AreEqual(true, totalMedia[i].Text.Contains(selectedMedia[i].Text), "'" + selectedMedia[i].Text + "' Value not Present on Section.");

            Results.WriteStatus(test, "Pass", "Verified, Selected Records on Selcet Displayed section.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Exclude button titles on Search screen
        /// </summary>
        /// <returns></returns>
        public Search verifyExcludeButtonTitlesOnSearchScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]"), "Exclude Button not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'ExclusionMode')]").Contains("Excluding"), "'Excluding' Title not change.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'deselectAll')]"), "'Clear Excluded' Button Title not change.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[@id='field-brand-n_occurrence_media']//.//button[contains(@ng-click,'deselectAll')]").Contains("Clear Excluded"), "'Clear Excluded' Button Title not change.");

            Results.WriteStatus(test, "Pass", "Verified, Exclude button titles on Search screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Field Menu and click on it on Search screen
        /// </summary>
        /// <param name="fieldName">Field Name to verify</param>
        /// <returns></returns>
        public Search verifyFieldMenuAndClickOnItOnSearchScreen(string fieldName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@role='tablist']//.//div[@role='tabpanel']/button/span[text() = '" + fieldName + "']"), "'" + fieldName + "' Menu Item not Present under Basic Fields Tab.");
            driver._clickByJavaScriptExecutor("//div[@role='tablist']//.//div[@role='tabpanel']/button/span[text() = '" + fieldName + "']");
            Thread.Sleep(2000);

            if (fieldName.Equals("Coop Advertisers"))
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']"), "'Coop Advertisers' Section not Present.");

            Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field Menu and Verified on Search screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Select records from coop advertisers section
        /// </summary>
        /// <param name="multiple">Select Multiple Records</param>
        /// <returns></returns>
        public Search selectRecordsFromCoopAdvertisersSection(bool multiple = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"), "Coop Advertisers' Records not Present.");

            IList<IWebElement> totalAdvertise = driver.FindElements(By.XPath("//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"));
            for (int i = 0; i < totalAdvertise.Count; i++)
            {
                IList<IWebElement> cells = totalAdvertise[i]._findElementsWithinElement("xpath", ".//label/input[contains(@class,'not-empty')]");
                if (cells.Count == 0)
                {
                    cells = totalAdvertise[i]._findElementsWithinElement("xpath", ".//label/span");
                    cells[0].Click();
                    Thread.Sleep(2000);
                    break;
                }
            }

            if (multiple)
            {
                for (int i = 0; i < totalAdvertise.Count; i++)
                {
                    IList<IWebElement> cells = totalAdvertise[i]._findElementsWithinElement("xpath", ".//label/input[contains(@class,'not-empty')]");
                    if (cells.Count == 0)
                    {
                        cells = totalAdvertise[i]._findElementsWithinElement("xpath", ".//label/span");
                        cells[0].Click();
                        Thread.Sleep(2000);
                        break;
                    }
                }
            }
            Results.WriteStatus(test, "Pass", "Selected Records from Coop Advertiser Section.");
            return new Search(driver, test);
        }

        /// <summary>
        /// unChecked item from Coop Advertiser Section and Verify
        /// </summary>
        /// <returns></returns>
        public Search unCheckedItemFromCoopAdvertisersSectionAndVerify()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"), "Coop Advertisers' Records not Present.");

            IList<IWebElement> totalAdvertise = driver.FindElements(By.XPath("//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"));
            string unCheckedItem = totalAdvertise[0].Text;
            IList<IWebElement> recordTitle = totalAdvertise[0]._findElementsWithinElement("xpath", ".//label/span");
            recordTitle[0].Click();
            Thread.Sleep(1000);

            totalAdvertise = driver.FindElements(By.XPath("//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"));
            for (int i = 0; i < totalAdvertise.Count; i++)
            {
                if (totalAdvertise[i].Text.Contains(unCheckedItem))
                {
                    IList<IWebElement> cells = totalAdvertise[i]._findElementsWithinElement("xpath", ".//label/input[contains(@class,'not-empty')]");
                    Assert.AreEqual(0, cells.Count, "'" + totalAdvertise[i].Text + "' Item not present on section.");
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "UChecked Item from Coop Advertiser Section and Verified.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Enter Value in Coop Advertisers input area on screen
        /// </summary>
        /// <param name="filterValue">Filter Value to search</param>
        /// <returns></returns>
        public String enterValueInCoopAdvertisersInputAreaOnScreen(string filterValue)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//input[contains(@placeholder,'Filter')]"), "'Filter Coop Advertisers' textarea not present.");
            string searchValue = "";

            if (filterValue.Equals("Keyword"))
            {
                searchValue = driver._getText("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div");
                searchValue = searchValue.Substring(0, 4);
            }

            if (filterValue.Equals("Letter"))
                searchValue = "12";

            if (filterValue.Equals("Random"))
                searchValue = driver._randomString(6) + driver._randomString(4, true);

            driver._type("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//input[contains(@placeholder,'Filter')]", searchValue);
            Results.WriteStatus(test, "Pass", "Entered, '" + searchValue + "' Keyword on Coop Advertisers input area on screen.");
            return searchValue;
        }

        /// <summary>
        /// Verify Filter value on coop Advertisers section
        /// </summary>
        /// <param name="filterValue">Filter Value on Section</param>
        /// <returns></returns>
        public Search verifyFilterValueOnCoopAdvertisersSection(string filterValue, bool noDataFound = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"), "Coop Advertisers' Records not Present.");

            if (noDataFound)
            {
                Assert.AreEqual(true, driver._getText("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div").Contains("No Rows To Show"), "'No Rows To Show' Message not found.");
                Results.WriteStatus(test, "Pass", "'No Rows To Show' Message not found on Coop Advertisers Section.");
            }
            else
            {
                if (driver._getText("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div").Contains("No Rows To Show"))
                    Results.WriteStatus(test, "Pass", "'No Rows To Show' Records found on Coop Advertisers Section.");
                else
                {
                    IList<IWebElement> totalAdvertise = driver.FindElements(By.XPath("//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"));
                    for (int i = 0; i < totalAdvertise.Count; i++)
                        Assert.AreEqual(true, totalAdvertise[i].Text.Contains(filterValue), "'" + filterValue + "' Filter Value not Present on '" + totalAdvertise[i].Text + "' Record");

                    Results.WriteStatus(test, "Pass", "Verified filter Value on Coop Advertisers Section.");
                }
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Verify tooltip for each records for Coop Advertiser section
        /// </summary>
        /// <returns></returns>
        public Search verifyTooltipForEachRecordsForCoopAdvertisersSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"), "Coop Advertisers' Records not Present.");

            IList<IWebElement> totalAdvertise = driver.FindElements(By.XPath("//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div/label/span"));
            for (int i = 0; i < totalAdvertise.Count; i++)
            {
                driver.MouseHoverUsingElement("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div[" + (i + 1) + "]/label/span");
                Thread.Sleep(500);
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='tooltip-inner']"), "Tooltip not present.");
                Assert.AreEqual(true, driver._getText("xpath", "//div[@class='tooltip-inner']").Contains(totalAdvertise[i].Text), "'" + totalAdvertise[i].Text + "' Record tooltip not match.");
            }

            Results.WriteStatus(test, "Pass", "Verified Tooltip for each record for Coop Advertisers Section.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Exclude Button after click on it for Coop Advertisers
        /// </summary>
        /// <param name="clearExclude">Verify Clear Exclude</param>
        /// <returns></returns>
        public Search verifyExcludeButtonAfterClickOnItForCoopAdvertisers(bool clearExclude = false)
        {
            if (clearExclude)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'deselectAll')]"), "'Clear Excluded' Button Title not change.");
                Assert.AreEqual(true, driver._getText("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'deselectAll')]").Contains("Clear Excluded"), "'Clear Excluded' Button Title not change.");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'Excluded')]"), "Exclude Button not Present.");
                Assert.AreEqual(true, driver._getText("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//button[contains(@ng-click,'Excluded')]").Contains("Excluding"), "'Excluding' Title not change.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Exclude button titles on Search screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Browse section for Coop Advertisers
        /// </summary>
        /// <returns></returns>
        public Search verifyBrowseSectionForCoopAdvertisers()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'input-group-btn-first')]"), "Browse Filter tabs not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]"), "Coop Advertisers' Records not Present.");

            IList<IWebElement> browseIcons = driver.FindElements(By.XPath("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'input-group-btn-first')]/a"));
            string[] browseNames = { "#", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            for (int i = 0; i < browseNames.Length; i++)
                Assert.AreEqual(browseNames[i], browseIcons[i].Text, "'" + browseIcons[i].Text + "' tab not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//a[contains(@ng-click,'IsExcluded')]"), "Exclude not present for coop advertisers section.");
            Results.WriteStatus(test, "Pass", "Verified, Browse Section for Coop Advertisers.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Select any Character from filter and verify Records
        /// </summary>
        /// <returns></returns>
        public Search selectAnyCharacterFromFilterAndVerifyRecords()
        {
            IList<IWebElement> browseIcons = driver.FindElements(By.XPath("//div[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'input-group-btn-first')]/a"));
            Random rand = new Random();
            int x = rand.Next(1, browseIcons.Count - 1);
            string selectedChar = browseIcons[x].Text;
            browseIcons[x].Click();
            Results.WriteStatus(test, "Pass", "Selected, '" + selectedChar + "' Character from Filter section.");
            Thread.Sleep(1000);

            if (driver._isElementPresent("xpath", "//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div/span[contains(text(),'No Rows To Show')]") == true)
                Results.WriteStatus(test, "Pass", "'No Rows To Show' for Coop Advertisers Section.");
            else
            {
                IList<IWebElement> totalRecords = driver.FindElements(By.XPath("//*[@id='field-brand-n_occurrence_coopAdvertiserNames']//.//div[contains(@class,'search-list-group-field-child')]/div/div"));
                for (int i = 0; i < totalRecords.Count; i++)
                    Assert.AreEqual(true, totalRecords[i].Text.Substring(0, 1).Equals(selectedChar), "'" + totalRecords[i].Text + "' Record not start with '" + selectedChar + "'.");

                Results.WriteStatus(test, "Pass", "Verified, Records with Selected Character from Filter section.");
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Keyword section on search screen
        /// </summary>
        /// <returns></returns>
        public Search verifyKeywordSectionOnSearchScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-keyword-search-my-search//.//div[@class='panel-summary-header']"), "Keyword Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-keyword-search-my-search//.//div[@class='panel-summary-header']"), "Keyword Header Section not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-keyword-search-my-search//.//div[@class='input-group input-no-btn-group']"), "Input area not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-keyword-search-my-search//.//input[contains(@placeholder,'Filter Text')]"), "Filter Text area not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-keyword-search-my-search//.//div[contains(@class,'CFT-search-list-group-field-child')]"), "Radio button section not present.");
            IList<IWebElement> radioCollections = driver.FindElements(By.XPath("//cft-field-editor-keyword-search-my-search//.//div[contains(@class,'CFT-search-list-group-field-child')]/div/div"));
            string[] radioButtonTitles = { "All Fields", "Headline", "Lead Text", "Visual", "Description" };
            for (int i = 0; i < radioCollections.Count; i++)
                Assert.AreEqual(true, radioCollections[i].Text.Contains(radioButtonTitles[i]), "'" + radioButtonTitles[i] + "' Radio Button not present.");

            Results.WriteStatus(test, "Pass", "Verified, Keyword section on search screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Enter Keyword in Search area on screen
        /// </summary>
        /// <param name="filterValue">Filter Value to enter</param>
        /// <returns></returns>
        public String enterKeywordInSearchAreaOnScreen(string filterValue)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-keyword-search-my-search//.//input[contains(@placeholder,'Filter Text')]"), "Filter Text area not present.");
            string searchValue = "";

            if (filterValue.Equals("Existing"))
                searchValue = "test";

            if (filterValue.Equals("Random"))
                searchValue = driver._randomString(6) + driver._randomString(4, true);

            driver._type("xpath", "//cft-field-editor-keyword-search-my-search//.//input[contains(@placeholder,'Filter Text')]", searchValue);
            Thread.Sleep(4000);
            Results.WriteStatus(test, "Pass", "Entered, '" + searchValue + "' Filter Text for Keyword section on screen.");
            return searchValue;
        }

        /// <summary>
        /// Enter Keyword in Search area and verify chart Value
        /// </summary>
        /// <returns></returns>
        public String enterKeywordInSearchAreaAndVerifyChartValue()
        {
            string recordCollection = "";
            string searchKeyword = "test";
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-keyword-search-my-search//.//input[contains(@placeholder,'Filter Text')]"), "Filter Text area not present.");
            driver._type("xpath", "//cft-field-editor-keyword-search-my-search//.//input[contains(@placeholder,'Filter Text')]", searchKeyword);
            Thread.Sleep(3000);

            for (int i = 0; i < 5; i++)
            {
                if (driver._getText("xpath", "//*[@id='CftSearchSummary']/div[1]").Contains("No Data Found") == true)
                {
                    recordCollection = driver._randomString(2);
                    driver._type("xpath", "//cft-field-editor-keyword-search-my-search//.//input[contains(@placeholder,'Filter Text')]", searchKeyword);
                    Thread.Sleep(3000);
                }
                else
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[1]//.//*[name()='text' and @class='highcharts-title']"), "Chart Record Collection not present.");
                    recordCollection = driver._getText("xpath", "//*[@id='CftSearchSummary']/div[1]//.//*[name()='text' and @class='highcharts-title']").Trim().Replace("\r\n", "").Replace(",", "");
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Entered, '" + searchKeyword + "' Keyword and Verified Record collection in chart is '" + recordCollection + "'.");
            return recordCollection;
        }

        /// <summary>
        /// Verify Reset Chages Message on screen
        /// </summary>
        /// <param name="resetChanges">Click on Reset Change button</param>
        /// <returns></returns>
        public Search verifyResetChangesMessageOnScreen(bool resetChanges)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//label[@class='field-title']", 20), "Reset Message not present.");
            Results.WriteStatus(test, "Pass", "Verified, Reset Changes message on screen.");

            if (resetChanges)
            {
                if (driver._isElementPresent("xpath", "//button[@class='btn btn-primary' and contains(text(),'Clear Search')]"))
                    driver._clickByJavaScriptExecutor("//button[@class='btn btn-primary' and contains(text(),'Clear Search')]");
                else
                    clickButtonOnSearchScreen("Reset Changes");
            }
            else
                clickButtonOnSearchScreen("Cancel");

            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Summary Details after keyword search
        /// </summary>
        /// <param name="radioOption">Radio option of selected</param>
        /// <param name="searchKeyword">Search Keyword to verify</param>
        /// <returns></returns>
        public Search verifySummaryDetailsAfterKeywordSearch(string radioOption, string searchKeyword, string fieldName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div"), "Summary Details section not present.");
            IList<IWebElement> detailCollections = driver.FindElements(By.XPath("//*[@id='CftSearchSummary']/div"));
            for (int i = 1; i < detailCollections.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-header')]") == true)
                    if (driver._getText("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-header')]").Contains(fieldName) == true)
                    {
                        if (radioOption.Equals(""))
                            Assert.AreEqual(true, driver._getText("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-item')]//.//p").Contains(searchKeyword), "'" + searchKeyword + "' not present in Summary Details.");
                        else
                            Assert.AreEqual(true, driver._getText("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-item')]//.//p").Contains(searchKeyword + " in " + radioOption), "'" + searchKeyword + " in " + radioOption + "' not present in Summary Details.");
                        break;
                    }
            }

            Results.WriteStatus(test, "Pass", "Verified, Summary Details after keyword search for '" + fieldName + "' Field.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Fields Refresh icon disable on summart details section
        /// </summary>
        /// <returns></returns>
        public Search verifyFieldsRefreshIconDisableOnSummaryDetailSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div"), "Summary Details section not present.");
            string headerName = "";
            IList<IWebElement> detailCollections = driver.FindElements(By.XPath("//*[@id='CftSearchSummary']/div"));
            for (int i = 1; i < detailCollections.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-header')]") == true)
                {
                    headerName = driver._getText("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-header')]");
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-header')]//.//div[@class='notice-action']/i[@class='fa fa-refresh disabled']"), "'" + headerName + "' Field Refresh icon not disable.");
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Fields Refresh icon disable on Summary Details section.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Select Radio option from keyword section
        /// </summary>
        /// <param name="radioOption">Radio optio to Select</param>
        /// <returns></returns>
        public String selectRadioOptionFormKeywordSection(string radioOption)
        {
            string selectedRaioOption = "";
            IList<IWebElement> radioCollections = driver.FindElements(By.XPath("//cft-field-editor-keyword-search-my-search//.//div[contains(@class,'CFT-search-list-group-field-child')]/div/div"));

            if (radioOption.Equals("Random"))
            {
                Random rand = new Random();
                int x = rand.Next(0, radioCollections.Count);
                IList<IWebElement> cells = radioCollections[x]._findElementsWithinElement("xpath", ".//label/span");
                selectedRaioOption = radioCollections[x].Text;
                cells[0].Click();
                Thread.Sleep(3000);
            }
            else
            {
                for (int i = 0; i < radioCollections.Count; i++)
                {
                    if (radioCollections[i].Text.Contains(radioOption))
                    {
                        selectedRaioOption = radioOption;
                        IList<IWebElement> cells = radioCollections[i]._findElementsWithinElement("xpath", ".//label/span");
                        cells[0].Click();
                        Thread.Sleep(3000);
                        break;
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, '" + selectedRaioOption + "' Radio Option from keyword section.");
            return selectedRaioOption;
        }

        /// <summary>
        /// Click Refresh icon and verify message for field section
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        public Search clickRefreshIconAndVerifyMessageForFieldSection(string fieldName, string defaultSelectedValue = "")
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div"), "Summary Details section not present.");
            IList<IWebElement> detailCollections = driver.FindElements(By.XPath("//*[@id='CftSearchSummary']/div"));
            for (int i = 1; i < detailCollections.Count; i++)
            {
                if (driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-header')]") == true)
                    if (driver._getText("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-header')]").Contains(fieldName) == true)
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-item')]//.//div[@class='notice-action']/i"));
                        driver._clickByJavaScriptExecutor("//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-item')]//.//div[@class='notice-action']/i");
                        Thread.Sleep(3000);
                        if (defaultSelectedValue == "")
                            Assert.AreEqual(true, driver._getText("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-item')]//.//p").Contains("No search set!"), "'No search set!' not present in Summary Details.");
                        else
                            Assert.AreEqual(true, driver._getText("xpath", "//*[@id='CftSearchSummary']/div[" + (i + 1) + "]//.//div[contains(@class,'search-summary-item')]//.//p").Contains(defaultSelectedValue), "'" + defaultSelectedValue + "' not present in Summary Details.");
                        break;
                    }
            }

            Results.WriteStatus(test, "Pass", "Clicked, Refresh Icon for '" + fieldName + "' Field and verified message for keyword section.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify No Data Found Message on Chart
        /// </summary>
        /// <returns></returns>
        public Search verifyNoDataFoundMessageOnChart()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[1]"), "Summary Details section not present.");
            Assert.AreEqual(true, driver._getText("xpath", "//*[@id='CftSearchSummary']/div[1]").Contains("No Data Found"), "'No Data Found' message on Chart not present.");
            Results.WriteStatus(test, "Pass", "Verified, No Data Found message on chart.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Number of Records Collections on Grid
        /// </summary>
        /// <param name="records">Records of Chart</param>
        /// <returns></returns>
        public Search verifyNumberOfRecordCollectionsOnGrid(string records)
        {
            selectNoOfRecordsOnPageFromGrid("10");
            string lastPage = "1";
            int totalRecords = 0;
            if (driver._isElementPresent("xpath", "//div[@class='CFT-view-actions-wrapper']//.//li[@class='pagination-last page-item']/a") == true)
            {
                driver._clickByJavaScriptExecutor("//div[@class='CFT-view-actions-wrapper']//.//li[@class='pagination-last page-item']/a");
                Thread.Sleep(4000);
            }
            lastPage = driver._getText("xpath", "//div[@class='CFT-view-actions-wrapper']//.//li[@class='pagination-page page-item active']");
            totalRecords = (Convert.ToInt32(lastPage) - 1) * 10;

            IList<IWebElement> recordsOnPage = driver.FindElements(By.XPath("//div[@class='CFT-view-actions-wrapper']//.//div[@class='ag-body-container']/div"));
            totalRecords = totalRecords + recordsOnPage.Count;
            Assert.AreEqual(Convert.ToInt32(records), totalRecords, "'" + records + "' Records on Grid not match");

            Results.WriteStatus(test, "Pass", "Verified, Number of Records on Grid not match with Chart Records.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Select No of Records on page from Grid
        /// </summary>
        /// <param name="pageNo">Record per Page</param>
        /// <returns></returns>
        public Search selectNoOfRecordsOnPageFromGrid(string pageNo)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='CFT-view-actions-wrapper']//.//div[@class='btn-group btn-grid-counts pull-right']/button", 20), "'Records per Page' list not present.");
            driver.MouseHoverUsingElement("xpath", "//div[@class='CFT-view-actions-wrapper']//.//div[@class='btn-group btn-grid-counts pull-right']/button");
            IList<IWebElement> noOfPages = driver.FindElements(By.XPath("//div[@class='CFT-view-actions-wrapper']//.//div[@class='btn-group btn-grid-counts pull-right']/button"));
            for (int i = 0; i < noOfPages.Count; i++)
            {
                if (noOfPages[i].Text.Contains(pageNo) == true)
                {
                    driver.MouseHoverByJavaScript(noOfPages[i]);
                    driver._clickByJavaScriptExecutor("//div[@class='CFT-view-actions-wrapper']//.//div[@class='btn-group btn-grid-counts pull-right']/button[" + (i + 1) + "]");
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, '" + pageNo + "' Records per page from Grid.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Search tab Button on saved searches screen
        /// </summary>
        /// <param name="buttonName">Button name to verify</param>
        /// <param name="disabled">Verify Button disable or not</param>
        /// <returns></returns>
        public Search verifySearchTabButtonOnSavedSearchesScreen(string buttonName, bool disabled)
        {
            IList<IWebElement> buttons = driver.FindElements(By.XPath("//div[@class='CFT-search-list-group-field']//.//button"));
            bool avail = false;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Text.Contains(buttonName))
                {
                    if (disabled)
                        Assert.AreEqual("true", buttons[i].GetAttribute("disabled"), "'" + buttons[i].Text + "' Search tab Button not disabled.");
                    else
                        Assert.AreEqual(null, buttons[i].GetAttribute("disabled"), "'" + buttons[i].Text + "' Search tab Button disabled.");

                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + buttonName + "' Search tab button not present.");
            Results.WriteStatus(test, "Pass", "Verified '" + buttonName + "' Search tab on Saved Searches screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Click Button for Saved Searches Card on screen
        /// </summary>
        /// <param name="savedSearchedName">Saved Searches Name on Screen</param>
        /// <param name="buttonName">Button Name for Click</param>
        /// <returns></returns>
        public String clickButtonForSavedSearchCardOnScreen(string savedSearchedName, string buttonName)
        {
            string searchTitle = "";
            IList<IWebElement> savedSearches = driver.FindElements(By.XPath("//cft-saved-search-list-item"));

            if (savedSearchedName.Equals("Random"))
            {
                Random rand = new Random();
                int x = rand.Next(0, savedSearches.Count);

                Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                searchTitle = savedSearches[x].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;

                IList<IWebElement> buttonLists = savedSearches[x]._findElementsWithinElement("xpath", ".//button[@class='btn btn-default btn-block custom-btn-default']");
                for (int b = 0; b < buttonLists.Count; b++)
                    if (buttonLists[b].Text.Contains(buttonName))
                    {
                        buttonLists[b].Click();
                        Thread.Sleep(3000);
                        driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");
                        break;
                    }

                Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button for '" + searchTitle + "' Saved Search.");
            }
            else
            {
                bool avail = false;
                for (int l = 0; l < savedSearches.Count; l++)
                {
                    Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                    searchTitle = savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;

                    if (searchTitle.Contains(savedSearchedName))
                    {
                        IList<IWebElement> buttonLists = savedSearches[l]._findElementsWithinElement("xpath", ".//button[@class='btn btn-default btn-block custom-btn-default']");
                        for (int b = 0; b < buttonLists.Count; b++)
                            if (buttonLists[b].Text.Contains(buttonName))
                            {
                                buttonLists[b].Click();
                                Thread.Sleep(3000);
                                avail = true;
                                driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading Pivot Data')]");
                                break;
                            }
                    }
                    if (avail)
                        break;
                }

                Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button for '" + searchTitle + "' Saved Search.");
            }

            return searchTitle;
        }

        /// <summary>
        /// Click Delete button for Saved Search Record from List and Verify Message
        /// </summary>
        /// <param name="simpleSavedRecord">Click Simple Saved Record or Scheduled Saved Record</param>
        /// <param name="okay">Click Okay option for Record or Cancel</param>
        /// <returns></returns>
        public String clickDeleteButtonForSavedSearchRecordFromListAndVerifyMessage(bool simpleSavedRecord = false, bool okay = true)
        {
            IList<IWebElement> savedSearches = driver.FindElements(By.XPath("//cft-saved-search-list-item"));
            string searchTitle = "";
            bool availCount = false;
            bool avail = false;
            for (int l = 0; l < savedSearches.Count; l++)
            {
                availCount = (savedSearches[l].FindElements(By.XPath(".//button[contains(@class,'nested-btn-default btn')]")).Count == 0);
                if (availCount == simpleSavedRecord)
                {
                    Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                    searchTitle = savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;

                    Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//button[@class='btn btn-default btn-block custom-btn-default' and contains(text(),'Delete')]")).Displayed, "'Delete' Button not present.");
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[l].FindElement(By.XPath(".//button[@class='btn btn-default btn-block custom-btn-default' and contains(text(),'Delete')]")));
                    Thread.Sleep(3000);
                    Results.WriteStatus(test, "Pass", "Clicked, Delete button for '" + searchTitle + "' Saved Search Record.");

                    if (simpleSavedRecord == true)
                    {
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/span[@class='inline-form-message']")).Displayed, "Content not presnt to 'Delete' Record.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/span[@class='inline-form-message']")).Text.Contains("Are you sure you want to delete this search?"), "'Are you sure you want to delete this search? ' Message not match.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/button[contains(@class,'btn btn-default') and contains(text(),'Cancel')]")).Displayed, "'Cancel' Button not present for Saved Record.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/button[contains(@class,'btn btn-primary') and contains(text(),'Okay')]")).Displayed, "'Okay' Button not present for Saved Record.");

                        if (okay)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/button[contains(@class,'btn btn-primary') and contains(text(),'Okay')]")));
                            Results.WriteStatus(test, "Pass", "Verfied, Message for Delete Saved Record and Clicked 'Okay' option for Record.");
                        }
                        else
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/button[contains(@class,'btn btn-default') and contains(text(),'Cancel')]")));
                            Results.WriteStatus(test, "Pass", "Verfied, Message for Delete Saved Record and Clicked 'Cancel' option for Record.");
                        }
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/span[@class='inline-form-message']")).Displayed, "Content not presnt to 'Delete' Record.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/button[contains(@class,'btn btn-default') and contains(text(),'Okay')]")).Displayed, "'Okay' Button not present for Saved Record.");
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[l].FindElement(By.XPath(".//div[@class='input-group']/button[contains(@class,'btn btn-default') and contains(text(),'Okay')]")));
                        Thread.Sleep(1000);
                        Results.WriteStatus(test, "Pass", "Verfied, Message for Delete Saved Record and Clicked 'Okay' option for Record.");
                    }

                    avail = true;
                    break;
                }
            }
            Assert.AreEqual(true, avail, "Record not present to Delete Record from list.");
            return searchTitle;
        }

        /// <summary>
        /// Get Save
        /// </summary>
        /// <param name="savedSearchedName"></param>
        /// <param name="buttonName"></param>
        /// <param name="enterNewAndSave"></param>
        /// <returns></returns>
        public Search getSavedSearchNameOrClickForSavedSearchRecordOnScreen(string savedSearchedName, string buttonName, bool enterNewAndSave = false)
        {
            string searchTitle = "";
            string newSearcheName = "Test" + driver._randomString(4, true);
            IList<IWebElement> savedSearches = driver.FindElements(By.XPath("//cft-saved-search-list-item"));

            if (savedSearchedName.Equals("Random"))
            {
                Random rand = new Random();
                int x = rand.Next(0, savedSearches.Count);

                Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                searchTitle = savedSearches[x].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[x].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")));

                Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]")).Displayed, "Edit Section for Name not present.");
                Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/input")).GetAttribute("value").Contains(searchTitle), "'" + searchTitle + "' Name not present.");
                Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'Cancel')]")).Displayed, "'Cancel Button not Present for '" + searchTitle + "' Name.");
                Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'Clear')]")).Displayed, "'Clear Button not Present for '" + searchTitle + "' Name.");
                Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'Save')]")).Displayed, "'Save Button not Present for '" + searchTitle + "' Name.");

                if (enterNewAndSave)
                {
                    IWebElement ele = savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/input"));
                    ele.Clear();
                    ele.SendKeys(newSearcheName);
                    Results.WriteStatus(test, "Pass", "Entered, '" + newSearcheName + "' Title Name on Saved Search Input area.");
                }

                if (buttonName != "")
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'" + buttonName + "')]")));
                    Thread.Sleep(3000);
                    Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button for Saved Search Record.");
                }

                if (buttonName.Contains("Save"))
                {
                    Assert.AreEqual(newSearcheName, savedSearches[x].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text, "'" + newSearcheName + "' Saved Search Name not Changed.");
                    Results.WriteStatus(test, "Pass", "Verified, New Saved Search Name on Screen.");
                }
                else
                    if (buttonName.Contains("Cancel"))
                    {
                        Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                        Results.WriteStatus(test, "Pass", "Verified, Section after Clicking Cancel Button for Search Name.");
                    }
                    else
                        if (buttonName.Contains("Clear"))
                        {
                            Assert.AreEqual(true, savedSearches[x].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/input")).GetAttribute("value").Contains(""), "'" + searchTitle + "' Name not not clear.");
                            Results.WriteStatus(test, "Pass", "Verified, Input Area clear for Saved Search Name.");
                        }

            }
            else
            {
                for (int l = 0; l < savedSearches.Count; l++)
                {
                    Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                    searchTitle = savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;

                    if (searchTitle.Contains(savedSearchedName))
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")));

                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath("//cft-saved-search-list-item//.//div[contains(@class,'inline-form-message')]")).Displayed, "Edit Section for Name not present.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/input")).GetAttribute("value").Contains(searchTitle), "'" + searchTitle + "' Name not present.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'Cancel')]")).Displayed, "'Cancel Button not Present for '" + searchTitle + "' Name.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'Clear')]")).Displayed, "'Clear Button not Present for '" + searchTitle + "' Name.");
                        Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'Save')]")).Displayed, "'Save Button not Present for '" + searchTitle + "' Name.");

                        if (enterNewAndSave)
                        {
                            IWebElement ele = savedSearches[l].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/input"));
                            ele.Clear();
                            ele.SendKeys(newSearcheName);
                            Results.WriteStatus(test, "Pass", "Entered, '" + newSearcheName + "' Title Name on Saved Search Input area.");
                        }

                        if (buttonName != "")
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", savedSearches[l].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/button[contains(text(),'" + buttonName + "')]")));
                            Thread.Sleep(3000);
                            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button for Saved Search Record.");
                        }

                        if (buttonName.Contains("Save"))
                        {
                            Assert.AreEqual(newSearcheName, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text, "'" + newSearcheName + "' Saved Search Name not Changed.");
                            Results.WriteStatus(test, "Pass", "Verified, New Saved Search Name on Screen.");
                        }
                        else
                            if (buttonName.Contains("Cancel"))
                            {
                                Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                                Results.WriteStatus(test, "Pass", "Verified, Section after Clicking Cancel Button for Search Name.");
                            }
                            else
                                if (buttonName.Contains("Clear"))
                                {
                                    Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[contains(@class,'inline-form-message')]/input")).GetAttribute("value").Contains(""), "'" + searchTitle + "' Name not not clear.");
                                    Results.WriteStatus(test, "Pass", "Verified, Input Area clear for Saved Search Name.");
                                }

                        break;
                    }
                }
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Saved Search Name from list
        /// </summary>
        /// <param name="savedSearchName">Saved Search Name to verify</param>
        /// <returns></returns>
        public Search verifySavedSearchNameFromList(string savedSearchName)
        {
            IList<IWebElement> savedSearches = driver.FindElements(By.XPath("//cft-saved-search-list-item"));
            bool avail = false;
            for (int l = 0; l < savedSearches.Count; l++)
            {
                Assert.AreEqual(true, savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Displayed, "Title of Saved Search not Present.");
                string searchTitle = savedSearches[l].FindElement(By.XPath(".//div[@class='checkbox-header-lead-text cursor-pointer']")).Text;

                if (searchTitle.Contains(savedSearchName))
                {
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + savedSearchName + "' Saved Search Record not Present.");
            Results.WriteStatus(test, "Pass", "Verified, '" + savedSearchName + "' Saved Search Record from List.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Save As Section after clicking on Save As button
        /// </summary>
        /// <returns></returns>
        public Search verifySaveAsSectionAfterClickingOnSaveAsButton()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//input[contains(@placeholder,'What would you like to call your search') and @type='text']", 20), "'What would you like to call your search?' textarea not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'Make Default')]"), "'Make Default' Checkbox Label not present.");
            Assert.AreEqual("rgba(119, 119, 119, 1)", driver.FindElement(By.XPath("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'Make Default')]")).GetCssValue("color"), "'Make Default' Button is not UnChecked.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'Continue Editing')]"), "'Continue Editing' Checkbox Label not present.");
            Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'Continue Editing')]")).GetCssValue("color"), "'Continue Editing' Button not Checked and not Highlighted with Blue color.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Cancel')]"), "'Cancel' Button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-success' and contains(text(),'Save!') and @disabled]"), "'Save!' Button in Disable manner not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Save As section after clicking on Save As button.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Check or UnChecked Checkbox for Saved Search
        /// </summary>
        /// <param name="checkboxName">Checkbox Name to perform action</param>
        /// <param name="unChecked">UnChecked Checkbox</param>
        /// <returns></returns>
        public Search checkOrUnCheckCheckboxForSavedSearch(string checkboxName, bool unChecked)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'" + checkboxName + "')]"), "'" + checkboxName + "' Checkbox Label not present.");

            if (unChecked)
            {
                if (driver.FindElement(By.XPath("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'" + checkboxName + "')]")).GetCssValue("color").Contains("0, 74, 82") == true)
                {
                    driver._clickByJavaScriptExecutor("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'" + checkboxName + "')]");
                    Results.WriteStatus(test, "Pass", "UnChecked, '" + checkboxName + "' Checkbox for Saved Search.");
                }
                else
                    Results.WriteStatus(test, "Pass", "'" + checkboxName + "' Checkbox Already Unchecked for Saved Search.");

                Assert.AreEqual("rgba(119, 119, 119, 1)", driver.FindElement(By.XPath("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'" + checkboxName + "')]")).GetCssValue("color"), "'" + checkboxName + "' Button not UnChecked.");
            }
            else
            {
                if (driver.FindElement(By.XPath("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'" + checkboxName + "')]")).GetCssValue("color").Contains("0, 74, 82") == false)
                {
                    driver._clickByJavaScriptExecutor("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'" + checkboxName + "')]");
                    Results.WriteStatus(test, "Pass", "Checked, '" + checkboxName + "' Checkbox for Saved Search.");
                }
                else
                    Results.WriteStatus(test, "Pass", "'" + checkboxName + "' Checkbox Already Checked for Saved Search.");

                Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//label[@class='field-checkbox']/span[@class='text-muted' and contains(text(),'" + checkboxName + "')]")).GetCssValue("color"), "'" + checkboxName + "' Button not Checked.");
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Applied Search field in chart details section
        /// </summary>
        /// <param name="savedSearchName">Saved Search Name to verify</param>
        /// <returns></returns>
        public Search verifyAppliedSearchFieldInChartDetailsSection(string savedSearchName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-applied-my-search-summary/div"), "'Applied Search' section not present on Detail section.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-applied-my-search-summary/div//.//div[contains(@class,'summary-header')]"), "'Applied Search' Header not present.");

            if (savedSearchName.Equals("None Selected") == false)
                Assert.AreEqual(false, driver._getText("xpath", "//cft-applied-my-search-summary/div[@class='search-summary-item']").Contains("None Selected"), "Applied Search not Applied for any Search.");
            Assert.AreEqual(true, driver._getText("xpath", "//cft-applied-my-search-summary/div[@class='search-summary-item']").Contains(savedSearchName), "'" + savedSearchName + " Saved Search not Applied.");

            Results.WriteStatus(test, "Pass", "Verified, Applied Search field in Chart Details Section.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Pagination Panel of Saved Searched
        /// </summary>
        /// <returns></returns>
        public Search verifyPaginationPanelOfSavedSearched()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']"), "Pagination Panel not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-prev page-item disabled']"), "Previous Icon Default not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-page page-item active']"), "First Page not Active.");
            if (driver._isElementPresent("xpath", "//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-next page-item']") == false)
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-next page-item disabled']"), "Next Icon not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Pagination Panel of Saved Searched.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Click on Button from Pagination Panel
        /// </summary>
        /// <param name="pagePosition">Page Position to Click</param>
        /// <returns></returns>
        public Search clickOnButtonFromPaginationPanel(string pagePosition)
        {
            if (driver._isElementPresent("xpath", "//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-" + pagePosition + " page-item']/a") == true)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-" + pagePosition + " page-item']/a"), "'" + pagePosition.ToUpper() + "' Icon not Enable.");
                driver._clickByJavaScriptExecutor("//cft-saved-search-list//.//div[@class='row cft-pagination-wrapper']//.//li[@class='pagination-" + pagePosition + " page-item']/a");
                Results.WriteStatus(test, "Pass", "Clicked, '" + pagePosition.ToUpper() + "' Icon Button from Pagination Panel.");
            }
            else
                Results.WriteStatus(test, "Pass", "Alerady on '" + pagePosition.ToUpper() + "' Page.");

            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Overwrite section with message on screen
        /// </summary>
        /// <param name="overWrite">Click Overwrite Button on screen</param>
        /// <returns></returns>
        public Search verifyOverwriteSectionWithMessageOnScreen(bool overWrite)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//label[@class='field-title']", 20), "Reset Message not present.");
            Assert.AreEqual("Great we see that you are trying to overwrite the current applied search. Are you sure you want to overwrite?", driver._getText("xpath", "//label[@class='field-title']"), "Overwrite Message not match.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Cancel')]"), "'Cancel' Button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Overwrite')]"), "'Overwrite' Button not Present.");
            Results.WriteStatus(test, "Pass", "Verified, Reset Changes message on screen.");

            if (overWrite)
                clickButtonOnSearchScreen("Overwrite");
            else
                clickButtonOnSearchScreen("Cancel");

            return new Search(driver, test);
        }

        #region Ad Code

        /// <summary>
        /// Verify Ad Code Section on screen
        /// </summary>
        /// <returns></returns>
        public Search verifyAdCodeSectionOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-adcode-search-my-search//.//div[@class='panel-summary-header']"), "Ad Code Section not present.");
            Assert.AreEqual(true, driver._getText("xpath", "//cft-field-editor-adcode-search-my-search//.//div[@class='panel-summary-header']").Contains("Ad Code"), "Ad Code Header not match.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-adcode-search-my-search//.//div[@class='CFT-textbox']"), "Enter adcodes... not present.");
            Assert.AreEqual(true, driver._getAttributeValue("xpath", "//cft-field-editor-adcode-search-my-search//.//div[@class='CFT-textbox']/textarea", "placeholder").Contains("Enter adcodes"), "Enter adcodes... Placeholder not present.");

            Results.WriteStatus(test, "Pass", "Verified, Ad Code section on search screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Enter Ad Code in Ad Code search area on screen
        /// </summary>
        /// <param name="adCode">Ad Code to Search</param>
        /// <returns></returns>
        public Search enterAdCodeInAdCodeSearchAreaOnScreen(string adCode)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-adcode-search-my-search//.//div[@class='CFT-textbox']/textarea"), "Ad Code Search area not present.");
            driver._type("xpath", "//cft-field-editor-adcode-search-my-search//.//div[@class='CFT-textbox']/textarea", adCode);
            Thread.Sleep(3000);
            Results.WriteStatus(test, "Pass", "Entered, '" + adCode + "' Ad Code on Search area on screen.");
            return new Search(driver, test);
        }

        /// <summary>
        /// Verify Chart Record value on Search screen
        /// </summary>
        /// <returns></returns>
        public String verifyChartRecordValueOnsearchScreen()
        {
            string recordCollection = "";
            if (driver._getText("xpath", "//*[@id='CftSearchSummary']/div[1]").Contains("No Data Found") == true)
            {
                recordCollection = "No items found!";
                Results.WriteStatus(test, "Pass", "'No Data Found' for Search Record");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='CftSearchSummary']/div[1]//.//*[name()='text' and @class='highcharts-title']"), "Chart Record Collection not present.");
                recordCollection = driver._getText("xpath", "//*[@id='CftSearchSummary']/div[1]//.//*[name()='text' and @class='highcharts-title']").Trim().Replace("\r\n", "").Replace(",", "");
                Results.WriteStatus(test, "Pass", "Verified, Chart Record Value on Search screen.");
            }

            return recordCollection;
        }

        /// <summary>
        /// Verify Grid Records on screen
        /// </summary>
        /// <param name="chartValue">Chart Record Value</param>
        /// <returns></returns>
        public Search verifyGridRecordsOnScreen(string chartValue)
        {
            if (chartValue.Equals("No items found!"))
            {
                Assert.AreEqual(false, driver._isElementPresent("xpath", "//div[@class='CFT-view-actions-wrapper']//.//div[@class='ag-body-container']/div"), "'No items found!' for Grid not found.");
                Results.WriteStatus(test, "Pass", "Verified, No items found! message for Grid.");
            }
            else
                verifyNumberOfRecordCollectionsOnGrid(chartValue);

            return new Search(driver, test);
        }

        #endregion

        #region Summary By Category

        /// <summary>
        /// Select Media checkbox option for Annual Summary
        /// </summary>
        /// <param name="mediaName">Media name to Select</param>
        /// <returns></returns>
        public Search selectMediaCheckboxOptionForAnnualSummary(string mediaName = "Random")
        {
            IList<IWebElement> mediaCollections = driver.FindElements(By.XPath("//div[contains(@id,'media')]//.//div[@class='ag-body-viewport']/div/div"));
            bool avail = false;

            if (mediaName.Equals("Random"))
            {
                Random rand = new Random();
                int x = rand.Next(0, mediaCollections.Count);
                mediaName = mediaCollections[x].Text;
                driver._clickByJavaScriptExecutor("//div[contains(@id,'media')]//.//div[@class='ag-body-viewport']/div/div[" + (x + 1) + "]/div/span");
                Results.WriteStatus(test, "Pass", "Selected, '" + mediaName + "' Date Range from Section.");
            }
            else
            {
                for (int i = 0; i < mediaCollections.Count; i++)
                {
                    if (mediaCollections[i].Text.Contains(mediaName))
                    {
                        driver._clickByJavaScriptExecutor("//div[contains(@id,'media')]//.//div[@class='ag-body-viewport']/div/div[" + (i + 1) + "]");
                        avail = true; Thread.Sleep(2000);
                        break;
                    }
                }

                Assert.AreEqual(true, avail, "'" + mediaName + "' Media not Present.");
                Results.WriteStatus(test, "Pass", "Selected, '" + mediaName + "' Media from Section.");
            }

            return new Search(driver, test);
        }

        /// <summary>
        /// Create New Search or click saved search to Apply Search on screen
        /// </summary>
        /// <param name="applySavedSearch">Click on Apply Saved Search</param>
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
                verifyFieldMenuAndClickOnItOnSearchScreen("Media").selectMediaCheckboxOptionForAnnualSummary();
                clickButtonOnSearchScreen("Save As");

                Assert.IsTrue(driver._waitForElement("xpath", "//input[contains(@placeholder,'What would you like to call your search') and @type='text']", 20), "'What would you like to call your search?' textarea not Present.");
                driver._clickByJavaScriptExecutor("//input[contains(@placeholder,'What would you like to call your search') and @type='text']");
                scheduleSearchName = "Test" + driver._randomString(4, true);
                driver._type("xpath", "//input[contains(@placeholder,'What would you like to call your search') and @type='text']", scheduleSearchName);
                Results.WriteStatus(test, "Pass", "Entered Save As Search Report Name on Screen.");

                clickButtonOnSearchScreen("Save!");
                if (applySavedSearch)
                    clickButtonOnSearchScreen("Apply Search");
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

            Thread.Sleep(2000);
            return scheduleSearchName;
        }

        #endregion

        #endregion
    }
}
