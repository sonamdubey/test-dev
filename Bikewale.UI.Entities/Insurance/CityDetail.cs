using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.UI.Entities.Insurance
{
    /// <summary>
    /// Created BY : Lucky Rathore on 18 November 2015.
    /// </summary>
    public class CityDetail
    {
        [JsonProperty("cityId")]
        public int CityId { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("stateName")]
        public string StateName { get; set; }
    }
}
