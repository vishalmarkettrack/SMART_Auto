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
    public class TestSuite003_PromoDashboard : Base
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
            Results.WriteTestSuiteHeading(typeof(TestSuite003_PromoDashboard).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite003_PromoDashboard).Name);

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
        public void TC001_VerifyPromoDashboard(String Bname)
        {
            TestFixtureSetUp(Bname, "TC001-Verify Promo Dashboard.");
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
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifyFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC002");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifyDateRangeFieldInFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify Date Range Field in Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                string[] opt = { "Custom Range", "Today", "Yesterday", "Last 7 Days", "Last 14 Days", "Last Month", "Last 3 Months", "Last 6 Months", "Year To Date", "Last Year" };
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "", opt);
                promoDashboard.verifyFromDateAndToDatePickerOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC003");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifyRetailerFieldDropdownForFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify 'Retailer' field drop down for Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Manufacturers");
                promoDashboard.verifyFilterSectionOnScreen("Manufacturers", true);
                promoDashboard.clickButtonOnFilterSectionOnScreen("Select Displayed");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(true);
                promoDashboard.clickButtonOnFilterSectionOnScreen("Slash").verifyExcludedButtonLabelOnFilterSection();
                promoDashboard.clickButtonOnFilterSectionOnScreen("Load More");
                promoDashboard.enterKeywordToSerachIntoFilterTextBox(1).verifyTooltipOnFilterSection("Min Char Limit 2");
                promoDashboard.clearKeywordFromSearchTextBox();

                promoDashboard.clickButtonOnFilterSectionOnScreen("Browse").verifyBrowseTabOnFilterSection();
                promoDashboard.clickButtonOnFilterSectionOnScreen("Excluded").verifyExcludedButtonLabelOnFilterSection();
                string characterName = promoDashboard.selectCharacterFromBrowserTab();
                promoDashboard.verifyFilterListRecordsValueWithSelectedCharacter(characterName);
                promoDashboard.clickButtonOnFilterSectionOnScreen("Excluded Remove").selectRecordsFromListOnFilterSection();
                promoDashboard.clickButtonOnFilterSectionOnScreen("Selected");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(true).clickButtonOnFilterSectionOnScreen("Clear Selected");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC004");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyMarketsFieldDropdownForFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify 'Markets' field drop down for Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Markets");
                promoDashboard.verifyFilterSectionOnScreen("Markets", false);
                promoDashboard.clickButtonOnFilterSectionOnScreen("Select Displayed");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(true);
                promoDashboard.clickButtonOnFilterSectionOnScreen("Slash").verifyExcludedButtonLabelOnFilterSection();

                promoDashboard.clickButtonOnFilterSectionOnScreen("Slash Remove").clickButtonOnFilterSectionOnScreen("Selected");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(true).clickButtonOnFilterSectionOnScreen("Clear Selected");
                promoDashboard.verifyFilterListRecordsSelectedOrNotOnFilterSection(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC005");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyPageLocationsFieldDropdownForFilterBar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify 'Page Locations' field drop down for Filter bar.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Page Locations");
                promoDashboard.verifyFilterSectionWithCheckbox("Page Locations");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC006");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyResetAllButtonWhenNoFilterIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify 'Reset All' button when no Filter is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC007");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyResetAllButtonWhenFilterIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify 'Reset All' button when Filter is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyAndClickResetAllButtonOnFilterSection(false);
                promoDashboard.clickOnFilterFieldAndVerifyOrClickOptions("Days", "Last 6 Months");
                promoDashboard.verifyAndClickResetAllButtonOnFilterSection(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC008");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyPreviousIconOnFilterSlider(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify Previous icon on Filter Slider.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyAndClickIconOnFilterSlider("Previous");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC009");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifyNextIconOnFilterSlider(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify Next icon on Filter Slider.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyAndClickIconOnFilterSlider("Next");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC010");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyProductCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify Product carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyProductThumbnailForProductCarousel();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC011");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_VerifyProductThumbnailInProductCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC012-Verify Product thumbnail in Product carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyProductThumbnailForProductCarousel(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC012");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC013_VerifyViewAdFunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC013-Verify 'View Ad' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyProductThumbnailForProductCarousel(true);
                promoDashboard.clickButtonLinkForProductOnCarouselSection("View Ad");
                string[] tabName = { "View Ad", "Map", "Stores", "More Details" };
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.verifyViewAdScreenOnPopupWindow().clickButtonOnPopupWindow("Close");
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(null, "", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC013");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC014_VerifyStoresFunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC014-Verify 'Stores' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyProductThumbnailForProductCarousel(true);
                promoDashboard.clickButtonLinkForProductOnCarouselSection("Stores");
                string[] tabName = { "View Ad", "Map", "Stores", "More Details" };
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "Stores");
                promoDashboard.verifyStoresScreenOnPopupWindow();

                //promoDashboard.clickFilterIconAndVerifySection("Retailer").selectOptionFromFilterBarSection("Select All", true);
                //promoDashboard.enterAndVerifyKeywordInToFilterSearchTextbox().clearSearchTextboxOnFilterSection();
                //promoDashboard.selectOptionFromFilterBarSection("Select All", false);
                ////promoDashboard.clickColumnHeaderSortAndCompareOnGrid("Retailer");

                //promoDashboard.clickFilterIconAndVerifySection("City").selectOptionFromFilterBarSection("Select All", true);
                //promoDashboard.enterAndVerifyKeywordInToFilterSearchTextbox().clearSearchTextboxOnFilterSection();
                //promoDashboard.selectOptionFromFilterBarSection("Select All", false);
                ////promoDashboard.clickColumnHeaderSortAndCompareOnGrid("City");

                //promoDashboard.clickFilterIconAndVerifySection("State").selectOptionFromFilterBarSection("Select All", true);
                //promoDashboard.enterAndVerifyKeywordInToFilterSearchTextbox().clearSearchTextboxOnFilterSection();
                //promoDashboard.selectOptionFromFilterBarSection("Select All", false);
                ////promoDashboard.clickColumnHeaderSortAndCompareOnGrid("State");

                string[,] records = promoDashboard.getProductDetailsGridRecordsFromPopupWindow();
                promoDashboard.clickButtonOnPopupWindow("Download Grid");
                promoDashboard.verifyValuesOfDownloadedFile("export", records);

                promoDashboard.clickButtonOnPopupWindow("Grid Options");
                string label = promoDashboard.checkedUncheckedGridOptionsFromPopupWindow(false);
                promoDashboard.verifyGridTitleOnPopupWindow(label, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC014");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC015_VerifyDetailsFunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC015-Verify 'Details' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyProductThumbnailForProductCarousel(true);
                promoDashboard.clickButtonLinkForProductOnCarouselSection("Details");
                string[] tabName = { "View Ad", "Map", "Stores", "More Details" };
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "More Details");
                promoDashboard.verifyMoreDetailsScreenOnPopupWindow().clickAdImageOnDetailsSection();
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC015");
                throw;
            }
            driver.Quit();
        }

        // Pending due to Map functionality properly not working on Dev site
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC016_VerifyMAPfunctionalityForIndividualProductThumbnail(String Bname)
        {
            TestFixtureSetUp(Bname, "TC016-Verify 'MAP' functionality for Individual Product Thumbnail.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyProductThumbnailForProductCarousel(true);
                promoDashboard.clickButtonLinkForProductOnCarouselSection("Ad Image");
                string[] tabName = { "View Ad", "Map", "Stores", "More Details" };
                promoDashboard.verifyProductDetailPopupWindowOnDashboardPage(tabName, "View Ad");
                promoDashboard.selectTabOnProductDetailsPopuWindow("Map");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC016");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC017_VerifyPreviousPageArrowForCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC017-Verify Previous Page arrow for carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyNavigationArrowForCarousel("Previous");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC017");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC018_VerifyNextPageArrowForCarousel(String Bname)
        {
            TestFixtureSetUp(Bname, "TC018-Verify Next Page arrow for carousel.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifyNavigationArrowForCarousel("Next");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC018");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC019_VerifySliderNavigationButtons(String Bname)
        {
            TestFixtureSetUp(Bname, "TC019-Verify Slider Navigation buttons.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifySliderNavigationButtonForCarousel();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC019");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC020_VerifyNavigationToDifferentPageThroughSliderNavigationButtons(String Bname)
        {
            TestFixtureSetUp(Bname, "TC020-Verify navigation to different page through Slider navigation buttons.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyFilterBarSectionOnScreen(false);
                promoDashboard.verifySliderNavigationButtonForCarousel(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC020");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC021_VerifyCarouselSortingWhenCircularWeekIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC021-Verify carousel sorting when Circular Week is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.selectRadioOptionFromPromoDashboard("Circular Week");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC021");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC022_VerifyCarouselSortingWhenNumberOfStoresIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC022-Verify carousel sorting when Number of Stores is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.selectRadioOptionFromPromoDashboard("Number Of Stores");
                promoDashboard.verifySortedByRecordsInCarouselForOption("Number Of Stores");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC022");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC023_VerifyPreviousSlideArrowForChartSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC023-Verify Previous Slide arrow for Chart Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyNavigationArrowForChartSection("Previous");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC023");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC024_VerifyNextSlideArrowForChartSection(String Bname)
        {
            TestFixtureSetUp(Bname, "TC024-Verify Next Slide arrow for Chart Section.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyNavigationArrowForChartSection("Next");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC024");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC025_VerifyFirstSlideAsTopDepartmentAndDepartmentFeatureShareByRetailer(String Bname)
        {
            TestFixtureSetUp(Bname, "TC025-Verify First Slide as 'Top Department' and 'Department Feature share by Retailer'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyAndSelectChartOnChartSection("Top Departments", false);
                string legendName = promoDashboard.selectLegendValueFromChart("Top Departments", false, "Random", true);
                promoDashboard.selectLegendValueFromChart("Top Departments", false, legendName, false);
                promoDashboard.verifyAndSelectChartOnChartSection("Department Feature Share by Retailer", true);
                promoDashboard.selectLegendValueFromChart("Department Feature Share by Retailer", true, "Random", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC025");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC026_VerifyFirstSlideAsTopSegmentsAndSegmentFeatureShareByRetailer(String Bname)
        {
            TestFixtureSetUp(Bname, "TC026-Verify First Slide as 'Top Segments' and 'Segment Feature share by Retailer'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyAndSelectChartOnChartSection("Top Segments", false);
                string legendName = promoDashboard.selectLegendValueFromChart("Top Segments", false, "Random", true);
                promoDashboard.selectLegendValueFromChart("Top Segments", false, legendName, false);
                promoDashboard.verifyAndSelectChartOnChartSection("Segment Feature Share by Retailer", true);
                promoDashboard.selectLegendValueFromChart("Segment Feature Share by Retailer", true, "Random", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC026");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC027_VerifyFirstSlideAsTopCategoriesAndCategoryFeatureShareByRetailer(String Bname)
        {
            TestFixtureSetUp(Bname, "TC027-Verify First Slide as 'Top Categories' and 'Category Feature share by Retailer'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyAndSelectChartOnChartSection("Top Categories", false);
                string legendName = promoDashboard.selectLegendValueFromChart("Top Categories", false, "Random", true);
                promoDashboard.selectLegendValueFromChart("Top Categories", false, legendName, false);
                promoDashboard.verifyAndSelectChartOnChartSection("Category Feature Share by Retailer", true);
                promoDashboard.selectLegendValueFromChart("Category Feature Share by Retailer", true, "Random", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC027");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC028_VerifyFourthSlideAsTopSubcategoriesAndSubcategoryFeatureShareByRetailer(String Bname)
        {
            TestFixtureSetUp(Bname, "TC028-Verify fourth Slide as 'Top Subcategories' and 'Subcategory Feature share by Retailer'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyAndSelectChartOnChartSection("Top Subcategories", false);
                string legendName = promoDashboard.selectLegendValueFromChart("Top Subcategories", false, "Random", true);
                promoDashboard.selectLegendValueFromChart("Top Subcategories", false, legendName, false);
                promoDashboard.verifyAndSelectChartOnChartSection("Subcategory Feature Share by Retailer", true);
                promoDashboard.selectLegendValueFromChart("Subcategory Feature Share by Retailer", true, "Random", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC028");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC029_VerifyFourthSlideAsTopManufacturesAndManufacturerFeatureShareByRetailer(String Bname)
        {
            TestFixtureSetUp(Bname, "TC029-Verify fourth Slide as 'Top Manufacturers' and 'Manufacturer Feature share by Retailer'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyAndSelectChartOnChartSection("Top Manufacturers", false);
                string legendName = promoDashboard.selectLegendValueFromChart("Top Manufacturers", false, "Random", true);
                promoDashboard.selectLegendValueFromChart("Top Manufacturers", false, legendName, false);
                promoDashboard.verifyAndSelectChartOnChartSection("Manufacturer Feature Share by Retailer", true);
                promoDashboard.selectLegendValueFromChart("Manufacturer Feature Share by Retailer", true, "Random", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC029");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC030_VerifySixthSlideAsTopBrandsAndBrandFeatureShareByRetailer(String Bname)
        {
            TestFixtureSetUp(Bname, "TC030-Verify Sixth Slide as 'Top Brands' and 'Brand Feature share by Retailer'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.verifyAndSelectChartOnChartSection("Top Brands", false);
                string legendName = promoDashboard.selectLegendValueFromChart("Top Brands", false, "Random", true);
                promoDashboard.selectLegendValueFromChart("Top Brands", false, legendName, false);
                promoDashboard.verifyAndSelectChartOnChartSection("Brand Feature Share by Retailer", true);
                promoDashboard.selectLegendValueFromChart("Brand Feature Share by Retailer", true, "Random", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC030");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC031_VerifyViewListUnderViewOptionsDefault(String Bname)
        {
            TestFixtureSetUp(Bname, "TC031-Verify View List under 'View' options (Default).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen().verifyActionButtonOnViewSection();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC031");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC032_VerifyGridWhenTableViewIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC032-Verify grid when 'Table View' is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

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
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC032");
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
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC033");
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

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Table").verifyActionButtonOnViewSection();
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
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC034");
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

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Table").verifyButtonDisableOrNotOnScreen("View Selected", true);
                promoDashboard.selectRecordFromViewSection().clickButtonOnViewActionSection("View Selected");
                promoDashboard.verifyButtonDisableOrNotOnScreen("View Selected", false);
                promoDashboard.verifyViewSelectedButtonCheckedOrNotOnScreen(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC035");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC036_VerifyThatLabelShouldChangeToViewAllAfterclickingOnViewSelectedButton(String Bname)
        {
            TestFixtureSetUp(Bname, "TC036-Verify that label should change to 'View all' after clicking on View Selected button.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Table").verifyButtonDisableOrNotOnScreen("View Selected", true);
                promoDashboard.selectRecordFromViewSection().clickButtonOnViewActionSection("View Selected");
                promoDashboard.verifyButtonDisableOrNotOnScreen("View Selected", false);
                promoDashboard.verifyViewSelectedButtonCheckedOrNotOnScreen(true);

                promoDashboard.clickButtonOnViewActionSection("View Selected");
                promoDashboard.verifyViewSelectedButtonCheckedOrNotOnScreen(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC036");
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
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Table").selectRecordFromViewSection();
                promoDashboard.verifyButtonDisableOrNotOnScreen("Reset Selected", false);
                promoDashboard.clickButtonOnViewActionSection("Reset Selected");
                promoDashboard.verifyViewSelectedButtonCheckedOrNotOnScreen(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC037");
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
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Table");
                promoDashboard.verifyButtonDisableOrNotOnScreen("Reset Selected", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC038");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC039_VerifyBottomPanelOfDashboardScreen(String Bname)
        {
            TestFixtureSetUp(Bname, "TC039-Verify bottom panel of Dashboard screen.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Promo");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Dashboard");

                promoDashboard.verifyPromoDashboardScreen();
                promoDashboard.clickDetailViewButtonAndVerifyOptionsToClick("Table");
                homePage.verifyBottomPanelOfScreen().clickMarketTrackLogoFromBottom();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                loginPage.verifyNavigateURLOnScreen("https://www.numerator.com/");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite003_PromoDashboard_TC039");
                throw;
            }

            driver.Quit();
        }

        #endregion
    }
}
