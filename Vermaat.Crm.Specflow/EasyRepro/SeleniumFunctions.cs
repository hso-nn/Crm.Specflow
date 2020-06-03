using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.FormLoadConditions;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    internal static class SeleniumFunctions
    {
        
        public static string GetErrorDialogMessage(IWebDriver driver, SeleniumSelectorData selectors)
        {
            if (driver.TryFindElement(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Dialog_Subtitle), out IWebElement subTitle))
                return subTitle.Text;
            else
                return null;
        }

        public static void WaitForFormLoad(IWebDriver driver, SeleniumSelectorData selectors, params IFormLoadCondition[] additionalConditions)
        {
            DateTime timeout = DateTime.Now.AddSeconds(30);

            bool loadComplete = false;
            while (!loadComplete)
            {
                loadComplete = true;

                TimeSpan timeLeft = timeout.Subtract(DateTime.Now);
                if (timeLeft.TotalMilliseconds > 0)
                {
                    driver.WaitForPageToLoad();
                    driver.WaitUntilClickable(selectors.GetXPathSeleniumSelector(SeleniumSelectorItems.Entity_FormLoad),
                        timeLeft,
                        null,
                        () => { throw new TestExecutionException(Constants.ErrorCodes.FORM_LOAD_TIMEOUT); }
                    );

                    if (additionalConditions != null)
                    {
                        foreach (var condition in additionalConditions)
                        {
                            if (!condition.Evaluate(driver, selectors))
                            {
                                Logger.WriteLine("Evaluation failed. Waiting for next attempt");
                                loadComplete = false;
                                Thread.Sleep(100);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    throw new TestExecutionException(Constants.ErrorCodes.FORM_LOAD_TIMEOUT);
                }
            }
            Logger.WriteLine("Form load completed");
        }


        public static bool TryFindElement(this IWebDriver driver, By by, out IWebElement element)
            {
                try
                {
                    element = driver.FindElement(by);
                    return true;
                }
                catch (NoSuchElementException)
                {
                    element = null;
                    return false;
                }
            }

        public static bool HasClass(this IWebElement element, string className)
        {
            return element.GetAttribute("class").Split(' ').Any(c => string.Equals(className, c, StringComparison.CurrentCultureIgnoreCase));
        }
      
        

    }
}
