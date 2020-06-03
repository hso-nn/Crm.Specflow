using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormTab
    {
        public string Name { get; }
        public string Label { get; }
        public List<FormSection> Sections { get; set; }

        public FormTab(string name, string label)
        {
            Sections = new List<FormSection>();
            Name = name;
            Label = label;
        }

       
    }
}
