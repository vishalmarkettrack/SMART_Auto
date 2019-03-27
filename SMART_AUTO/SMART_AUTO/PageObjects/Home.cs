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
    public class Home
    {
        #region Private Variables

        private IWebDriver home;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public Home(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.home = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.home; }
            set { this.home = value; }
        }

        /// <summary>
        /// To Verify Home Page
        /// </summary>
        /// <returns></returns>
        public Home verifyHomePage()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//img[@alt='Product Logo']", 20), "Home Page Logo not Present.");
            Thread.Sleep(5000);
            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'Almost there')]"))
                driver._waitForElementToBeHidden("xpath", "//span[@class='lead' and contains(text(),'Almost there')]");
            Assert.IsTrue(driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20), "Carousel not Present on Screen.");
            driver._waitForElementToBeHidden("xpath", "//p[@class='lead' and contains(text(),'Loading')]");
            int cnt = 0;
            IList<IWebElement> loadingCount = driver.FindElements(By.XPath("//p[@class='lead' and contains(text(),'Loading')]"));
            do
            {
                Thread.Sleep(1000);
                loadingCount = driver.FindElements(By.XPath("//p[@class='lead' and contains(text(),'Loading')]"));
                cnt++;
                if (cnt == 15)
                    break;

            } while (loadingCount.Count.Equals(0) == false);

            driver._waitForElementToBeHidden("xpath", "//p[@class='lead' and contains(text(),'Loading')]");
            Assert.AreEqual(0, loadingCount.Count, "Home Page Not Load Properly.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn btn-default active']"), "Dashboard Page not Display Properly.");
            Results.WriteStatus(test, "Pass", "Verified, Home Page Screen.");

            verifyRecordsOnReportScreen();
            return new Home(driver, test);
        }

        /// <summary>
        /// Expand Menu Option and Select Option from List on Page
        /// </summary>
        /// <param name="optionName">Option Name to Select</param>
        /// <returns></returns>
        public Home clickSiteNavigationMenuIconAndSelectOptionFromListOnPage(string optionName)
        {
            bool avail = false;
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='baseexpand']"), "Base Icon not Present on Page.");

            if (driver._isElementPresent("xpath", "//button[@id='baseexpand' and contains(@class,'active')]") == false)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver._findElement("xpath", "//*[@id='baseexpand']"));
                driver._waitForElement("xpath", "//button[@id='baseexpand' and contains(@class,'active')]", 20);
                Results.WriteStatus(test, "Pass", "Clicked, Navigation Menu Icon on Page.");
            }

            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='levelHolderClass visible']/ul/li"), "Options List not Found.");
            IList<IWebElement> ElementCollections = driver.FindElements(By.XPath("//div[@class='levelHolderClass visible']/ul/li"));

            for (int i = 0; i <= ElementCollections.Count; i++)
            {
                if (ElementCollections[i].Text.ToLower().Contains(optionName.ToLower()))
                {
                    if (ElementCollections[i].GetAttribute("class") == "active")
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver._findElement("xpath", "//*[@id='baseexpand']"));
                        Results.WriteStatus(test, "Pass", "'" + optionName + "' Navigation Option Already Open.");
                    }
                    else
                    {
                        ElementCollections[i].Click();
                        Thread.Sleep(8000);
                        Results.WriteStatus(test, "Pass", "'" + optionName + "' Navigation Option Clicked.");
                    }
                    avail = true;
                    break;
                }
            }
            Assert.AreEqual(true, avail, "'" + optionName + "' Navigation Option not Present on List.");
            return new Home(driver, test);
        }

        /// <summary>
        /// Click User Menu and Select Account from List
        /// </summary>
        /// <param name="accountName">Account Name to select</param>
        /// <returns></returns>
        public Home clickUserMenuAndSelectAccountFromList(string accountName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'btn-group ng-scope dropdown btn-group-info')]"), "User Menu not Present on screen.");

            if (driver._isElementPresent("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']") == false)
            {
                driver._clickByJavaScriptExecutor("//div[@class='btn-group ng-scope dropdown btn-group-info']/button");
                Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']", 10), "User Menu Icon List not Open.");
            }

            bool avail = false;
            IList<IWebElement> accounts = driver.FindElements(By.XPath("//ul[contains(@class,'dropdown-menu dropdown-menu-form dropdown-menu-scroll')]/li"));
            for (int i = 0; i < accounts.Count; i++)
            {
                if (accounts[i].Text.Contains(accountName))
                {
                    accounts[i].Click();
                    Thread.Sleep(5000);
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "'" + accountName + "' Account not Present on List.");
            Results.WriteStatus(test, "Pass", "Clicked, User Menu and Selected '" + accountName + "' Account name from List.");
            return new Home(driver, test);
        }

        /// <summary>
        /// Click Menu Icon from Screen
        /// </summary>
        /// <param name="menuName">Manu Name for Click</param>
        /// <returns></returns>
        public Home clickMenuIconFromScreen(string menuName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right menuItem']/div/a"), "Menu Items not Present on Page.");
            bool avail = false;

            IList<IWebElement> menuIcons = driver.FindElements(By.XPath("//div[@class='pull-right menuItem']/div/a"));
            for (int i = 0; i < menuIcons.Count; i++)
            {
                if (menuIcons[i].Text.Contains(menuName))
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", menuIcons[i]);
                    Thread.Sleep(5000);
                    avail = true;
                    break;
                }
            }

            Assert.AreEqual(true, avail, "Menu Name not Present on Screen.");
            Results.WriteStatus(test, "Pass", "Clicked, '" + menuName + "' Menu Icon from Screen");
            return new Home(driver, test);
        }

        /// <summary>
        /// Verify Bottom Panel of screen
        /// </summary>
        /// <returns></returns>
        public Home verifyBottomPanelOfScreen()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@ng-repeat='crumb in footerCtrl.data.breadcrumb']", 20), "'Screen Path' at Bottom not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ng-if='footerCtrl.data.contactUs.contactNumber']"), "Call for more info text not present.");
            Assert.AreEqual("Call 888-503-7533 for more info", driver._getText("xpath", "//div[@ng-if='footerCtrl.data.contactUs.contactNumber']"), "'Call 888-503-7533 for more info' text not match.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ng-if='footerCtrl.data.company.isVisible']/a"), "'Numerator' Logo not present at bottom.");
            Results.WriteStatus(test, "Pass", "Verified, Bottom Panel of Screen.");
            return new Home(driver, test);
        }

        /// <summary>
        /// Click Numerator logo from bottom
        /// </summary>
        /// <returns></returns>
        public Home clickMarketTrackLogoFromBottom()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@ng-if='footerCtrl.data.company.isVisible']/a"), "'Numerator' Logo not present at bottom.");
            driver._clickByJavaScriptExecutor("//div[@ng-if='footerCtrl.data.company.isVisible']/a");
            Results.WriteStatus(test, "Pass", "Clicked, Numerator Logo from Bottom.");
            return new Home(driver, test);
        }

        /// <summary>
        /// Expand Menu Option and Select Option from List on Page
        /// </summary>
        /// <param name="optionName">Option Name to Select</param>
        /// <param name="subOption">Sub Option Name to Select</param>
        /// <returns></returns>
        public Home clickSiteNavigationMenuIconAndSelectOptionAndSubOptionFromListOnPage(string optionName, string subOption)
        {
            bool avail = false;
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='baseexpand']"), "Base Icon not Present on Page.");

            if (driver._isElementPresent("xpath", "//button[@id='baseexpand' and contains(@class,'active')]") == false)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver._findElement("xpath", "//*[@id='baseexpand']"));
                driver._waitForElement("xpath", "//button[@id='baseexpand' and contains(@class,'active')]", 20);
                Results.WriteStatus(test, "Pass", "Clicked, Navigation Menu Icon on Page.");
            }

            if (driver._isElementPresent("xpath", "//div[@class='backItemClass']") == true)
            {
                driver._clickByJavaScriptExecutor("//div[@class='backItemClass']/a");
                Thread.Sleep(500);
            }

            Assert.IsTrue(driver._isElementPresent("xpath", "//div[@class='levelHolderClass visible']/ul/li"), "Options List not Found.");
            IList<IWebElement> ElementCollections = driver.FindElements(By.XPath("//div[@class='levelHolderClass visible']/ul/li"));

            for (int i = 0; i <= ElementCollections.Count; i++)
            {
                if (ElementCollections[i].Text.Contains(optionName))
                {
                    if (ElementCollections[i].GetAttribute("class") == "active")
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver._findElement("xpath", "//*[@id='baseexpand']"));
                        Results.WriteStatus(test, "Pass", "'" + optionName + "' Navigation Option Already Open.");
                    }
                    else
                    {
                        ElementCollections[i].Click();
                        Thread.Sleep(1000);

                        ElementCollections = driver.FindElements(By.XPath("//div[@class='levelHolderClass visible']/ul/li"));
                        for (int j = 0; j <= ElementCollections.Count; j++)
                        {
                            if (ElementCollections[j].Text.Contains(subOption))
                            {
                                if (ElementCollections[j].GetAttribute("class") == "active")
                                {
                                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver._findElement("xpath", "//*[@id='baseexpand']"));
                                    Results.WriteStatus(test, "Pass", "'" + optionName + "' Navigation Option Already Open.");
                                }
                                else
                                {
                                    ElementCollections[j].Click();
                                    Thread.Sleep(8000);
                                    avail = true;
                                    break;
                                }
                            }
                        }
                        avail = true;
                        Results.WriteStatus(test, "Pass", "'" + optionName + "' Navigation Option Clicked.");
                    }
                    avail = true;
                    break;
                }
            }
            Assert.AreEqual(true, avail, "'" + optionName + "' Navigation Option not Present on List.");
            return new Home(driver, test);
        }

        /// <summary>
        /// Verify Records on Reports Screen
        /// </summary>
        /// <returns></returns>
        public Home verifyRecordsOnReportScreen()
        {
            if (driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No items were found')]") || driver._isElementPresent("xpath", "//span[@class='lead' and contains(text(),'No results found')]"))
                clickOnDayFilterFieldAndClickOption("Last Month");

            return new Home(driver, test);
        }

        /// <summary>
        /// Click on day Filter Field and Click on Option
        /// </summary>
        /// <param name="optionName">Option Name for Click</param>
        /// <returns></returns>
        public Home clickOnDayFilterFieldAndClickOption(string optionName = "")
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//nav[@class='navbar' and @role='navigation']/ul/li"), "Filter Options not Present.");
            IList<IWebElement> fieldsCollection = driver.FindElements(By.XPath("//nav[@class='navbar' and @role='navigation']/ul/li"));

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
                        Thread.Sleep(5000);
                        driver._waitForElement("xpath", "//*[@id='domain-carousel']/ol", 20);
                        break;
                    }
                }

            Results.WriteStatus(test, "Pass", "Clicked, Days Filter field and Clicked '" + optionName + "' option");
            return new Home(driver, test);
        }

        /// <summary>
        /// Verify Menus Icon Buttons on Top of Screen
        /// </summary>
        /// <param name="iconNames">Menu Icon Names to Verify</param>
        /// <returns></returns>
        public Home verifyMenusIconButtonsOnTopOfScreen(string[] iconNames = null)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//*[@id='baseexpand']"), "'Navigation Menu' Icon not Present on Page.");

            if (iconNames != null)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='pull-right menuItem']"), "'Menu Icons' not Present on top of Screen.");
                IList<IWebElement> menuCollections = driver._findElements("xpath", "//div[@class='pull-right menuItem']");
                foreach (IWebElement menus in menuCollections)
                    Assert.AreEqual(iconNames[menuCollections.IndexOf(menus)], menus.Text, "'" + menus.Text + "' Menu Icon not Present on Top.");
            }

            Results.WriteStatus(test, "Pass", "Verified, Menu Icon Buttons on Top of Screen.");
            return new Home(driver, test);
        }

        #endregion
    }
}
