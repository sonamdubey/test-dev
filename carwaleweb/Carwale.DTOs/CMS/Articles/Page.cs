using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 13 Aug 2014
    /// </summary>
    public class Page
    {
        public ulong pageId { get; set; }
        public ushort Priority { get; set; }
        public string PageName { get; set; }
        public string Content { get; set; }
    }

    public class Page_V1
    {
        [JsonProperty("pageId")]
        public ulong PageId { get; set; }
        [JsonProperty("priority")]
        public ushort Priority { get; set; }
        [JsonProperty("pageName")]
        public string PageName { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
