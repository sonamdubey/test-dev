using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Classified
{
    [Serializable]
    public class UsedCarComparisonEntity
    {
        public string Count { get; set; }
        public string MinPrice { get; set; }
        public int RootId { get; set; }
    }
}
