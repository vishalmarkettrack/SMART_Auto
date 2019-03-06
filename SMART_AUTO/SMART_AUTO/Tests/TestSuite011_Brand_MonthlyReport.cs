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
    public class TestSuite011_Brand_MonthlyReport : Base
    {
        #region Private Variables

        private IWebDriver driver;
        Login loginPage;
        Home homePage;
        UserProfile userProfile;
        BrandMonthlyReport brandMonthlyReport;
        Search searchPage;
        Schedule schedule;
        PromoDashboard promoDashboard;

        #endregion

        #region Public Fixture Methods

        public IWebDriver TestFixtureSetUp(string Bname, string testCaseName)
        {
            driver = StartBrowser(Bname);
            Common.CurrentDriver = driver;
            Results.WriteTestSuiteHeading(typeof(TestSuite011_Brand_MonthlyReport).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite011_Brand_MonthlyReport).Name);

            loginPage = new Login(driver, test);
            homePage = new Home(driver, test);
            userProfile = new UserProfile(driver, test);
            brandMonthlyReport = new BrandMonthlyReport(driver, test);
            searchPage = new Search(driver, test);
            schedule = new Schedule(driver, test);
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
        public void TC001_VerifyHeaderPanel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC001-Verify Header panel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifySearchFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify Search functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                searchPage.selectDateRangeOptionFromSection().selectMediaCheckboxOptionFromSection();
                searchPage.clickButtonOnSearchScreen("Save As");
                searchPage.verifySaveAsSectionAfterClickingOnSaveAsButton().enterSearchValueOnSearchScreen();
                searchPage.clickButtonOnSearchScreen("Save!").clickButtonOnSearchScreen("Apply Search");
                brandMonthlyReport.verifyBrandMonthlyReportScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC002");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifySaveSearchCardInSavedSearchScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify Save search Card in Saved search screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.verifySavedSearchesSectionOnScreen(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC003");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifyUserAbleToResetSavedSearch(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify User able to Reset saved search.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                searchPage.selectDateRangeOptionFromSection().selectMediaCheckboxOptionFromSection();
                searchPage.clickButtonOnSearchScreen("Reset").verifyResetChangesMessageOnScreen(true);
                searchPage.verifyAppliedSearchFieldInChartDetailsSection("None Selected");
                searchPage.verifyFieldsRefreshIconDisableOnSummaryDetailSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC004");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyDeleteSavedSearchFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify Delete Saved Search Functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.verifySavedSearchesSectionOnScreen(false);
                searchPage.clickDeleteButtonForSavedSearchRecordFromListAndVerifyMessage(true, false);
                searchPage.clickDeleteButtonForSavedSearchRecordFromListAndVerifyMessage(true, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC005");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyEditSavedSearchFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify Edit saved Search functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.verifySavedSearchesSectionOnScreen(false).clickButtonOnSearchScreen("Edit Search");
                searchPage.verifyMySearchScreen("Brand Monthly");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC006");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyAfterClickingApplySearch(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-verify After Clicking Apply search.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(true);
                brandMonthlyReport.verifyBrandMonthlyReportScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC007");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                brandMonthlyReport.verifyFilterBarSectionOnScreen(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC008");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyDateRangeFieldInFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify Date Range Field in Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                brandMonthlyReport.verifyFilterBarSectionOnScreen(true);
                string[] options = { "Custom Range", "Last Month", "Last 2 Months", "Last 3 Months", "Last 6 Months", "Year To Date", "Last Year" };
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "", options);
                brandMonthlyReport.verifyFromAndToMonthSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC009");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifyAllMediaTypeFieldDropdownForFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify 'All Media Type' field drop down for Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("All Media Types");
                promoDashboard.verifyFilterSectionWithCheckbox("Media Types");
                string mediaName = brandMonthlyReport.verifyFilterSectionWithCheckboxAndSelectOption("Media Types");
                brandMonthlyReport.verifySelectedRecordsOnCarouselSection(mediaName);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC010");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyAllAdvertiserProductsFieldDropdownForFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify 'All Advertiser Products' field drop down for Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen();
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("All Advertiser Products");
                promoDashboard.verifyFilterSectionOnScreen("All Advertiser Products", false);
                promoDashboard.clickButtonOnFilterSectionOnScreen("Select Displayed");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(true);
                promoDashboard.enterKeywordToSerachIntoFilterTextBox(5);
                promoDashboard.clearKeywordFromSearchTextBox();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC011");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_VerifyResetAllButtonWhenNoFilterIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC012-Verify 'Reset All' button when no Filter is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC012");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC013_VerifyResetAllButtonWhenFilterIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC013-Verify 'Reset All' button when Filter is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyAndClickResetAllButtonOnFilterSection(false);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 3 Months");
                promoDashboard.verifyAndClickResetAllButtonOnFilterSection(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC013");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC014_VerifyAdvertiserRankingsPeriodOverPeriodChartDetails(String Bname)
        {
            TestFixtureSetUp(Bname, "TC014-Verify 'Advertiser Rankings Period over Period' chart details.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC014");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC015_VerifyUserAbleToExpandChart(String Bname)
        {
            TestFixtureSetUp(Bname, "TC015-Verify User able to expand chart.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen();
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period", "Expand");
                brandMonthlyReport.verifyFullScreenOfAdvertiserRankingsPeriodOverPeriodChart();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC015");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC016_VerifyGoBackButtonFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC016-Verify Go back button functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen();
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period", "Expand");
                brandMonthlyReport.verifyFullScreenOfAdvertiserRankingsPeriodOverPeriodChart();
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period Full Screen", "Go Back");
                brandMonthlyReport.verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC016");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC017_VerifyUserAbleToViewChartInTabularView(String Bname)
        {
            TestFixtureSetUp(Bname, "TC017-Verify User able to view chart in Tabular view.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen();
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period", "Tabular");
                brandMonthlyReport.verifyTabularViewSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC017");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC018_VerifyUserAbleToDownloadGrid(String Bname)
        {
            TestFixtureSetUp(Bname, "TC018-Verify User able to download grid.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen();
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period", "Tabular");
                brandMonthlyReport.verifyTabularViewSectionOnScreen().clickButtonFromTabularViewScreen("Download Grid");
                brandMonthlyReport.verifyExportingGridProcessToComplete();
                brandMonthlyReport.verifyExcelFileDownloadedOrNotOnScreen("advertiseractivity");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC018");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC019_VerifyTabularOptionsFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC019-Verify Tabular options functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyAdvertiserRankingsPeriodOverPeriodSectionOnScreen();
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period", "Tabular");
                brandMonthlyReport.verifyTabularViewSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC019");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC020_VerifyUserAbleToCreateNewSchedule(String Bname)
        {
            TestFixtureSetUp(Bname, "TC020-Verify User able to create new schedule.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period", "Schedule");
                brandMonthlyReport.verifyScheduleWindow(searchTitle);
                schedule.clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC020");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC021_VerifyUserAbleToDownloadChartInDifferentFormat(String Bname)
        {
            TestFixtureSetUp(Bname, "TC021-Verify User able to download chart in different format.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.clickIconButtonOnScreenForChart("Advertiser Rankings Period over Period", "Download");
                brandMonthlyReport.verifyDownloadPopupWindowAndClickOnOption("Download PNG");
                brandMonthlyReport.verifyFileDownloadedOrNotOnScreen("AdvertiserActivity", "*.png");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC021");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC022_VerifyChartShouldBeUpdatedAccordingToUserSelect_DeselectTheLegends(String Bname)
        {
            TestFixtureSetUp(Bname, "TC022-Verify Chart should be updated according to user select/deselect the legends.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyLegendToClickAndVerifyForChart("Advertiser Rankings Period over Period");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC022");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC023_VerifyProductThumbnailAndProductDetailsInProductCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC023-Verify Product thumbnail and product details in Product carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyProductThumbnailInProductCarousel();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC023");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC024_VerifyViewAdFunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC024-Verify 'View Ad' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyProductThumbnailInProductCarousel();
                brandMonthlyReport.clickButtonLinkForProductOnCarouselSection("View Ad");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.verifyViewAdScreenOnPopupWindow().clickButtonOnPopupWindow("Close");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(null, "", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC024");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC025_VerifyMarketsFunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC025-Verify 'Markets' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyProductThumbnailInProductCarousel();
                brandMonthlyReport.clickButtonLinkForProductOnCarouselSection("Markets");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Markets");
                brandMonthlyReport.verifyMarketsTabOnPopupWindow().clickOnGridHeaderToVerifySortingFunctionality();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC025");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC026_VerifyDetailsFunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC026-Verify 'Details' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyProductThumbnailInProductCarousel(true);
                brandMonthlyReport.clickButtonLinkForProductOnCarouselSection("Details");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                brandMonthlyReport.verifyMoreDetailsScreenOnPopupWindow();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC026");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC027_VerifyPreviousAndNextPageArrowForCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC027-Verify Previous and Next Page arrow for carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyNavigationArrowForCarousel("Next", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC027");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC028_VerifyCarouselSortingWhen_Spend_And_FirstRunDate_IsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC028-Verify Carousel sorting when 'Spend' and 'First Run Date' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyProductThumbnailInProductCarousel(true);
                brandMonthlyReport.clickCarouselRadioOptionAndVerifyProduct("Spend");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC028");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC029_VerifyUserAbleToChange_HiddenFields_OrderByDraggingItToUpOrDownSide(String Bname)
        {
            TestFixtureSetUp(Bname, "TC029-Verify User able to change 'Hidden Fields' order by dragging it to up or down side.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen().verifyVisibleFieldsInFieldsOptionsSection();
                promoDashboard.clickFieldIconAndVerifyFieldNameOnFieldsOptions(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC029");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC030_VerifyThatDraggingHiddenFieldsToVisibleFieldsAndViceVersa(String Bname)
        {
            TestFixtureSetUp(Bname, "TC030-Verify that dragging Hidden Fields to Visible Fields and vice versa.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                brandMonthlyReport.dragAndDropFieldFromFieldOptionsSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC030");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC031_VerifyResetFieldsFunctionalityInVisibleFieldsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC031-Verify 'Reset Fields' functionality in Visible Fields section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                brandMonthlyReport.dragAndDropFieldFromFieldOptionsSection().verifyAndClickButtonFromFieldOptionsSection("Reset Fields");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC031");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC032_VerifySortingFromVisibleFieldOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC032-Verify Sorting from Visible Field options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                promoDashboard.clickOnSignForAnyFieldOnVisibleFieldsSection(true);
                promoDashboard.clickOnSignForAnyFieldOnVisibleFieldsSection(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC032");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC033_VerifyGridWhenTableViewIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC033-Verify grid when 'Table View' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.clickViewButtonIconToVerifyOptionsAndClick("Table");
                brandMonthlyReport.verifyTableViewSectionOnScreen().verifyGridSectionForTableView();
                brandMonthlyReport.verifyPaginationPanelForViewSection("Table View");
                brandMonthlyReport.clickPageNumberAndIconFromGrid();
                brandMonthlyReport.clickPageNumberAndIconFromGrid("Last").clickPageNumberAndIconFromGrid("First");
                brandMonthlyReport.clickPageNumberAndIconFromGrid("Prev").clickPageNumberAndIconFromGrid("Next");
                brandMonthlyReport.verifyThumbnailSectionOnScreen();
                brandMonthlyReport.clickButtonOnViewSection("Ad Image", "Table View");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("View Ad", "Table View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("Markets", "Table View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Markets");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("Details", "Table View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC033");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC034_VerifyGridWhenDetailsViewIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC034-Verify Grid when 'Details View' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.clickViewButtonIconToVerifyOptionsAndClick("Details");
                brandMonthlyReport.verifyDetailsViewSectionOnScreen();
                brandMonthlyReport.verifyPaginationPanelForViewSection("Details View");

                brandMonthlyReport.clickButtonOnViewSection("Ad Image", "Details View");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("View Ad", "Details View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("Markets", "Details View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Markets");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("Details", "Details View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("Download", "Details View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Download");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickPageNumberAndIconFromGrid();
                brandMonthlyReport.clickPageNumberAndIconFromGrid("Last").clickPageNumberAndIconFromGrid("First");
                brandMonthlyReport.clickPageNumberAndIconFromGrid("Prev").clickPageNumberAndIconFromGrid("Next");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC034");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC035_VerifyGridWhenThumbnailViewIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC035-Verify Grid when 'Thumbnail View' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.clickViewButtonIconToVerifyOptionsAndClick("Thumbnail");
                brandMonthlyReport.verifyThumbnailViewSectionOnScreen();
                brandMonthlyReport.verifyPaginationPanelForViewSection("Thumbnail View");

                brandMonthlyReport.clickButtonOnViewSection("Ad Image", "Thumbnail View");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("View Ad", "Thumbnail View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("Markets", "Thumbnail View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Markets");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickButtonOnViewSection("Details", "Thumbnail View");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                brandMonthlyReport.clickButtonOnPopupWindow("Close");

                brandMonthlyReport.clickPageNumberAndIconFromGrid();
                brandMonthlyReport.clickPageNumberAndIconFromGrid("Last").clickPageNumberAndIconFromGrid("First");
                brandMonthlyReport.clickPageNumberAndIconFromGrid("Prev").clickPageNumberAndIconFromGrid("Next");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC035");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC036_VerifyViewSelectedFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC036-Verify 'View Selected' functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyButtonDisableOrNotOnScreen("View Selected", true);
                brandMonthlyReport.selectRecordFromGridPanelOnScreen().clickButtonOnViewActionSection("View Selected");
                brandMonthlyReport.verifyButtonDisableOrNotOnScreen("View Selected", false);
                brandMonthlyReport.verifyViewSelectedButtonCheckedOrNotOnScreen(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC036");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC037_VerifyResetSelectedButtonWhenRecordsAreSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC037-Verify 'Reset Selected' button when records are selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true).selectRecordFromGridPanelOnScreen();
                brandMonthlyReport.verifyButtonDisableOrNotOnScreen("Reset Selected", false);
                brandMonthlyReport.clickButtonOnViewActionSection("Reset Selected");
                brandMonthlyReport.verifyViewSelectedButtonCheckedOrNotOnScreen(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC037");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC038_VerifyResetSelectedButtonWhenRecordsAreNotSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC038-Verify 'Reset Selected' button when records are not selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                brandMonthlyReport.verifyButtonDisableOrNotOnScreen("Reset Selected", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC038");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC039_VerifyThatLabelShouldChangeToViewAllAfterClickingOnViewSelectedButton(String Bname)
        {
            TestFixtureSetUp(Bname, "TC039-Verify that label should change to 'View all' after clicking on View Selected button.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true).selectRecordFromGridPanelOnScreen();
                brandMonthlyReport.verifyButtonDisableOrNotOnScreen("Reset Selected", false);
                brandMonthlyReport.clickButtonOnViewActionSection("Reset Selected");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC039");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC040_VerifyExportSelectedFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC040-Verify 'Export Selected' Functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true).selectRecordFromGridPanelOnScreen();
                brandMonthlyReport.clickButtonOnViewActionSection("Export Selected").verifyExportAllSectionOnDashboardScreen();
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports", "Download");
                brandMonthlyReport.verifyFileDownloadedOrNotForRecordsOnScreen("qatest-mr", "*.xlsx");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC040");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC041_VerifyExportSelectedFunctionalityForDataReportsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC041-Verify 'Export Selected' Functionality for Data Reports Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true).selectRecordFromGridPanelOnScreen();
                brandMonthlyReport.clickButtonOnViewActionSection("Export Selected").verifyExportAllSectionOnDashboardScreen();
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports");
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports", "Download");
                brandMonthlyReport.verifyFileDownloadedOrNotForRecordsOnScreen("qatest-mr", "*.xlsx");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC041");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC042_VerifyExportSelectedFunctionalityForPowerPointReports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC042-Verify 'Export Selected' Functionality for Power Point Reports.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true).selectRecordFromGridPanelOnScreen();
                brandMonthlyReport.clickButtonOnViewActionSection("Export Selected").verifyExportAllSectionOnDashboardScreen();
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Download", "1 Creative / Slide");
                brandMonthlyReport.verifyFileDownloadedOrNotForRecordsOnScreen("qatest-mr", "*.pptx");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC042");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC043_VerifyExportSelectedFunctionalityForAssetReports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC043-Verify 'Export Selected' Functionality for Asset Reports.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true).selectRecordFromGridPanelOnScreen();
                brandMonthlyReport.clickButtonOnViewActionSection("Export Selected").verifyExportAllSectionOnDashboardScreen();
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads");
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads", "Download");
                brandMonthlyReport.verifyFileDownloadedOrNotForRecordsOnScreen("qatest-mr", "*.zip");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC043");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC044_VerifyDataReportsWhenTotalRecordsAreMoreThan5000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC044-Verify Data Reports when total records are more than 5000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                homePage.clickOnDayFilterFieldAndClickOption("Last Year");
                brandMonthlyReport.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports", "Email");
                // Email Functionality can not be verify.

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC044");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC045_VerifyPowerPointReportsWhenTotalRecordsAreMoreThan1000(String Bname)
        {
            TestFixtureSetUp(Bname, "TC045-Verify Power Point Reports when total records are more than 1000.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                homePage.clickOnDayFilterFieldAndClickOption("Last Year");
                brandMonthlyReport.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Email");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Power Point Reports", "Email", "Send results via email");
                // Email Functionality can not be verify.

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC045");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC046_VerifyAssetDownloadsWhenTotalRecordsAreMoreThan250(String Bname)
        {
            TestFixtureSetUp(Bname, "TC046-Verify Asset Downloads when total records are more than 250.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                //homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("QA Testing - Brand - Monthly Report");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Monthly Report");

                brandMonthlyReport.verifyBrandMonthlyReportScreen(true);
                homePage.clickOnDayFilterFieldAndClickOption("Last Year");
                brandMonthlyReport.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                brandMonthlyReport.verifyOrClickExportAllSectionInDetailOnScreen("Asset Downloads");
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Asset Downloads", "Email", "Send results via email");
                // Email Functionality can not be verify.

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite011_Brand_MonthlyReport_TC046");
                throw;
            }
            driver.Quit();
        }

        #endregion
    }
}
