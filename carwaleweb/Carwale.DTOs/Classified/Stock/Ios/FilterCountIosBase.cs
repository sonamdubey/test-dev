using Carwale.Entity.Classified;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock.Ios
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 28 May 2015
    /// </summary>
    public class FilterCountIosBase
    {
        [JsonProperty("stockCount")]
        public StockCount StockCount { get; set; }

        [JsonProperty("filterTypeAdditionalCount")]
        public FilterByAdditional FilterTypeAdditional { get; set; }

        [JsonProperty("sellerTypeCount")]
        public SellerType SellerTypeCount { get; set; }

        [JsonProperty("makeCount")]
        public List<StockMake> MakeCount { get; set; }

        [JsonProperty("fuelTypeCount")]
        public FuelType FuelTypeCount { get; set; }

        [JsonProperty("bodyTypeCount")]
        public BodyType BodyTypeCount { get; set; }

        [JsonProperty("ownersTypeCount")]
        public Owners OwnersTypeCount { get; set; }

        [JsonProperty("transmissionTypeCount")]
        public Transmission TransmissionTypeCount { get; set; }

        [JsonProperty("cityCount")]
        public List<Carwale.Entity.Classified.City> CityCount { get; set; }

        [JsonProperty("colorCount")]
        public AvailableColors ColorTypeCount { get; set; }
    }
}
