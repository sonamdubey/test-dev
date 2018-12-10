using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Elastic
{
    [Serializable]
    public class UsedCarCityPayLoad
    {
        [JsonProperty("cityId")]
        public int Id { get; set; }
    }
}
