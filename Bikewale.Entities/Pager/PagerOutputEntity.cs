﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Pager
{
    public class PagerOutputEntity
    {
        public List<PagerUrlList> PagesDetail { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
        public string FirstPageUrl { get; set; }
        public string LastPageUrl { get; set; }
    }
}
