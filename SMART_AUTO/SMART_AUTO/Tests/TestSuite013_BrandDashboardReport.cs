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
    public class TestSuite013_BrandDashboardReport : Base
    {
        #region Private Variables

        private IWebDriver driver;
        Login loginPage;
        Home homePage;
        Search searchPage;
        PromoDashboard promoDashboard;
        BrandMonthlyReport brandMonthlyReport;
        Schedule schedule;
        BrandDashboard brandDashboard;

        #endregion

        #region Public Fixture Methods

        public IWebDriver TestFixtureSetUp(string Bname, string testCaseName)
        {
            driver = StartBrowser(Bname);
            Common.CurrentDriver = driver;
            Results.WriteTestSuiteHeading(typeof(TestSuite013_BrandDashboardReport).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite013_BrandDashboardReport).Name);

            loginPage = new Login(driver, test);
            homePage = new Home(driver, test);
            searchPage = new Search(driver, test);
            promoDashboard = new PromoDashboard(driver, test);
            brandMonthlyReport = new BrandMonthlyReport(driver, test);
            schedule = new Schedule(driver, test);
            brandDashboard = new BrandDashboard(driver, test);

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
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC001");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                searchPage.selectDateRangeOptionFromSection().selectMediaCheckboxOptionFromSection();
                searchPage.clickButtonOnSearchScreen("Save As");
                searchPage.verifySaveAsSectionAfterClickingOnSaveAsButton().enterSearchValueOnSearchScreen();
                searchPage.clickButtonOnSearchScreen("Save!").clickButtonOnSearchScreen("Apply Search");
                brandDashboard.verifyBrandDashboardScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC002");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.verifySavedSearchesSectionOnScreen(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC003");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                searchPage.selectDateRangeOptionFromSection().selectMediaCheckboxOptionFromSection();
                searchPage.clickButtonOnSearchScreen("Reset").verifyResetChangesMessageOnScreen(true);
                searchPage.verifyAppliedSearchFieldInChartDetailsSection("None Selected");
                searchPage.verifyFieldsRefreshIconDisableOnSummaryDetailSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC004");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.verifySavedSearchesSectionOnScreen(false);
                searchPage.clickDeleteButtonForSavedSearchRecordFromListAndVerifyMessage(true, false);
                searchPage.clickDeleteButtonForSavedSearchRecordFromListAndVerifyMessage(true, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC005");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.verifySavedSearchesSectionOnScreen(false).clickButtonOnSearchScreen("Edit Search");
                searchPage.verifyMySearchScreen("Brand Monthly");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC006");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyAfterClickingApplySearch(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify After Clicking Apply search.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.verifyMySearchScreen("Brand Canada");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(true);
                brandDashboard.verifyBrandDashboardScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC007");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyFilterBarSectionOnScreen(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC008");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyFilterBarSectionOnScreen(true);
                string[] options = { "Custom Range", "Last Month", "Last 2 Months", "Last 3 Months", "Last 6 Months", "Year To Date", "Last Year" };
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "", options);
                brandMonthlyReport.verifyFromAndToMonthSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC009");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Media Types");
                promoDashboard.verifyFilterSectionWithCheckbox("Media Types");
                string mediaName = brandMonthlyReport.verifyFilterSectionWithCheckboxAndSelectOption("Media Types");
                brandMonthlyReport.verifySelectedRecordsOnCarouselSection(mediaName);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC010");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Advertiser Products");
                promoDashboard.verifyFilterSectionOnScreen("All Advertiser Products", false);
                promoDashboard.clickButtonOnFilterSectionOnScreen("Select Displayed");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(true);
                promoDashboard.enterKeywordToSerachIntoFilterTextBox(5);
                promoDashboard.clearKeywordFromSearchTextBox();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC011");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC012");
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
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyAndClickResetAllButtonOnFilterSection(false);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 3 Months");
                promoDashboard.verifyAndClickResetAllButtonOnFilterSection(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC013");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC014_VerifyProductThumbnailAndProductDetailsInProductCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC014-Verify Product thumbnail and product details in Product carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyProductThumbnailInProductCarousel(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC014");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC015_Verify_ViewAd_FunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC015-Verify 'View Ad' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyProductThumbnailInProductCarousel(true);
                brandMonthlyReport.clickButtonLinkForProductOnCarouselSection("View Ad");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                brandMonthlyReport.verifyViewAdScreenOnPopupWindow().clickButtonOnPopupWindow("Close");
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(null, "", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC015");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC016_Verify_Markets_FunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC016-Verify 'Markets' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyProductThumbnailInProductCarousel(true);
                brandMonthlyReport.clickButtonLinkForProductOnCarouselSection("Markets");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Markets");
                brandMonthlyReport.verifyMarketsTabOnPopupWindow().clickOnGridHeaderToVerifySortingFunctionality();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC016");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC017_Verify_Details_FunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC017-Verify 'Details' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyProductThumbnailInProductCarousel(true);
                brandMonthlyReport.clickButtonLinkForProductOnCarouselSection("Details");
                string[] tabName = { "View Ad", "Markets", "More Details", "Download" };
                brandMonthlyReport.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                brandMonthlyReport.verifyMoreDetailsScreenOnPopupWindow();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC017");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC018_VerifyPreviousAndNextPageArrowForCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC018-Verify Previous and Next Page arrow for carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyNavigationArrowForCarousel("Next", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC018");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC019_VerifyCarouselSortingWhen_Spend_And_FirstRunDate_IsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC019-Verify Carousel sorting when 'Spend' and 'First Run Date' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandMonthlyReport.verifyProductThumbnailInProductCarousel(true);
                brandMonthlyReport.clickCarouselRadioOptionAndVerifyProduct("Spend");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC019");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC020_Verify_CountOfCreativesRunningByAdvertiseAndMediaType_ChartDetails(String Bname)
        {
            TestFixtureSetUp(Bname, "TC020-Verify 'Count of Creatives Running by Advertiser and Media Type' chart details.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandDashboard.verifyCountOfCreativesRunningByAdvertiserAndMediaTypeChart();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC020");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC021_VerifyUserAbleToExpandChart(String Bname)
        {
            TestFixtureSetUp(Bname, "TC021-Verify User able to expand chart.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandDashboard.verifyCountOfCreativesRunningByAdvertiserAndMediaTypeChart();
                brandDashboard.clickIconButtonOnScreenForChart("Count of Creatives Running by Advertiser and Media Type", "Expand");
                brandDashboard.verifyFullScreenOf_CountOfCreativesRunningByAdvertiserAndMediaType_Chart();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC021");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC022_VerifyGoBackButtonFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC022-Verify Go back button functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandDashboard.verifyCountOfCreativesRunningByAdvertiserAndMediaTypeChart();
                brandDashboard.clickIconButtonOnScreenForChart("Count of Creatives Running by Advertiser and Media Type", "Expand");
                brandDashboard.verifyFullScreenOf_CountOfCreativesRunningByAdvertiserAndMediaType_Chart();
                brandDashboard.clickIconButtonOnScreenForChart("Full Screen", "Go Back");
                brandDashboard.verifyCountOfCreativesRunningByAdvertiserAndMediaTypeChart();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC022");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC023_VerifyUserAbleToDownloadChartInDifferentFormat(String Bname)
        {
            TestFixtureSetUp(Bname, "TC023-Verify User able to download chart in different format.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandDashboard.verifyCountOfCreativesRunningByAdvertiserAndMediaTypeChart();
                brandDashboard.clickIconButtonOnScreenForChart("Count of Creatives Running by Advertiser and Media Type", "Download");
                brandDashboard.verifyDownloadPopupWindowAndClickOnOption("Download PNG");
                brandDashboard.verifyFileDownloadedOrNotOnScreen("MediaType", "*.png");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC023");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC024_VerifyUserAbleToCreateNewSchedule(String Bname)
        {
            TestFixtureSetUp(Bname, "TC024-Verify User able to create new schedule.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();
                brandDashboard.clickIconButtonOnScreenForChart("Count of Creatives Running by Advertiser and Media Type", "Schedule");
                brandDashboard.verifyScheduleWindow(searchTitle);
                schedule.clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC024");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC025_VerifyChartShouldBeUpdatedAccordingToUserSelectDeselectTheLegends(String Bname)
        {
            TestFixtureSetUp(Bname, "TC025-Verify Chart should be updated according to user select/deselect the legends.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandDashboard.verifyCountOfCreativesRunningByAdvertiserAndMediaTypeChart();
                brandDashboard.verifyLegendToClickAndVerifyForChart("Count of Creatives Running by Advertiser and Media Type");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC025");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC026_VerifyPopUpDetailsByHoveringMouseOnBarChart(String Bname)
        {
            TestFixtureSetUp(Bname, "TC026-Verify Pop-up details by hovering mouse on Bar chart.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                brandDashboard.hoverMouseOnBarChartAndGetTheTooltipRecords("Count of Creatives Running by Advertiser and Media Type");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC026");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC027_Verify_CountOfCreativesRunningByCompetitor_PieChartDrillDownFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC027-Verify 'Count of Creatives Running by Competitor' Pie chart drill down functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                string divID= brandDashboard.verifyChartDetailsOnScreem("Count of Creatives Running by Competitor");
                brandDashboard.clickOnPieChartAndVerifyDrillDownLevel(divID);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC027");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC028_VerifyUserAbleToChange_HiddenFields_OrderByDraggingItToUpOrDownSide(String Bname)
        {
            TestFixtureSetUp(Bname, "TC028-Verify User able to change 'Hidden Fields' order by dragging it to up or down side.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen().verifyVisibleFieldsInFieldsOptionsSection();
                promoDashboard.clickFieldIconAndVerifyFieldNameOnFieldsOptions(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC028");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC029_VerifyThatDraggingHiddenFieldsToVisibleFieldsAndViceVersa(String Bname)
        {
            TestFixtureSetUp(Bname, "TC029-Verify that dragging Hidden Fields to Visible Fields and vice versa.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                brandMonthlyReport.dragAndDropFieldFromFieldOptionsSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC029");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC030_Verify_ResetFields_FunctionalityInVisibleFieldsSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC030-Verify 'Reset Fields' functionality in Visible Fields section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                brandMonthlyReport.dragAndDropFieldFromFieldOptionsSection().verifyAndClickButtonFromFieldOptionsSection("Reset Fields");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC030");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC031_VerifySortingFromVisibleFieldOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC031-Verify Sorting from Visible Field options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.clickButtonOnViewActionSection("Field Options");
                promoDashboard.verifyFieldsOptionsSectionOnDashboardScreen();
                promoDashboard.clickOnSignForAnyFieldOnVisibleFieldsSection(true).clickOnSignForAnyFieldOnVisibleFieldsSection(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC031");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC032_VerifyGridWhen_TableView_IsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC032-Verify grid when 'Table View' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword(1);

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().verifyActionButtonOnViewSection();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Table");
                promoDashboard.verifyTableViewSectionOnScreen().verifyGridSectionForTableView();
                promoDashboard.verifyPaginationPanelForViewSection("Table View");
                promoDashboard.clickPageNumberAndIconFromGrid();
                promoDashboard.clickPageNumberAndIconFromGrid("Last").clickPageNumberAndIconFromGrid("First");
                promoDashboard.clickPageNumberAndIconFromGrid("Prev").clickPageNumberAndIconFromGrid("Next");
                promoDashboard.verifyThumbnailSectionOnScreen();
                promoDashboard.clickButtonOnViewSection("Ad Image", "Table View");
                string[] tabName = { "View Ad", "Map", "Stores", "More Details" };
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("View Ad", "Table View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("Stores", "Table View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Stores");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("Details", "Table View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                promoDashboard.clickButtonOnPopupWindow("Close");
                
            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC032");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC033_VerifyGridWhenDetailsViewIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC033-Verify Grid when 'Details View' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().verifyActionButtonOnViewSection();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Details");
                promoDashboard.verifyDetailsViewSectionOnScreen();
                promoDashboard.verifyPaginationPanelForViewSection("Details View");

                promoDashboard.clickButtonOnViewSection("Ad Image", "Details View");
                string[] tabName = { "View Ad", "Map", "Stores", "More Details" };
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("View Ad", "Details View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("Stores", "Details View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Stores");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("Details", "Details View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("Map", "Details View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Map");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickPageNumberAndIconFromGrid();
                promoDashboard.clickPageNumberAndIconFromGrid("Last").clickPageNumberAndIconFromGrid("First");
                promoDashboard.clickPageNumberAndIconFromGrid("Prev").clickPageNumberAndIconFromGrid("Next");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC033");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC034_VerifyGridWhenThumbnailViewIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC034-Verify Grid when 'Thumbnail View' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().verifyActionButtonOnViewSection();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Thumbnail");
                promoDashboard.verifyThumbnailViewSectionOnScreen();
                promoDashboard.verifyPaginationPanelForViewSection("Thumbnail View");

                promoDashboard.clickButtonOnViewSection("Ad Image", "Thumbnail View");
                string[] tabName = { "View Ad", "Map", "Stores", "More Details" };
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("View Ad", "Thumbnail View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("Stores", "Thumbnail View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Stores");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickButtonOnViewSection("Details", "Thumbnail View");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                promoDashboard.clickButtonOnPopupWindow("Close");

                promoDashboard.clickPageNumberAndIconFromGrid();
                promoDashboard.clickPageNumberAndIconFromGrid("Last").clickPageNumberAndIconFromGrid("First");
                promoDashboard.clickPageNumberAndIconFromGrid("Prev").clickPageNumberAndIconFromGrid("Next");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC034");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC035_VerifyViewSelectedFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC035-Verify 'view Selected' functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                brandDashboard.verifyBrandDashboardScreen();
                promoDashboard.verifyButtonDisableOrNotOnScreen("View Selected", true);
                promoDashboard.selectRecordFromViewSection().clickButtonOnViewActionSection("View Selected");
                promoDashboard.verifyButtonDisableOrNotOnScreen("View Selected", false);
                promoDashboard.verifyViewSelectedButtonCheckedOrNotOnScreen(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite013_BrandDashboardReport_TC035");
                throw;
            }
            driver.Quit();
        }

        #endregion
    }
}