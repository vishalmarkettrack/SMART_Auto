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
    public class TestSuite006_Schedulers : Base
    {
        #region Private Variables

        private IWebDriver driver;
        Login loginPage;
        Home homePage;
        BrandCanada_Screen brandCanada_Screen;
        Search searchPage;
        PromoDashboard promoDashboard;
        Schedule schedule;

        #endregion

        #region Public Fixture Methods

        public IWebDriver TestFixtureSetUp(string Bname, string testCaseName)
        {
            driver = StartBrowser(Bname);
            Common.CurrentDriver = driver;
            Results.WriteTestSuiteHeading(typeof(TestSuite006_Schedulers).Name);
            starttest(Bname + " - " + testCaseName, typeof(TestSuite006_Schedulers).Name);

            loginPage = new Login(driver, test);
            homePage = new Home(driver, test);
            brandCanada_Screen = new BrandCanada_Screen(driver, test);
            searchPage = new Search(driver, test);
            promoDashboard = new PromoDashboard(driver, test);
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
        public void TC001_VerifyScheduleButtonShouldBeDisabledWhenNoSearchIsApplied(String Bname)
        {
            TestFixtureSetUp(Bname, "TC001-Verify 'Schedule' button should be disabled when no search is applied.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.selectDateRangeOptionFromSection();
                searchPage.clickButtonOnSearchScreen("Reset").verifyResetChangesMessageOnScreen(true);
                searchPage.clickButtonOnSearchScreen("Apply Search");
                schedule.verifyReportScreenDetails();
                schedule.verifyTooltipMessageOrClickButtonOnScreen("Schedule", "You must have an applied search");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC001");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC002_VerifyScheduleButtonShouldGetsEnabledWhenSearchIsApplied(String Bname)
        {
            TestFixtureSetUp(Bname, "TC002-Verify 'Schedule' button should gets enabled when search is applied.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();
                schedule.verifyTooltipMessageOrClickButtonOnScreen("Schedule", "Schedule");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC002");
                throw;
            }
            driver.Quit();
        }

        // Email Functionality not verify
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC003_VerifyDailyScheduleExcludingWeekendDeliveryCreatedSuccessfullyForPivotTable(String Bname)
        {
            TestFixtureSetUp(Bname, "TC003-Verify Daily schedule excluding weekend delivery created successfully for Pivot table.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();
                schedule.verifyTooltipMessageOrClickButtonOnScreen("Schedule", "Schedule", true);
                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick().clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC003");
                throw;
            }
            driver.Quit();
        }

        // Pending Due to WEB-6033 (Email Functionality not verify)
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC004_VerifyDailyScheduleIncludingWeekendDeliveryCreatedSuccessfullyForDataReoprtsInExportAll(String Bname)
        {
            TestFixtureSetUp(Bname, "TC004-Verify Daily schedule including weekend delivery created successfully for Data reoprts in Export All.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Data Reports", "Schedule");

                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick().clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC004");
                throw;
            }
            driver.Quit();
        }

        // Email Functionality not verify
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC005_VerifyDailyScheduleIncludingWeekendDeliveryCreatedSuccessfullyForPowerPointReportsInExportAll(String Bname)
        {
            TestFixtureSetUp(Bname, "TC005-Verify Daily schedule including weekend delivery created successfully for Power point reports in Export All.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Schedule", "2 Creatives / Slide (1x2)");

                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick("Weekly").verifyAllDaysLabelOrSelectOnScheduleWindow("");
                schedule.verifyAllDaysLabelOrSelectOnScheduleWindow("M").clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC005");
                throw;
            }
            driver.Quit();
        }

        // Email Functionality not verify
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC006_VerifyDailyScheduleIncludingWeekendDeliveryCreatedSuccessfullyForAssetDownloadsInExportAll(String Bname)
        {
            TestFixtureSetUp(Bname, "TC006-Verify Daily schedule including weekend delivery created successfully for Asset downloads in Export All.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Schedule");

                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick("Monthly").verifyMonthlySectionOnScheduleWindow();
                schedule.enterDayInMonthlySectionOnScheduleWindow("");
                schedule.clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC006");
                throw;
            }
            driver.Quit();
        }

        // Email Functionality not verify
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC007_VerifyMonthlyScheduleWithSpecialCharacters(String Bname)
        {
            TestFixtureSetUp(Bname, "TC007-Verify Monthly schedule with special characters.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Schedule");

                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick("Monthly").verifyMonthlySectionOnScheduleWindow();
                schedule.enterDayInMonthlySectionOnScheduleWindow("abf@#");
                schedule.clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyMessageForTheMonthOnScheduleWindow("will be delivered every Invalid date day of the month.");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC007");
                throw;
            }
            driver.Quit();
        }

        // Email Functionality not verify
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC008_VerifyMonthlyScheduleWithInvalidDateRange_DateAbove30(String Bname)
        {
            TestFixtureSetUp(Bname, "TC008-Verify Monthly schedule with Invalid date range (Date above 30).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Schedule");

                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick("Monthly").verifyMonthlySectionOnScheduleWindow();
                schedule.enterDayInMonthlySectionOnScheduleWindow("35");
                schedule.clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC008");
                throw;
            }
            driver.Quit();
        }

        // Email Functionality not verify
        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC009_VerifyMonthlyScheduleWithInvalidDateRange_NegativeDate(String Bname)
        {
            TestFixtureSetUp(Bname, "TC009-Verify Monthly schedule with Invalid date range (Negative date).");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports");
                promoDashboard.verifyOrClickExportAllSectionInDetailOnScreen("Power Point Reports", "Schedule");

                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick("Monthly").verifyMonthlySectionOnScheduleWindow();
                schedule.enterDayInMonthlySectionOnScheduleWindow("-15");
                schedule.clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC009");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC010_VerifyDataReportsScheduleIconWhenAppliedSearchHas50000PlusRecords(String Bname)
        {
            TestFixtureSetUp(Bname, "TC010-Verify Data reports Schedule icon when applied search has 50,000 plus records.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.selectDateRangeOptionFromSection("Last Year");
                schedule.clickButtonOnScreen("Apply Search");

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Data Reports", "Ban", "You have too many items selected. Select fewer than: 50000");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC010");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC011_VerifyPowerPointsReportsScheduleIconWhenAppliedSearchHas2000PlusRecords(String Bname)
        {
            TestFixtureSetUp(Bname, "TC011-Verify Power points reports Schedule icon when applied search has 2000 plus records.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.selectDateRangeOptionFromSection("Last Year");
                schedule.clickButtonOnScreen("Apply Search");

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Power Point Reports", "Ban", "You have too many items selected. Select fewer than: 2000");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC011");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC012_VerifyScheduleIconWhenAppliedSearchHas250PlusRecords(String Bname)
        {
            TestFixtureSetUp(Bname, "TC012-Verify Schedule icon when applied search has 250 plus records.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                searchPage.selectDateRangeOptionFromSection("Last Year");
                schedule.clickButtonOnScreen("Apply Search");

                promoDashboard.clickButtonOnViewActionSection("Export All").verifyExportAllSectionOnDashboardScreen();
                promoDashboard.verifyTiooltipFunctionalityForReportsSection("Asset Downloads", "Ban", "You have too many items selected. Select fewer than: 250");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC012");
                throw;
            }
            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC013_VerifyCreatedScheduleShouldBeDisplayedOnSavedSearchListAsScheduledExports(String Bname)
        {
            TestFixtureSetUp(Bname, "TC013-Verify Created schedule should be displayed on Saved search list as 'Scheduled Exports'.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.verifySavedSearchesSectionOnScreen();

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC013");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC014_VerifyUserShouldBeAbleToDeleteScheduleSuccessfully(String Bname)
        {
            TestFixtureSetUp(Bname, "TC014-Verify User should be able to delete Schedule successfully.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.clickScheduleFromScheduleExportAndPerformAction("Delete");

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC014");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC015_VerifyUserShouldBeAbleToUpdateScheduleSuccessfully(String Bname)
        {
            TestFixtureSetUp(Bname, "TC015-Verify User should be able to update Schedule successfully.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen(false);
                searchPage.clickScheduleFromScheduleExportAndPerformAction("");
                schedule.clickScheduleDropdownAndVerifyListsOrClick("Weekly").verifyAllDaysLabelOrSelectOnScheduleWindow("");
                schedule.verifyAllDaysLabelOrSelectOnScheduleWindow("M");
                searchPage.clickButtonFromScheduleWindow("Update");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC015");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC016_VerifyUserShouldBeAbleToCreateScheduleForPieAndBarChartSuccessfully(String Bname)
        {
            TestFixtureSetUp(Bname, "TC016-Verify User should be able to create Schedule for Pie and bar chart successfully.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                schedule.verifyTooltipMessageOrClickButtonOnScreen("Schedule", "", true);
                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick().clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC016");
                throw;
            }

            driver.Quit();
        }

        [Test]
        [TestCaseSource(typeof(Base), "BrowserToRun")]
        public void TC017_VerifyUserShouldNotBeAbleToDownloadReportWithDifferentUserCredentials(String Bname)
        {
            TestFixtureSetUp(Bname, "TC017-Verify User should not be able to download report with different user credentials.");
            try
            {
                loginPage.navigateToLoginPage().verifyLoginPageScreenInDetail();
                loginPage.loginUsingValidEmailIdAndPassword();

                homePage.verifyHomePage().clickUserMenuAndSelectAccountFromList("QA Testing - Brand Canada");
                homePage.clickSiteNavigationMenuIconAndSelectOptionFromListOnPage("Media");

                brandCanada_Screen.verifyBrandCanadaScreen();
                homePage.clickMenuIconFromScreen("Search");
                string searchTitle = schedule.createNewSearchOrClickSavedSearchToApplySearchOnScreen();

                schedule.verifyTooltipMessageOrClickButtonOnScreen("Schedule", "", true);
                schedule.verifyScheduleWindow(searchTitle);
                schedule.clickScheduleDropdownAndVerifyListsOrClick().clickButtonOnScreen("Create Scheduled Export");
                schedule.verifyScheduleMessageOnScreen("Successfully created a scheduled export for");

                // not able to verify Email Functionality

            }
            catch (Exception e)
            {
                Logging.LogStop(this.driver, test, e, MethodBase.GetCurrentMethod(), Bname + "_TestSuite006_Schedulers_TC017");
                throw;
            }

            driver.Quit();
        }

        // TC018 & TC019 not able to Create Due to Email functionality

        #endregion
    }
}
