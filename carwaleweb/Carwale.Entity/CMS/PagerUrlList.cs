using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carwale.Entity
{
    public class PagerUrlList
    {
        //[JsonProperty("pageNo")]
        public int PageNo { get; set; }

        //[JsonProperty("pageUrl")]
        public string PageUrl { get; set; }
    }
}
