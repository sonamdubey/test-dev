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
    /// Description : For Bike Manufacture List. 
    /// </summary>
    public class MakeDetail
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }
    }
}
