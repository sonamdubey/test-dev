using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Carwale.Entity.Classified
{
    public class CountData
    {
        [JsonProperty("stockcount")]
        public StockCount StockCount { get; set; }

        [JsonProperty("filterby")]
        public FilterBy FilterTypeCount { get; set; }

        [JsonProperty("filterbyadditional")]        // Added for additional filters
        public FilterByAdditional FilterTypeAdditional { get; set; }

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
    }
}
