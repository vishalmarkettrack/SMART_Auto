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
    public class PromoDashboard
    {
        #region Private Variables

        private IWebDriver promoDashboard;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public PromoDashboard(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.promoDashboard = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.promoDashboard; }
            set { this.promoDashboard = value; }
        }

        /// <summary>
        /// Verify Promo Dashboard Screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyPromoDashboardScreen()
        {
            string[] menuIcons = { "User", "Files", "Help", "Search" };
            verifyMenuIconOnTopOfScreen(menuIcons);

            verifyFilterBarSectionOnScreen(false);

            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]"))
                clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 6 Months");

            Assert.AreEqual(true, driver._isElementPresent("id", "domain-carousel"), "Carousel Section not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='carousel-filters']/label/span[text() = 'Circular Week']"), "'Circular Week' Radio option not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='carousel-filters']/label/span[text() = 'Number Of Stores']"), "'Number Of Stores' Radio option not Present.");

            Assert.AreEqual(true, driver._isElementPresent("id", "key-metrics-creative-carousel"), "Charts Not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("id", "viewCustomizerButton"), "'Field Options' Not Present on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='btn-group btn-grid-actions dropdown']/button/i"), "'Details View' Option Not Present on Screen.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[@class='btn-group btn-grid-actions dropdown']/button").Contains("Details View"), "'Details View' Option Label not match.");
            Assert.AreEqual(true, driver._isElementPresent("id", "cft-detail-view-2"), "Detail View Section not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Promo Dashboard Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Menu Icons on Top of Screen
        /// </summary>
        /// <param name="iconsName">Menu Icon Names to Verify</param>
        /// <returns></returns>
        public PromoDashboard verifyMenuIconOnTopOfScreen(string[] iconsName = null)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='baseexpand']"), "'Navigation Menu' Icon not Present on Page.");

            if (iconsName != null)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right menuItem']"), "'Menu Icons' not Present on top of Screen.");
                IList<IWebElement> menuCollections = driver._findElements("xpath", "//div[@class='pull-right menuItem']");
                foreach (IWebElement menus in menuCollections)
                {
                    Assert.AreEqual(iconsName[menuCollections.IndexOf(menus)], menus.Text, "'" + menus.Text + "' Menu Icon not Present on Top.");
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Menu Icons on Top of Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Filter Bar Section on Category Summary Screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyFilterBarSectionOnScreen(bool fields = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("id", "filter-menu"), "'Filter Bar' not Present on Screen.");

            //Assert.IsTrue(driver._waitForElement("xpath", "//li[@id='side-menu-button' and @class='filter-menu-prev-button disabled']", 20), "'Previous' Arrow Default not Disable.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul/li"), "Filter Fields not Present.");

            IList<IWebElement> fieldsCollection = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li"));
            string[] filterLabels = { "Last 7 Days", "All Retailers", "All Markets", "All Categories", "All Manufacturers", "All Brands", "All Offer Types", "All Page Locations" };
            string[] filterIDs = { "timeframe", "advertiser", "market", "category", "manufacturerName", "brand", "offerTypeName", "pageTypeName", "field-promo_adblock_eventName" };
            if (fields)
            {
                for (int i = 0; i < filterLabels.Length; i++)
                    Assert.AreEqual(true, fieldsCollection[i].GetAttribute("id").ToLower().Contains(filterIDs[i].ToLower()), "'" + filterLabels[i] + "' not found or match.");
            }

            //Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button']"), "'Next' Arrow not Enable.");
            //Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li[@class='disabled']"), "'Reset All' Button Default not Disable.");
            Results.WriteStatus(test, "Pass", "Verified, Filter Bar Section Category Summary on Screen.");

            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]"))
                clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 6 Months");

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify and Click Reset All Button on Filter Section
        /// </summary>
        /// <param name="clickButton">Click on Reset Button</param>
        /// <returns></returns>
        public PromoDashboard verifyAndClickResetAllButtonOnFilterSection(bool clickButton = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li/a[contains(text(),'Reset All')]"), "'Reset All' Button not Present.");

            if (clickButton)
            {
                Assert.AreEqual(false, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li[@class='disabled']"), "'Reset All' Button not Enable.");
                driver._click("xpath", "//div[@class='pull-right filter-reset-button']/ul/li/a[contains(text(),'Reset All')]");
                Thread.Sleep(500);
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li[@class='disabled']"), "'Reset All' Button not Disable.");
                Results.WriteStatus(test, "Pass", "Verified, 'Reset All' Button Enable and Clicked on Button.");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right filter-reset-button']/ul/li[@class='disabled']"), "'Reset All' Button Default not Disable.");
                Results.WriteStatus(test, "Pass", "Verified, 'Reset All' Button Default Disable on Filer section.");
            }

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click on Filter Field and Verify / Click on Options
        /// </summary>
        /// <param name="fieldName">Field Name to Click</param>
        /// <param name="optionName">Option Name for Click</param>
        /// <param name="options">Verify Options Collections</param>
        /// <returns></returns>
        public PromoDashboard clickOnFilterFieldAndVerifyOrClickOptions(string fieldName, string optionName = "", string[] options = null)
        {
            bool avail = false;
            int cnt = 0;

            if (fieldName.Equals("Days"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-field-editor-timeframe-calendar//.//a"));
                driver._clickByJavaScriptExecutor("//cft-field-editor-timeframe-calendar//.//a");
                Thread.Sleep(500);
                IList<IWebElement> optionsCollections = driver.FindElements(By.XPath("//cft-field-editor-timeframe-calendar//.//ul[contains(@class,'insert-ranges')]/li"));

                if (options != null)
                    for (int o = 0; o < options.Length; o++)
                    {
                        for (int j = 0; j < optionsCollections.Count; j++)
                            if (options[o] == optionsCollections[j].Text)
                                cnt++;
                    }

                if (optionName != "")
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
                avail = true;
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul/li"), "Filter Options not Present.");
                IList<IWebElement> fieldsCollection = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li"));

                for (int i = 0; i < fieldsCollection.Count; i++)
                {
                    if (fieldsCollection[i].Text.Contains(fieldName))
                    {
                        fieldsCollection[i].Click();
                        Thread.Sleep(200);
                        Assert.AreEqual("dropdown open", fieldsCollection[i].GetAttribute("class"), "'" + fieldName + "' Field not Open.");
                        IList<IWebElement> optionsCollections = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li[" + (i + 1) + "]//.//ul[contains(@class,'insert-ranges')]/li"));

                        if (options != null)
                            for (int o = 0; o < options.Length; o++)
                            {
                                for (int j = 0; j < optionsCollections.Count; j++)
                                    if (options[o] == optionsCollections[j].Text)
                                        cnt++;
                            }

                        if (optionName != "")
                            for (int j = 0; j < optionsCollections.Count; j++)
                            {
                                if (optionName == optionsCollections[j].Text)
                                {
                                    optionsCollections[j].Click();
                                    Thread.Sleep(500);
                                    driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20);
                                    break;
                                }
                            }

                        avail = true;
                        break;
                    }

                    if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button']/a"))
                        driver._clickByJavaScriptExecutor("//li[@id='side-menu-button' and @class='filter-menu-next-button']/a");
                }
            }

            Assert.AreEqual(true, avail, "'" + fieldName + "' Field not Present.");
            if (options != null)
                Assert.AreEqual(cnt, options.Length, "Options Colletions not match.");

            if (optionName == "")
                Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field and Verified Options.");
            else
                Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field and Verified options & Clicked '" + optionName + "' Option.");

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify From Date and To Date Picker on Screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyFromDateAndToDatePickerOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_day"), "'Day' tab button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_week"), "'Week' tab button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_month"), "'Month' tab button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_quarter"), "'Quarter' tab button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("id", "daterangepicker_year"), "'Year' tab button not Present.");

            driver.FindElement(By.XPath("//div[@class='calendar first left']")).Displayed.Equals(true);
            driver.FindElement(By.XPath("//div[@class='calendar second right']")).Displayed.Equals(true);

            Assert.True(driver._isElementPresent("xpath", "//div[@class='calendar first left']//.//td[@class='available active start-date']"), "Start Sat in From Date Picker not Present.");
            Assert.True(driver.FindElement(By.XPath("//div[@class='calendar first left']//.//td[@class='available active start-date']")).GetCssValue("background-color").Contains("rgba(0, 74, 82, 1)"), "'Navy Blue' color not match for Start Date in From Date picker.");

            Assert.True(driver._isElementPresent("xpath", "//div[@class='calendar second right']//.//td[@class='available active end-date']"), "End Sate in To Date Picker not Present.");
            Assert.True(driver.FindElement(By.XPath("//div[@class='calendar second right']//.//td[@class='available active end-date']")).GetCssValue("background-color").Contains("rgba(0, 74, 82, 1)"), "'Navy Blue' color not match for End Date in To Date picker.");

            Results.WriteStatus(test, "Pass", "Verified, 'From Date' and 'To Date' Picker on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify and Click icon on Filter Slider
        /// </summary>
        /// <param name="iconName">Icon Name for Click</param>
        /// <returns></returns>
        public PromoDashboard verifyAndClickIconOnFilterSlider(string iconName)
        {
            if (iconName.Equals("Next"))
            {
                if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button']/a") && (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button disabled']/a")) == false)
                    Results.WriteStatus(test, "Pass", "'Next' Icon not Present on Filter Slider.");
                else
                    if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button disabled']/a"))
                        Results.WriteStatus(test, "Pass", "'Next' Icon not Disable on Filter Slider.");
                    else
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button']/a"), "Next Icon not Present on Filter Slider.");
                        driver._clickByJavaScriptExecutor("//li[@id='side-menu-button' and @class='filter-menu-next-button']/a");
                        Results.WriteStatus(test, "Pass", "Verified & Clicked icon on Filter Slider.");
                    }
            }

            if (iconName.Equals("Previous"))
            {
                if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-prev-button']/a") && (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-prev-button disabled']/a")) == false)
                    Results.WriteStatus(test, "Pass", "'Previous' Icon not Present on Filter Slider.");
                else
                    if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-prev-button disabled']/a"))
                        Results.WriteStatus(test, "Pass", "'Next' Icon not Disable on Filter Slider.");
                    else
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-prev-button']/a"), "Previous Icon not Present on Filter Slider.");
                        driver._clickByJavaScriptExecutor("//li[@id='side-menu-button' and @class='filter-menu-prev-button']/a");
                        Results.WriteStatus(test, "Pass", "Verified & Clicked icon on Filter Slider.");
                    }
            }

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Filter Section on screen
        /// </summary>
        /// <param name="filterName">Filter Name to Verify Section</param>
        /// <returns></returns>
        public PromoDashboard verifyFilterSectionOnScreen(string filterName, bool browser = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Selected']"), "'Selected' Button not Present.");
            Assert.AreEqual(browser, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Browse']"), "'Browse' Button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Keyword']"), "'Keyword' Button not Present.");

            Assert.IsTrue(driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='input-group']/div/input"), "Search Text area for '" + filterName + "' filter not Present.");
            //Assert.IsTrue(driver._getAttributeValue("xpath", "//li[@class='dropdown open']//.//div[@class='input-group']/div/input", "placeholder").Contains("Filter " + filterName), "'" + filterName + "' Placeholder text not match.");

            Assert.IsTrue(driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='input-group-btn']"), "'Selected Displayed' Button Icon not Present.");

            if (driver._isElementPresent("xpath", "//div[@class='ag-body-container']"))
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='ag-body-container']"), "List of '" + filterName + "' not Present.");
            else
                Assert.IsTrue(driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul"), "List of '" + filterName + "' not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='btn-group']/button"), "'Load More' Option not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//li[@class='dropdown open']//.//div[@class='btn-group']/button").Contains("Load More"), "'Load More' Label not match.");

            Results.WriteStatus(test, "Pass", "Verified, Filter Section on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Button on Filter Section on Screen
        /// </summary>
        /// <param name="buttonName">Button Name to click</param>
        /// <returns></returns>
        public PromoDashboard clickButtonOnFilterSectionOnScreen(string buttonName)
        {
            switch (buttonName.ToLower())
            {
                case "select displayed":
                    {
                        driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='input-group-btn']/button/span[text() ='Select Displayed']");
                        break;
                    }
                case "browse":
                    {
                        driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Browse']");
                        break;
                    }
                case "keyword":
                    {
                        driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Keyword']");
                        break;
                    }
                case "excluded":
                    {
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='btn-group']/button[@class='btn btn-default field-checkbox label-no-text btn-icon active']") == false)
                            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='btn-group']/button[@class='btn btn-default field-checkbox label-no-text btn-icon']");
                        break;
                    }
                case "excluded remove":
                    {
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='btn-group']/button[@class='btn btn-default field-checkbox label-no-text btn-icon active']") == true)
                            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='btn-group']/button[@class='btn btn-default field-checkbox label-no-text btn-icon active']");
                        break;
                    }
                case "selected":
                    {
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button[@disabled='disabled']/span[text()='Selected']") == false)
                            if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Selected']"))
                                driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Selected']");
                        break;
                    }
                case "slash":
                    {
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='input-group-btn']/button[@class='btn btn-default field-checkbox label-no-text btn-icon']"))
                            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='input-group-btn']/button[@class='btn btn-default field-checkbox label-no-text btn-icon']");
                        Assert.IsTrue(driver._waitForElement("xpath", "//li[@class='dropdown open']//.//div[@class='input-group-btn']/button[contains(@class,'active')]", 10), "Exclusive List not Properly done.");
                        break;
                    }
                case "slash remove":
                    {
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='input-group-btn']/button[@class='btn btn-default field-checkbox label-no-text btn-icon active']"))
                            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='input-group-btn']/button[@class='btn btn-default field-checkbox label-no-text btn-icon active']");
                        break;
                    }
                case "clear selected":
                    {
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='btn-group']/button/i[@class='fa fa-times-circle']") == true)
                            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='btn-group']/button/i[@class='fa fa-times-circle']");
                        break;
                    }
                case "load more":
                    {
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='btn-group']/button/i[@class='fa fa-plus']") == false)
                            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='btn-group']/button/i[@class='fa fa-plus']");
                        break;
                    }
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on Filter Section on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Selecte Characted from Browse Tab
        /// </summary>
        /// <returns></returns>
        public String selectCharacterFromBrowserTab()
        {
            IList<IWebElement> characterCollections = driver.FindElements(By.XPath("//div[@class='btn-group' and @role='group' and contains(@ng-repeat,'char')]"));
            string charValue = "A";
            for (int i = 0; i < characterCollections.Count; i++)
            {
                if (characterCollections[i].Text.Contains("#") == false)
                {
                    charValue = characterCollections[i].Text;
                    characterCollections[i].Click();
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, '" + charValue + "' Character from Browse Tab.");
            return charValue;
        }

        /// <summary>
        /// Verify List Records on Filter Bar
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyFilterListRecordsValueWithSelectedCharacter(string listStartingVal)
        {
            if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']/div"), "Filter List not Present.");
                IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']/div"));
                for (int i = 0; i < filterLists.Count; i++)
                    Assert.AreEqual(true, filterLists[i].GetAttribute("class").Contains("ag-row-selected"), "[" + i + "] Record not Selected on Filter List section.");
            }
            else
            {
                IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li"));
                int cnt = filterLists.Count;
                for (int i = 0; i < cnt; i++)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label")));
                    Assert.AreEqual(true, driver._getText("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label/span").StartsWith(listStartingVal), "[" + i + "] Record not Selected on Filter List section.");
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Filter List Records Value with Selected Character..");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Filter List Records Selected or not on Filter section
        /// </summary>
        /// <param name="selected">Filter List Selcted or Not</param>
        /// <returns></returns>
        public PromoDashboard verifyFilterListRecordsSelectedOrNotOnFilterSection(bool selected = true)
        {
            string lists = "Selected";
            if (selected)
                lists = "Not Selected";
            if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']/div"), "Filter List not Present.");
                IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']/div"));
                for (int i = 0; i < filterLists.Count; i++)
                    Assert.AreEqual(selected, filterLists[i].GetAttribute("class").Contains("ag-row-selected"), "[" + (i + 1) + "] Record " + lists + " on Filter List section.");
            }
            else
            {
                IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li"));
                for (int i = 0; i < filterLists.Count; i++)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label")));
                    IWebElement element = driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label/input"));
                    Assert.AreEqual(selected, element.GetAttribute("class").Contains("not-empty"), "[" + (i + 1) + "] Record " + lists + " on Filter List section.");
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Filter List Records selected or Not on Filter Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Enter Keyword to Search into Filter textbox
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public PromoDashboard enterKeywordToSerachIntoFilterTextBox(int length = 5)
        {
            Assert.IsTrue(driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='input-group']/div/input"), "Search Text area not Present.");
            string value = "";

            clearKeywordFromSearchTextBox();
            if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='ag-body-container']/div[@row-index='1']//.//span[@class='ag-group-value']") == true)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='ag-body-container']/div[@row-index='1']//.//span[@class='ag-group-value']"), "Filter List not Present.");
                value = driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='ag-body-container']/div[@row-index='1']//.//span[@class='ag-group-value']")).Text.Substring(0, length);
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='filtered-items']/div/div/ul/li"), "Filter List not Present.");
                value = driver.FindElement(By.XPath("//div[@class='filtered-items']/div/div/ul/li[1]")).Text.Substring(0, length);
            }

            driver._type("xpath", "//li[@class='dropdown open']//.//div[@class='input-group']/div/input", value);
            Thread.Sleep(2000);
            Results.WriteStatus(test, "Pass", "Entered, '" + value + "' Keyword to Search area to Filter List.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Clear Keyword from Search Textbox
        /// </summary>
        /// <returns></returns>
        public PromoDashboard clearKeywordFromSearchTextBox()
        {
            if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//button[@class='btn CFT-textbox-inner-button']"))
            {
                driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//button[@class='btn CFT-textbox-inner-button']");
                Results.WriteStatus(test, "Pass", "Cleared, Keyword from Search Textbox.");
            }

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Tooltip message on filter section
        /// </summary>
        /// <param name="message">Tooltip Message to Verify</param>
        /// <returns></returns>
        public PromoDashboard verifyTooltipOnFilterSection(string message)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='tooltip-inner']"), "Tooltip not Present on Filter section.");
            Assert.AreEqual(message, driver._getText("xpath", "//div[@class='tooltip-inner']"), "'" + message + "' Tooltip message not match.");
            Results.WriteStatus(test, "Pass", "Verified, '" + message + "' Tooltip message on Filter section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Browse tab on Filter section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyBrowseTabOnFilterSection()
        {
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='btn-group']/button[@disabled='disabled']/span[text()='«']"), "'First' Icon not Default not Disable.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='btn-group']/button[@disabled='disabled']/span[text()='‹']"), "'Previous' Icon not Default not Disable.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='btn-group']/button/span[text()='#']"), "'#' Alpha Letter not not Present.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='btn-group']/button/span[text()='A']"), "'A' Alpha Letter not not Present.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='btn-group']/button[@class='btn btn-default']/span[text()='›']"), "'Next' Icon not Present.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='btn-group']/button[@class='btn btn-default' and text()='»']"), "'Last' Icon not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Browse tab on Filter Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Excluded Button label on Filter section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyExcludedButtonLabelOnFilterSection()
        {
            Assert.AreEqual(false, driver._isElementPresent("xpath", "//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Selected']"), "'Selected' Button name not Change.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-tabs blocksParentScroll']/button/span[text()='Excluded']"), "'Excluded' Button name not Present.");
            Results.WriteStatus(test, "Pass", "Verified, Excluded button Label on Filter section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Selecte Records from List on Filter Section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard selectRecordsFromListOnFilterSection()
        {
            if (driver._isElementPresent("xpath", "//div[@class='CFT-tabs blocksParentScroll']/button[@disabled='disabled']/span[text()='Selected']") || driver._isElementPresent("xpath", "//div[@class='CFT-tabs blocksParentScroll']/button[@disabled='disabled']/span[text()='Excluded']") == false)
            {
                if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']"))
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']/div"), "Filter List not Present.");
                    IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']//.//div[@class='ag-body-container']/div"));
                    for (int i = 0; i < filterLists.Count; i++)
                        Assert.AreEqual(true, filterLists[i].GetAttribute("class").Contains("ag-row-selected"), "[" + i + "] Record not Selected on Filter List section.");
                }
                else
                {
                    IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li"));
                    int cnt = filterLists.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label")));
                        if (driver._isElementPresent("xpath", "//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label/input[contains(@class,'not-empty')]") == false)
                        {
                            driver._clickByJavaScriptExecutor("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label");
                            break;
                        }
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, Records from List on Filter Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Filter Section with Checkbox
        /// </summary>
        /// <param name="filterName">Filter Name to Verify</param>
        /// <returns></returns>
        public PromoDashboard verifyFilterSectionWithCheckbox(string filterName)
        {
            IList<IWebElement> filterLists = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li"));
            int cnt = filterLists.Count;
            for (int i = 0; i < cnt; i++)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label")));
                IWebElement element = driver.FindElement(By.XPath("//li[@class='dropdown open']//.//div[@class='filtered-items']/div/div/ul/li[" + (i + 1) + "]/label/input"));
                Assert.AreEqual(true, element.GetAttribute("type").Contains("checkbox"), "[" + i + "] Record Checkbox not Present on '" + filterName + "' Filter List section.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Filter section with checkbox.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Select Radio option from Promo Dashboard
        /// </summary>
        /// <param name="radioOption">Radio Option for Select</param>
        /// <returns></returns>
        public PromoDashboard selectRadioOptionFromPromoDashboard(string radioOption)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='carousel-filters']/label/span[text() = '" + radioOption + "']"), "'" + radioOption + "' Radio option not Present.");
            driver._clickByJavaScriptExecutor("//div[@class='carousel-filters']/label/span[text() = '" + radioOption + "']");
            Thread.Sleep(1000);
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Almost there')]");
            Results.WriteStatus(test, "Pass", "Selected, '" + radioOption + "' Radio Option from Promo Dashboard Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Sorted by Records in Carousel for option
        /// </summary>
        /// <param name="radioOption">Option name to Verify Records Value</param>
        /// <returns></returns>
        public PromoDashboard verifySortedByRecordsInCarouselForOption(string radioOption)
        {
            IList<IWebElement> productCounts = driver.FindElements(By.XPath("//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div//.//div[@class='aditem-details']/div[@class='row aditem-header-row']/div[2]"));
            for (int i = 0; i < productCounts.Count; i++)
            {
                int stores = Convert.ToInt32(productCounts[i].Text.Substring(0, productCounts[i].Text.IndexOf("S")).Replace(" ", "").Replace(",", ""));
                if (i != 0)
                    Assert.GreaterOrEqual(Convert.ToInt32(productCounts[i].Text.Substring(0, productCounts[i].Text.IndexOf("S")).Replace(" ", "").Replace(",", "")), Convert.ToInt32(productCounts[i - 1].Text.Substring(0, productCounts[i - 1].Text.IndexOf("S")).Replace(" ", "").Replace(",", "")));
            }

            Results.WriteStatus(test, "Pass", "Verified, '" + radioOption + "' Sorted By Records in Carousel for option.");
            return new PromoDashboard(driver, test);
        }

        #region Product Carousel

        /// <summary>
        /// Verify Product Carousel
        /// </summary>
        /// <param name="thumbnail">Products Thumbnail</param>
        /// <returns></returns>
        public PromoDashboard verifyProductThumbnailForProductCarousel(bool thumbnail = false)
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
                        Assert.AreEqual(true, driver._waitForElement("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//img", 20), "image not load properly.");
                        IWebElement image = driver._findElement("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//img");
                        bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
                        Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Carousel Section.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row aditem-header-row']/div"), "Parent Retailer Name not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row aditem-header-row']/div[2]"), "Number of Stores not Present for [" + (i + 1) + "] Record.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[2]/div"), "Start Date not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[2]/div[2]"), "End Date not Present for [" + (i + 1) + "] Record.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[3]/div"), "Number of Pages not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[3]/div[2]"), "Page Location not Present for [" + (i + 1) + "] Record.");

                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']//.//button[text()='View Ad']"), "'View Ad' Button not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']//.//button[text()='Stores']"), "'Stores' Button not Present for [" + (i + 1) + "] Record.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/div/div[contains(@class,'active')]/div[" + (i + 1) + "]//.//div[@class='aditem-details']/div[@class='row custom-btn-default-wrapper']//.//button[text()='Details']"), "'Details' Button not Present for [" + (i + 1) + "] Record.");
                    }
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/a[@class='right carousel-control']"), "Next Page Arrow not Present.");
            }

            Results.WriteStatus(test, "Pass", "Verify Product Thumbnail for Product Carousel.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Button lonk for product on carousel section
        /// </summary>
        /// <param name="linkName">Link Name for Clicking</param>
        /// <returns></returns>
        public PromoDashboard clickButtonLinkForProductOnCarouselSection(string linkName)
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
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Navigation Arrow for Carousel
        /// </summary>
        /// <param name="pageArrow">Page Arrow for carousel</param>
        /// <returns></returns>
        public PromoDashboard verifyNavigationArrowForCarousel(string pageArrow)
        {
            string ArrowLocation = "left";
            if (pageArrow.Equals("Next"))
                ArrowLocation = "right";
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/a[@class='" + ArrowLocation + " carousel-control']"), "" + pageArrow + " Page Arrow not Present.");
            driver.MouseHoverUsingElement("xpath", "//*[@id='domain-carousel']/a[@class='" + ArrowLocation + " carousel-control']/span");
            Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//*[@id='domain-carousel']/a[@class='" + ArrowLocation + " carousel-control']/span")).GetCssValue("color"), "'" + pageArrow + "' Arrow not Highlighted with Blue color.");

            Results.WriteStatus(test, "Pass", "Verified, '" + ArrowLocation + "' Navigation Arrow for Carousel.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Slider Navigation button for carousel
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifySliderNavigationButtonForCarousel(bool clickSlider = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='domain-carousel']/ol[@class='carousel-indicators']/li"), "Slider Ingicators not Present.");
            Assert.AreEqual(true, driver.FindElement(By.XPath("//*[@id='domain-carousel']/ol[@class='carousel-indicators']/li")).GetCssValue("background").Contains("rgb(0, 74, 82)"), "First Slider Default not Highlighted with Blue color.");

            if (clickSlider)
            {
                IList<IWebElement> sliderCollections = driver.FindElements(By.XPath("//*[@id='domain-carousel']/ol[@class='carousel-indicators']/li"));
                Random rand = new Random();
                int x = rand.Next(0, sliderCollections.Count);
                sliderCollections[x].Click();
                Thread.Sleep(1000);
                Assert.AreEqual(true, sliderCollections[x].GetCssValue("background").Contains("rgb(0, 74, 82)"), "Slider not Highlighted with Tile color.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Slider Navigation button for Carousel.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Product Detail Popup Window on Dashboard Page
        /// </summary>
        /// <param name="tabsName">TabNames to Veriy on Window</param>
        /// <param name="defaultView">Default Selected Tab Name</param>
        /// <returns></returns>
        public PromoDashboard verifyProductDetailPopupWindowOnDashboardPage(string[] tabNames, string defaultView, bool enable = true)
        {
            if (enable)
            {
                driver._waitForElementToBeHidden("xpath", "//i[@class='fa fa-fw fa-spinner fa-spin']");
                Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='modal-content']", 20), "Product Details Popup Window not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='panel-heading modal-header-inner']/span"), "Product Label not found on popup window.");

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

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify View Ad Screen on Popup Window
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyViewAdScreenOnPopupWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='row aditem-image-row']", 20), "Pages of Ad block not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='creative-thumbnail-container']/div/div/div"), "List of Ad Block Pages not found.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='creative-thumbnail-container']/div/div/div//.//div[contains(@class,'active')]"), "Current Selected Page not Display on Screen.");

            IWebElement image = driver._findElement("xpath", "//div[@class='zoom-gallery-active-item market-table-wrapper']/img");
            bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
            Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Popup Window.");

            IList<IWebElement> totalPages = driver.FindElements(By.XPath("//*[@id='creative-thumbnail-container']/div/div/div"));
            for (int i = 0; i < totalPages.Count; i++)
            {
                IWebElement pages = driver._findElement("xpath", "//*[@id='creative-thumbnail-container']/div/div/div[" + (i + 1) + "]//.//img");
                bool pageLoaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", pages));
                Assert.AreEqual(true, pageLoaded, "'(" + image.GetAttribute("src") + ")' Page Image Not Load on Popup Window.");
            }

            Assert.AreEqual(false, driver._isElementPresent("xpath", "//span[@class='rz-pointer rz-pointer-min' and @style='left: 0px;']"), "Image Default not in Zoom mode.");

            Results.WriteStatus(test, "Pass", "Verified, View Ad Screen on Popup window.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Stores Screen on Popup Window
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyStoresScreenOnPopupWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//*[@ref='center']//.//div[contains(@class,'ag-body')]/div[@class='ag-body-viewport-wrapper']/div/div/div[1]", 20), "Grid Proper not Loading.");

            IList<IWebElement> gridLists = driver.FindElements(By.XPath("//*[@ref='center']//.//div[contains(@class,'ag-header')]/div[@class='ag-header-viewport']//.//div[@class='ag-header-row']/div"));
            string[] headers = { "Store Number", "Retailer", "City", "State", "Zip Code" };
            for (int i = 0; i < gridLists.Count; i++)
                Assert.AreEqual(true, gridLists[i].Text.Contains(headers[i]), "'" + headers[i] + "' Header Label not match.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='btn-group']//.//i[@class='fa fa-cloud-download']"), "'Download Grid' Button not Present on Window.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='btn-group']//.//i[@class='fa fa-gear']"), "'Grid Options' Button not Present on Window.");

            Results.WriteStatus(test, "Pass", "Verified, Stores Screen on popup window.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify More Details Screen on Popup Window
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyMoreDetailsScreenOnPopupWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='aditem aditem-thumbnail-view aditem-modal']", 20), "Image Section not Present on Details Section.");
            IWebElement image = driver._findElement("xpath", "//div[@class='aditem aditem-thumbnail-view aditem-modal']//.//img");
            bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
            Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Details Section.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='aditem aditem-detail-view aditem-detail-view-modal']/div[@class=' detail-view-content']"), "Detail View Content not Present.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='panel-heading modal-header-inner']/button[@class='pull-right modal-close' and text()='×']"), "Close Icon not Present on Popup Window.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-default' and contains(text(),'Close')]"), "'Close' Button not Present on Popup Window.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-primary']"), "'Export' Button not Present on Popup Window.");

            Results.WriteStatus(test, "Pass", "Verified, More Details Screen on popup window.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Map Screen on Popup Window
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyMapScreenOnPopupWindow()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='aditem aditem-thumbnail-view aditem-modal']", 20), "Image Section not Present on Details Section.");

            Results.WriteStatus(test, "Pass", "Verified, Map Screen on popup window.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Button on Popup Window
        /// </summary>
        /// <param name="buttonName">Button Name to Click</param>
        /// <returns></returns>
        public PromoDashboard clickButtonOnPopupWindow(string buttonName)
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
                        Assert.IsTrue(driver._isElementPresent("xpath", "//button[@class='btn btn-primary']"), "'Export' Button not Present on Popup Window.");
                        driver._clickByJavaScriptExecutor("//button[@class='btn btn-primary']");
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
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Filter icon and Verify Section
        /// </summary>
        /// <param name="labelName">Label Name for click and verify</param>
        /// <returns></returns>
        public PromoDashboard clickFilterIconAndVerifySection(string labelName)
        {
            IList<IWebElement> gridLists = driver.FindElements(By.XPath("//*[@ref='center']//.//div[contains(@class,'ag-header')]/div[@class='ag-header-viewport']//.//div[@class='ag-header-row']/div"));
            for (int i = 0; i < gridLists.Count; i++)
            {
                if (gridLists[i].Text.Contains(labelName))
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@ref='center']//.//div[contains(@class,'ag-header')]/div[@class='ag-header-viewport']//.//div[@class='ag-header-row']/div[" + (i + 1) + "]//.//span[@ref='eMenu']/i"), "'" + labelName + "' Filter Icon not Present.");
                    driver._clickByJavaScriptExecutor("//*[@ref='center']//.//div[contains(@class,'ag-header')]/div[@class='ag-header-viewport']//.//div[@class='ag-header-row']/div[" + (i + 1) + "]//.//span[@ref='eMenu']/i");
                    Thread.Sleep(500);
                    Assert.IsTrue(driver._isElementPresent("id", "tabBody"), "'" + labelName + "' Section not Present.");
                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='ag-mini-filter']/input"), "'" + labelName + "' Textbox not Present.");

                    Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='ag-filter-header-container']/label/span[contains(text(),'Select All')]"), "'" + labelName + "' Select All Checkbox not Present.");
                    Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='ag-filter-header-container']/label/div[@id='selectAll']/span")).GetCssValue("color").Contains("74, 82, 1"), "'Select All' Option Default not Selected.");

                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='richList']"), "'" + labelName + "' Fields Values not Present.");
                    IList<IWebElement> filterValues = driver.FindElements(By.XPath("//*[@id='richList']/div/div/div"));
                    for (int j = 0; j < filterValues.Count; j++)
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='richList']/div/div/div[" + (j + 1) + "]/label/div[@class='ag-filter-checkbox']"), "Values Option Default not Selected.");

                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + labelName + "' Filter Icon and Verified Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Select Option from Filterbar Section
        /// </summary>
        /// <param name="optionName">Option Name to Click</param>
        /// <returns></returns>
        public String selectOptionFromFilterBarSection(string optionName, bool unChecked = false)
        {
            string fieldValue = "Select All";
            if (optionName.Equals("Select All"))
            {
                if (driver._isElementPresent("xpath", "//div[@class='ag-filter-header-container']/label/div[@id='selectAll']/span[contains(@class,'fa-check-square')]") == unChecked)
                {
                    driver._clickByJavaScriptExecutor("//div[@class='ag-filter-header-container']/label/span[contains(text(),'Select All')]");
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
        /// Enter and Verify Keyword into Filter Search textbox
        /// </summary>
        /// <returns></returns>
        public PromoDashboard enterAndVerifyKeywordInToFilterSearchTextbox()
        {
            IList<IWebElement> filterValues = driver.FindElements(By.XPath("//*[@id='richList']/div/div/div"));
            Random rand = new Random();
            int x = rand.Next(0, filterValues.Count);
            string searchTextbox = filterValues[x].Text;
            driver._type("xpath", "//*[@id='ag-mini-filter']/input", searchTextbox);
            Thread.Sleep(500);

            filterValues = driver.FindElements(By.XPath("//*[@id='richList']/div/div/div"));
            for (int j = 0; j < filterValues.Count; j++)
                Assert.AreEqual(true, filterValues[j].Text.Contains(searchTextbox), "'" + searchTextbox + "' Keyword not Present.");

            Results.WriteStatus(test, "Pass", "Entered, '" + searchTextbox + "' Keyword in Filter Search textbox and Verified Filter Record.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Clear Search textbox on filter section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard clearSearchTextboxOnFilterSection()
        {
            IWebElement toClear = driver.FindElement(By.XPath("//*[@id='ag-mini-filter']/input"));
            toClear.SendKeys(Keys.Control + "a");
            toClear.SendKeys(Keys.Delete);
            Thread.Sleep(500);
            Results.WriteStatus(test, "Pass", "Cleared, Search textbox on Filter Section.");
            return new PromoDashboard(driver, test);
        }

        ///// <summary>
        ///// Click Column Header Sort and Compare on Grid
        ///// </summary>
        ///// <param name="columnName">Column Name to Sort</param>
        ///// <returns></returns>
        //public PromoDashboard clickColumnHeaderSortAndCompareOnGrid(string columnName)
        //{
        //    driver._waitForElement("xpath", "//*[@id='center']/div/div[4]/div[3]/div/div/div");

        //    IList<string> data = driver.GetArrayValue("xpath", "", "//div[@class='ag-header-container']/div/div", "//div[@class='ag-body-container']/div", "", "", columnName, false);
        //    Results.WriteStatus(test, "Pass", "Default " + columnName + " Data retrived successfully.");

        //    IList<string> DT = driver._sortArraydata(data, "Ascending", "string");
        //    Results.WriteStatus(test, "Pass", "Array sorted successfully.");

        //    IList<string> DT1 = driver._sortArraydata(data, "Descending", "string");
        //    Results.WriteStatus(test, "Pass", "Array sorted successfully.");

        //    //sort " + columnName + " column in ascending order.
        //    clickColumnNameAndVerifyArrowForColumn(columnName, true);
        //    Results.WriteStatus(test, "Pass", "Clicked " + columnName + " column header in grid to sort by ascending.");

        //    IList<string> sData = driver.GetArrayValue("xpath", "", "//div[@class='ag-header-container']/div/div", "//div[@class='ag-body-container']/div", "", "", columnName, false);
        //    Results.WriteStatus(test, "Pass", "Sorted " + columnName + " column Data retrived successfully.");

        //    driver._compareArrayValues(sData, DT);
        //    Results.WriteStatus(test, "Pass", "Verified Sorted ascending data in Grid.");

        //    //to sort " + columnName + " column in descending order.
        //    clickColumnNameAndVerifyArrowForColumn(columnName, false);
        //    Results.WriteStatus(test, "Pass", "Clicked on " + columnName + " column header in grid to sort by descending.");

        //    Thread.Sleep(5000);
        //    IList<string> sData1 = driver.GetArrayValue("xpath", "", "//div[@class='ag-header-container']/div/div", "//div[@class='ag-body-container']/div", "", "", columnName, false);
        //    Results.WriteStatus(test, "Pass", "Sorted " + columnName + " Data retrived successfully.");

        //    driver._compareArrayValues(sData1, DT1);
        //    Results.WriteStatus(test, "Pass", "Verified Sorted descending data in Grid.");

        //    Thread.Sleep(5000);
        //    return new PromoDashboard(driver, test);
        //}

        /// <summary>
        /// Click Column Name & Verify Arrow for Column
        /// </summary>
        /// <param name="columnName">ColumnName to Clicked</param>
        /// <param name="descending">To Verify Sorting Arrow</param>
        /// <returns></returns>
        public PromoDashboard clickColumnNameAndVerifyArrowForColumn(string columnName, bool descending)
        {
            IList<IWebElement> columnCollections = driver._findElements("xpath", "//div[@class='ag-header-container']/div/div");
            bool avail = false;
            for (int i = 0; i < columnCollections.Count; i++)
            {
                if (columnCollections[i].Text.Contains(columnName))
                {
                    driver._clickByJavaScriptExecutor("//div[@class='ag-header-container']/div/div[" + (i + 1) + "]//.//div[@class='ag-header-cell-label']/span[@ref='eText']");
                    Thread.Sleep(1000);

                    if (descending)
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='ag-header-container']/div/div[" + (i + 1) + "]//.//div[@class='ag-header-cell-label']/span[@ref='eSortDesc']"), "'" + columnName + "' Column not Display in Descending manner.");
                        Assert.AreEqual("fa fa-sort-desc", driver._getAttributeValue("xpath", "//div[@class='ag-header-container']/div/div[" + (i + 1) + "]//.//div[@class='ag-header-cell-label']/span[@ref='eSortDesc']/i", "class"), "Descending Arrow for '" + columnName + "' Column not Present.");
                        Results.WriteStatus(test, "Pass", "Clicked, '" + columnName + "' Column and Verified Down side Arrow for Column.");
                    }
                    else
                    {
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='ag-header-container']/div/div[" + (i + 1) + "]//.//div[@class='ag-header-cell-label']/span[@ref='eSortAsc']"), "'" + columnName + "' Column not Display in Ascending manner.");
                        Assert.AreEqual("fa fa-sort-asc", driver._getAttributeValue("xpath", "//div[@class='ag-header-container']/div/div[" + (i + 1) + "]//.//div[@class='ag-header-cell-label']/span[@ref='eSortAsc']/i", "class"), "Ascending Arrow for '" + columnName + "' Column not Present.");
                        Results.WriteStatus(test, "Pass", "Clicked, '" + columnName + "' Column and Verified Up side Arrow for Column.");
                    }
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + columnName + "' Column not Present on Grid.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Get Calendar Data from Calendar Screen
        /// </summary>
        /// <returns></returns>
        public String[,] getCalendarDataFromCalendarScreen()
        {
            IList<IWebElement> totalRows = driver._findElements("xpath", "//div[@class='ag-header-container']/div/div");
            IList<IWebElement> totalColumns = driver._findElements("xpath", "//div[@class='ag-body-container']/div");
            string[,] accountLists = new string[totalColumns.Count + 1, totalRows.Count];
            int cnt = 0;
            string add = "";
            for (int i = 0; i < totalRows.Count; i++)
            {
                accountLists[cnt, i] = totalRows[i].Text.Replace("\r\n", "");

                if (i == 0)
                    add = add + "\"" + accountLists[cnt, i] + "\",";
                else
                    add = add + "\"" + accountLists[cnt, i] + "\"";
                //Console.WriteLine("accountLists[cnt, i] [" + cnt + " - " + i + "] : " + accountLists[cnt, i]);
            }

            cnt++;

            for (int i = 0; i < totalColumns.Count; i++)
            {
                for (int j = 0; j < totalRows.Count; j++)
                {
                    //Assert.AreEqual(true, driver.FindElement(By.XPath("//div[@class='ag-body-container']/div[" + (i + 1) + "]/div[" + (j + 1) + "]"), "Value not Present.");
                    if (driver._isElementPresent("xpath", "//div[@class='ag-body-container']/div[" + (i + 1) + "]/div[" + (j + 1) + "]") == true)
                    {
                        accountLists[cnt, j] = driver._getText("xpath", "//div[@class='ag-body-container']/div[" + (i + 1) + "]/div[" + (j + 1) + "]");

                        //Console.WriteLine("accountLists[cnt, i] [" + cnt + " - " + j + "] : " + accountLists[cnt, j]);
                    }
                }
                cnt++;
            }

            //Console.WriteLine("Total accountLists.GetLength(1) : " + accountLists.GetLength(0));
            //Console.WriteLine("Total Columns : " + totalColumns.Count);
            //Console.WriteLine("Total Rows : " + totalRows.Count);
            //for (int i = 0; i < accountLists.GetLength(0); i++)
            //{
            //    for (int j = 0; j < totalRows.Count; j++)
            //    {
            //        Console.WriteLine("Columns Name [" + i + "] [" + j + "]: " + accountLists[i, j]);
            //    }
            //}

            Results.WriteStatus(test, "Pass", "Get Calendar Data from Calendar Screen.");
            return accountLists;
        }

        /// <summary>
        /// Get Product Details Grid Records from popup window
        /// </summary>
        /// <returns></returns>
        public String[,] getProductDetailsGridRecordsFromPopupWindow()
        {
            IList<IWebElement> totalRows = driver._findElements("xpath", "//div[@class='ag-header-container']/div/div");
            IList<IWebElement> totalColumns = driver._findElements("xpath", "//div[@class='ag-body-container']/div");
            //Console.WriteLine("Total Column : " + totalColumns.Count);
            //Console.WriteLine("Total Rows : " + totalRows.Count);
            string[,] accountLists = new string[totalColumns.Count + 1, 1];
            string[,] columnIds = new string[totalRows.Count, 1];
            int cnt = 0;
            string add = "";
            for (int i = 0; i < totalRows.Count; i++)
            {
                if (i == 0)
                    add = add + "\"" + totalRows[i].Text.Replace("\r\n", "") + "\"";
                else
                    add = add + ",\"" + totalRows[i].Text.Replace("\r\n", "") + "\"";
                //Console.WriteLine("totalRows[i] Att : " + totalRows[i].GetAttribute("col-id"));
                columnIds[i, 0] = totalRows[i].GetAttribute("col-id");
            }
            accountLists[cnt, 0] = add;
            cnt++;

            for (int i = 0; i < totalColumns.Count; i++)
            {
                add = "";
                for (int j = 0; j < totalRows.Count; j++)
                {
                    //if (driver._isElementPresent("xpath", "//div[@class='ag-body-container']/div[@row-index='" + i + "']/div[" + (j + 1) + "]") == true)
                    if (driver._isElementPresent("xpath", "//div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + columnIds[j, 0] + "']") == true)
                    {
                        driver._scrollintoViewElement("xpath", "//div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + columnIds[j, 0] + "']");
                        if (j == 0)
                            add = add + "\"" + driver._getText("xpath", "//div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + columnIds[j, 0] + "']") + "\"";
                        else
                            add = add + ",\"" + driver._getText("xpath", "//div[@class='ag-body-container']/div[@row-index='" + i + "']/div[@col-id='" + columnIds[j, 0] + "']") + "\"";
                        if (add == "")
                            break;
                        //if (j == 0)
                        //    add = add + "\"" + driver._getText("xpath", "//div[@class='ag-body-container']/div[@row-index='" + i + "']/div[" + (j + 1) + "]") + "\"";
                        //else
                        //    add = add + ",\"" + driver._getText("xpath", "//div[@class='ag-body-container']/div[@row-index='" + i + "']/div[" + (j + 1) + "]") + "\"";
                    }
                }
                accountLists[cnt, 0] = add;
                cnt++;
            }

            //Console.WriteLine("Grid Records");
            //Console.WriteLine("cnt : " + cnt);
            //Console.WriteLine("accountLists : " + accountLists);
            //for (int i = 0; i < cnt; i++)
            //{
            //    Console.WriteLine("accountLists [" + i + ", 0] : " + accountLists[i, 0]);
            //}

            Results.WriteStatus(test, "Pass", "Get Product Details Grid Records from Popup window.");
            return accountLists;
        }

        /// <summary>
        /// This Values of Downloaded file with Chart Value
        /// </summary>
        /// <param name="chartValue">Values fetched from Chart</param>
        /// <returns></returns>
        public PromoDashboard verifyValuesOfDownloadedFile(string sheetName, string[,] gridValues)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            int rw = 0;
            int cl = 0;
            int cnt = 0;
            string FilePath = "";

            string sourceDir = ExtentManager.ResultsDir + "\\";
            string[] fileEntries = Directory.GetFiles(sourceDir);

            foreach (string fileName in fileEntries)
            {
                if (fileName.Contains(sheetName))
                {
                    FilePath = fileName;
                    break;
                }
            }

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(FilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;

            int startCellRow = 1;
            bool startRow = false;
            for (int rCnt = 1; rCnt <= rw; rCnt++)
            {
                for (int cCnt = 1; cCnt <= cl; cCnt++)
                {
                    if (((range.Cells[rCnt, cCnt] as Excel.Range).Text) == gridValues[cnt, 0])
                    {
                        startCellRow = rCnt;
                        startRow = true;
                        break;
                    }
                }
                if (startRow)
                    break;
            }

            int lastCell = startCellRow + gridValues.Length;
            for (int rCnt = startCellRow; rCnt < lastCell; rCnt++)
            {
                Assert.AreEqual((range.Cells[rCnt, 1] as Excel.Range).Text, gridValues[cnt, 0], "'" + gridValues[cnt, 0] + "' Name not match.");
                cnt++;
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            File.Delete(FilePath);

            Results.WriteStatus(test, "Pass", "Verified, Values of Downloaded '" + sheetName + "' File.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Grid Label Options on popup window
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyGridLabelOptionsOnPopupWindow()
        {
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='ag-column-select-panel']/div"), "Grid Label Options not Present.");
            string[] options = { "Store Number", "Retailer", "Address", "City", "State", "Zip Code", "DMA" };

            IList<IWebElement> labelCollections = driver.FindElements(By.XPath("//div[@class='ag-column-select-panel']/div"));
            for (int i = 0; i < labelCollections.Count; i++)
                Assert.AreEqual(labelCollections[i].Text, options[i], "'" + options[i] + "' Checkbox Label not match.");

            Results.WriteStatus(test, "Pass", "Verified, Grid Label Options on Popup Window.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Checked / UnChecked Grid Options from popup window
        /// </summary>
        /// <param name="unChecked">UnChecked Selected Option</param>
        /// <returns></returns>
        public String checkedUncheckedGridOptionsFromPopupWindow(bool unChecked)
        {
            string optionName = "";
            string checkboxStatus = "ag-checkbox-checked ag-hidden";
            if (unChecked)
                checkboxStatus = "ag-checkbox-checked";
            IList<IWebElement> labelCollections = driver.FindElements(By.XPath("//div[@class='ag-column-select-panel']/div[@class='ag-column-container']/div"));
            for (int i = 0; i < labelCollections.Count; i++)
            {
                if (driver._getAttributeValue("xpath", "//div[@class='ag-column-select-panel']/div[@class='ag-column-container']/div[" + (i + 1) + "]/span[@class='ag-column-select-checkbox']/span[1]", "class") == checkboxStatus)
                {
                    optionName = labelCollections[i].Text;
                    driver._clickByJavaScriptExecutor("//div[@class='ag-column-select-panel']/div[@class='ag-column-container']/div[" + (i + 1) + "]/span[@class='ag-column-select-checkbox']/span");
                    Thread.Sleep(500);
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, '" + optionName + "' Option from Popup Window.");
            return optionName;
        }

        /// <summary>
        /// Verify Grid Title on Popup Window
        /// </summary>
        /// <param name="gridOption">Grid Option Name to Verify</param>
        /// <param name="visible">To Option Availble or not on Grid</param>
        /// <returns></returns>
        public PromoDashboard verifyGridTitleOnPopupWindow(string gridOption, bool visible)
        {
            IList<IWebElement> gridTitles = driver._findElements("xpath", "//div[@class='ag-header-container']/div/div");
            bool verify = false;
            for (int i = 0; i < gridTitles.Count; i++)
            {
                if (gridTitles[i].Text == gridOption)
                {
                    verify = true;
                    break;
                }
            }

            Assert.AreEqual(verify, visible, "'" + gridOption + "' Grid Title not verify.");
            Results.WriteStatus(test, "Pass", "Verified, Grid Title on popup window.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Ad Image on Details Section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard clickAdImageOnDetailsSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='aditem aditem-thumbnail-view aditem-modal']//.//img"), "Ad Image not Present on Details Section.");
            driver._clickByJavaScriptExecutor("//div[@class='aditem aditem-thumbnail-view aditem-modal']//.//img");
            Results.WriteStatus(test, "Pass", "Clicked, Ad Image on Details Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Select tab on Product Details Popup window
        /// </summary>
        /// <param name="tabName">TabName for Select</param>
        /// <returns></returns>
        public PromoDashboard selectTabOnProductDetailsPopuWindow(string tabName)
        {
            IList<IWebElement> tabCollections = driver.FindElements(By.XPath("//div[@class='panel-heading modal-header-inner']/ul/li"));
            for (int i = 0; i < tabCollections.Count; i++)
            {
                if (tabCollections[i].Text.Contains(tabName))
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='panel-heading modal-header-inner']/ul/li[" + (i + 1) + "]/a"), "");
                    driver._clickByJavaScriptExecutor("//div[@class='panel-heading modal-header-inner']/ul/li[" + (i + 1) + "]/a");
                    Thread.Sleep(1000);
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Selected, '" + tabName + "' Tab on Product Details popup window.");
            return new PromoDashboard(driver, test);
        }

        #endregion

        #region Chart Section

        /// <summary>
        /// Verify Navigation Arrow for Chart Section
        /// </summary>
        /// <param name="pageArrow">Page Arrow</param>
        /// <returns></returns>
        public PromoDashboard verifyNavigationArrowForChartSection(string pageArrow)
        {
            string ArrowLocation = "left";
            if (pageArrow.Equals("Next"))
                ArrowLocation = "right";
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/a[@class='" + ArrowLocation + " carousel-control']"), "" + pageArrow + " Page Arrow not Present.");
            driver.MouseHoverUsingElement("xpath", "//*[@id='key-metrics-creative-carousel']/a[@class='" + ArrowLocation + " carousel-control']/span");
            Assert.AreEqual("rgba(0, 74, 82, 1)", driver.FindElement(By.XPath("//*[@id='key-metrics-creative-carousel']/a[@class='" + ArrowLocation + " carousel-control']/span")).GetCssValue("color"), "'" + pageArrow + "' Arrow not Highlighted with Blue color.");

            Results.WriteStatus(test, "Pass", "Verified, '" + ArrowLocation + "' Navigation Arrow for Chart Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify and Select Chart on Chart Section
        /// </summary>
        /// <param name="chartName">Chart Name to Select</param>
        /// <param name="secondChart"></param>
        /// <returns></returns>
        public PromoDashboard verifyAndSelectChartOnChartSection(string chartName, bool secondChart)
        {
            int div = 1;
            int chartDiv = 1;
            if (secondChart)
                chartDiv = 2;
            if (chartName.Equals("Top Segments") || chartName.Equals("Segment Feature Share by Retailer"))
                div = 2;
            if (chartName.Equals("Top Categories") || chartName.Equals("Category Feature Share by Retailer"))
                div = 3;
            if (chartName.Equals("Top Subcategories") || chartName.Equals("Subcategory Feature Share by Retailer"))
                div = 4;
            if (chartName.Equals("Top Manufacturers") || chartName.Equals("Manufacturer Feature Share by Retailer"))
                div = 5;
            if (chartName.Equals("Top Brands") || chartName.Equals("Brand Feature Share by Retailer"))
                div = 6;

            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/ol/li[" + div + "]"));
            driver._clickByJavaScriptExecutor("//*[@id='key-metrics-creative-carousel']/ol/li[" + div + "]");
            Results.WriteStatus(test, "Pass", "Clicked, '" + chartName + "' Chart Carousel on Chart Section.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart"), "'" + chartName + "' Chart Not Present on Section.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/div[contains(@id,'chart')]/div/*[name()='svg']/*[name()='text']/*[name()='tspan' and contains(text(),'" + chartName + "')]"), "'" + chartName + "' Chart Header Not found on Section.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/chart-export/div/button[1]"), "'" + chartName + "' Chart Expand Option Not Present on Section.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/chart-export/div/button[2]"), "'" + chartName + "' Chart Download Option Not Present on Section.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/div[contains(@id,'chart')]/div/*[name()='svg']/*[@class='highcharts-legend']"), "All legends not present for chart.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/div[contains(@id,'chart')]/div/*[name()='svg']/*[name()='text'][1]"), "'" + chartName + "' Chart Date Range Not found on Section.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/div[contains(@id,'chart')]/div/*[name()='svg']/*[name()='text' and contains(text(),'Numerator')]"), " Numerator Link not Present for '" + chartName + "' Chart.");

            Results.WriteStatus(test, "Pass", "Verified, '" + chartName + "' Chart Section on Chart Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Legend Value from Chart
        /// </summary>
        /// <param name="chartName">Chart Name to Verify</param>
        /// <param name="secondChart">For Second Chart on Carousel</param>
        /// <param name="legendName">Legend Name to Select or Unselect</param>
        /// <param name="unSelect">To Unselect Legend</param>
        /// <returns></returns>
        public String selectLegendValueFromChart(string chartName, bool secondChart, string legendName = "Random", bool unSelect = false)
        {
            int div = 1;
            int chartDiv = 1;
            if (secondChart)
                chartDiv = 2;
            if (chartName.Equals("Top Segments") || chartName.Equals("Segment Feature Share by Retailer"))
                div = 2;
            if (chartName.Equals("Top Categories") || chartName.Equals("Category Feature Share by Retailer"))
                div = 3;
            if (chartName.Equals("Top Subcategories") || chartName.Equals("Subcategory Feature Share by Retailer"))
                div = 4;
            if (chartName.Equals("Top Manufacturers") || chartName.Equals("Manufacturer Feature Share by Retailer"))
                div = 5;
            if (chartName.Equals("Top Brands") || chartName.Equals("Brand Feature Share by Retailer"))
                div = 6;

            Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/div[contains(@id,'chart')]/div/*[name()='svg']/*[@class='highcharts-legend']//.//*[contains(@class,'highcharts')]"), "All legends not present for chart.");
            IList<IWebElement> legendCollections = driver.FindElements(By.XPath("//*[@id='key-metrics-creative-carousel']/div/div[" + div + "]/div[" + chartDiv + "]/chart/div[contains(@id,'chart')]/div/*[name()='svg']/*[@class='highcharts-legend']//.//*[contains(@class,'highcharts')]"));
            string selectedLegendName = "";
            if (legendName.Equals("Random"))
            {
                Random rand = new Random();
                int x = rand.Next(0, legendCollections.Count);
                selectedLegendName = legendCollections[x].Text;
                legendCollections[x].Click();
                Thread.Sleep(500);
                IList<IWebElement> elment = legendCollections[x]._findElementsWithinElement("xpath", ".//*[name()='rect']");
                Assert.AreEqual("#93A2AD", elment[0].GetAttribute("fill"), "'" + selectedLegendName + "' Legent not UnSelected.");
                Results.WriteStatus(test, "Pass", "Unslected and Verified '" + selectedLegendName + "' Legend from Chart.");
            }
            else
            {
                for (int i = 0; i < legendCollections.Count; i++)
                {
                    if (legendCollections[i].Text == legendName)
                    {
                        IList<IWebElement> elment = legendCollections[i]._findElementsWithinElement("xpath", ".//*[name()='rect']");
                        if (unSelect)
                        {
                            if (elment[0].GetAttribute("fill").Contains("#93A2AD") == false)
                            {
                                selectedLegendName = legendCollections[i].Text;
                                legendCollections[i].Click();
                                Thread.Sleep(500);
                                Assert.AreEqual("#93A2AD", elment[0].GetAttribute("fill"), "'" + selectedLegendName + "' Legent not UnSelected.");
                                Results.WriteStatus(test, "Pass", "Unslected and Verified '" + selectedLegendName + "' Legend from Chart.");
                            }
                        }
                        else
                        {
                            if (elment[0].GetAttribute("fill").Contains("#93A2AD") == true)
                            {
                                selectedLegendName = legendCollections[i].Text;
                                legendCollections[i].Click();
                                Thread.Sleep(500);
                                Assert.AreNotEqual("#93A2AD", elment[0].GetAttribute("fill"), "'" + selectedLegendName + "' Legent not Selected.");
                                Results.WriteStatus(test, "Pass", "Selected and Verified '" + selectedLegendName + "' Legend from Chart.");
                            }
                        }
                        break;
                    }
                }
            }

            return selectedLegendName;
        }

        #endregion

        #region View Actions Section

        /// <summary>
        /// Verify Action Button on View Section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyActionButtonOnViewSection()
        {
            Assert.IsTrue(driver._isElementPresent("id", "affixViewActions"), "View Action Section not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions']/button"), "Action Button not Present on Section.");
            IList<IWebElement> actionCollection = driver.FindElements(By.XPath("//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions']/button"));
            string[] buttonNames = { "Export All", "View Selected", "Reset Selected", "Field Options" };
            string[] buttonVisiblity = { null, "true", "true", null };

            for (int i = 0; i < actionCollection.Count; i++)
            {
                Assert.IsTrue(actionCollection[i].Text.Contains(buttonNames[i]), "'" + buttonNames[i] + "' Button not Present.");
                //Assert.AreEqual(actionCollection[i].GetAttribute("disabled"), buttonVisiblity[i], "'" + buttonNames[i] + "' Button not " + buttonVisiblity[i] + " Present.");
            }

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='affixViewActions']//.//button/i[contains(@ng-class,'viewIcon')]"), "'Details View' Button not Present on Section.");
            Results.WriteStatus(test, "Pass", "Verified, Action Button on View Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Button on View Action Section
        /// </summary>
        /// <param name="buttonName">Button Name for click</param>
        /// <returns></returns>
        public PromoDashboard clickButtonOnViewActionSection(string buttonName)
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
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Detail View Button and Verify options to click
        /// </summary>
        /// <param name="optionName">Option Name for Click</param>
        /// <returns></returns>
        public PromoDashboard clickDetailViewButtonAndVerifyOptionsToClick(string optionName = "")
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

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Table View Sectipn on screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyTableViewSectionOnScreen()
        {
            driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Loading')]");
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ref='centerRow']", 10), "Grid not Present for Table View Screen.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']"), "Ad Thumbnail not Present for Table View Screen.");
            Results.WriteStatus(test, "Pass", "Verified, Table View section on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Details View Section on Screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyDetailsViewSectionOnScreen()
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
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Map')]"), "'Map' Button not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Stores')]"), "'Stores' Button not Present for Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//div[contains(@id,'cft-detail-view')]//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-detail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Details')]"), "'Details' Button not Present for Ad Block.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Details View section on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Thumbnail View Section on Screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyThumbnailViewSectionOnScreen()
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
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Stores')]"), "'Stores' Button not Present for Record " + (i + 1) + " on Ad Block.");
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][" + (i + 1) + "]/div[@class='aditem aditem-thumbnail-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'Details')]"), "'Details' Button not Present for Record " + (i + 1) + " on Ad Block.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Details Thumbnail section on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Grid Section for Table View
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyGridSectionForTableView()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//.//div[@class='ag-header-container']/div[@class='ag-header-row']"), "Column Header not Present on Grid.");
            IList<IWebElement> gridRecords = driver.FindElements(By.XPath("//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div"));
            for (int i = 0; i < gridRecords.Count; i++)
                if (driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[" + (i + 1) + "]//.//span[@class='ag-selection-checkbox']"))
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[" + (i + 1) + "]//.//span[@class='ag-selection-checkbox']"), "Checkbox not Present on Grid for [" + (i + 1) + "] Record number.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']"), "Pagination Section not Present.");
            Results.WriteStatus(test, "Pass", "Verified, Grid Section for Table View.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Select Record from Grid and Verify Thumbnail Record
        /// </summary>
        /// <returns></returns>
        public PromoDashboard selectRecordFromGridAndVerifyThumbnailRecord()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//*[@ref='center']//.//div[contains(@class,'ag-body')]/div[@class='ag-body-viewport-wrapper']/div/div/div", 20), "Grid Proper not Loading.");
            IList<IWebElement> gridRecords = driver.FindElements(By.XPath("//*[@ref='center']//.//div[contains(@class,'ag-body')]/div[@class='ag-body-viewport-wrapper']/div/div/div"));
            Random rand = new Random();
            int x = rand.Next(0, gridRecords.Count);

            IList<IWebElement> element = gridRecords[x]._findElementsWithinElement("xpath", ".//div");
            string[,] gridColIds = new string[element.Count, 2];
            for (int i = 0; i < element.Count; i++)
            {
                gridColIds[i, 0] = element[i].GetAttribute("colid").Replace(".", "-").Replace("/", "-");
                gridColIds[i, 1] = element[i].Text;
            }

            IList<IWebElement> adImageDetails = driver.FindElements(By.XPath("//table[@class='table table-details-content']/tbody/tr"));
            for (int j = 0; j < adImageDetails.Count; j++)
            {
                Console.WriteLine("\nadImageDetails.Count : " + adImageDetails.Count);
                Console.WriteLine("gridColIds.GetLength(1) : " + gridColIds.GetLength(1));
                for (int c = 0; c < gridColIds.GetLength(1); c++)
                {
                    Console.WriteLine("\nadImageDetails[j].GetAttribute('class') : " + adImageDetails[j].GetAttribute("class"));
                    Console.WriteLine("gridColIds[c, 0] : " + gridColIds[c, 0]);
                    Console.WriteLine("gridColIds[c, 1] : " + gridColIds[c, 1]);
                    Console.WriteLine("adImageDetails[j].Text : " + adImageDetails[j].Text);
                    if (adImageDetails[j].GetAttribute("class").ToLower().Contains(gridColIds[c, 0].ToLower()))
                    {
                        Assert.AreEqual(true, adImageDetails[j].Text.Contains(gridColIds[c, 1]), "'" + gridColIds[c, 1] + "' Name not match.");
                        Console.WriteLine("Verified.");
                        break;
                    }
                }
            }

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Pagination Panel for Table view Section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyPaginationPanelForViewSection(string viewName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']"), "Pagination Section not Present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'row cft-pagination-wrapper')]//.//li[contains(@class,'pagination-first') and contains(@class,'disabled')]"), "First Icon Default Disable not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'row cft-pagination-wrapper')]//.//li[contains(@class,'pagination-prev') and contains(@class,'disabled')]"), "Previous Icon Default Disable not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'row cft-pagination-wrapper')]//.//li[contains(@class,'pagination-page') and contains(@class,'active')]"), "Actice First Page not Present.");
            if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[contains(@class,'pagination-next')]") == false)
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[contains(@class,'pagination-next') and contains(@class,'disabled')]"), "Next Icon not Present for Grid.");
            if (driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[contains(@class,'pagination-last')]") == false)
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//li[contains(@class,'pagination-last') and contains(@class,'disabled')]"), "Last Icon not Present for Grid.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'10')]"), "Item Per Page '10' not Present.");
            if (viewName == "Table View")
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'25')]"), "Item Per Page '25' not Present.");
            else
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'20')]"), "Item Per Page '20' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'50')]"), "Item Per Page '50' not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row cft-pagination-wrapper']//.//button[contains(text(),'100')]"), "Item Per Page '100' not Present.");

            Results.WriteStatus(test, "Pass", "Verified, Pagination Panel for '" + viewName + "' Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Page Number and Icon from Grid
        /// </summary>
        /// <param name="pageIcon">Page Icon for Click</param>
        /// <returns></returns>
        public PromoDashboard clickPageNumberAndIconFromGrid(string pageIcon = "Page Number")
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

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Thumbnail Section on Screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyThumbnailSectionOnScreen()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']"), "Ad Thumbnail not Present for Table View Screen.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//img"), "Ad Image not Present on Section.");
            IWebElement image = driver._findElement("xpath", "//div[@class='aditem aditem-long']//.//img");
            bool loaded = Convert.ToBoolean(((IJavaScriptExecutor)driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));
            Assert.AreEqual(true, loaded, "'(" + image.GetAttribute("src") + ")' Image Not Load on Thumbnail Section.");

            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//div[@class='detail-view-content']"), "Detail View section not Present on Ad Image Section.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'View Ad')]"), "View Ad Icon not Present on Ad Image.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Stores')]"), "Stores Icon not Present on Ad Image.");
            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'Details')]"), "Details Icon not Present on Ad Image.");

            Results.WriteStatus(test, "Pass", "Verified, Thumbnail Section on Screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Button on Thumbnails Section
        /// </summary>
        /// <param name="buttonName">Button name to click</param>
        /// <returns></returns>
        public PromoDashboard clickButtonOnViewSection(string buttonName, string viewName)
        {
            if (viewName.Equals("Table View"))
            {
                if (buttonName.Equals("Ad Image"))
                {
                    Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//img"), "Ad Image not Present on Section.");
                    driver._clickByJavaScriptExecutor("//div[@class='aditem aditem-long']//.//img");
                }

                if (buttonName.Equals("View Ad") || buttonName.Equals("Stores") || buttonName.Equals("Details"))
                {
                    Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='aditem aditem-long']//.//button[contains(text(),'" + buttonName + "')]"), "'" + buttonName + "' Icon not Present on Ad Image.");
                    driver._clickByJavaScriptExecutor("//div[@class='aditem aditem-long']//.//button[contains(text(),'" + buttonName + "')]");
                }
            }
            string viewId = "";
            string viewState = "";
            if (viewName.Equals("Details View"))
            { viewId = "2"; viewState = "detail"; }

            if (viewName.Equals("Thumbnail View"))
            { viewId = "4"; viewState = "thumbnail"; }

            if (viewName.Equals("Details View") || viewName.Equals("Thumbnail View"))
            {
                if (buttonName.Equals("Ad Image"))
                {
                    driver.MouseHoverUsingElement("xpath", "//*[@id='cft-detail-view-" + viewId + "']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//img[contains(@class,'aditem-image-layout')]");
                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-" + viewId + "']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//img[contains(@class,'aditem-image-layout')]"), "Ad Image not Present on Section.");
                    driver._clickByJavaScriptExecutor("//*[@id='cft-detail-view-" + viewId + "']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//img[contains(@class,'aditem-image-layout')]");
                }

                if (buttonName.Equals("View Ad") || buttonName.Equals("Stores") || buttonName.Equals("Details") || buttonName.Equals("Map"))
                {
                    driver.MouseHoverUsingElement("xpath", "//*[@id='cft-detail-view-" + viewId + "']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'" + buttonName + "')]");
                    Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-" + viewId + "']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'" + buttonName + "')]"), "'" + buttonName + "' Icon not Present on " + viewName + " Section.");
                    driver._clickByJavaScriptExecutor("//*[@id='cft-detail-view-" + viewId + "']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-" + viewState + "-view']//.//div[contains(@class,'row custom-btn-default-wrapper')]//.//button[contains(text(),'" + buttonName + "')]");
                }
            }

            Thread.Sleep(1000);
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' on '" + viewName + "' Section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Select Record from View Section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard selectRecordFromViewSection()
        {
            if (driver._isElementPresent("xpath", "//*[@id='cft-detail-view-2']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-detail-view']") == true)
            {
                Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-2']//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-detail-view']//.//div[@class='row checkbox checkbox-header']/label"), "Header not Present for First Ad Block.");
                driver._scrollintoViewElement("xpath", "//*[@id='cft-detail-view-2']//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-detail-view']//.//div[@class='row checkbox checkbox-header']/label");
                driver._clickByJavaScriptExecutor("//*[@id='cft-detail-view-2']//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-detail-view']//.//div[@class='row checkbox checkbox-header']/label");
            }
            else
                if (driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//.//div[@class='ag-header-container']/div[@class='ag-header-row']") == true)
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='borderLayout_eGridPanel']//.//div[@class='ag-pinned-left-cols-container']/div[1]//.//span[@class='ag-selection-checkbox']"), "Checkbox not Present on Grid for First Record number.");
                    driver._scrollintoViewElement("xpath", "//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[1]//.//span[@class='ag-selection-checkbox']");
                    driver._clickByJavaScriptExecutor("//*[@id='borderLayout_eGridPanel']//./div[@class='ag-pinned-left-cols-container']/div[1]//.//span[@class='ag-selection-checkbox']/i[contains(@class,'fa-square')]");
                }
                else
                    if (driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')]/div[@class='aditem aditem-thumbnail-view']") == true)
                    {
                        Assert.IsTrue(driver._isElementPresent("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-thumbnail-view']//.//div[@class='row checkbox checkbox-header']"), "Header not Present for First Record on Ad Block.");
                        driver._scrollintoViewElement("xpath", "//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-thumbnail-view']//.//div[@class='row checkbox checkbox-header']");
                        driver._clickByJavaScriptExecutor("//*[@id='cft-detail-view-4']//.//div[contains(@ng-repeat,'item')][1]/div[@class='aditem aditem-thumbnail-view']//.//div[@class='row checkbox checkbox-header']");
                    }

            Thread.Sleep(1000);
            Results.WriteStatus(test, "Pass", "Selected Record from View Section");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Button Disable or Not on Screen
        /// </summary>
        /// <param name="buttonName">Button Name to verify</param>
        /// <param name="Disabled">to verify button Disable state</param>
        /// <returns></returns>
        public PromoDashboard verifyButtonDisableOrNotOnScreen(string buttonName, bool Disabled = true)
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
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify View Selected Button Checked or not on Screen
        /// </summary>
        /// <param name="Checked"></param>
        /// <returns></returns>
        public PromoDashboard verifyViewSelectedButtonCheckedOrNotOnScreen(bool Checked = false)
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
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Fields options section on Dashboard screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyFieldsOptionsSectionOnDashboardScreen()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='CFT-view-customizer']", 20), "Fields Options Section not Present on screen.");

            IList<IWebElement> fields = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']"));
            string[] fieldsHeader = { "About Field Options", "Hidden Fields", "Visible Fields" };
            for (int i = 0; i < fields.Count; i++)
            {
                Assert.AreEqual(true, fields[i].Text.Contains(fieldsHeader[i]), "'" + fieldsHeader[i] + "' Header not match with '" + fields[i].Text + "' Header.");
                if (fieldsHeader[i] == "Visible Fields")
                {
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button"), "'Reset Fields' Button not Present.");
                    Assert.AreEqual(true, driver._getText("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button").Contains("Reset Fields"), "'Reset Fields' not match.");
                    Assert.AreEqual("true", driver._getAttributeValue("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button", "disabled"), "'Reset Fields' Button not Disable.");
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Fields options section on Dashboard screen.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Reset Fields button from Visible Fields section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard clickResetFieldsButtonFromVisibleFieldsSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button"), "'Reset Fields' Button not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button").Contains("Reset Fields"), "'Reset Fields' not match.");
            Assert.AreEqual(null, driver._getAttributeValue("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button", "disabled"), "'Reset Fields' Button not Enable.");
            driver._clickByJavaScriptExecutor("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']//.//button");
            Thread.Sleep(500);
            Results.WriteStatus(test, "Pass", "Clicked, Reset Fields button from Visible Fields section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify About Field Option in Fields options section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyAboutFieldOptionInFieldsOptionsSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']/div[text()='About Field Options']"), "'About Field Options' header not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='row view-customizer-help']"), "'About Field Options' content not Present.");
            Assert.AreEqual("To show a hidden field select or just drag it into 'Visible Fields'Hide a visible field by selecting or by dragging it back into 'Hidden Fields'Reorder columns in search results by using the and , or by dragging and droppingAdjust search result sort order usingReset changes by clicking 'Reset Fields' at the top", driver._getText("xpath", "//div[@class='row view-customizer-help']").Trim().Replace("\r\n", ""), "'About Field Options' content not match.");
            Results.WriteStatus(test, "Pass", "Verified, About Field Options on Field options section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Hidden Fields in fields options section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyHiddenFieldsInFieldsOptionsSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']/div[text()='Hidden Fields']"), "'Hidden Fields' header not present.");
            IList<IWebElement> sections = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']"));

            for (int i = 0; i < sections.Count; i++)
            {
                IWebElement headerName = sections[i].FindElement(By.XPath(".//div[@class='row view-customizer-header']"));
                if (headerName.Text.Contains("Hidden Fields"))
                {
                    IList<IWebElement> listCollections = sections[i]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                    for (int j = 0; j < listCollections.Count; j++)
                    {
                        Assert.AreEqual(true, listCollections[j].FindElement(By.XPath(".//span[@class='view-customizer-label']")).Displayed, "Field Name not Present for Record.");
                        string fieldName = listCollections[j].FindElement(By.XPath(".//span[@class='view-customizer-label']")).Text;
                        Assert.AreEqual(true, listCollections[j].FindElement(By.XPath(".//button[@class='btn btn-default btn-add-field']/i[@class='fa fa-plus-circle']")).Displayed, "+ Sign not Present for '" + fieldName + "' Field Value.");
                    }
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Hidden Fields in Field options section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click Field Icon and Verify field name on Field options
        /// </summary>
        /// <param name="plusIcon">Click Plus icon from Hidden Field</param>
        /// <returns></returns>
        public String clickFieldIconAndVerifyFieldNameOnFieldsOptions(bool plusIcon)
        {
            string fieldValue = "";
            bool avail = false;

            IList<IWebElement> sections = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']"));

            if (plusIcon)
            {
                IList<IWebElement> hiddenLists = sections[1]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                Random rand = new Random();
                int x = rand.Next(0, hiddenLists.Count);

                Assert.AreEqual(true, hiddenLists[x].FindElement(By.XPath(".//span[@class='view-customizer-label']")).Displayed, "Field Name not Present for Record.");
                fieldValue = hiddenLists[x].FindElement(By.XPath(".//span[@class='view-customizer-label']")).Text;
                Assert.AreEqual(true, hiddenLists[x].FindElement(By.XPath(".//button[@class='btn btn-default btn-add-field']/i[@class='fa fa-plus-circle']")).Displayed, "+ Sign not Present for '" + fieldValue + "' Field Value.");
                hiddenLists[x].FindElement(By.XPath(".//button[@class='btn btn-default btn-add-field']/i[@class='fa fa-plus-circle']")).Click();
                Thread.Sleep(1000);

                IList<IWebElement> visibleFieldsLists = sections[2]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                for (int i = 0; i < visibleFieldsLists.Count; i++)
                {
                    if (visibleFieldsLists[i].Text.Contains(fieldValue))
                    {
                        avail = true;
                        break;
                    }
                }

                Assert.AreEqual(true, avail, "'" + fieldValue + "' Field Value not Present on Visible Field Option.");
                Results.WriteStatus(test, "Pass", "Clicked, Plus Icon from Hidden Field and Verified Field Value on Visible Fields Option.");
            }
            else
            {
                sections = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']"));
                IList<IWebElement> visibleFieldsLists = sections[2]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                Random rand = new Random();
                int x = rand.Next(0, visibleFieldsLists.Count);

                Assert.AreEqual(true, visibleFieldsLists[x].FindElement(By.XPath(".//th")).Displayed, "Field Name not Present for Record.");
                fieldValue = visibleFieldsLists[x].FindElement(By.XPath(".//th")).Text.Trim().Replace("\r\n", "");
                Assert.AreEqual(true, visibleFieldsLists[x].FindElement(By.XPath(".//button[@class='btn btn-default btn-remove-field']/i[@class='fa fa-minus-circle']")).Displayed, "- Sign not Present for '" + fieldValue + "' Field Value.");
                visibleFieldsLists[x].FindElement(By.XPath(".//button[@class='btn btn-default btn-remove-field']/i[@class='fa fa-minus-circle']")).Click();
                Thread.Sleep(1000);

                IList<IWebElement> hiddenLists = sections[1]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                for (int i = 0; i < hiddenLists.Count; i++)
                {
                    if (hiddenLists[i].Text.Contains(fieldValue))
                    {
                        avail = true;
                        break;
                    }
                }

                Assert.AreEqual(true, avail, "'" + fieldValue + "' Field Value not Present on Hidden Field Option.");
                Results.WriteStatus(test, "Pass", "Clicked, Minus Icon from Visible Fields and Verified Field Value on Hidde Fields Option.");
            }
            return fieldValue;
        }

        /// <summary>
        /// Verify Visible Fields in fields options section
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyVisibleFieldsInFieldsOptionsSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']/div[text()='Hidden Fields']"), "'Hidden Fields' header not present.");
            IList<IWebElement> sections = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']"));

            for (int i = 0; i < sections.Count; i++)
            {
                IWebElement headerName = sections[i].FindElement(By.XPath(".//div[@class='row view-customizer-header']"));
                if (headerName.Text.Contains("Visible Fields"))
                {
                    IList<IWebElement> listCollections = sections[i]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                    for (int j = 0; j < listCollections.Count; j++)
                    {
                        Assert.AreEqual(true, listCollections[j].FindElement(By.XPath(".//th")).Displayed, "Field Name not Present for Record.");
                        string fieldName = listCollections[j].FindElement(By.XPath(".//th")).Text;
                        Assert.AreEqual(true, listCollections[j].FindElement(By.XPath(".//button[@class='btn btn-default btn-remove-field']/i[@class='fa fa-minus-circle']")).Displayed, "- Minus not Present for '" + fieldName + "' Field Value.");
                        Assert.AreEqual(true, listCollections[j].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']/i[@class='fa fa-arrow-up']")).Displayed, "Up Arrow not present for '" + fieldName + "' Field Value .");
                        Assert.AreEqual(true, listCollections[j].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']/i[@class='fa fa-arrow-down']")).Displayed, "Down Arrow not present for '" + fieldName + "' Field Value .");
                        if (listCollections[j].FindElement(By.XPath(".//button[@class='btn btn-default btn-sort-field']")).Displayed)
                            Assert.AreEqual(true, listCollections[j].FindElement(By.XPath(".//button[@class='btn btn-default btn-sort-field']")).Displayed, "Sorting Arrow not present for '" + fieldName + "' Field Value .");
                    }
                    break;
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Visible Fields in Field options section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify or Get Default Fields in Visible Fields section
        /// </summary>
        /// <param name="fieldsValue">To Verify Fields Value</param>
        /// <param name="verify">Verify Value or Get Value</param>
        /// <returns></returns>
        public String[] verifyOrGetDefaultFieldsInVisibleFieldsSection(string[] fieldsValue, bool verify)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']/div[contains(text(),'Visible Fields')]"), "'Visible Fields' header not present.");
            IList<IWebElement> sections = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']"));

            IList<IWebElement> visibleLists = sections[2]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
            int cnt = 0;

            if (verify)
            {
                for (int i = 0; i < visibleLists.Count; i++)
                {
                    for (int j = 0; j < fieldsValue.Length; j++)
                    {
                        if (visibleLists[i].Text.Contains(fieldsValue[j]))
                        {
                            cnt++;
                            break;
                        }
                    }
                }
                Assert.AreEqual(cnt, fieldsValue.Length, "List Collections not match with Default Collection.");
                Results.WriteStatus(test, "Pass", "Verified, Fields collection in Visible Fields Section.");
            }
            else
            {
                fieldsValue = new string[visibleLists.Count];
                for (int i = 0; i < visibleLists.Count; i++)
                {
                    string fieldName = visibleLists[i].FindElement(By.XPath(".//th")).Text.Trim().Replace("\r\n", "");
                    fieldsValue[i] = fieldName;
                }
                Results.WriteStatus(test, "Pass", "Get, Fields collection from Visible Fields Section.");
            }

            return fieldsValue;

        }

        /// <summary>
        /// Click on Sorting icon for field on Fields Section
        /// </summary>
        /// <param name="sortingOrder">Sorting order</param>
        /// <returns></returns>
        public String clickOnSortingIconForFieldOnFieldsSection(string sortingOrder)
        {
            IList<IWebElement> visibleFieldsLists = driver.FindElements(By.XPath("//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block']"));
            string sortingFieldName = "";

            for (int i = 0; i < visibleFieldsLists.Count; i++)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']"), "Sorting Arrow not present for Field Value .");

                if (sortingOrder.Equals("Descending"))
                    if (driver._isElementPresent("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort']") == true)
                    {
                        driver._clickByJavaScriptExecutor("//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort']");
                        Thread.Sleep(500);
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort detail-view-sorted']//.//i[@class='fa fa-sort-desc']"));
                        sortingFieldName = driver._getText("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]").Trim().Replace("\r\n", "").Replace("1", "");
                        break;
                    }

                if (sortingOrder.Equals("Ascending"))
                    if (driver._isElementPresent("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort detail-view-sorted']//.//i[@class='fa fa-sort-desc']") == true)
                    {
                        driver._clickByJavaScriptExecutor("//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort detail-view-sorted']");
                        Thread.Sleep(500);
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort detail-view-sorted']//.//i[@class='fa fa-sort-asc']"));
                        sortingFieldName = driver._getText("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]").Trim().Replace("\r\n", "").Replace("1", "");
                        break;
                    }

                if (sortingOrder.Equals("Default"))
                    if (driver._isElementPresent("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort detail-view-sorted']//.//i[@class='fa fa-sort-asc']") == true)
                    {
                        driver._clickByJavaScriptExecutor("//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort detail-view-sorted']");
                        Thread.Sleep(500);
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]//.//button[@class='btn btn-default btn-sort-field']/div[@class='detail-view-sort detail-view-sorted']//.//i[@class='fa fa-sort']"));
                        sortingFieldName = driver._getText("xpath", "//tbody[@dnd-list='domainViewCustomizerCtrl.activeFields']/tr[@class='btn btn-default btn-block'][" + (i + 1) + "]").Trim().Replace("\r\n", "").Replace("1", "");
                        break;
                    }
            }

            Results.WriteStatus(test, "Pass", "Clicked, On Sorting Icon of '" + sortingFieldName + "' Field on Fields Section.");
            return sortingFieldName;
        }

        /// <summary>
        /// Verify Sorted Fields Records on Section
        /// </summary>
        /// <param name="fieldName">Field Name to verify on Section</param>
        /// <param name="sortingOrder">Sorting Order</param>
        /// <returns></returns>
        public PromoDashboard verifySortedFieldsRecordsOnSection(string fieldName, string sortingOrder)
        {
            IList<IWebElement> ItemCollections = driver.FindElements(By.XPath("//div[@ng-switch='domainItemDetailsCtrl.config.viewStyle']/div[@dynamic-class='dynamic-field']"));
            for (int i = 0; i < ItemCollections.Count; i++)
            {
                IList<IWebElement> itemDetails = ItemCollections[i]._findElementsWithinElement("xpath", ".//table[@class='table table-details-content']/tbody/tr");
                for (int j = 0; j < itemDetails.Count; j++)
                {
                    if (itemDetails[j].Text.Contains(fieldName))
                    {
                        if (i != 0)
                        {
                            string value = driver._getText("xpath", "//div[@ng-switch='domainItemDetailsCtrl.config.viewStyle']/div[@dynamic-class='dynamic-field'][" + i + "]//.//table[@class='table table-details-content']/tbody/tr[" + (j + 1) + "]/td");
                            string nextValue = driver._getText("xpath", "//div[@ng-switch='domainItemDetailsCtrl.config.viewStyle']/div[@dynamic-class='dynamic-field'][" + (i + 1) + "]//.//table[@class='table table-details-content']/tbody/tr[" + (j + 1) + "]/td");
                            if (sortingOrder.Equals("Descending"))
                                Assert.GreaterOrEqual(value, nextValue);
                            if (sortingOrder.Equals("Ascending"))
                                Assert.LessOrEqual(value, nextValue);
                        }
                        break;
                    }
                }
            }

            Results.WriteStatus(test, "Pass", "Verified, Sorted '" + fieldName + "' Fields Records on section.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click on Sign for any Field on Visible fields section
        /// </summary>
        /// <param name="upwardSign">Click Upward Sign</param>
        /// <returns></returns>
        public PromoDashboard clickOnSignForAnyFieldOnVisibleFieldsSection(bool upwardSign)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']//.//div[@class='row view-customizer-header']/div[contains(text(),'Visible Fields')]"), "'Visible Fields' header not present.");
            IList<IWebElement> sections = driver.FindElements(By.XPath("//div[@class='CFT-view-customizer']//.//div[@class='CFT-view-customizer-section']"));
            IList<IWebElement> visibleLists = sections[2]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
            for (int i = 0; i < visibleLists.Count; i++)
            {
                if (upwardSign)
                {
                    Assert.AreEqual(true, visibleLists[i + 1].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']/i[@class='fa fa-arrow-up']")).Displayed, "Up Arrow not present for Field Value .");
                    if (visibleLists[i + 1].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']")).GetAttribute("disabled") == null)
                    {
                        string valueName = visibleLists[i + 1].FindElement(By.XPath(".//th")).Text;
                        visibleLists[i + 1].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']/i[@class='fa fa-arrow-up']")).Click();
                        Thread.Sleep(1000);
                        visibleLists = sections[2]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                        string firstValue = visibleLists[i].FindElement(By.XPath(".//th")).Text;
                        Assert.AreEqual(valueName, firstValue, "Value on Upward not Move.");
                        Results.WriteStatus(test, "Pass", "Clicked, Sign for Field on Visible section and Verified.");
                        break;
                    }
                }
                else
                {
                    Assert.AreEqual(true, visibleLists[i].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']/i[@class='fa fa-arrow-down']")).Displayed, "Down Arrow not present for Field Value .");
                    if (visibleLists[i].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']")).GetAttribute("disabled") == null)
                    {
                        string valueName = visibleLists[i].FindElement(By.XPath(".//th")).Text;
                        visibleLists[i].FindElement(By.XPath(".//button[@class='btn btn-default btn-move-field']/i[@class='fa fa-arrow-down']")).Click();
                        Thread.Sleep(1000);
                        visibleLists = sections[2]._findElementsWithinElement("xpath", ".//tr[@class='btn btn-default btn-block']");
                        string firstValue = visibleLists[i + 1].FindElement(By.XPath(".//th")).Text;
                        Assert.AreEqual(valueName, firstValue, "Value on Downward not Move.");
                        Results.WriteStatus(test, "Pass", "Clicked, Sign for Field on Visible section and Verified.");
                        break;
                    }
                }
            }

            return new PromoDashboard(driver, test);
        }

        #endregion

        #region Export All Functionality

        /// <summary>
        /// Verify Exort All Section on Dashboard Screen
        /// </summary>
        /// <returns></returns>
        public PromoDashboard verifyExportAllSectionOnDashboardScreen()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']", 20), "Export All Section not Present on screen.");

            IList<IWebElement> sections = driver.FindElements(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div"));
            string[] sectionNames = { "Data Reports", "Power Point Reports", "Asset Downloads" };
            for (int i = 0; i < sections.Count; i++)
                Assert.AreEqual(sectionNames[i], driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='row view-customizer-header']"), "'" + sectionNames + "' Header not Present or match.");

            Results.WriteStatus(test, "Pass", "Verified, Export All section on Dashboard screen.");
            return new PromoDashboard(driver, test);
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
        /// Verify File Downloaded Or Not for Ad Sharing And Exclusivity Screen
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="FileType"></param>
        /// <returns></returns>
        public PromoDashboard verifyFileDownloadedOrNotOnScreen(string fileName, string FileType)
        {
            bool Exist = false;
            string FilePath = "";
            string Path = ExtentManager.ResultsDir;
            string[] filePaths = Directory.GetFiles(Path, FileType);

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

            Assert.AreEqual(true, Exist, "'" + FileType.Replace("*", "").ToUpper() + "'" + fileName + "' File Not Exported Properly.");
            Results.WriteStatus(test, "Pass", "Verified, <b>'" + FileType.Replace("*", "").ToUpper() + "'</b> File Exported Properly for '" + fileName + "' Report File.");
            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Tooltip functionaltity for Reports Section
        /// </summary>
        /// <param name="reportSection">Report Section Name</param>
        /// <param name="option">Option Name of Report</param>
        /// <param name="content">Verify Content of tooltip</param>
        /// <returns></returns>
        public PromoDashboard verifyTiooltipFunctionalityForReportsSection(string reportSection, string option, string content = "")
        {
            IList<IWebElement> sections = driver.FindElements(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div"));
            string reportName = "";

            IList<IWebElement> actionCollection = driver.FindElements(By.XPath("//*[@id='affixViewActions']//.//div[@class='btn-group btn-grid-actions']/button"));
            for (int i = 0; i < actionCollection.Count; i++)
            {
                if (actionCollection[i].Text.Contains("Export"))
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", actionCollection[i]);
                    break;
                }
            }

            for (int i = 0; i < sections.Count; i++)
            {
                if (reportSection.Contains(driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='row view-customizer-header']")) == true)
                {
                    IList<IWebElement> lists = driver.FindElements(By.XPath("//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div"));

                    if (option.Equals("Download"))
                    {
                        Random rand = new Random();
                        int x = rand.Next(0, lists.Count);
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'download')]"), "Download Button not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                        Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                        reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]");

                        if (content.Equals(""))
                            content = "Download";
                        driver.MouseHoverUsingElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'download')]");
                        Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='tooltip-inner']", 20), "'" + content + "' tooltip not present.");
                        Assert.AreEqual(true, driver._getText("xpath", "//div[@class='tooltip-inner']").Contains(content), "Tooltip not Present with '" + content + "' Label.");
                        Results.WriteStatus(test, "Pass", "Verified, '" + option + "' Tooltip for '" + reportName + "' Report for '" + reportSection + "' Section.");
                        break;
                    }
                    else
                        if (option.Equals("Email"))
                        {
                            Random rand = new Random();
                            int x = rand.Next(0, lists.Count);
                            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'envelope')]"), "Email Button not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                            Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                            reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]");

                            if (content.Equals(""))
                                content = "Email";
                            driver.MouseHoverUsingElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'envelope')]");
                            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='tooltip-inner']", 20), "'" + content + "' tooltip not present.");
                            Assert.AreEqual(true, driver._getText("xpath", "//div[@class='tooltip-inner']").Contains(content), "Tooltip not Present with '" + content + "' Label.");
                            Results.WriteStatus(test, "Pass", "Verified, '" + option + "' Tooltip for '" + reportName + "' Report for '" + reportSection + "' Section.");
                            break;
                        }
                        else
                            if (option.Equals("Ban"))
                            {
                                Random rand = new Random();
                                int x = rand.Next(0, lists.Count);
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'ban')]"), "Download Button not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                                reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]");

                                if (content.Equals(""))
                                    content = "Ban";
                                driver.MouseHoverUsingElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/div/button/i[contains(@class,'ban')]");
                                Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='tooltip-inner']", 20), "'" + content + "' tooltip not present.");
                                Assert.AreEqual(true, driver._getText("xpath", "//div[@class='tooltip-inner']").Contains(content), "Tooltip not Present with '" + content + "' Label.");
                                Results.WriteStatus(test, "Pass", "Verified, '" + option + "' Tooltip for '" + reportName + "' Report for '" + reportSection + "' Section.");
                                break;
                            }
                            else
                            {
                                Random rand = new Random();
                                int x = rand.Next(0, lists.Count);
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/cft-scheduled-export-popover"), "Schedule Icon not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                                Assert.AreEqual(true, driver._isElementPresent("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]"), "Reports Name not Present for Record number [" + (i + 1) + "] for '" + reportSection + "'.");
                                reportName = driver._getText("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[1]/div[2]");

                                if (content.Equals(""))
                                    content = "Schedule";
                                driver.MouseHoverUsingElement("xpath", "//cft-domain-item-export-customizer//.//div[@class='CFT-view-customizer']/div/div[" + (i + 1) + "]//.//div[@class='list-group list-group-flex list-group-export']/div[" + (x + 1) + "]/div[2]/cft-scheduled-export-popover");
                                Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='tooltip-inner']", 20), "'" + content + "' tooltip not present.");
                                Assert.AreEqual(true, driver._getText("xpath", "//div[@class='tooltip-inner']").Contains(content), "Tooltip not Present with '" + content + "' Label.");
                                Results.WriteStatus(test, "Pass", "Verified, '" + option + "' Tooltip for '" + reportName + "' Report for '" + reportSection + "' Section.");
                                break;
                            }
                }
            }

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Verify Values of Downloaded file with Grid Data
        /// </summary>
        /// <param name="chartName">Chart Name to Verify Data</param>
        /// <param name="dataFromGrid">Grid Records</param>
        /// <returns></returns>
        public PromoDashboard verifyValuesOfDownloadedExclusiveAndSharedAdBlocksFile(string chartName, string[] dataFromGrid)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            string str;
            string FilePath = "";
            chartName = chartName.Replace("(", "").Replace(")", "");

            string sourceDir = ExtentManager.ResultsDir + "\\";
            string[] fileEntries = Directory.GetFiles(sourceDir);

            foreach (string fileName in fileEntries)
            {
                if (fileName.Contains(chartName))
                {
                    FilePath = fileName;
                    break;
                }
            }

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(FilePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            int rw = range.Rows.Count;
            int cl = range.Columns.Count;

            for (int rCnt = 1; rCnt <= rw; rCnt++)
            {
                Console.WriteLine("\n");
                for (int cCnt = 1; cCnt <= cl; cCnt++)
                {
                    var cellVal = (range.Cells[rCnt, cCnt] as Excel.Range).Text;
                    str = cellVal.ToString();

                    Console.WriteLine("String Row [" + rCnt + "] Column [" + cCnt + "]: " + str);

                    if (str.Contains("View Ad"))
                    {
                        Console.WriteLine("Link Address");

                        string cellVal11 = Convert.ToString((range.Cells[rCnt, cCnt] as Excel.Range).Value2);
                        Console.WriteLine("Link Address Value : " + cellVal11);
                    }
                }
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            File.Delete(FilePath);

            Results.WriteStatus(test, "Pass", "Verified, Values of Downloaded File with " + chartName + " Chart.");
            return new PromoDashboard(driver, test);
        }

        #endregion

        /// <summary>
        /// Click on Filter Field and Verify / Click on Options
        /// </summary>
        /// <param name="fieldName">Field Name to Click</param>
        /// <param name="optionName">Option Name for Click</param>
        /// <param name="options">Verify Options Collections</param>
        /// <returns></returns>
        public PromoDashboard filterTest(string fieldName, string optionName = "", string[] options = null)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul/li"), "Filter Options not Present.");
            IList<IWebElement> fieldsCollection = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li"));
            bool avail = false;
            int cnt = 0;

            if (fieldName.Equals("Days"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[contains(@id,'internal_timeframe')]/a"));
                driver._clickByJavaScriptExecutor("//*[contains(@id,'internal_timeframe')]/a");
                Thread.Sleep(500);

                IList<IWebElement> optionsCollections = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//ul/li[1]//.//ul[contains(@class,'insert-ranges')]/li"));
                if (optionName != "")
                    for (int j = 0; j < optionsCollections.Count; j++)
                    {
                        if (optionName == optionsCollections[j].Text)
                        {
                            optionsCollections[j].Click();
                            Thread.Sleep(500);
                            driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20);
                            break;
                        }
                    }
                avail = true;
            }
            else
            {
                string[] filterLabels = { "Days Filter", "Retailers", "Markets", "Categories", "Manufacturers", "Brands", "Offer Types", "Page Locations" };
                string[] filterIDs = { "timeframe", "advertiser", "market", "category", "manufacturerName", "brand", "offerTypeName", "pageTypeName", "field-promo_adblock_eventName" };
                string fieldId = "";

                for (int f = 0; f < filterLabels.Length; f++)
                {
                    if (fieldName.Contains(filterLabels[f]) == true)
                    {
                        fieldId = filterIDs[f];
                        break;
                    }
                }

                for (int i = 0; i < fieldsCollection.Count; i++)
                {
                    if (fieldsCollection[i].GetAttribute("id").ToLower().Contains(fieldId) == true)
                    {
                        fieldsCollection[i].Click();
                        Thread.Sleep(200);
                        Assert.AreEqual("dropdown open", fieldsCollection[i].GetAttribute("class"), "'" + fieldName + "' Field not Open.");
                        IList<IWebElement> optionsCollections = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li[" + (i + 1) + "]//.//ul[contains(@class,'insert-ranges')]/li"));

                        if (options != null)
                            for (int o = 0; o < options.Length; o++)
                            {
                                for (int j = 0; j < optionsCollections.Count; j++)
                                    if (options[o] == optionsCollections[j].Text)
                                        cnt++;
                            }

                        if (optionName != "")
                            for (int j = 0; j < optionsCollections.Count; j++)
                            {
                                if (optionName == optionsCollections[j].Text)
                                {
                                    optionsCollections[j].Click();
                                    Thread.Sleep(500);
                                    driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20);
                                    break;
                                }
                            }

                        avail = true;
                        break;
                    }

                    if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button']/a"))
                        driver._clickByJavaScriptExecutor("//li[@id='side-menu-button' and @class='filter-menu-next-button']/a");
                }
            }

            Assert.AreEqual(true, avail, "'" + fieldName + "' Field not Present.");
            if (options != null)
                Assert.AreEqual(cnt, options.Length, "Options Colletions not match.");

            if (optionName == "")
                Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field and Verified Options.");
            else
                Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field and Verified options & Clicked '" + optionName + "' Option.");

            return new PromoDashboard(driver, test);
        }

        /// <summary>
        /// Click on Filter Field and Verify / Click on Options
        /// </summary>
        /// <param name="fieldName">Field Name to Click</param>
        /// <param name="optionName">Option Name for Click</param>
        /// <param name="options">Verify Options Collections</param>
        /// <returns></returns>
        public PromoDashboard verifyFilterTabAndSelectOptionFromFilterBar(string fieldName, string optionName = "", string[] options = null)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul/li"), "Filter Options not Present.");
            IList<IWebElement> fieldsCollection = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li"));
            bool avail = false;
            int cnt = 0;

            if (fieldName.Equals("Days"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[contains(@id,'internal_timeframe')]/a"));
                driver._clickByJavaScriptExecutor("//*[contains(@id,'internal_timeframe')]/a");
                Thread.Sleep(500);

                IList<IWebElement> optionsCollections = driver.FindElements(By.XPath("//li[@class='dropdown open']//.//ul/li[1]//.//ul[contains(@class,'insert-ranges')]/li"));
                if (optionName != "")
                    for (int j = 0; j < optionsCollections.Count; j++)
                    {
                        if (optionName == optionsCollections[j].Text)
                        {
                            optionsCollections[j].Click();
                            Thread.Sleep(500);
                            driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20);
                            break;
                        }
                    }
                avail = true;
            }
            else
            {
                string[] filterLabels = { "Days Filter", "Retailers", "Markets", "Categories", "Manufacturers", "Brands", "Offer Types", "Page Locations" };
                string[] filterIDs = { "timeframe", "advertiser", "market", "category", "manufacturerName", "brand", "offerTypeName", "pageTypeName", "field-promo_adblock_eventName" };
                string fieldId = "";

                for (int f = 0; f < filterLabels.Length; f++)
                {
                    if (fieldName.Contains(filterLabels[f]) == true)
                    {
                        fieldId = filterIDs[f];
                        break;
                    }
                }

                for (int i = 0; i < fieldsCollection.Count; i++)
                {
                    if (fieldsCollection[i].GetAttribute("id").ToLower().Contains(fieldId) == true)
                    {
                        fieldsCollection[i].Click();
                        Thread.Sleep(200);
                        Assert.AreEqual("dropdown open", fieldsCollection[i].GetAttribute("class"), "'" + fieldName + "' Field not Open.");
                        IList<IWebElement> optionsCollections = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li[" + (i + 1) + "]//.//ul[contains(@class,'insert-ranges')]/li"));

                        if (options != null)
                            for (int o = 0; o < options.Length; o++)
                            {
                                for (int j = 0; j < optionsCollections.Count; j++)
                                    if (options[o] == optionsCollections[j].Text)
                                        cnt++;
                            }

                        if (optionName != "")
                            for (int j = 0; j < optionsCollections.Count; j++)
                            {
                                if (optionName == optionsCollections[j].Text)
                                {
                                    optionsCollections[j].Click();
                                    Thread.Sleep(500);
                                    driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20);
                                    break;
                                }
                            }

                        avail = true;
                        break;
                    }

                    if (driver._isElementPresent("xpath", "//li[@id='side-menu-button' and @class='filter-menu-next-button']/a"))
                        driver._clickByJavaScriptExecutor("//li[@id='side-menu-button' and @class='filter-menu-next-button']/a");
                }
            }

            Assert.AreEqual(true, avail, "'" + fieldName + "' Field not Present.");
            if (options != null)
                Assert.AreEqual(cnt, options.Length, "Options Colletions not match.");

            if (optionName == "")
                Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field and Verified Options.");
            else
                Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field and Verified options & Clicked '" + optionName + "' Option.");

            return new PromoDashboard(driver, test);
        }

        #endregion
    }
}
