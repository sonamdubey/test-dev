using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Make
{
    /// <summary>
    /// Author  :   Sumit Kate
    /// Date    :   04 Sept 2015
    /// New Bike Dealer makes list
    /// </summary>
    public class NewBikeDealersMakeList
    {
        /// <summary>
        /// List of makes
        /// </summary>
        [JsonProperty("makes")]
        public IEnumerable<NewBikeDealersMakeBase> Makes { get; set; }
    }
}
