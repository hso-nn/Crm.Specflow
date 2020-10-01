using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using System;
using Vermaat.Crm.Specflow.EasyRepro;

namespace Vermaat.Crm.Specflow
{
    public class SeleniumTestingContext
    {

        private readonly CrmTestingContext _crmContext;

        public BrowserOptions BrowserOptions { get; }
        public string CurrentApp { get; set; }
        public bool IsSessionActive { get; private set; }

        public SeleniumTestingContext(CrmTestingContext crmContext)
        {
            _crmContext = crmContext;
            BrowserOptions = new BrowserOptions()
            {
                CleanSession = true,
                DriversPath = null,
                StartMaximized = true,
                PrivateMode = true,
                UCITestMode = true,
            };
            CurrentApp = HelperMethods.GetAppSettingsValue("AppName", true);

        }

        public T GetBrowser<T>(IBrowserFactory<T> browserFactory)
        {
            if (_crmContext.IsTarget("API"))
                throw new TestExecutionException(Constants.ErrorCodes.CANT_START_BROWSER_FOR_API_TESTS);

            var browser = GlobalTestingContext.BrowserManager.GetBrowser<T>(browserFactory, BrowserOptions, GlobalTestingContext.ConnectionManager.CurrentBrowserLoginDetails);
            IsSessionActive = true;
            return browser;
        }

    }
}
