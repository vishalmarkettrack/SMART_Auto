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
    public class TestSuite005_PromoExportFunctionality : Base
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
            Results.WriteTestSuiteHeading(typeof(TestSuite005_PromoExportFunctionality).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite005_PromoExportFunctionality).Name);

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
        public void TC001_VerifyExportAllFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC001-Verify 'Export All' Functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyButtonDisableOrNotOnScreen("Export All", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifyExportFunctionalityForDataReportsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify 'Export' Functionality for Data Reports Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.selectRecordFromViewSection();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports", "Download");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.xlsx");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC002");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifyDownloadTooltipWhenDownloadIsAvailableForDataReportsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify Download tooltip when download is available for Data Reports Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.selectRecordFromViewSection();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Data Reports", "Download");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC003");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifyExportFunctionalityForPowerPointReports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify 'Export' Functionality For Power Point Reports.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.selectRecordFromViewSection();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Download", "1 Product / Slide");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.pptx");

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Download", "2 Products / Slide (1x2)");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.pptx");

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Download", "2 Products / Slide (2x1)");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.pptx");

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Download", "4 Products / Slide");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.pptx");

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Download", "10 Products / Slide");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.pptx");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC004");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyDownloadTooltipWhenDownloadIsAvailableForPowerPointReportsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify Download tooltip when download is available  for Power Point Reports Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.selectRecordFromViewSection();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");                
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Power Point Reports", "Download");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC005");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyExportFunctionalityForAssetReports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify 'Export' Functionality For Asset Reports.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.selectRecordFromViewSection();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads", "Download");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.zip");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC006");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyDownloadTooltipWhenDownloadIsAvailableForAssetReportsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify Download tooltip when download is available  for Asset Reports Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.selectRecordFromViewSection();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Asset Downloads", "Download");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC007");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyDataReportsWhenTotalRecordsAreMoreThan5000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify Data Reports when total records are more than 5000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Data Reports", "Email", "Send results via email");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC008");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyPowerPointReportsWhenTotalRecordsAreMoreThan1000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify Power Point Reports when total records are more than 1000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Power Point Reports", "Email", "Send results via email");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC009");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifyAssetDownloadsWhenTotalRecordsAreMoreThan1000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify Asset Downloads when total records are more than 1000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Asset Downloads", "Email", "Send results via email");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC010");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyDataReportsWhenTotalRecordsAreMoreThan50000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify Data Reports when total records are more than 50000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Data Reports", "Ban", "You have too many items selected. Select fewer than: 50000");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC011");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_VerifyPowerPointReportsWhenTotalRecordsAreMoreThan2000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC012-Verify Power Point Reports when total records are more than 2000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Power Point Reports", "Ban", "You have too many items selected. Select fewer than: 2000");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC012");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC013_VerifyAssetDownloadsWhenTotalRecordsAreMoreThan1000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC013-Verify Asset Downloads when total records are more than 1000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Asset Downloads", "Ban", "You have too many items selected. Select fewer than: 2000");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC013");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC014_VerifyViewSelectedAndResetSelectedButtonsWhenExportAllIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC014-Verify 'View Selected' and 'Reset Selected' buttons when 'Export All' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();

                promoDashboard.verifyButtonDisableOrNotOnScreen("View Selected", true);
                promoDashboard.verifyButtonDisableOrNotOnScreen("Reset Selected", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC014");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC015_VerifyExportSelectedFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC015-Verify 'Export Selected' Functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.selectRecordFromViewSection().verifyButtonDisableOrNotOnScreen("Export Selected", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC015");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC016_VerifyExportSelectedFunctionalityForDataReportsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC016-Verify 'Export Selected' Functionality for Data Reports Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.selectRecordFromViewSection();
                promoDashboard.clickButtonOnViewActionSection("Export Selected").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports", "Download");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.xlsx");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC016");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC017_VerifyExportSelectedFunctionalityForPowerPointReports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC017-Verify 'Export Selected' Functionality for Power Point Reports.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.selectRecordFromViewSection();
                promoDashboard.clickButtonOnViewActionSection("Export Selected").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Download");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.pptx");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC017");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC018_VerifyExportSelectedFunctionalityForAssetReports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC018-Verify 'Export Selected' Functionality for Asset Reports.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.selectRecordFromViewSection();
                promoDashboard.clickButtonOnViewActionSection("Export Selected").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads", "Download");
                promoDashboard.verifyFileDownloadedOrNotOnScreen("promo_dashtest", "*.zip");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite005_PromoExportFunctionality_TC018");
                throw;
            }

            driver.Quit();
        }

        #endregion
    }
}
