using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.Connectivity;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public interface IBrowserFactory<T>
    {
        T FromApp(UCIApp app);
        void PrepareBrowser(T browser, BrowserLoginDetails browserLoginDetails);
    }
}
