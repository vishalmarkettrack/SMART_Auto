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
    public class UserProfile
    {
        #region Private Variables

        private IWebDriver userProfile;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public UserProfile(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.userProfile = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.userProfile; }
            set { this.userProfile = value; }
        }

        /// <summary>
        /// Click User icon and Verify User Profile Section Content
        /// </summary>
        /// <param name="editProfile">Verify Edit Profile Present or not</param>
        /// <returns></returns>
        public UserProfile clickUserIconverifyUserProfileSectionContent(bool editProfile)
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//div[contains(@class,'btn-group ng-scope dropdown btn-group-info')]", 20), "User Menu not Present on screen.");
            Assert.AreEqual(true, driver._isElementPresent("id", "filter-menu"), "'Filter Bar' not Present on Screen.");

            if (driver._isElementPresent("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']") == false)
            {
                driver._clickByJavaScriptExecutor("//div[@class='btn-group ng-scope dropdown btn-group-info']/button");
                Results.WriteStatus(test, "Pass", "Clicked, User Icon on Screen.");
            }

            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']", 10), "User Menu Icon List not Open.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'user-profile-name-email')]"), "User Profile Section not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[contains(@class,'user-profile-name-email')]/strong[@class='media-heading']"), "User Name not present.");

            string dataFromSheet = Common.DirectoryPath + ConfigurationManager.AppSettings["DataSheetDir"] + "\\Login.xlsx";
            string[] Email = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Email", "Valid");
            Assert.AreEqual(Email[0].ToString(), driver._getText("xpath", "//div[contains(@class,'user-profile-name-email')]/p"), "'" + Email[0].ToString() + "' Email Id not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='CFT-textbox']/input"), "'Filter Your Accounts...' input area not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='CFT-textbox']/span[contains(@class,'fa-pencil')]"), "'Filter Your Accounts...' input area not present.");

            IList<IWebElement> accounts = driver.FindElements(By.XPath("//ul[contains(@class,'dropdown-menu dropdown-menu-form dropdown-menu-scroll')]/li"));
            for (int i = 0; i < accounts.Count; i++)
            {
                string accountName = accounts[i].Text;
                Assert.AreEqual(true, accounts[i].GetAttribute("class").Contains("radio-menu"), "Radio Option not present for '" + accountName + "' Account.");
                Console.WriteLine("Account Name [" + i + "] : " + accounts[i].Text);
                Assert.AreNotEqual("", accounts[i].Text, "('" + accountName + "') Account Name is Blank.");
            }

            Assert.AreEqual(editProfile, driver._isElementPresent("xpath", "//button[@class='btn btn-default btn-sign-in']"), "Edit Profile not Present.");
            if (editProfile)
                Assert.AreEqual(editProfile, driver._getText("xpath", "//button[@class='btn btn-default btn-sign-in']").Contains("Edit Profile"), "'Edit Profile' label not match.");

            Assert.AreEqual(editProfile, driver._isElementPresent("xpath", "//button[@class='btn btn-default btn-sign-in']/span/i[@class='fa fa-pencil']"), "Edit icon nor prenset for Edit Profile button.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-info btn-sign-out']"), "Sign Out not Present.");
            Assert.AreEqual(true, driver._getText("xpath", "//button[@class='btn btn-info btn-sign-out']").Contains("Sign Out"), "'Sign Out' label not match.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-info btn-sign-out']/span/i[@class='fa fa-sign-out']"), "Sign Out icon nor prenset for Sign Out button.");

            Results.WriteStatus(test, "Pass", "Verified, User Profile Section Content on Screen.");
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Click User Profile Options on Screen
        /// </summary>
        /// <param name="buttonName">Button Name to Click</param>
        /// <returns></returns>
        public UserProfile clickUserProfileOptionsOnScreen(string buttonName)
        {
            if (driver._isElementPresent("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']") == false)
            {
                driver._clickByJavaScriptExecutor("//div[@class='btn-group ng-scope dropdown btn-group-info']/button");
                Results.WriteStatus(test, "Pass", "Clicked, User Icon on Screen.");
            }
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']", 10), "User Menu Icon List not Open.");

            if (buttonName.Equals("Sign Out"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-info btn-sign-out']"), "Sign Out not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-info btn-sign-out']");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='btn btn-default btn-sign-in']"), "Edit Profile not Present.");
                driver._clickByJavaScriptExecutor("//button[@class='btn btn-default btn-sign-in']");
            }

            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on User Section.");
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Verify Edit Profile Section on screen
        /// </summary>
        /// <returns></returns>
        public UserProfile verifyEditProfileSectionOnScreen()
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");

            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='CFT-auth-header']", 20), "Image Logo Header not present.");
            Assert.AreEqual(true, driver._waitForElement("xpath", "//img[@class='CFT-login-logo']", 20), "Image Logo not present.");

            Assert.IsTrue(driver._waitForElement("xpath", "//form[@class='edit-profile track-dirty-state']", 20), "Edit Profile Form not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@for='firstName' and text()='First name']"), "'First Name' Label not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@name='firstName']"), "'First Name' Text area not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@for='lastName' and text()='Last name']"), "'Last name' Label not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@name='lastName']"), "'Last Name' Text area not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@for='country' and text()='Country']"), "'Country' Label not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[contains(@id,'country-container')]"), "'Country' List not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@for='language' and text()='Language']"), "'Language' Label not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[contains(@id,'language-container')]"), "'Language' List not present.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@for='timezone' and text()='Timezone']"), "'Timezone' Label not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[contains(@id,'timezone-container')]"), "'Timezone' List not present.");

            Assert.AreEqual(true, driver._isElementPresent("id", "update-profile"), "Update Profile Button not present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//a[contains(@class,'btn btn-warning btn-block')]"), "Change Password Button not present.");
            driver._selectFrameToDefaultContent();
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//div[@class='cfp-hotkeys-close']"), "Close Icon not present.");

            Results.WriteStatus(test, "Pass", "Verified, Edit Profile Section on screen.");
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Enter Account Name on Input area on screen
        /// </summary>
        /// <param name="accountName">Account Name to Enter</param>
        /// <returns></returns>
        public String enterAccountNameOnInputAreaOnScreen(string accountName)
        {
            if (driver._isElementPresent("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']") == false)
            {
                driver._clickByJavaScriptExecutor("//div[@class='btn-group ng-scope dropdown btn-group-info']/button");
                Results.WriteStatus(test, "Pass", "Clicked, User Icon on Screen.");
            }
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@class='btn-group ng-scope dropdown btn-group-info open']", 10), "User Menu Icon List not Open.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='CFT-textbox']/input"), "'Filter Your Accounts...' input area not present.");
            if (accountName.Equals("Random"))
                accountName = driver._randomString(8);

            driver._type("xpath", "//li[@class='CFT-textbox']/input", accountName);
            Thread.Sleep(1000);
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='CFT-textbox']/input[contains(@class,'ng-not-empty')]"), "");
            Results.WriteStatus(test, "Pass", "Entered, '" + accountName + "' Value on Account name textarea.");
            return accountName;
        }

        /// <summary>
        /// Verify search Value With Account name list
        /// </summary>
        /// <param name="accountName">Search Account name</param>
        /// <param name="invalid">Verify Invalid Search Account name</param>
        /// <returns></returns>
        public UserProfile verifySearchValueWithAccountNameList(string accountName, bool invalid = false)
        {
            IList<IWebElement> accounts = driver.FindElements(By.XPath("//ul[contains(@class,'dropdown-menu dropdown-menu-form dropdown-menu-scroll')]/li"));

            if (invalid)
            {
                Assert.AreEqual(0, accounts.Count, "Account list should be Blank.");
                Results.WriteStatus(test, "Pass", "Verified, Account Name not present on Screen.");
            }
            else
            {
                for (int i = 0; i < accounts.Count; i++)
                    Assert.AreEqual(true, accounts[i].Text.Contains(accountName), "('" + accountName + "') search name not present on Account list.");

                Results.WriteStatus(test, "Pass", "Verified, Search Account name on List.");
            }

            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Verify and Click Clear button for search value
        /// </summary>
        /// <returns></returns>
        public UserProfile verifyAndClickClearButtonForSearchValue()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='CFT-textbox']/span[contains(@class,'fa-times-circle')]"), "Clear Icon for input area not present.");
            driver._clickByJavaScriptExecutor("//li[@class='CFT-textbox']/span[contains(@class,'fa-times-circle')]");
            Thread.Sleep(1000);
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//li[@class='CFT-textbox']/input[contains(@class,'ng-empty')]"), "Input Area not Clear.");

            Results.WriteStatus(test, "Pass", "Verified and Clicked Clear button for search value.");
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Click any Field from Form and Verify Selected Section in color
        /// </summary>
        /// <param name="fieldName">Field Name to Click and Verify</param>
        /// <returns></returns>
        public UserProfile clickAnyFieldFromFormAndVerifySelectedSectionInColor(string fieldName)
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");

            switch (fieldName)
            {
                case "First Name":
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@name='firstName']"), "'" + fieldName + "' Text area not present.");
                    driver._click("xpath", "//input[@name='firstName']");
                    Thread.Sleep(3000);
                    Assert.AreEqual("rgb(0, 88, 169)", driver.FindElement(By.XPath("//input[@name='firstName']")).GetCssValue("border-color"), "'" + fieldName + "' Textarea should not be Highlighted in Blue color.");
                    break;

                case "Last Name":
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@name='lastName']"), "''" + fieldName + "'' Text area not present.");
                    driver._click("xpath", "//input[@name='lastName']");
                    Thread.Sleep(3000);
                    Assert.AreEqual("rgb(0, 88, 169)", driver.FindElement(By.XPath("//input[@name='lastName']")).GetCssValue("border-color"), "'" + fieldName + "' Textarea should not be Highlighted in Blue color.");
                    break;

                case "Country":
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'country-container')]"), "'" + fieldName + "' Field not present.");
                    driver._click("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'country-container')]");
                    Thread.Sleep(3000);
                    Assert.AreEqual(true, driver.FindElement(By.XPath("//span[@role='combobox' and contains(@aria-labelledby,'country-container')]")).GetCssValue("border-color").Contains("rgb(102, 175, 233)"), "'" + fieldName + "' Field should not be Highlighted in Blue color.");
                    break;

                case "Language":
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'language-container')]"), "'" + fieldName + "' Field not present.");
                    driver._click("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'language-container')]");
                    Thread.Sleep(3000);
                    Assert.AreEqual(true, driver.FindElement(By.XPath("//span[@role='combobox' and contains(@aria-labelledby,'language-container')]")).GetCssValue("border-color").Contains("rgb(102, 175, 233)"), "'" + fieldName + "' Field should not be Highlighted in Blue color.");
                    break;

                case "Timezone":
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'timezone-container')]"), "'" + fieldName + "' Field not present.");
                    driver._click("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'timezone-container')]");
                    Thread.Sleep(3000);
                    Assert.AreEqual(true, driver.FindElement(By.XPath("//span[@role='combobox' and contains(@aria-labelledby,'timezone-container')]")).GetCssValue("border-color").Contains("rgb(102, 175, 233)"), "'" + fieldName + "' Field should not be Highlighted in Blue color.");
                    break;

                default:
                    fieldName = "First Name";
                    Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@name='firstName']"), "'" + fieldName + "' Text area not present.");
                    driver._click("xpath", "//input[@name='firstName']");
                    Thread.Sleep(3000);
                    Assert.AreEqual("rgb(0, 88, 169)", driver.FindElement(By.XPath("//input[@name='firstName']")).GetCssValue("border-color"), "'" + fieldName + "' Textarea should not be Highlighted in Blue color.");
                    break;
            }
            driver._selectFrameToDefaultContent();
            Results.WriteStatus(test, "Pass", "Clicked, '" + fieldName + "' Field from Form and Verified Highlighted Blue Color.");
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Click Dropdown List and Verify Options
        /// </summary>
        /// <param name="fieldName">Field Name for Click and Verify</param>
        /// <param name="options">Dropdown Collection of Field</param>
        /// <param name="optionName">Option Name for Select</param>
        /// <returns></returns>
        public UserProfile clickDropdownListAndVerifyOptions(string fieldName, string[] options, string optionName)
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'" + fieldName.ToLower() + "-container')]"), "'" + fieldName + "' Field not present.");
            if (driver._isElementPresent("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'" + fieldName.ToLower() + "-container') and @aria-expanded='true']") == false)
                driver._click("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'" + fieldName.ToLower() + "-container')]");

            Assert.IsTrue(driver._waitForElement("xpath", "//ul[contains(@id,'" + fieldName.ToLower() + "-results')]/li", 20), "Options not present for Country Dropdown.");
            Results.WriteStatus(test, "Pass", "Verified, '" + fieldName + "' Field Option on form.");

            IList<IWebElement> ddlCollections = driver.FindElements(By.XPath("//ul[contains(@id,'" + fieldName.ToLower() + "-results')]/li"));
            int cnt = 0;

            if (options != null)
            {
                for (int i = 0; i < ddlCollections.Count; i++)
                {
                    for (int j = 0; j < options.Length; j++)
                    {
                        if (ddlCollections[i].Text.Contains(options[j]) == true)
                        {
                            cnt++;
                            break;
                        }
                    }
                }
                Assert.AreEqual(options.Length, cnt, "" + fieldName + " Options not present properly.");
                Results.WriteStatus(test, "Pass", "Verified, " + fieldName + " Options from List.");
            }
            if (optionName != "")
            {
                for (int i = 0; i < ddlCollections.Count; i++)
                {
                    if (ddlCollections[i].Text.Contains(optionName))
                    {
                        ddlCollections[i].Click();
                        break;
                    }
                }
                Results.WriteStatus(test, "Pass", "Selected, '" + optionName + "' Option from Country Droddown.");
            }

            driver._selectFrameToDefaultContent();
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Click Dropdown list and Enter value
        /// </summary>
        /// <param name="fieldName">Field Name to Click</param>
        /// <param name="value">Value to Enter</param>
        /// <returns></returns>
        public UserProfile clickDropdownListAndEnterValue(string fieldName, string value)
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'" + fieldName.ToLower() + "-container')]"), "'" + fieldName + "' Field not present.");
            if (driver._isElementPresent("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'" + fieldName.ToLower() + "-container') and @aria-expanded='true']") == false)
                driver._click("xpath", "//span[@role='combobox' and contains(@aria-labelledby,'" + fieldName.ToLower() + "-container')]");

            Assert.IsTrue(driver._waitForElement("xpath", "//ul[contains(@id,'" + fieldName.ToLower() + "-results')]/li", 20), "Options not present for Country Dropdown.");
            Results.WriteStatus(test, "Pass", "Verified, '" + fieldName + "' Field Option on form.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[contains(@class,'search__field') and @type='search']"), "Search textarea not present for '" + fieldName + "' Field.");
            string enterValue = value;
            if (value.Equals("Random"))
                enterValue = driver._randomString(8);

            driver._type("xpath", "//input[contains(@class,'search__field') and @type='search']", enterValue);
            Results.WriteStatus(test, "Pass", "Entered, value on '" + fieldName + "' Search Area.");

            IList<IWebElement> ddlCollections = driver.FindElements(By.XPath("//ul[contains(@id,'" + fieldName.ToLower() + "-results')]/li"));

            if (value.Equals("Random"))
            {
                Assert.AreEqual(1, ddlCollections.Count, "'No results found' message not found.");
                Assert.AreEqual(true, ddlCollections[0].Text.Contains("No results found"), "'No results found' Message not match.");
                Results.WriteStatus(test, "Pass", "Verified, 'No results found' Message for '" + fieldName + "' Field.");
            }
            else
            {
                for (int i = 0; i < ddlCollections.Count; i++)
                    Assert.AreEqual(true, ddlCollections[i].Text.ToLower().Contains(value.ToLower()), "'" + value + "' Search Value not Present on '" + ddlCollections[i].Text + "' Option.");

                Results.WriteStatus(test, "Pass", "Verified, 'No results found' Message for '" + fieldName + "' Field.");
            }

            driver._selectFrameToDefaultContent();
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Enter First or Last Name on Edit Profile Form
        /// </summary>
        /// <param name="firstName">where to enter value</param>
        /// <returns></returns>
        public String enterFirstOrLastNameOnEditProfileForm(bool blankValue, bool firstName)
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");
            string fieldValue = "";
            if (firstName)
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@id='firstName']"), "'First Name' Text area not present.");
                if (blankValue == false)
                    fieldValue = driver._getValue("xpath", "//input[@id='firstName']") + driver._randomString(3, true);
                driver._type("xpath", "//input[@id='firstName']", fieldValue);
                Results.WriteStatus(test, "Pass", "Entered, '" + fieldValue + "' Value for 'First Name' Field.");
            }
            else
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@id='lastName']"), "'Last Name' Text area not present.");
                if (blankValue == false)
                    fieldValue = driver._getValue("xpath", "//input[@id='lastName']") + driver._randomString(3, true);
                driver._type("xpath", "//input[@id='lastName']", fieldValue);
                Results.WriteStatus(test, "Pass", "Entered, '" + fieldValue + "' Value for 'Last Name' Field.");
            }

            Thread.Sleep(1000);
            driver._selectFrameToDefaultContent();
            return fieldValue;
        }

        /// <summary>
        /// Verify Update Profile button Disable or Not
        /// </summary>
        /// <param name="Disabled">Button should be Disable</param>
        /// <returns></returns>
        public UserProfile verifyUpdateProfileButtonDisableOrNot(bool Disabled = true)
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");
            if (Disabled)
            {
                Assert.AreEqual(driver._getAttributeValue("id", "update-profile", "disabled"), ("true"), "'Update Profile' Button not Disabled.");
                Results.WriteStatus(test, "Pass", "Verified, 'Update Profile' Button Disabled on screen.");
            }
            else
            {
                Assert.AreEqual(driver._getAttributeValue("id", "update-profile", "disabled"), null, "'Update Profile' Button is Disabled.");
                Results.WriteStatus(test, "Pass", "Verified, 'Update Profile' Button Enabled on screen.");
            }
            driver._selectFrameToDefaultContent();
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Click Button on Edit Profile screen
        /// </summary>
        /// <param name="buttonName">Button Name to Clicl</param>
        /// <returns></returns>
        public UserProfile clickButtonOnEditProfileScreen(string buttonName)
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");

            if (buttonName.Equals("Update Profile"))
            {
                Assert.AreEqual(true, driver._isElementPresent("id", "update-profile"), "" + buttonName + " Button not present.");
                Assert.AreEqual(driver._getAttributeValue("id", "update-profile", "disabled"), null, "'" + buttonName + "' Button is Disabled.");
                driver._click("id", "update-profile");
            }

            if (buttonName.Equals("Change Password"))
            {
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//a[contains(@class,'btn btn-warning btn-block')]"), "'" + buttonName + "' Button not present.");
                driver._clickByJavaScriptExecutor("//a[contains(@class,'btn btn-warning btn-block')]");
            }

            Thread.Sleep(3000);
            driver._selectFrameToDefaultContent();
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on Edit Profile screen.");
            return new UserProfile(driver, test);
        }

        /// <summary>
        /// Verify Value of Form on Edit Profile screen
        /// </summary>
        /// <param name="fieldName">Field Name</param>
        /// <param name="fieldValue">Field Value to verify</param>
        /// <returns></returns>
        public UserProfile verifyValueOfFormOnEditProfileScreen(string fieldName, string fieldValue)
        {
            driver._selectFrameFromDefaultContent("xpath", "//div[contains(@class,'brand-item-container')]/iframe");

            if (fieldName.Equals("First Name"))
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//input[@name='firstName']", 20), "'First Name' Text area not present.");
                Assert.AreEqual(fieldValue, driver._getValue("xpath", "//input[@name='firstName']"), "'" + fieldValue + "' Value not match for First Name field.");
                Results.WriteStatus(test, "Pass", "Verified, '" + fieldValue + "' Value for '" + fieldName + "' Field.");
            }

            if (fieldName.Equals("Last Name"))
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//input[@name='lastName']", 20), "'" + fieldValue + "' Text area not present.");
                Assert.AreEqual(fieldValue, driver._getValue("xpath", "//input[@name='lastName']"), "'" + fieldValue + "' Value not match for First Name field.");
                Results.WriteStatus(test, "Pass", "Verified, '" + fieldValue + "' Value for '" + fieldName + "' Field.");
            }
            driver._selectFrameToDefaultContent();
            return new UserProfile(driver, test);
        }

        #endregion
    }
}
