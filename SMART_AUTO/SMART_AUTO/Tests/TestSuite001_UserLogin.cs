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
    public class TestSuite001_UserLogin : Base
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
            Results.WriteTestSuiteHeading(typeof(TestSuite001_UserLogin).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite001_UserLogin).Name);

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
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifyScreenAfterInsertingValidUsername(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify screen after inserting Valid Username.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.verifyImageSectionOnLoginPage().enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC002");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifyScreenAfterInsertingValidAndInactiveUsername(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify screen after inserting Valid and Inactive Username.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.verifyImageSectionOnLoginPage();
                loginPage.enterInvalidEmailAddressAndClickNextButtonOnLoginPage("email@test.com");
                loginPage.verifyValidationMessageOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC003");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifyScreenAfterInsertingInvalidUsername(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify screen after inserting Invalid Username.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterInvalidEmailAddressAndClickNextButtonOnLoginPage("dummyEmail");
                loginPage.verifyAlertTooltipMessageOnLoginPage("Please include an '@' in the email address. 'dummyEmail' is missing an '@'.");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC004");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyScreenWithoutInsertingUsername(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify screen without inserting Username.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterInvalidEmailAddressAndClickNextButtonOnLoginPage("");
                loginPage.verifyAlertTooltipMessageOnLoginPage("Please fill out this field.", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC005");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyUsernameForPasswordScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify Username for Password screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC006");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyScreenAfterInsertingValidPassword(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify screen after inserting valid Password.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true);

                loginPage.enterValidEmailIdOrPassword(false, false);
                loginPage.checkShowPasswordCheckboxOnPasswordScreen(false).clickButtonOnLoginPage("Sign in");
                homePage.verifyHomePage();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC007");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyScreenAfterInsertingInvalidPassword(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify screen after inserting Invalid Password.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true);

                loginPage.enterInvalidPasswordAndClickSignInButtonOnLoginPage("testPassword");
                loginPage.clickButtonOnLoginPage("Sign in").verifyValidationMessageOnScreen("Invalid password.");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC008");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyScreenWithoutInsertingPassword(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify screen without inserting Password.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true);

                loginPage.enterInvalidPasswordAndClickSignInButtonOnLoginPage("");
                loginPage.clickButtonOnLoginPage("Sign in").verifyAlertTooltipMessageOnLoginPage("Please fill out this field.", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC009");
                throw;
            }
            driver.Quit();
        }

        // Pending Functionality for Copy / Paste Password
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifyThatUserShouldNotBeAbleToCopyPasswordField(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify that user should not be able to copy Password field.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC010");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyScreenAfterClickingOnTryADifferentEmailAddressButton(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify screen after clicking on 'Try a Different email address' button.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true);

                loginPage.clickButtonOnLoginPage("different email");
                loginPage.verifyLoginPageScreenInDetail();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC011");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_Verify_TroubleSigningIn_HyperlinkInBothScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC012-Verify 'Trouble Signing in ?' hyperlink in both screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.clickLinkOnLoginPage("Trouble signing in");
                loginPage.switchTabAndVerifyNavigateURLOnScreen("https://gcs-vimeo.akamaized.net");

                loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true);
                loginPage.clickLinkOnLoginPage("Trouble signing in");
                loginPage.switchTabAndVerifyNavigateURLOnScreen("https://gcs-vimeo.akamaized.net");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC012");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC013_VerifyForgetPasswordHyperlinkInPasswordScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC013-Verify 'Forget Password' hyper link in Password screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                string emailId = loginPage.enterValidEmailIdOrPassword(true);
                loginPage.verifyPasswordScreenOnLoginPage(true).clickLinkOnLoginPage("Forgot password");
                loginPage.verifyResetPasswordScreenOnLoginPage(emailId);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite001_UserLogin_TC013");
                throw;
            }
            driver.Quit();
        }

        #endregion
    }
}
