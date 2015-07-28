using System;
using System.Collections.Generic;
using System.Text;

namespace BikeWaleOpr.Entities.Pager
{
    public class PagerEntity
    {
        public int PageNo { get; set; }
        public int PagerSlotSize { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
        public string BaseUrl { get; set; }
        public string PageUrlType { get; set; }
    }
}
