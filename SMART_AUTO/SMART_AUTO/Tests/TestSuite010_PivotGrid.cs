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
    public class TestSuite010_PivotGrid : Base
    {
        #region Private Variables

        private IWebDriver driver;
        Login loginPage;
        Home homePage;
        PivotReportScreen pivotReportScreen;
        Schedule schedule;
        UserProfile userProfile;

        #endregion

        #region Public Fixture Methods

        public IWebDriver TestFixtureSetUp(string Bname, string testCaseName)
        {
            driver = StartBrowser(Bname);
            Common.CurrentDriver = driver;
            Results.WriteTestSuiteHeading(typeof(TestSuite010_PivotGrid).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite010_PivotGrid).Name);

            loginPage = new Login(driver, test);
            homePage = new Home(driver, test);
            pivotReportScreen = new PivotReportScreen(driver, test);
            userProfile = new UserProfile(driver, test);
            schedule = new Schedule(driver, test);

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
        public void TC001_VerifyPivotOptionsButtonFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC001-Verify Pivot options button functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);

                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifyPivotFieldsCheckboxOptionsFromPivotOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify Pivot fields checkbox options from Pivot Options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                string[] titles = { "Category", "Class", "Company", "Division", "Brand", "Media Outlet", "Submedia" };
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true, "Pivot Fields", titles);
                string[] pivotHeades = { "Class", "Company", "Division" };
                pivotReportScreen.verifyPivotFieldsHeaderOnPivotGrid(pivotHeades, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC002");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifyPivotFieldsCheckboxOptionsFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify Pivot fields checkbox options functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                string[] titles = { "Category", "Class", "Company", "Division", "Brand", "Media Outlet", "Submedia" };
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true, "Pivot Fields", titles);
                pivotReportScreen.checkedOrUnCheckedPivotFieldsFromOptions("Division", false);
                pivotReportScreen.checkedOrUnCheckedPivotFieldsFromOptions("Market", false);
                string[] headers = { "Division", "Market" };
                pivotReportScreen.verifyPivotFieldsHeaderOnPivotGrid(headers, false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC003");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifyUserAbleToChangeOrderOfPivotFieldsCheckboxOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify User able to change order of Pivot fields checkbox options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.dragAndDropFieldFromPivotFieldsSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC004");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyPivotGridWhenNoPivotFieldsOptionsAreSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify Pivot Grid when no Pivot fields options are selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOrUnCheckedPivotFieldsFromOptions("All Fields", true);
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("Export Grid", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC005");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyFormattingOptionFieldsInPivotOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify Formatting option fields  in 'Pivot options'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.verifyFormattingOptionsFieldsOnPivotOptionsSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC006");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyFormattingOptionInPivotOptionsFunctionality(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify Formatting option in 'Pivot options' functionality.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.verifyFormattingOptionsFieldsOnPivotOptionsSection();
                pivotReportScreen.selectOptionFromFormattingSection("Spend in Thousands");
                pivotReportScreen.verifyPivotGridDataWithProperFormat(false);
                pivotReportScreen.selectOptionFromFormattingSection("Spend in Dollars");
                pivotReportScreen.verifyPivotGridDataWithProperFormat(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC007");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyRankOnFunctionalityInOtherOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify 'Rank On' functionality in other options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.verifyOtherOptionsSectionOnPivotOptions();

                string[] option = { "Total Spend Current Period (CP)", "Submedia Spend Current Period (CP)" };
                pivotReportScreen.verifyMetricsSectionOnPivotOptions(option);

                pivotReportScreen.selectMetricsFromSection("Total Spend Last Year (LY)");
                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Rank on", false);
                pivotReportScreen.clickRankOnDropdownAndSelectOptionFromList("Total - Spend LY");
                string[] header = { "Rank" };
                pivotReportScreen.verifyPivotFieldsHeaderOnPivotGrid(header, false);
                pivotReportScreen.clickRankOnDropdownAndSelectOptionFromList("Random");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC008");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyRankOnFunctionalityShouldBeWorkWithOtherMetricsOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify 'Rank On' functionality should be work with other metrics options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);

                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Rank on", false);
                pivotReportScreen.clickRankOnDropdownAndSelectOptionFromList("Total - Spend CP");
                string[] header = { "Rank" };
                pivotReportScreen.verifyPivotFieldsHeaderOnPivotGrid(header, false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC009");
                throw;
            }
            driver.Quit();
        }

        // Pending due to issue WEB-6101
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifyRankOnDropdownOptionWhenNoMetricsAreSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify Rank on dropdown option when no 'Metrics' are selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.verifyOtherOptionsSectionOnPivotOptions();


            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC010");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyShowSummaryTotalsFunctionalityInOtherOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify 'Show summary totals' functionality in Other options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true).verifyOtherOptionsSectionOnPivotOptions();

                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Show Summary Totals", false);
                pivotReportScreen.verifyTotalSummarySectionBelowGridOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC011");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_VerifyUserAbleToPerfromSortingInDescendingOrder(String Bname)
        {
            TestFixtureSetUp(Bname, "TC012-Verify User able to perfrom sorting in Descending order.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().verifyPivotGridScreen();
                pivotReportScreen.clickColumnHeaderToSortDataFromPivotGrid("Class", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC012");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC013_VerifyUserAbleToPerfromSortingInAscendingOrder(String Bname)
        {
            TestFixtureSetUp(Bname, "TC013-Verify User able to perfrom sorting in Ascending order.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().verifyPivotGridScreen();
                pivotReportScreen.clickColumnHeaderToSortDataFromPivotGrid("Class", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC013");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC014_VerifyUserAbleToMinimizeMaximizePivotGridColumn(String Bname)
        {
            TestFixtureSetUp(Bname, "TC014-Verify User able to minimize-maximize pivot grid column.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().verifyPivotGridScreen();
                pivotReportScreen.clickPlus_MinusButtoIconOfTotalHeaderFromPivotGrid(true);
                pivotReportScreen.clickPlus_MinusButtoIconOfTotalHeaderFromPivotGrid(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC014");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC015_VerifyPivotGridTotalButtonWhenNonlyneMetricsOptionsAreSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC015-Verify Pivot grid Total(-) button when nonly one metrics options are selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options");
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromMetricsSection("All", true);
                pivotReportScreen.checkedOUnCheckedOptionFromMetricsSection("Total Spend Current Period (CP)", false);
                pivotReportScreen.verifyPlus_MinusIconButtonOnGrid(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC015");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC016_VerifyFilterFunctionalityOfPivotFieldsColumns_Class_Company(String Bname)
        {
            TestFixtureSetUp(Bname, "TC016-Verify Filter functionality of pivot fields columns (Class,Company etc…).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickFilterIconAndVerifySection("Company").selectOptionFromFilterBarSection("Select All", true);
                pivotReportScreen.enterAndVerifyKeywordInToFilterSearchTextbox().clearSearchTextboxOnFilterSection();
                pivotReportScreen.selectOptionFromFilterBarSection("Select All", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC016");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC017_VerifyUserAbleToSearchRecordUsingKeywordFromFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC017-Verify User able to search record using keyword from filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickFilterIconAndVerifySection("Company");
                pivotReportScreen.enterAndVerifyKeywordInToFilterSearchTextbox(5).verifyFontColorOfSearchValueOnFilterSection();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC017");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC018_VerifyFilterFunctionalityOfPivotFieldsColumns_SpendCP_OCCCP_PagesCP_LineageCP_Etc(String Bname)
        {
            TestFixtureSetUp(Bname, "TC018-Verify Filter functionality of pivot fields columns (Spend CP, OCC CP, Pages CP, Lineage CP etc…).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC018");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC019_Verify_Equals_FunctionalityOfPivotGridFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC019-Verify 'Equals' functionality of Pivot Grid filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Equals");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Equals");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC019");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC020_Verify_NotEqual_FunctionalityOfPivotGridFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC020-Verify 'Not equal' functionality of Pivot Grid filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Not equal");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Not Equals");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC020");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC021_Verify_LessThan_FunctionalityOfPivotGridFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC021-Verify 'Less than' functionality of Pivot Grid filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Less than");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Less than");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC021");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC022_Verify_LessThanOrEquals_FunctionalityOfPivotGridFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC022-Verify 'Less than or Equals' functionality of Pivot Grid filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Less than or equals");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Less than or equals");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC022");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC023_Verify_GreaterThan_FunctionalityOfPivotGridFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC023-Verify 'Greater than' functionality of Pivot Grid filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Greater than");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Greater than");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC023");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC024_Verify_GreaterThanOrEquals_FunctionalityOfPivotGridFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC024-Verify 'Greater than or Equals' functionality of Pivot Grid filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Greater than or equals");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Greater than or equals");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC024");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC025_Verify_InRange_FunctionalityOfPivotGridFilter(String Bname)
        {
            TestFixtureSetUp(Bname, "TC025-Verify 'In range' functionality of Pivot Grid filter.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "In range");
                string searchValue = pivotReportScreen.enterOrClearInRangeValueOnFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredInRangeValueWithGridValueForColumn(columnId, searchValue);
                pivotReportScreen.enterOrClearInRangeValueOnFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC025");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC026_VerifyPivotGridResultWhenUserOnlyChangeOptionFromFilterAndValueKeptItAsSame(String Bname)
        {
            TestFixtureSetUp(Bname, "TC026-Verify Pivot Grid result when user only change option from filter and value kept it as same.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "In range");
                string searchValue = pivotReportScreen.enterOrClearInRangeValueOnFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredInRangeValueWithGridValueForColumn(columnId, searchValue);
                pivotReportScreen.enterOrClearInRangeValueOnFilterTextArea(columnId, true);

                string SpendColumnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP", false);
                pivotReportScreen.selectConditionFromFilterOption(SpendColumnId, "Equals");
                string SpendSearchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(SpendColumnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(SpendColumnId, SpendSearchValue, "Equals");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(SpendColumnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC026");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC027_VerifyUserAbleToFilterRecordsByFractionalValues(String Bname)
        {
            TestFixtureSetUp(Bname, "TC027-Verify User able to filter records by fractional values.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Greater than or equals");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false, "5.5");
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Greater than or equals");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC027");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC028_VerifyFilterResultWhenUserEnterCharacterValues(String Bname)
        {
            TestFixtureSetUp(Bname, "TC028-Verify Filter result when user enter character values.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.verifyFormattingOptionsFieldsOnPivotOptionsSection().selectOptionFromFormattingSection("Spend in Thousands");
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Greater than or equals");
                string searchValue = pivotReportScreen.enterTextInFilterInputAreaAndVerifyFilterIconNotDisplay(columnId, "Test");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC028");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC029_VerifyUserAbleToFilterRecordDirectlyFromPivotGrid_PivotFieldOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC029-Verify User able to filter record directly from Pivot Grid (Pivot field options).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotGrid("Class", 1);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC029");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC030_VerifyUserAbleToPerformFilterOnMultipleRowsOfSameColumn(String Bname)
        {
            TestFixtureSetUp(Bname, "TC030-Verify User able to perform filter on multiple rows of same column.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotGrid("Company", 3);
                pivotReportScreen.selectRecordsFromPivotGrid("Division", 2);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC030");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC031_VerifyUserAbleToPerformFilterOnMultipleRowsOfSameColumn(String Bname)
        {
            TestFixtureSetUp(Bname, "TC031-Verify User able to perform filter on multiple rows of same column.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectEachColumnRecordAndVerifyRemovedPreviousOptionFromPivotGrid();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC031");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC032_VerifyTableGridViewResultWhenSelectedRecordFromPerticularColumnHasBeenDisabledFromPivotOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC032-Verify Table grid view result when selected record from perticular column has been disabled from Pivot options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                string[] titles = { "Category", "Class", "Company", "Division", "Brand", "Media Outlet", "Submedia" };
                pivotReportScreen.verifyPivotOptionsSectionOnScreen(true, "Pivot Fields", titles);
                pivotReportScreen.checkedOrUnCheckedPivotFieldsFromOptions("Class", true);
                pivotReportScreen.verifyColumnPresentOrNotOnPivotGrid("Class", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC032");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC033_VerifyTableGridViewResultWhenSelectedRecordFromPerticularColumnHasBeenUncheckedFromPivotGridView(String Bname)
        {
            TestFixtureSetUp(Bname, "TC033-Verify Table grid view result when selected record from perticular column has been Unchecked from Pivot Grid view.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotGrid("Class", 1);
                pivotReportScreen.unSelectRecordsFromPivotGrid("Class", 1);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC033");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC034_VerifyPivotGridColumnShouldBeDisplayedAccordingToSelectedReports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC034-Verify Pivot Grid column should be displayed according to Selected reports.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC034");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC035_VerifyUserAbleToFilterRecordBySelectingRowsFromPivotGrid_MetricsFieldOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC035-Verify User able to filter record by selecting rows from Pivot Grid (Metrics field options).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotViewReportGrid("Spend CP", 2);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC035");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC036_VerifyIconWhenMetricsValueSpendCPIs_Dollar0(String Bname)
        {
            TestFixtureSetUp(Bname, "TC036-Verify Icon when metrics value (i.e Spend CP) is $0.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.verifyDisablePivotViewReportGridRecords();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC036");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC037_VerifyScrollBarFunctionalityOfPivotGrid(String Bname)
        {
            TestFixtureSetUp(Bname, "TC037-Verify Scroll bar functionality of Pivot grid.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.scrollAndVerifyAllRecordsFromPivotGrid();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC037");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC038_VerifyViewSelectedButtonWhenNoRecordIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC038-Verify View selected button when no Record is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("View Selected", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC038");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC039_VerifyViewSelectedFunctionalityWhenRecordIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC039-Verify View selected functionality when Record is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().selectRecordsFromPivotGrid("Company", 2);
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("View Selected", false);
                pivotReportScreen.clickButtonAndVerifyButtonCheckedOrNot("View Selected", true);
                pivotReportScreen.clickButtonAndVerifyButtonCheckedOrNot("View Selected", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC039");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC040_VerifyResetSelectedButtonWhenNoRecordIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC040-Verify Reset selected button when no Record is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("Reset Selected", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC040");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC041_VerifyResetSelectedFunctionalityWhenRecordIsSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC041-Verify Reset selected functionality when Record is selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().selectRecordsFromPivotGrid("Company", 2);
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("Reset Selected", false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC041");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC042_VerifyPivotGridWhenHoverMouseOnAnyRecords(String Bname)
        {
            TestFixtureSetUp(Bname, "TC042-Verify Pivot Grid when hover mouse on any records.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.mouseHoverOnPivotFieldGridRecordAndVerifyColor();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC042");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC043_VerifyUserShouldNotBeAbleToSelect_0_ValuesFor_PagesCP_And_SpendCP_FromPivotTables(String Bname)
        {
            TestFixtureSetUp(Bname, "TC043-Verify User should not be able to select “0” values for Pages CP and Spend CP from Pivot tables.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.mouseHoverOnNonSelectableValueFromPivotGrid();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC043");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC044_VerifyPivotGridWhenUserSelectRecordsFromPivotGridAndThenSelects_ShowSummaryTotals_CheckboxFromPivotOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC044-Verify Pivot Grid When user Select Records from Pivot grid and then selects “Show Summary totals” checkbox from Pivot Options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().selectRecordsFromPivotGrid("Company", 2);
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("View Selected", false);
                pivotReportScreen.clickButtonAndVerifyButtonCheckedOrNot("View Selected", true);

                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Show Summary Totals", false);
                pivotReportScreen.verifyTotalSummarySectionBelowSideOfPivotGrid(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC044");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC045_VerifyPivotGridWhenUserSelectRecordsFromPivotGridAndThenUncheck_ShowSummaryTotals_CheckboxFromPivotOptions(String Bname)
        {
            TestFixtureSetUp(Bname, "TC045-Verify Pivot Grid When user Select Records from Pivot grid and then selects “Show Summary totals” checkbox from Pivot Options.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().selectRecordsFromPivotGrid("Company", 2);
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("View Selected", false);
                pivotReportScreen.clickButtonAndVerifyButtonCheckedOrNot("View Selected", true);

                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Show Summary Totals", false);
                pivotReportScreen.verifyTotalSummarySectionBelowSideOfPivotGrid(true);

                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Show Summary Totals", true);
                pivotReportScreen.verifyTotalSummarySectionBelowSideOfPivotGrid(false);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC045");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC046_VerifySummaryTotalsWhenUserApplyFiltersOnPivotGrid(String Bname)
        {
            TestFixtureSetUp(Bname, "TC046-Verify Summary Totals when user apply filters on Pivot grid.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Show Summary Totals", false);
                pivotReportScreen.verifyTotalSummarySectionBelowSideOfPivotGrid(true);

                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Less than");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Less than");
                pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC046");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC047_VerifyExportedGridRecordsShouldSameAsPivotTableOnSite(String Bname)
        {
            TestFixtureSetUp(Bname, "TC047-Verify Exported Grid records should same as Pivot table on site.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Less than");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Less than");

                pivotReportScreen.clickButtonOnPivotScreen("Export Grid").verifyExportingGridProcessToComplete();
                pivotReportScreen.verifyFileDownloadedOrNotOnScreen("brand_canada_-_weekly_spend-pivot");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC047");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC048_VerifyPivotAndAgGridWhenUserClick_OpenAnyAdViewPopup_ViewAd_Markets_Details(String Bname)
        {
            TestFixtureSetUp(Bname, "TC048-Verify Pivot and AgGrid when user click/open any Ad View Pop up ('view ad/markets /details').");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails().selectRecordsFromPivotGrid("Class", 1);
                pivotReportScreen.verifyGridSectionBelowPivotGrid();
                pivotReportScreen.clickButtonFromViewAndVerifyPoupWindow("View Ad");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC048");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC049_VerifyTotalSummaryResultInPivotTableShouldBeAccurate(String Bname)
        {
            TestFixtureSetUp(Bname, "TC049-Verify Total summary result in pivot table should be accurate.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Show Summary Totals", false);
                pivotReportScreen.verifyTotalSummarySectionBelowSideOfPivotGrid(true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC049");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC050_VerifySelectedRecordShouldBeRemainSelectedAfterSorting(String Bname)
        {
            TestFixtureSetUp(Bname, "TC050-Verify selected record should be remain selected after sorting.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotViewReportGrid("Spend CP", 1);
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Equals");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Equals");
                pivotReportScreen.verifyColumnSelectedOrNotOnGrid("Spend CP", true, 1);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC050");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC051_VerifyRecordsShouldBeRemainSelectedWhenUserApplyFilterOnAnyValueBothNumericAndCharacterValue(String Bname)
        {
            TestFixtureSetUp(Bname, "TC051-Verify Records should be remain selected when user apply filter on any value (both numeric and character value).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotViewReportGrid("Spend CP", 1);
                string columnId = pivotReportScreen.clickColumnHeaderFilterIconAndVerifyFilterSection("Spend CP");
                pivotReportScreen.selectConditionFromFilterOption(columnId, "Less than or equals");
                string searchValue = pivotReportScreen.enterOrClearValueFromFilterTextArea(columnId, false);
                pivotReportScreen.verifyFilteredValueOnGridForColumn(columnId, searchValue, "Less than or equals");
                pivotReportScreen.verifyColumnSelectedOrNotOnGrid("Spend CP", true, 1);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC051");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC052_VerifyPivotWhenRecordIsSelectedAndUserAddOrRemoveAnyOption(String Bname)
        {
            TestFixtureSetUp(Bname, "TC052-Verify Pivot when record is selected and user add or remove any option.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotViewReportGrid("Spend CP", 1);
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromMetricsSection("Total Spend Current Period (CP)", true);
                pivotReportScreen.verifyColumnSelectedOrNotOnGrid("Spend CP", false, 1);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC052");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC053_VerifyPivotGridWhenNoMetricsOptionsAreSelected(String Bname)
        {
            TestFixtureSetUp(Bname, "TC053-Verify Pivot Grid when no metrics options are selected.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.selectRecordsFromPivotViewReportGrid("Spend CP", 1);
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromMetricsSection("All", true);
                pivotReportScreen.verifyButtonDisableOrNotOnScreen("Export Grid", true);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC053");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC054_VerifyPivotOptionsMetricsShouldBeDisplayedAccordingToSelecetdReport_i_e_QATesting_AdExMediaSpendAndMonthlySpendIn_BrandMonthlyAccount(String Bname)
        {
            TestFixtureSetUp(Bname, "TC054-Verify Pivot options metrics should be displayed according to selecetd report (i.e QA Testing - Ad Ex - Media Spend  and Monthly spend) in Brand monthly account.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");
                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyMetricsSectionOnPivotOptions(null);

                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");
                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyMetricsSectionOnPivotOptions(null);

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC054");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC055_VerifyExcelValueForSpendCPAndSpendLYColumnWhoseValueIs0Dollar(String Bname)
        {
            TestFixtureSetUp(Bname, "TC055-Verify Excel value for Spend CP and Spend LY column whose value is 0$.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");
                
                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Export Grid").verifyExportingGridProcessToComplete();
                pivotReportScreen.verifyFileDownloadedOrNotOnScreen("brand_canada_-_weekly_spend-pivot");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC055");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC056_VerifyUserAbleToDownloadExcelFileWithoutSelectingCheckOnRankOnField(String Bname)
        {
            TestFixtureSetUp(Bname, "TC056-Verify User able to download excel file without selecting check on “Rank On” field.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media Spend");

                pivotReportScreen.verifyReportScreenDetails();
                pivotReportScreen.clickButtonOnPivotScreen("Pivot Options").verifyPivotOptionsSectionOnScreen(true);
                pivotReportScreen.checkedOUnCheckedOptionFromOtherOptionsSection("Rank on", false);
                pivotReportScreen.clickRankOnDropdownAndSelectOptionFromList("Total - Spend");

                pivotReportScreen.clickButtonOnPivotScreen("Export Grid").verifyExportingGridProcessToComplete();
                pivotReportScreen.verifyFileDownloadedOrNotOnScreen("brand_canada_-_weekly_spend-pivot");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite010_PivotGrid_TC056");
                throw;
            }
            driver.Quit();
        }

        #endregion
    }
}
