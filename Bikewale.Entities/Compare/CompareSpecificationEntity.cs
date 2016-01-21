using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate 21 Jan 2016
    /// Description :   Compare Specification Entity
    /// </summary>
    public class CompareMainCategory
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public List<CompareSubMainCategory> Spec { get; set; }
    }
}
