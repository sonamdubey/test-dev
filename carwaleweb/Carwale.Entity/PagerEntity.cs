using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity
{
    /// <summary>
    /// This entity class holds the necessary properties for the pager.
    /// </summary>
    public class PagerEntity
    {
        public int pageNo { get; set; }
        public int pagerSlotSize { get; set; }
        public int pageSize { get; set; }
        public int totalResults { get; set; }
        public string baseUrl { get; set; }
        public string pageUrlType { get; set; }
    }
}
