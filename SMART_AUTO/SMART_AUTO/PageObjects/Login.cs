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
    public class Login
    {
        #region Private Variables

        private IWebDriver login;
        private ExtentTest test;

        #endregion

        #region Public Methods

        public Login(IWebDriver driver, ExtentTest testReturn)
        {
            // TODO: Complete member initialization
            this.login = driver;
            test = testReturn;
        }

        public IWebDriver driver
        {
            get { return this.login; }
            set { this.login = value; }
        }

        /// <summary>
        /// Navigate to login page (Login URL get From the Login.xlsx Sheet)
        /// </summary>
        /// <returns></returns>
        public Login navigateToLoginPage()
        {
            driver.Navigate().GoToUrl(Common.ApplicationURL);
            Results.WriteStatus(test, "Pass", "Launched, URL <b>" + Common.ApplicationURL + "</b> successfully.");
            return new Login(driver, test);
        }

        /// <summary>
        /// To Verify Login Page Screen in detail
        /// </summary>
        /// <returns></returns>
        public Login verifyLoginPageScreenInDetail()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//img[@class='CFT-login-logo']"), "Numerator Logo not found on Page.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//p[text() = 'Sign in to your Numerator account']"), "'Sign in to your Numerator account' Label not match.");

            Assert.IsTrue(driver._waitForElement("xpath", "//input[@type='email' and @name = 'email']"), "Email input area not found on Page.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@type='email' and @name = 'email' and @placeholder='Please enter your email address']"), "'Please enter your email address' Placeholder not found on match.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='CFT-auth-btn btn btn-primary btn-block']/span"), "Next Button not found on Page.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//p[@class='text-center link-wrapper']/a"), "'Trouble signing in?' Link not found on Page.");
            Assert.AreEqual("Trouble signing in?", driver._getText("xpath", "//p[@class='text-center link-wrapper']"), "'Trouble signing in?' Link Label not match.");

            Results.WriteStatus(test, "Pass", "Verified, Login page screen in detail.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Image Section on Login Page
        /// </summary>
        /// <returns></returns>
        public Login verifyImageSectionOnLoginPage()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//iframe[@src='https://markettrack.com/login1']"), "Image Section Frame not present.");
            driver._selectFrameWithinFrame("xpath", "//iframe[@src='https://markettrack.com/login1']");

            IWebElement templateCollections = driver.FindElement(By.XPath("//div[@class='panel-grid-cell']"));
            string[] text = { "People can't buy what they can't find.", "Discover Numerator Digital Shelf" };
            IList<IWebElement> textCollections = templateCollections._findElementsWithinElement("xpath", ".//*[local-name(.)='h2']");
            int cnt = 0;

            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < textCollections.Count; j++)
                {
                    if (text[i] == textCollections[j].Text)
                    {
                        cnt++;
                        break;
                    }
                }
            }

            driver._selectFrameToDefaultContent();
            Assert.AreEqual(cnt, text.Length, "Sentence not match on Image Section.");
            Results.WriteStatus(test, "Pass", "Verified, Image Section on Login page.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Click Learn More Link on Image Section
        /// </summary>
        /// <returns></returns>
        public Login clickLearnMoreLinkOnImageSection()
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//iframe[@src='https://markettrack.com/login1']"), "Image Section Frame not present.");
            driver._selectFrameWithinFrame("xpath", "//iframe[@src='https://markettrack.com/login1']");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//a[@class='btn btn-md btn-warning' and text() = 'LEARN MORE']"), "'LEARN MORE' Link not Present.");
            driver._clickByJavaScriptExecutor("//a[@class='btn btn-md btn-warning' and text() = 'LEARN MORE']");
            driver._selectFrameToDefaultContent();

            Results.WriteStatus(test, "Pass", "Clicked, Learn More Link on Image Section.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Navigate URL on Screen
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public Login verifyNavigateURLOnScreen(string url)
        {
            Assert.AreEqual(true, driver.Url.Contains(url), "Navigate URL not Match. Url is : " + driver.Url);
            Results.WriteStatus(test, "Pass", "Verified, Navigate Url " + url + " on Screen.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Password Screen on Login Page
        /// </summary>
        /// <returns></returns>
        public Login verifyPasswordScreenOnLoginPage(bool EmailId = false)
        {
            #region Datasheet

            string dataFromSheet = Common.DirectoryPath + ConfigurationManager.AppSettings["DataSheetDir"] + "\\Login.xlsx";
            string[] Email = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Email", "Valid");

            #endregion

            Assert.IsTrue(driver._waitForElement("xpath", "//p[contains(text(),'Enter the password for')]"), "'Enter the password for' Label not present.");
            if (EmailId)
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//b[text() = '" + Email[0].ToString() + "']"), "Entered Email not match.");

            Assert.AreEqual(true, driver._isElementPresent("id", "password"), "Email input area not found on Page.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@id='password' and @placeholder='Please enter your password']"), "'Please enter your password' Placeholder not found on match.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@class='field-checkbox show-password']/span"), "Show Password Checkbox not found on Page.");
            Assert.AreEqual("Show password", driver._getText("xpath", "//label[@class='field-checkbox show-password']/span"), "Show Password Checkbox label not match.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//button[@class='CFT-auth-btn btn btn-primary btn-block']/span"), "Sigin in Button not found on Page.");
            //Assert.AreEqual(true, driver._isElementPresent("xpath", "//a[contains(@class,'CFT-auth-btn btn btn-default btn-block')]/span"), "'Try a different email address' Button not found on Page.");

            Assert.AreEqual(true, driver._isElementPresent("xpath", "//a[@class='forgot-password' and text() = 'Forgot password?']"), "'Forgot password?' Link not found on Page.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//a[text() = 'Trouble signing in?']"), "'Trouble signing in?' Link not found on Page.");

            Results.WriteStatus(test, "Pass", "Verified, Password Screen on Login Page.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Login using Valid Email Id & Password
        /// </summary>
        /// <param name="column">Column Number to Find Data from the Excel Sheet</param>
        /// <returns></returns>
        public Login loginUsingValidEmailIdAndPassword(int column = 0)
        {
            #region Datasheet

            string dataFromSheet = Common.DirectoryPath + ConfigurationManager.AppSettings["DataSheetDir"] + "\\Login.xlsx";
            string[] Email = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Email", "Valid");
            string[] Password = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Password", "Valid");
            string email, password = "";

            #endregion

            email = Email[column].ToString();
            password = Password[column].ToString();

            Assert.IsTrue(driver._isElementPresent("xpath", "//input[@type='email' and @name = 'email']"), "Email Address Textarea not Present.");
            driver._type("xpath", "//input[@type='email' and @name = 'email']", email);
            Results.WriteStatus(test, "Pass", "Information Inputed successfully.<b> Email Address : " + email);
            clickButtonOnLoginPage("Next");

            verifyPasswordScreenOnLoginPage();
            driver._type("id", "password", password);
            Results.WriteStatus(test, "Pass", "Information Inputed successfully.<b> Password : " + password);
            clickButtonOnLoginPage("Sign in");

            return new Login(driver, test);
        }

        /// <summary>
        /// Enter Valid Email Id or Password
        /// </summary>
        /// <param name="emailOption">Email Option</param>
        /// <returns></returns>
        public String enterValidEmailIdOrPassword(bool emailOption, bool clickButton = true)
        {
            #region Datasheet

            string dataFromSheet = Common.DirectoryPath + ConfigurationManager.AppSettings["DataSheetDir"] + "\\Login.xlsx";
            string[] Email = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Email", "Valid");
            string[] Password = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Password", "Valid");
            string email, password = "";

            #endregion

            email = Email[0].ToString();
            password = Password[0].ToString();
            string value = "";

            if (emailOption)
            {
                Assert.IsTrue(driver._isElementPresent("xpath", "//input[@type='email' and @name = 'email']"), "Email Address Textarea not Present.");
                driver._type("xpath", "//input[@type='email' and @name = 'email']", email);
                value = email;
                Results.WriteStatus(test, "Pass", "Information Inputed successfully.<b> Email Address : " + email);

                if (clickButton)
                    clickButtonOnLoginPage("Next");
            }
            else
            {
                driver._type("id", "password", password);
                value = password;
                Results.WriteStatus(test, "Pass", "Information Inputed successfully.<b> Password : " + password);

                if (clickButton)
                    clickButtonOnLoginPage("Sign in");
            }

            return value;
        }

        /// <summary>
        /// Enter Invalid Email Address on Login Page
        /// </summary>
        /// <param name="email">Email Address to Enter</param>
        /// <returns></returns>
        public Login enterInvalidEmailAddressAndClickNextButtonOnLoginPage(string email)
        {
            Assert.IsTrue(driver._isElementPresent("xpath", "//input[@type='email' and @name = 'email']"), "Email Address Textarea not Present.");
            driver._type("xpath", "//input[@type='email' and @name = 'email']", email);
            Results.WriteStatus(test, "Pass", "Entered, (" + email + ") Invalid Email address on Login Page.");
            clickButtonOnLoginPage("Next");

            return new Login(driver, test);
        }

        /// <summary>
        /// Enter Invalid Password on Login Page
        /// </summary>
        /// <param name="password">Password to Enter</param>
        /// <returns></returns>
        public Login enterInvalidPasswordAndClickSignInButtonOnLoginPage(string password, bool clickSignIn = true)
        {
            Assert.IsTrue(driver._isElementPresent("id", "password"), "Password Textarea not Present.");
            driver._type("id", "password", password);
            Results.WriteStatus(test, "Pass", "Entered, (" + password + ") Invalid Password on Login Page.");

            if (clickSignIn)
                clickButtonOnLoginPage("Sign in");

            return new Login(driver, test);
        }

        /// <summary>
        /// Click Button on Login Page
        /// </summary>
        /// <param name="buttonName">Button Name to Click</param>
        /// <returns></returns>
        public Login clickButtonOnLoginPage(string buttonName)
        {
            if (buttonName.Contains("different email"))
                driver._clickByJavaScriptExecutor("//a[@class='CFT-auth-btn btn btn-warning btn-block']/span");

            if (buttonName.Contains("Sign in") || buttonName.Contains("Next"))
                driver._clickByJavaScriptExecutor("//button[@class='CFT-auth-btn btn btn-primary btn-block']/span");

            Thread.Sleep(5000);
            Results.WriteStatus(test, "Pass", "Clicked, " + buttonName + " Button On Login Page.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Click Link on Login Page
        /// </summary>
        /// <param name="linkName">LinkName for Link</param>
        /// <returns></returns>
        public Login clickLinkOnLoginPage(string linkName)
        {
            if (linkName.Contains("Trouble signing in"))
                driver._clickByJavaScriptExecutor("//a[text()='Trouble signing in?']");

            if (linkName.Contains("Forgot password"))
                driver._clickByJavaScriptExecutor("//a[@class='forgot-password' and text() = 'Forgot password?']");

            Results.WriteStatus(test, "Pass", "Clicked, " + linkName + " Link On Login Page.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Check Show Password Checkbox on Password Screen
        /// </summary>
        /// <param name="unChecked">unChecked Show password Checkbox</param>
        /// <returns></returns>
        public Login checkShowPasswordCheckboxOnPasswordScreen(bool unChecked = false)
        {
            if (unChecked)
            {
                if (driver._getAttributeValue("id", "password", "type").Equals("text"))
                    driver._clickByJavaScriptExecutor("//label[@class='field-checkbox show-password']/span");

                Assert.AreEqual("password", driver._getAttributeValue("id", "password", "type"), "'Show password' Checkbox not UnChecked.");
                Results.WriteStatus(test, "Pass", "UnChecked 'Show password' Checkbox on Password Screen.");
            }
            else
            {
                if (driver._getAttributeValue("id", "password", "type").Equals("password"))
                    driver._clickByJavaScriptExecutor("//label[@class='field-checkbox show-password']/span");

                Assert.AreEqual("text", driver._getAttributeValue("id", "password", "type"), "'Show password' Checkbox not Checked.");
                Results.WriteStatus(test, "Pass", "Checked 'Show password' Checkbox on Password Screen.");
            }

            return new Login(driver, test);
        }

        /// <summary>
        /// Switch Tab and Verify Navigate URL on Screen
        /// </summary>
        /// <param name="url">URL to Verify</param>
        /// <returns></returns>
        public Login switchTabAndVerifyNavigateURLOnScreen(string url)
        {
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Results.WriteStatus(test, "Pass", "Switching Into New Tab on Browser.");

            verifyNavigateURLOnScreen(url);
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.First());
            Results.WriteStatus(test, "Pass", "Closed Last open tab and Switched Into First tab on Browser.");

            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Alert Tooltip message on login Page
        /// </summary>
        /// <param name="message">Message to Verify</param>
        /// <param name="email">verify for Email Option</param>
        /// <returns></returns>
        public Login verifyAlertTooltipMessageOnLoginPage(string message, bool email = true)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            string errorMessage = "";
            if (email)
                errorMessage = (string)js.ExecuteScript("var inpObj = document.getElementById(\"helloEmail\");if (inpObj.checkValidity() == false) { return inpObj.validationMessage;}");
            else
                errorMessage = (string)js.ExecuteScript("var inpObj = document.getElementById(\"password\");if (inpObj.checkValidity() == false) { return inpObj.validationMessage;}");

            Assert.AreEqual(message, errorMessage, "'" + message + "' Alert Tooltip Message not match.");
            Results.WriteStatus(test, "Pass", "Verified, '" + message + "' Error Tooltip Message on Login Page.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Validation Message on Screen
        /// </summary>
        /// <param name="message">Message to Verify</param>
        /// <returns></returns>
        public Login verifyValidationMessageOnScreen(string message = "We could not find that email address in our system. If you believe this is an error, please contact us at signin@markettrack.com.")
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//p[@class='small help-block']"), "Error Message not Present.");
            Assert.AreEqual(message, driver._getText("xpath", "//p[@class='small help-block']"), "'" + message + "' Error Message not Match.");
            string getColor = driver.FindElement(By.XPath("//p[@class='small help-block']")).GetCssValue("color");
            Assert.AreEqual(true, getColor.Contains("rgba(255, 28, 45, 1)") || getColor.Contains("rgb(255, 28, 45)"), "'Red' Color for Error Message not match.");
            Results.WriteStatus(test, "Pass", "Verified, Validation Message on Screen.");
            return new Login(driver, test);
        }

        #region Reset Password

        /// <summary>
        /// Verify Reset Password screen on Login Page
        /// </summary>
        /// <param name="emailAddress">Email Address to Verify</param>
        /// <returns></returns>
        public Login verifyResetPasswordScreenOnLoginPage(string emailAddress, bool resetInstructions = false)
        {
            Assert.IsTrue(driver._isElementPresent("xpath", "//img[@class='CFT-login-logo']"), "Numerator Logo not Present on Reset Password Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//p[@class='text-center']"), "Instructions Not Present on Reset Password Screen.");

            if (resetInstructions)
            {
                Assert.AreEqual(true, driver._getText("xpath", "//p[@class='text-center']").Contains("We've sent instructions to " + emailAddress + "."), "'We've sent instructions to' Message Not match on Reset Password Screen.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Resend password instructions']"), "'Resend password instructions' Button not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Proceed to sign in']"), "'Proceed to sign in' Button not Present.");

                Results.WriteStatus(test, "Pass", "Verified, Resend password instructions Screen on Login Page.");
            }
            else
            {
                Assert.AreEqual(true, driver._getText("xpath", "//p[@class='text-center']").Contains("We'll send instructions on how to reset your password to " + emailAddress + "."), "Instructions Not match on Reset Password Screen.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Send instructions']"), "'Send instructions' Button not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Back to sign in']"), "'Back to sign in' Button not Present.");

                Results.WriteStatus(test, "Pass", "Verified, Reset Password Screen on Login Page.");
            }
            return new Login(driver, test);
        }

        /// <summary>
        /// Click Button on Reset Password screen
        /// </summary>
        /// <param name="buttonName">Button Name to Click</param>
        /// <returns></returns>
        public Login clickButtonOnResetPasswordScreen(string buttonName)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = '" + buttonName + "']"), "'" + buttonName + "' Button not Present.");
            driver._clickByJavaScriptExecutor("//span[@class='btn-left-side' and text() = '" + buttonName + "']");
            Results.WriteStatus(test, "Pass", "Clicked, '" + buttonName + "' Button on Reset Password Screen.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Create New Password or Successfull Password Screen
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <param name="successfullMessage">Verify Successfull Message Screen</param>
        /// <returns></returns>
        public Login verifyCreateANewPasswordOrSuccessfullPasswordScreen(string emailAddress, bool successfullMessage = false)
        {
            Assert.IsTrue(driver._isElementPresent("xpath", "//img[@class='CFT-login-logo']"), "Numerator Logo not Present on Reset Password Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//p[@class='text-center']"), "Instructions Not Present on Reset Password Screen.");

            if (successfullMessage)
            {
                Assert.AreEqual(true, driver._getText("xpath", "//p[@class='text-center']").Contains("You successfully entered your current password for " + emailAddress + "."), "'You successfully entered your current password for' Message Not match on Screen.");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Continue to site']"), "'Continue to site' Button not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Back to set new password']"), "'Back to set new password' Button not Present.");

                Results.WriteStatus(test, "Pass", "Verified, Successfull Entered Password Screen on Page.");
            }
            else
            {
                Assert.AreEqual(true, driver._getText("xpath", "//p[@class='text-center']").Contains("Create a new password for " + emailAddress + "."), "'Create a new password for' Message Not match on Reset Password Screen.");

                Assert.AreEqual(true, driver._isElementPresent("id", "password"), "New Password textarea not Present.");
                Assert.AreEqual(true, driver._isElementPresent("id", "password_confirmation"), "Confirm Password textarea not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//label[@class='field-checkbox show-password']"), "Show Password Checkbox not Present.");

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Set password']"), "'Set password' Button not Present.");
                Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Back to sign in']"), "'Back to sign in' Button not Present.");

                Results.WriteStatus(test, "Pass", "Verified, Create a new password Screen.");
            }

            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Password Updated Label and Click Login Link
        /// </summary>
        /// <returns></returns>
        public Login enterPasswordAndClickButton(string buttonName, string passwordType)
        {
            string dataFromSheet = Common.DirectoryPath + ConfigurationManager.AppSettings["DataSheetDir"] + "\\Login.xlsx";
            string[] Password = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Password", "Valid");
            string newPassword = ""; string confirmPassword = "";

            if (passwordType == "Invalid")
            {
                newPassword = "InvalidPass123"; confirmPassword = "Invalid222";
            }
            else
                if (passwordType == "New Same Password")
                {
                    newPassword = "Success1234"; confirmPassword = "Success1234";
                }
                else
                    if (passwordType == "Lower Case")
                    {
                        newPassword = "testpassword"; confirmPassword = "testpassword";
                    }
                    else
                    {
                        newPassword = Password[0].ToString(); confirmPassword = Password[0].ToString();
                    }

            driver._type("id", "password", newPassword);
            Thread.Sleep(1000);
            driver._type("id", "password_confirmation", confirmPassword);
            Thread.Sleep(1000);
            driver._clickByJavaScriptExecutor("//span[@class='btn-left-side' and text() = '" + buttonName + "']");
            Thread.Sleep(5000);
            Results.WriteStatus(test, "Pass", "Entered, Password & Confirm Password and Clicked " + buttonName + " Button.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Verify No Longer screen active Page
        /// </summary>
        /// <param name="emailAddress">Verify Email Address</param>
        /// <returns></returns>
        public Login verifyNoLongerScreenActivePage(string emailAddress)
        {
            Assert.IsTrue(driver._isElementPresent("xpath", "//img[@class='CFT-login-logo']"), "Numerator Logo not Present on Reset Password Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//p[@class='text-center']"), "Instructions Not Present on Reset Password Screen.");
            Assert.AreEqual(true, driver._getText("xpath", "//p[@class='text-center']").Contains("Sorry, this reset-password link is no longer active. You can request a replacement one below. New instructions will be sent to " + emailAddress + "."), "'Link is No Longer active' Message Not match on Screen.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Resend password instructions']"), "'Resend password instructions' Button not Present.");
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='btn-left-side' and text() = 'Get me out of here']"), "'Get me out of here' Button not Present.");

            Results.WriteStatus(test, "Pass", "Verified, No Longer Screen active Page.");
            return new Login(driver, test);
        }

        #endregion

        #region Outlook Mails Methods

        /// <summary>
        /// Verify Outlook Login Screen to Enter Credential and Click SignIn Button
        /// </summary>
        /// <returns></returns>
        public Login verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton()
        {
            string dataFromSheet = Common.DirectoryPath + ConfigurationManager.AppSettings["DataSheetDir"] + "\\Login.xlsx";

            string[] Email = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Email", "Outlook");
            string[] Password = Spreadsheet.GetMultipleValueOfField(dataFromSheet, "Password", "Outlook");

            if (driver._isElementPresent("id", "cred_userid_inputtext"))
            {
                Assert.IsTrue(driver._waitForElement("id", "cred_userid_inputtext", 20), "Email Address Textarea not Present.");
                Assert.AreEqual(true, driver._isElementPresent("id", "cred_password_inputtext"), "Password Textarea not Present.");
                Assert.AreEqual(true, driver._isElementPresent("id", "cred_sign_in_button"), "SignIn Button not Present.");
                Results.WriteStatus(test, "Pass", "Verified, Outlook Login Screen.");

                driver._type("id", "cred_userid_inputtext", Email[0].ToString());
                Thread.Sleep(1000);
                driver._type("id", "cred_password_inputtext", Password[0].ToString());
                Thread.Sleep(3000);

                driver._click("id", "cred_sign_in_button");
                Thread.Sleep(5000);
                Results.WriteStatus(test, "Pass", "Entered, Credential and Clicked SignIn Button.");
            }
            else
            {
                Assert.IsTrue(driver._waitForElement("xpath", "//input[@type='email' and @name='loginfmt']", 20), "Email Address Textarea not Present.");
                driver._type("xpath", "//input[@type='email' and @name='loginfmt']", Email[0].ToString());
                Thread.Sleep(1000);

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@type='submit' and @value='Next']"), "Next Button not Present.");
                driver._clickByJavaScriptExecutor("//input[@type='submit' and @value='Next']");
                Thread.Sleep(1000);

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@name='passwd' and @type='password']"), "Password Textarea not Present.");
                driver._type("xpath", "//input[@name='passwd' and @type='password']", Password[0].ToString());
                Thread.Sleep(1000);

                Assert.AreEqual(true, driver._isElementPresent("xpath", "//input[@type='submit' and @value='Sign in']"), "Sign in Button not Present.");
                driver._clickByJavaScriptExecutor("//input[@type='submit' and @value='Sign in']");
                Thread.Sleep(1000);

                if (driver._isElementPresent("xpath", "//input[@type='button' and @value='No']"))
                    driver._clickByJavaScriptExecutor("//input[@type='button' and @value='No']");
                Thread.Sleep(1000);
            }

            Results.WriteStatus(test, "Pass", "Entered, Credential and Clicked SignIn Button.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Verify Outlook Home Page 
        /// </summary>
        /// <returns></returns>
        public Login verifyOutlookHomePage()
        {
            Assert.IsTrue(driver._waitForElement("xpath", "//span[@title='Inbox' and text() = 'Inbox']"), "Inbox Folder not Present.");
            Assert.IsTrue(driver._waitForElement("xpath", "//div[@role='option' and @aria-haspopup='true']"), "Emails List not Present.");
            Results.WriteStatus(test, "Pass", "Verified, Outlook Home Page.");
            return new Login(driver, test);
        }

        /// <summary>
        /// Select Reset Password Mail to Open Reset Link
        /// </summary>
        /// <returns></returns>
        public Login selectResetPasswordMailToOpenResetLink(bool mailContent = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='lvHighlightAllClass lvHighlightSubjectClass']"), "Mails Subject not Present.");
            IList<IWebElement> mailSubjects = driver._findElements("xpath", "//span[@class='lvHighlightAllClass lvHighlightSubjectClass']");
            bool avail = false;

            for (int m = 0; m < mailSubjects.Count; m++)
            {
                if (mailSubjects[m].Text.Contains("Your password reset instructions from Numerator"))
                {
                    mailSubjects[m].Click();
                    Thread.Sleep(2000);
                    avail = true;
                    break;
                }
            }
            Assert.AreEqual(true, avail, "'Your FeatureVision(R) Password Reset Request' Mail not Present.");
            Results.WriteStatus(test, "Pass", "Selected, Reset Password Email.");

            Assert.IsTrue(driver._waitForElement("xpath", "//div[@aria-label='Message Contents']"), "Message Content not Present.");
            IWebElement body = driver._findElement("xpath", "//div[@aria-label='Message Contents']");

            if (mailContent)
            {
                Assert.AreEqual(true, body.Text.Contains("We got a request to reset your Numerator password. This link is only valid for 36 hours."), "'We got a request to reset your Numerator password. This link is only valid for 36 hours.' Message not Present or match.");
                Assert.AreEqual(true, body.Text.Contains("If you ignore this message, your password won't be changed."), "'If you ignore this message, your password won't be changed.' Message not Present or match.");
                Assert.AreEqual(true, body.Text.Contains("If you didn't request a password reset, please visit our help center at help.markettrack.com."), "'If you didn't request a password reset, please visit our help center at help.markettrack.com.' Message not Present or match.");
                Assert.AreEqual(true, body.Text.Trim().Replace("\r\n", "").Contains("Thanks,The Numerator Team"), "'Thanks, The Numerator Team' Message not Present or match.");

                Results.WriteStatus(test, "Pass", "Verified, Mail Contents.");
            }

            IList<IWebElement> content = body.FindElements(By.TagName("a"));
            bool resetLink = false;

            for (int i = 0; i < content.Count(); i++)
            {
                if (content[i].Text.Contains("Reset your password"))
                {
                    content[i].Click();
                    resetLink = true;
                    Thread.Sleep(5000);
                    break;
                }
            }
            Assert.AreEqual(true, resetLink, "'Reset Password' Link not Present on Content.");
            Results.WriteStatus(test, "Pass", "Clicked, Reset Password Link from Email.");
            return new Login(driver, test);
        }

        #endregion
        
        /// <summary>
        /// Select Second Reset password mail to open Reset link
        /// </summary>
        /// <param name="mailContent">Mail Content to verify and open mail</param>
        /// <returns></returns>
        public Login selectSecondResetPasswordMailToOpenResetLink(bool mailContent = false)
        {
            Assert.AreEqual(true, driver._isElementPresent("xpath", "//span[@class='lvHighlightAllClass lvHighlightSubjectClass']"), "Mails Subject not Present.");
            IList<IWebElement> mailSubjects = driver._findElements("xpath", "//span[@class='lvHighlightAllClass lvHighlightSubjectClass']");
            bool avail = false;

            for (int m = 0; m < mailSubjects.Count; m++)
            {
                if (mailSubjects[m].Text.Contains("Your password reset instructions from Numerator"))
                {
                    for (int j = m + 1; j < mailSubjects.Count; j++)
                    {
                        if (mailSubjects[j].Text.Contains("Your password reset instructions from Numerator"))
                        {
                            mailSubjects[j].Click();
                            Thread.Sleep(2000);
                            avail = true;
                            break;
                        }
                    }
                }
                if (avail)
                    break;
            }

            Assert.AreEqual(true, avail, "'Your FeatureVision(R) Password Reset Request' Mail not Present.");
            Results.WriteStatus(test, "Pass", "Selected, Reset Password Email.");

            Assert.IsTrue(driver._waitForElement("xpath", "//div[@aria-label='Message Contents']"), "Message Content not Present.");
            IWebElement body = driver._findElement("xpath", "//div[@aria-label='Message Contents']");

            if (mailContent)
            {
                Assert.AreEqual(true, body.Text.Contains("We got a request to reset your Numerator password. This link is only valid for 36 hours."), "'We got a request to reset your Numerator password. This link is only valid for 36 hours.' Message not Present or match.");
                Assert.AreEqual(true, body.Text.Contains("If you ignore this message, your password won't be changed."), "'If you ignore this message, your password won't be changed.' Message not Present or match.");
                Assert.AreEqual(true, body.Text.Contains("If you didn't request a password reset, please visit our help center at help.markettrack.com."), "'If you didn't request a password reset, please visit our help center at help.markettrack.com.' Message not Present or match.");
                Assert.AreEqual(true, body.Text.Trim().Replace("\r\n", "").Contains("Thanks,The Numerator Team"), "'Thanks, The Numerator Team' Message not Present or match.");

                Results.WriteStatus(test, "Pass", "Verified, Mail Contents.");
            }

            IList<IWebElement> content = body.FindElements(By.TagName("a"));
            bool resetLink = false;

            for (int i = 0; i < content.Count(); i++)
            {
                if (content[i].Text.Contains("Reset your password"))
                {
                    content[i].Click();
                    resetLink = true;
                    Thread.Sleep(5000);
                    break;
                }
            }
            Assert.AreEqual(true, resetLink, "'Reset Password' Link not Present on Content.");
            Results.WriteStatus(test, "Pass", "Clicked, Reset Password Link from Email.");
            return new Login(driver, test);
        }

        #endregion
    }
}
