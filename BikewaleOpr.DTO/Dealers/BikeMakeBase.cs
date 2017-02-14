using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.Dealers
{
    /// <summary>
    /// Created by : Aditi Srivastava on 9 Feb 2017
    /// Summary    : Makes having dealers in a city
    /// </summary>
    public class BikeMakeBase
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }
    }
}
