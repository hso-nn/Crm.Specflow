using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Api.UCI.DTO;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro.FieldTypes;

namespace Vermaat.Crm.Specflow.EasyRepro.Controls
{
    public abstract class Control
    {
        public string ControlName { get; private set; }
        protected UCIApp App { get; }

        
        public Control(UCIApp app, string control)
        {
            App = app;
            ControlName = control;
        }

       

        


        

       
        

        

        

        

       
    }
}
