using Carwale.Entity;
using Carwale.Entity.Classified;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class FiltersCountAndroidBase
    {
        /*
         *Author: Jugal Singh
         *Date Created: 26/08/2014
         *DESC: Contains Filters count Separately
         *Modified By : Sadhana Upadhyay on 27 Apr 2015
         *Summary : Added properties for FilterTypeAdditional and ColorTypeCount
         */

        [JsonProperty("stockCount")]
        public StockCount StockCount { get; set; }
        
        [JsonProperty("filterTypeCount")]
        public FilterBy FilterTypeCount { get; set; }

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
