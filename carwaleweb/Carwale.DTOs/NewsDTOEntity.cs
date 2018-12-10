using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.DTOs
{
    /// <summary>
    /// Created by : Supriya K on 12/6/2014 
    /// Desc : Class for entity required in news list api of android
    /// </summary>
    public class NewsDTOEntity
    {
        public List<NewsItemDTOEntity> newsItems = new List<NewsItemDTOEntity>();

        [JsonProperty("nextPageUrl")]
        public string NextPageUrl { get; set; }
    }
}

