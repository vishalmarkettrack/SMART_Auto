using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SMART_AUTO.SMART_AUTO
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TestSuite002_ForgotPassword : Base
    {
        #region Private Variables

        private IWebDriver driver;
        Login loginPage;
        Home homePage;
        Search searchPage;

        #endregion

        #region Public Fixture Methods

        public IWebDriver TestFixtureSetUp(string Bname, string testCaseName)
        {
            driver = StartBrowser(Bname);
            Common.CurrentDriver = driver;
            Results.WriteTestSuiteHeading(typeof(TestSuite002_ForgotPassword).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite002_ForgotPassword).Name);

            loginPage = new Login(driver, test);
            homePage = new Home(driver, test);
            searchPage = new Search(driver, test);

            return driver;
        }

        [TearDown]
        public void TestFixtureTearDown()
        {
            extent.Flush();
            driver.Quit();
        }

        #endregion

        #region Test Methods

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC001_VerifySignInScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC001-Verify Sign in screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.verifyImageSectionOnLoginPage().clickLearnMoreLinkOnImageSection();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                loginPage.verifyNavigateURLOnScreen("https://numerator.com/");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifyScreenAfterInsertingValidAndActiveUsername(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify screen after inserting Valid and Active Username.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC002");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifyForgotPasswordLinkInPasswordScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify 'forgot Password' link in Password screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC003");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifySendInstructionButtonFunctionalityOnForgotPasswordScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify 'Send Instruction' button functionality on Forgot Password screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC004");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyEmailContent(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify Email Content.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

                //driver.Navigate().GoToUrl("https://outlook.office365.com");

                //loginPage.verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton();
                //loginPage.verifyOutlookHomePage();
                //loginPage.selectResetPasswordMailToOpenResetLink(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC005");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyResetYourPasswordLinkFromMail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify 'Reset your Password' link from mail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

                //driver.Navigate().GoToUrl("https://outlook.office365.com");

                //loginPage.verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton();
                //loginPage.verifyOutlookHomePage();
                //loginPage.selectResetPasswordMailToOpenResetLink(true);

                //driver.SwitchTo().Window(driver.WindowHandles.Last());
                //loginPage.verifyCreateANewPasswordOrSuccessfullPasswordScreen(emailId).enterPasswordAndClickButton("Set password", "");
                //loginPage.verifyCreateANewPasswordOrSuccessfullPasswordScreen(emailId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC006");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifySetPasswordScreenWhenUserHasEnteredNewPasswordAndConfirmPasswordDifferent(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify set Password screen when user has entered New Password and Confirm password different.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

                //driver.Navigate().GoToUrl("https://outlook.office365.com");

                //loginPage.verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton();
                //loginPage.verifyOutlookHomePage();
                //loginPage.selectResetPasswordMailToOpenResetLink(true);

                //driver.SwitchTo().Window(driver.WindowHandles.Last());
                //loginPage.verifyCreateANewPasswordOrSuccessfullPasswordScreen(emailId).enterPasswordAndClickButton("Set password", "Invalid");
                //loginPage.verifyValidationMessageOnScreen("Your password confirmation does not match your password.");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC007");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyResetYourPasswordForOldEmailLink(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify 'Reset your Password' for old Email link.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);
                loginPage.clickButtonOnResetPasswordScreen("Resend password instructions");

                //driver.Navigate().GoToUrl("https://outlook.office365.com");

                //loginPage.verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton();
                //loginPage.verifyOutlookHomePage();
                //loginPage.selectSecondResetPasswordMailToOpenResetLink(true);

                //driver.SwitchTo().Window(driver.WindowHandles.Last());
                //loginPage.verifyNoLongerScreenActivePage(emailId).clickButtonOnResetPasswordScreen("Get me out of here");
                //loginPage.verifyPasswordScreenOnLoginPage(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC008");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyResetPasswordFunctionalityWithHyperlinkMentionedInBottomOfTheScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify reset password functionality with hyperlink mentioned in bottom of the screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

                //driver.Navigate().GoToUrl("https://outlook.office365.com");

                //loginPage.verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton();
                //loginPage.verifyOutlookHomePage();
                //loginPage.selectResetPasswordMailToOpenResetLink(true);

                //driver.SwitchTo().Window(driver.WindowHandles.Last());
                //loginPage.verifyCreateANewPasswordOrSuccessfullPasswordScreen(emailId).enterPasswordAndClickButton("Set password", "New Same Password");
                //homePage.verifyHomePage();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC009");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifySetPasswordScreenWhenUserHasEnteredNewPasswordAndConfirmPasswordSameAsCURRENTPassword(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify set Password screen when user has entered New Password and Confirm password same as 'CURRENT' Password.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

                //driver.Navigate().GoToUrl("https://outlook.office365.com");

                //loginPage.verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton();
                //loginPage.verifyOutlookHomePage();
                //loginPage.selectResetPasswordMailToOpenResetLink(true);

                //driver.SwitchTo().Window(driver.WindowHandles.Last());
                //loginPage.verifyCreateANewPasswordOrSuccessfullPasswordScreen(emailId).enterPasswordAndClickButton("Set password", "");
                //loginPage.verifyCreateANewPasswordOrSuccessfullPasswordScreen(emailId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC010");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyValidationsForPasswordScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify Validations for Password screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

                //driver.Navigate().GoToUrl("https://outlook.office365.com");

                //loginPage.verifyOutlookLoginScreenToEnterCredentialAndClickSignInButton();
                //loginPage.verifyOutlookHomePage();
                //loginPage.selectResetPasswordMailToOpenResetLink(true);

                //driver.SwitchTo().Window(driver.WindowHandles.Last());
                //loginPage.verifyCreateANewPasswordOrSuccessfullPasswordScreen(emailId).enterPasswordAndClickButton("Set password", "Lower Case");
                //loginPage.verifyValidationMessageOnScreen("Passwords must contain letters in mixed case." + "\r\n" + "Passwords must contain at least 1 numeric or special character.");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC011");
                throw;
            }
            driver.Quit();
        }

        // not able to verify Email Functionality
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_VerifyPasswordChangedEmailDetails(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify Password changed email details.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);
                loginPage.clickButtonOnResetPasswordScreen("Send instructions").verifyResetPasswordScreenOnLoginPage(emailId, true);

                // not able to verify Email Functionality
            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite002_ForgotPassword_TC011");
                throw;
            }
            driver.Quit();
        }

        #endregion
    }
}
