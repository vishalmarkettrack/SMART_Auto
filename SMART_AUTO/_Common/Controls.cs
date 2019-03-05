using System;
using System.Web;
using NUnit.Framework;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace SMART_AUTO
{
    public static class Controls
    {
        #region Click on main menu items

        public static void _clickMenuItems(this IWebDriver driver, string When, string How)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver._findElement(When, How));

        }

        #endregion

        /// <summary>
        /// This method create random string for input. Also "isNumberOnly" parameter used to generat random number.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="size"></param>
        /// <param name="isNumberOnly"></param>
        /// <returns></returns>
        public static string _randomString(this IWebDriver driver, int size, bool isNumberOnly = false)
        {
            string input = string.Empty;
            string randText = string.Empty;
            Random rand = new Random();
            if (isNumberOnly)
                input = "0123456789";
            else
                input = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < size; i++)
            {
                int x = rand.Next(0, input.Length);
                randText = randText + input[x];
                x = 0;
            }
            return randText;
        }

        /// <summary>
        /// mouse hover by java script
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="targetElement"></param>
        public static void MouseHoverByJavaScript(this IWebDriver driver, IWebElement targetElement)
        {
            string javaScript = "var evObj = document.createEvent('MouseEvents');" +
                                "evObj.initMouseEvent(\"mouseover\",true, false, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);" +
                                "arguments[0].dispatchEvent(evObj);";
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            js.ExecuteScript(javaScript, targetElement);
        }

        /// <summary>
        /// to wait for element to be hidden
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool _waitForElementToBeHidden(this IWebDriver driver, string When, string How, int timeout = 30)
        {
            int i;
            bool isHidden = false;
            for (i = 0; i < timeout; i++)
            {
                if (driver._isElementPresent(When, How))
                    Thread.Sleep(1000);
                else
                {
                    isHidden = true;
                    break;
                }
            }

            return isHidden;
        }

        /// <summary>
        /// to wait for element to be populated or loaded with given timeout
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <param name="timeout"></param>
        public static void _waitForElementToBePopulated(this IWebDriver driver, string When, string How, int timeout = 30)
        {
            for (int i = 0; i < timeout; i++)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

                if (driver._isElementPresent(When, How))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// to wait for element for given time
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool _waitForElement(this IWebDriver driver, string When, string How, int timeout = 40)
        {
            bool isDisplayed = false;
            for (int i = 0; i < timeout; i++)
            {
                Thread.Sleep(1000);
                if (driver._isElementPresent(When, How))
                {
                    isDisplayed = true;
                    break;
                }
            }
            return isDisplayed;
        }

        /// <summary>
        /// if element is not immediately present specifies the time driver should wait to search
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="x"></param>
        public static void _wait(this IWebDriver driver, int x)
        {
            for (int i = 0; i <= x; i++)
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            }
        }

        /// <summary>
        /// returns true if element is present
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <returns></returns>
        public static bool _isElementPresent(this IWebDriver driver, string When, string How)
        {
            bool isDisplayed = false;

            try
            {
                switch (When.ToLower())
                {
                    case "id":
                        isDisplayed = driver.FindElement(By.Id(How)).Displayed;
                        break;
                    case "css":
                    case "cssselector":
                        isDisplayed = driver.FindElement(By.CssSelector(How)).Displayed;
                        break;
                    case "name":
                        isDisplayed = driver.FindElement(By.Name(How)).Displayed;
                        break;
                    case "xpath":
                        isDisplayed = driver.FindElement(By.XPath(How)).Displayed;
                        break;
                    case "linktext":
                        isDisplayed = driver.FindElement(By.LinkText(How)).Displayed;
                        break;
                    case "partiallinktext":
                        isDisplayed = driver.FindElement(By.PartialLinkText(How)).Displayed;
                        break;
                    case "class":
                    case "classname":
                        isDisplayed = driver.FindElement(By.ClassName(How)).Displayed;
                        break;
                    case "tagname":
                        isDisplayed = driver.FindElement(By.TagName(How)).Displayed;
                        break;

                    default:
                        break;
                }
            }
            catch (NoSuchElementException)
            {
                isDisplayed = false;
            }

            return isDisplayed;
        }

        /// <summary>
        /// return text from element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <returns></returns>
        public static string _getText(this IWebDriver driver, string When, string How)
        {
            IWebElement ele = driver._findElement(When, How);

            return ele.Text;
        }

        /// <summary>
        /// return value of given attribute for element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static string _getAttributeValue(this IWebDriver driver, string When, string How, string attribute)
        {
            IWebElement ele = driver._findElement(When, How);

            return ele.GetAttribute(attribute.ToLower());
        }

        /// <summary>
        /// return value attribute of element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <returns></returns>
        public static string _getValue(this IWebDriver driver, string When, string How)
        {
            IWebElement ele = driver._findElement(When, How);

            return ele.GetAttribute("value");
        }

        /// <summary>
        /// click on the element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void _click(this IWebDriver driver, string When, string How)
        {
            IWebElement ele = driver._findElement(When, How);
            ele.Click();
        }

        /// <summary>
        /// double click on element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void _doubleClick(this IWebDriver driver, string When, string How)
        {
            IWebElement ele = driver._findElement(When, How);
            Actions action = new Actions(driver);
            action.DoubleClick();
            action.Perform();
        }

        /// <summary>
        /// click on the element by javascript executor with xpath
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void _clickByJavaScriptExecutor(this IWebDriver driver, string How)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", driver.FindElement(By.XPath(How)));
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Double click on the element by javascript executor with xpath
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="How"></param>
        public static void _doubleClickByJavaScriptExecutor(this IWebDriver driver, string How)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].doubleClick();", driver.FindElement(By.XPath(How)));
            Thread.Sleep(500);
        }

        /// <summary>
        /// returns true if element is selected
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <returns></returns>
        public static bool _isChecked(this IWebDriver driver, string When, string How)
        {
            IWebElement element = driver._findElement(When, How);
            return element.Selected;
        }

        /// <summary>
        /// enter value in element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <param name="TextToInput"></param>
        public static void _type(this IWebDriver driver, string When, string How, String TextToInput)
        {
            driver._waitForElementToBePopulated(When, How);
            IWebElement ele = driver._findElement(When, How);
            ele.Clear();
            ele.SendKeys(TextToInput);
        }

        /// <summary>
        /// clear text from given element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void _clearText(this IWebDriver driver, string When, string How)
        {
            driver._waitForElementToBePopulated(When, How);
            IWebElement ele = driver._findElement(When, How);
            ele.Clear();
        }

        /// <summary>
        /// to use send keys with various options like tab, alt or escape.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <param name="key"></param>
        public static void _sendKeys(this IWebDriver driver, string When, string How, String key)
        {
            IWebElement ele = driver._findElement(When, How);
            switch (key.ToLower().Trim())
            {
                case "tab":
                    ele.SendKeys(Keys.Tab);
                    break;
                case "alt":
                    ele.SendKeys(Keys.Alt);
                    break;
                case "esc":
                    ele.SendKeys(Keys.Escape);
                    break;
                default:
                    //Console.WriteLine("Key is not added in the case of _sendKeys function.");
                    break;
            }
        }

        /// <summary>
        /// finds element and returns element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <returns></returns>
        public static IWebElement _findElement(this IWebDriver driver, string When, string How)
        {
            IWebElement element = null;
            for (int i = 0; i <= 10; i++) //instead of 30 written 10 as in Feature Vision project control.cs _findelement()
            {
                try
                {
                    switch (When.ToLower())
                    {
                        case "id":
                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.Id(How));
                            break;
                        case "css":
                        case "cssselector":

                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.CssSelector(How));
                            break;
                        case "name":
                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.Name(How));
                            break;
                        case "xpath":
                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.XPath(How));
                            break;
                        case "linktext":
                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.LinkText(How));
                            break;
                        case "partiallinktext":
                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.PartialLinkText(How));
                            break;
                        case "class":
                        case "classname":
                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.ClassName(How));
                            break;
                        case "tagname":
                            driver._waitForElementToBePopulated(When, How);
                            element = driver.FindElement(By.TagName(How));
                            break;
                        default:
                            //element = null;
                            //Console.WriteLine("Incorrect By Selector!");
                            break;
                    }
                }
                catch (NoSuchElementException)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    continue;
                }
                catch (ElementNotVisibleException)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    continue;
                }

                if (element.Displayed)
                {
                    break;
                }
                else
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                }

            }
            return element;

        }

        /// <summary>
        /// return list of elements within element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <returns></returns>
        public static IList<IWebElement> _findElements(this IWebDriver driver, string When, string How)
        {
            IList<IWebElement> elements = null;
            for (int i = 0; i <= 30; i++)
            {
                try
                {
                    switch (When.ToLower())
                    {
                        case "id":
                            elements = driver.FindElements(By.Id(How));
                            break;
                        case "css":
                        case "cssselector":
                            elements = driver.FindElements(By.CssSelector(How));
                            break;
                        case "name":
                            elements = driver.FindElements(By.Name(How));
                            break;
                        case "xpath":
                            elements = driver.FindElements(By.XPath(How));
                            break;
                        case "class":
                        case "classname":
                            elements = driver.FindElements(By.ClassName(How));
                            break;
                        case "tagname":
                        case "tag":
                            elements = driver.FindElements(By.TagName(How));
                            break;
                        case "linktext":
                            elements = driver.FindElements(By.LinkText(How));
                            break;
                        default:
                            Assert.Fail("Incorrect By Selector!");
                            break;
                    }
                }
                catch (NoSuchElementException)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    continue;
                }
                catch (ElementNotVisibleException)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    continue;
                }
            }

            return elements;

        }

        /// <summary>
        /// returns list of elements within element
        /// </summary>
        /// <param name="element"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <returns></returns>
        public static IList<IWebElement> _findElementsWithinElement(this IWebElement element, string When, string How)
        {
            IList<IWebElement> elements = null;

            switch (When.ToLower())
            {
                case "id":
                    elements = element.FindElements(By.Id(How));
                    break;
                case "css":
                case "cssselector":
                    elements = element.FindElements(By.CssSelector(How));
                    break;
                case "name":
                    elements = element.FindElements(By.Name(How));
                    break;
                case "xpath":
                    elements = element.FindElements(By.XPath(How));
                    break;
                case "class":
                case "classname":
                    elements = element.FindElements(By.ClassName(How));
                    break;
                case "tag":
                case "tagname":
                    elements = element.FindElements(By.TagName(How));
                    break;
                default:
                    //element = null;
                    //Console.WriteLine("Incorrect By Selector!");
                    break;
            }
            return elements;
        }

        /// <summary>
        /// takes screenshot
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="saveLocation"></param>
        public static void _takeScreenshot(this IWebDriver driver, string saveLocation)
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(saveLocation, ScreenshotImageFormat.Png);
        }

        /// <summary>
        /// mouse hover using Element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void MouseHoverUsingElement(this IWebDriver driver, string When, string How)
        {
            ILocatable hoverItem = (ILocatable)driver._findElement(When, How);
            IMouse mouse = ((IHasInputDevices)driver).Mouse;
            mouse.MouseMove(hoverItem.Coordinates);
        }

        public static void MouseHoverUsingIWebElement(this IWebDriver driver, IWebElement element)
        {
            ILocatable hoverItem = (ILocatable)element;
            IMouse mouse = ((IHasInputDevices)element).Mouse;
            mouse.MouseMove(hoverItem.Coordinates);
        }

        /// <summary>
        /// Scroll Into View Element
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// 
        public static void _scrollintoViewElement(this IWebDriver driver, string when, string how)
        {
            //IWebElement ele = driver._findElement(when, how);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver._findElement(when, how));
            Thread.Sleep(1000);
        }

        /// <summary>
        /// return javascript executor 
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public static IJavaScriptExecutor _scripts(this IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        }

        /// <summary>
        /// check if image is broken or not
        /// </summary>
        /// <param name="driver"></param>
        public static void _checkForBrokenImages(this IWebDriver driver, string when, string how)
        {
            IList<IWebElement> allImages = driver._findElements(when, how);
            foreach (IWebElement image in allImages)
            {
                //bool loaded = Convert.ToBoolean(driver._scripts().ExecuteScript("return arguments[0].complete", image));
                bool loaded = Convert.ToBoolean(driver._scripts().ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", image));

                if (!loaded)
                {
                }
            }
            allImages = null;
        }

        /// <summary>
        /// mouse hover on the element 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void _mouseOver(this IWebDriver driver, string When, string How, string When1, string How1)
        {
            IWebElement element = driver._findElement(When, How);
            ILocatable hoverItem = (ILocatable)element;
            IMouse mouse = ((IHasInputDevices)driver).Mouse;
            mouse.MouseMove(hoverItem.Coordinates);
            //logger.debug("Hover on 'Action' and click '); 
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(x => x._findElement(When1, How1));

            driver._findElement(When1, How1).Click();

        }

        #region Frame Methods

        /// <summary>
        /// select frame within other frame
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void _selectFrameWithinFrame(this IWebDriver driver, string When, string How)
        {
            IWebElement ele = driver._findElement(When, How);
            driver.SwitchTo().Frame(ele);
        }

        /// <summary>
        /// select frame from default frame
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        public static void _selectFrameFromDefaultContent(this IWebDriver driver, string When, string How)
        {
            IWebElement ele = driver._findElement(When, How);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(ele);
        }

        /// <summary>
        /// select frame to default content
        /// </summary>
        /// <param name="driver"></param>
        public static void _selectFrameToDefaultContent(this IWebDriver driver)
        {
            driver.SwitchTo().DefaultContent();
        }

        #endregion

        /// <summary>
        /// sort given array data and return sorted array on the basis of order parameter
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="arr"></param>
        /// <param name="order"></param>
        /// <param name="ColNo"></param>
        /// <returns></returns>
        public static IList<string> _sortArraydata(this IWebDriver driver, IList<string> arr, string order, string ColNo, bool dateFormat = false)
        {
            int listCount = 0;
            if (arr is IList)
            {
                listCount = ((IList)arr).Count;
            }

            DateTime[] dateTimes = new DateTime[listCount];

            decimal[] Intval = new decimal[listCount];
            string[] strVal = new string[listCount];

            IList<string> all = new List<string>(listCount);

            switch (ColNo.ToLower())
            {
                case "date":
                    for (int d = 0; d < listCount; d++)
                    {
                        dateTimes[d] = Convert.ToDateTime(arr[d]);
                    }
                    if (order == "Ascending")
                    {
                        Array.Sort<DateTime>(dateTimes);
                    }
                    else
                    {
                        Array.Sort<DateTime>(dateTimes);
                        Array.Reverse(dateTimes);
                    }

                    for (int v = 0; v < listCount; v++)
                    {
                        if (dateFormat)
                        {
                            all.Add(dateTimes[v].ToString("MM-dd-yyyy h:mm tt").ToLower());
                        }
                        else
                        {
                            all.Add(dateTimes[v].ToString("MM-dd-yyyy"));
                        }
                    }

                    break;
                case "int":
                    for (int d = 0; d < listCount; d++)
                    {
                        if (arr[d].Contains("%"))
                            arr[d] = arr[d].Replace("%", "");
                        Intval[d] = Decimal.Parse(arr[d]);

                    }
                    if (order == "Ascending")
                    {
                        Array.Sort<decimal>(Intval);
                    }
                    else
                    {
                        Array.Sort<decimal>(Intval);
                        Array.Reverse(Intval);
                    }

                    for (int v = 0; v < listCount; v++)
                    {
                        all.Add(Intval[v].ToString());

                    }
                    break;
                case "string":
                    for (int d = 0; d < listCount; d++)
                    {
                        strVal[d] = arr[d].ToString();
                    }
                    if (order == "Ascending")
                    {
                        Array.Sort<string>(strVal);
                    }
                    else
                    {
                        Array.Sort<string>(strVal);
                        Array.Reverse(strVal);
                    }

                    for (int v = 0; v < listCount; v++)
                    {
                        all.Add(strVal[v].ToString());
                    }

                    break;

                case "alphanumeric":
                    for (int d = 0; d < listCount; d++)
                    {
                        strVal[d] = arr[d].ToString();

                    }
                    if (order == "Ascending")
                    {
                        Array.Sort<string>(strVal);
                        Array.Sort(strVal, new AlphanumComparatorFast());
                    }
                    else
                    {
                        Array.Sort(strVal, new AlphanumComparatorFast());
                        Array.Reverse(strVal);
                    }

                    for (int v = 0; v < listCount; v++)
                    {
                        all.Add(strVal[v].ToString());
                    }

                    break;

                case "alfabetically":

                    for (int d = 0; d < listCount; d++)
                    {
                        strVal[d] = arr[d].ToString();
                    }

                    if (order == "Ascending")
                    {
                        Array.Sort(strVal);
                    }
                    else
                    {
                        Array.Sort(strVal);
                        Array.Reverse(strVal);
                    }

                    for (int v = 0; v < listCount; v++)
                    {
                        all.Add(strVal[v].ToString());
                    }

                    break;
            }

            return all;
        }

        /// <summary>
        /// compare the given array values
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        public static void _compareArrayValues(this IWebDriver driver, IList<string> arr1, IList<string> arr2)
        {
            int listCount1 = arr1.Count;
            int listCount2 = arr2.Count;

            if (listCount1 == listCount2)
            {
                for (int c = 0; c < listCount1; c++)
                {
                    Assert.AreEqual(arr1[c], arr2[c], "Data is not matching");
                }
            }
            else
            {
                Assert.Fail("Array count is not matching.");
            }

        }

        /// <summary>
        /// get value from grid and return array of values
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How">First Page</param>
        /// <param name="How1">Column Row</param>
        /// <param name="How2">Grid Row</param>
        /// <param name="Next">Next Page</param>
        /// <param name="ColNo">Column Number</param>
        /// <returns></returns>
        public static IList<string> _getValueInArray(this IWebDriver driver, string When, string How, string How1, string How2, string Next, string pages, string ColName, bool withdollar = false)
        {
            int PN = 1, IN = 1;

            if (driver._isElementPresent(When, How) == true)
            {
                //string items = driver._getText(When, How);
                //string itemno = items.Split(new char[] { 'f', 'e' })[1];
                //itemno = itemno.Replace(",", "");
                //IN = int.Parse(itemno);

                //IWebElement pageElement = driver._findElement(When, pages);
                //IList<IWebElement> pageElementCollection = pageElement._findElementsWithinElement(When, pages);

                //int x = pageElementCollection.Count - 2;
                //string name = pageElementCollection[x].Text;
                //PN = Convert.ToInt32(name);
            }

            DateTime[] dateTimes = new DateTime[IN];

            IList<string> all = new List<string>(IN);
            int blankCount = 0;
            int count = 0;
            int ColNo = 0;

            IList<IWebElement> columnCollections = driver._findElements(When, How1);
            for (int c = 0; c < columnCollections.Count; c++)
            {
                if (columnCollections[c].Text.Contains(ColName))
                {
                    ColNo = c;
                    break;
                }
            }

            for (int k = 0; k < PN; k++)
            {
                if (driver._findElement(When, How2).Displayed)
                {
                    IWebElement webElementBody = driver._findElement(When, How2);
                    IList<IWebElement> ElementCollectionBody = webElementBody._findElementsWithinElement(When, How2);
                    int l = 0;
                    foreach (IWebElement item in ElementCollectionBody)
                    {
                        IList<IWebElement> cells = item._findElementsWithinElement("xpath", ".//*[local-name(.)='div']");

                        for (int i = 0; i < cells.Count; i++)
                        {
                            if (i == ColNo)
                            {
                                if (cells[i].Text == "")
                                {
                                    blankCount = blankCount + 1;
                                }
                                else
                                {
                                    if (withdollar == true)
                                    {
                                        all.Add(cells[i].Text.Replace("$-", ""));
                                        count = count + 1;
                                    }
                                    else
                                    {
                                        //all.Add(cells[i].Text);
                                        all.Add(cells[i].Text.Replace(",", ""));
                                        count = count + 1;
                                    }
                                }
                            }
                        }
                        l = l + 1;
                    }
                }
                if (PN > 1)
                {
                    driver._click("xpath", Next);
                    Thread.Sleep(1000);
                }
            }

            return all;
        }

        /// <summary>
        /// get value from grid and return array of values
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How">First Page</param>
        /// <param name="How1">Column Row</param>
        /// <param name="How2">Grid Row</param>
        /// <param name="Next">Next Page</param>
        /// <param name="ColNo">Column Number</param>
        /// <returns></returns>
        public static IList<string> GetArrayValue(this IWebDriver driver, string When, string How, string How1, string How2, string Next, string pages, string ColName, bool withdollar = false)
        {
            int PN = 1, IN = 1;
            DateTime[] dateTimes = new DateTime[IN];

            IList<string> all = new List<string>(IN);
            int blankCount = 0;
            int count = 0;
            int ColNo = 0;
            string ColId = "";

            IList<IWebElement> columnCollections = driver._findElements(When, How1);
            for (int c = 0; c < columnCollections.Count; c++)
            {
                if (columnCollections[c].Text.Contains(ColName))
                {
                    ColNo = c;
                    ColId = columnCollections[c].GetAttribute("colid");
                    break;
                }
            }

            for (int k = 0; k < PN; k++)
            {
                for (int rows = 0; rows < 50; rows++)
                {
                    string elePresent = How2 + "[@row='" + rows + "']";
                    if (driver._isElementPresent("xpath", elePresent) == true)
                    {
                        driver._scrollintoViewElement("xpath", elePresent);
                        string rowLine = How2 + "[@row='" + rows + "']/div[@colid='" + ColId + "']";
                        IWebElement webElementBody = driver._findElement(When, rowLine);

                        if (webElementBody.Text == "")
                        {
                            blankCount = blankCount + 1;
                        }
                        else
                        {
                            if (withdollar == true)
                            {
                                all.Add(webElementBody.Text.Replace("$-", ""));
                                count = count + 1;
                            }
                            else
                            {
                                all.Add(webElementBody.Text.Replace(",", ""));
                                count = count + 1;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return all;
        }

        /// <summary>
        /// Set Attrubute Value
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="When"></param>
        /// <param name="How"></param>
        /// <param name="attributeName"></param>
        /// <param name="Value"></param>
        public static void _setAttributeValue(this IWebDriver driver, string When, string How, string attributeName, string Value)
        {
            string getValueName = "";

            switch (When.ToLower())
            {
                case "id":
                    getValueName = "getElementsById";
                    break;
                case "css":
                case "cssselector":
                    getValueName = "getElementsByCssSelector";
                    break;
                case "name":
                    getValueName = "getElementsByName";
                    break;
                case "xpath":
                    getValueName = "getElementsByXPath";
                    break;
                case "linktext":
                    getValueName = "getElementsByLinkText";
                    break;
                case "partiallinktext":
                    getValueName = "getElementsByPartialLinkText";
                    break;
                case "class":
                case "classname":
                    getValueName = "getElementsByClassName";
                    break;
                default:
                    //element = null;
                    //Console.WriteLine("Incorrect By Selector!");
                    break;
            }

            ((IJavaScriptExecutor)driver).ExecuteScript("document." + getValueName + "('" + How + "')[0].setAttribute('" + attributeName + "', '" + Value + "')");
        }

    }

    /// <summary>
    /// Class for alphanumeric sort comparision
    /// </summary>
    public class AlphanumComparatorFast : IComparer
    {
        public int Compare(object x, object y)
        {
            string s1 = x as string;
            if (s1 == null)
            {
                return 0;
            }
            string s2 = y as string;
            if (s2 == null)
            {
                return 0;
            }

            int len1 = s1.Length;
            int len2 = s2.Length;
            int marker1 = 0;
            int marker2 = 0;

            // Walk through two the strings with two markers.
            while (marker1 < len1 && marker2 < len2)
            {
                char ch1 = s1[marker1];
                char ch2 = s2[marker2];

                // Some buffers we can build up characters in for each chunk.
                char[] space1 = new char[len1];
                int loc1 = 0;
                char[] space2 = new char[len2];
                int loc2 = 0;

                // Walk through all following characters that are digits or
                // characters in BOTH strings starting at the appropriate marker.
                // Collect char arrays.
                do
                {
                    space1[loc1++] = ch1;
                    marker1++;

                    if (marker1 < len1)
                    {
                        ch1 = s1[marker1];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

                do
                {
                    space2[loc2++] = ch2;
                    marker2++;

                    if (marker2 < len2)
                    {
                        ch2 = s2[marker2];
                    }
                    else
                    {
                        break;
                    }
                } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

                // If we have collected numbers, compare them numerically.
                // Otherwise, if we have strings, compare them alphabetically.
                string str1 = new string(space1);
                string str2 = new string(space2);

                int result;

                if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
                {
                    int thisNumericChunk = int.Parse(str1);
                    int thatNumericChunk = int.Parse(str2);
                    result = thisNumericChunk.CompareTo(thatNumericChunk);
                }
                else
                {
                    result = str1.CompareTo(str2);
                }

                if (result != 0)
                {
                    return result;
                }
            }
            return len1 - len2;
        }
    }

}
