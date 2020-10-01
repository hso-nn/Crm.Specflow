using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Vermaat.Crm.Specflow.Entities;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    class UCIBrowserFactory : IBrowserFactory<UCIBrowser>
    {
        private static readonly Lazy<CrmModelApps> _appCache;

        static UCIBrowserFactory()
        {
            _appCache = new Lazy<CrmModelApps>(InitializeCache);
        }

        private static CrmModelApps InitializeCache()
        {
            Logger.WriteLine("Initializing App Cache");
            return CrmModelApps.GetApps();
        }

        public UCIBrowser FromApp(UCIApp app)
        {
            return new UCIBrowser(app, _appCache.Value);

        }
    }
}
