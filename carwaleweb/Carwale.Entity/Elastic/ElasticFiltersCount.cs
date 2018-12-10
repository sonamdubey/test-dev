using Carwale.Entity.Classified;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.Elastic
{
    public class ElasticFiltersCount
    {
        /// <summary>
        /// Class Added by Jugal for ES Filters Count Separately || 13/11/14
        /// </summary>
        ///

        [JsonProperty("stockcount")]
        public StockCount StockCount { get; set; }

        [JsonProperty("filterby")]
        public FilterBy FilterTypeCount { get; set; }

        [JsonProperty("seller")]
        public SellerType SellerTypeCount { get; set; }

        [JsonProperty("Makes")]
        public List<StockMake> MakeCount { get; set; }

        [JsonProperty("fuel")]
        public FuelType FuelTypeCount { get; set; }

        [JsonProperty("bodytype")]
        public BodyType BodyTypeCount { get; set; }

        [JsonProperty("owners")]
        public Owners OwnersTypeCount { get; set; }

        [JsonProperty("trans")]
        public Transmission TransmissionTypeCount { get; set; }

        [JsonProperty("color")]
        public AvailableColors ColorTypeCount { get; set; }

        [JsonProperty("city")]
        public List<City> CityCount { get; set; }

        [JsonProperty("Area")]
        public List<Area> AreaCount { get; set; }
    }
}
