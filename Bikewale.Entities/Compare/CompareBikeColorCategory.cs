using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Bike Color Category. It contains version-wise color lists.
    /// </summary>
    [Serializable]
    public class CompareBikeColorCategory
    {
        public List<CompareBikeColor> bikes { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
