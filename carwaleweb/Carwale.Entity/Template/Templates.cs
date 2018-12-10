using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Template
{
    [Serializable]
    public class Templates
    {
        public string UniqueName { get; set; }
        public string Html { get; set; }
        public int TemplateType { get; set; }
    }
}
