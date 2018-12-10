using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class PagerAndroid
    {
        [JsonProperty("pagerResults")]
        public PagerAndroidBase PagerResults { get; set; }  
    }
}
