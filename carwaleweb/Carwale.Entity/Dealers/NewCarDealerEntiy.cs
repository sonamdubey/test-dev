using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Geolocation;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    public class NewCarDealerEntiy
    {
        [JsonProperty("newCarDealers")]
        public List<NewCarDealer> NewCarDealers = new List<NewCarDealer>();

        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

		[JsonProperty("cityMaskingName")]
		public string CityMaskingName { get; set; }

        [JsonProperty("custLocation")]
        public Location CustLocation { get; set; }
	}
}
