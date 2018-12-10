using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// written by Ashish Kamble
    /// </summary>
    public class ArticleBase
    {
        public ulong BasicId { get; set; }
        public string Title { get; set; }
        public string ArticleUrl { get; set; }
    }

    public class ArticleBaseV2
    {
        [JsonProperty("basicId")]
        public ulong BasicId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("articleUrl")]
        public string ArticleUrl { get; set; }
    }
}
