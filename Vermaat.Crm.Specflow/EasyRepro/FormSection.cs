using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vermaat.Crm.Specflow.EasyRepro.Controls;

namespace Vermaat.Crm.Specflow.EasyRepro
{
    public class FormSection
    {
        public string Name { get; }
        public string Label { get; }

        public List<FormField> Fields { get; }
        public List<Subgrid> Subgrids { get; }

        public FormSection(string name, string label)
        {
            Name = name;
            Label = label;
            Fields = new List<FormField>();
            Subgrids = new List<Subgrid>();
        }

       
    }
}
