using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.DTOs.CMS.Articles
{
    /// <summary>
    /// Created By : Meet Shah on 5 Aug 2016
    /// </summary>
    public class ArticleGist
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("articleUrl")]
        public string ArticleUrl { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("categoryMaskingName")]
        public string CategoryMaskingName { get; set; }
    }
}
