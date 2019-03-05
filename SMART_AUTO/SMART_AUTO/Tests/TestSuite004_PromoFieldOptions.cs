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
    [Parallelizable(ParallelScope.None)]
    public class TestSuite004_PromoFieldOptions : Base
    {
        #region Private Variables

        private IWebDriver driver;
        Login loginPage;
        Home homePage;
        Search searchPage;
        PromoDashboard promoDashboard;

        #endregion

        #region Public Fixture Methods

        public IWebDriver TestFixtureSetUp(string Bname, string testCaseName)
        {
            driver = StartBrowser(Bname);
            Common.CurrentDriver = driver;
            Results.WriteTestSuiteHeading(typeof(TestSuite004_PromoFieldOptions).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite004_PromoFieldOptions).Name);

            loginPage = new Login(driver, test);
            homePage = new Home(driver, test);
            searchPage = new Search(driver, test);
            promoDashboard = new PromoDashboard(driver, test);

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
        public void TC001_VerifyFieldOptionsOnPromoDashboard(String Bname)
        {
            TestFixtureSetUp(Bname, "TC001-Verify Field Options on Promo Dashboard.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifyFieldOptionsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify Field Options section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC002");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifyAboutFieldOptionsInFieldOptionsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify 'About Field Options' in Field options section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                promoDashboard.verifyAboutFieldOptionInFieldsOptionsSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC003");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifyHiddenOptionsInFieldOptionsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify 'Hidden Options' in Field options section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                promoDashboard.verifyHiddenFieldsInFieldsOptionsSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC004");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyThatDraggingHiddenFieldsToVisibleFields(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify that dragging Hidden Fields to Visible Fields.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                promoDashboard.clickFieldIconAndVerifyFieldNameOnFieldsOptions(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC005");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyVisibleFieldsInFieldOptionsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify 'Visible Fields' in Field options section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                promoDashboard.verifyVisibleFieldsInFieldsOptionsSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC006");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyThatDraggingHiddenFieldsToHiddenFields(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify that dragging Hidden Fields to Hidden Fields.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen().verifyVisibleFieldsInFieldsOptionsSection();
                promoDashboard.clickFieldIconAndVerifyFieldNameOnFieldsOptions(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC007");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyResetFieldsFunctionalityInVisibleFieldsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify 'Reset Fields' functionality in Visible Fields section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                string[] collections = promoDashboard.verifyOrGetDefaultFieldsInVisibleFieldsSection(null, false);
                promoDashboard.clickFieldIconAndVerifyFieldNameOnFieldsOptions(false);
                promoDashboard.clickResetFieldsButtonFromVisibleFieldsSection();
                promoDashboard.verifyOrGetDefaultFieldsInVisibleFieldsSection(collections, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC008");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyViewButtonWhenFieldOptionsSectionIsExpanded(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify 'View' button when Field Options section is expanded.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                promoDashboard.verifyButtonDisableOrNotOnScreen("View Option", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC009");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifySortingFromFieldOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify Sorting from Field options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                string fieldName = promoDashboard.clickOnSortingIconForFieldOnFieldsSection("Descending");
                promoDashboard.verifySortedFieldsRecordsOnSection(fieldName, "Descending");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC010");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyUpwardSign(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify Upward sign.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                string[] collections = promoDashboard.verifyOrGetDefaultFieldsInVisibleFieldsSection(null, false);
                promoDashboard.clickOnSignForAnyFieldOnVisibleFieldsSection(true);
                promoDashboard.clickResetFieldsButtonFromVisibleFieldsSection();
                promoDashboard.verifyOrGetDefaultFieldsInVisibleFieldsSection(collections, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC011");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_VerifyDownwardSign(String Bname)
        {
            TestFixtureSetUp(Bname, "TC012-Verify Downward sign.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                string[] collections = promoDashboard.verifyOrGetDefaultFieldsInVisibleFieldsSection(null, false);
                promoDashboard.clickOnSignForAnyFieldOnVisibleFieldsSection(false);
                promoDashboard.clickResetFieldsButtonFromVisibleFieldsSection();
                promoDashboard.verifyOrGetDefaultFieldsInVisibleFieldsSection(collections, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite004_PromoFieldOptions_TC012");
                throw;
            }
            driver.Quit();
        }

        #endregion
    }
}
