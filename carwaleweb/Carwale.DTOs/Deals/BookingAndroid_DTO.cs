using Carwale.DTOs.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class BookingAndroid_DTO
    {
        [JsonProperty("make")]
        public CarMakesDTO Make
        {
            get;
            set;
        }
        [JsonProperty("model")]
        public CarModelsDTO Model
        {
            get;
            set;
        }
        [JsonProperty("version")]
        public CarVersionsDTO Version
        {
            get;
            set;
        }
        [JsonProperty("onRoadPrice")]
        public int OnRoadPrice
        {
            get;
            set;
        }
        [JsonProperty("savings")]
        public int Savings
        {
            get;
            set;
        }
        [JsonProperty("offerPrice")]
        public int OfferPrice
        {
            get;
            set;
        }
        [JsonProperty("carImageDetails")]
        public CarImageBaseDTO CarImageDetails
        {
            get;
            set;
        }
        [JsonProperty("offers")]
        public string Offers
        {
            get;
            set;
        }
        [JsonProperty("termsConditions")]
        public string TermsConditions
        {
            get;
            set;
        }
        [JsonProperty("stockCount")]
        public int StockCount
        {
            get;
            set;
        }
        [JsonProperty("manufacturingYear")]
        public int ManufacturingYear
        {
            get;
            set;
        }
        [JsonProperty("tollFreeNo")]
        public string TollFreeNumber
        {
            get;
            set;
        }
        [JsonProperty("bookingAmount")]
        public int BookingAmount
        {
            get;
            set;
        }
        [JsonProperty("color")]
        public CarColorDTO Color
        {
            get;
            set;
        }
        [JsonProperty("city")]
        public City City
        {
            get;
            set;
        }

        [JsonProperty("priceUpdated")]
        public bool PriceUpdated
        {
            get;
            set;
        }


        [JsonProperty("dealsPriceBreakupList")]
        public List<KeyValuePair<string, string>> BreakUpList { get; set; }

        [JsonProperty("offerValue")]
        public int OfferValue { get; set; }

        [JsonProperty("isBreakUpAvailable")]
        public bool IsBreakUpAvailable { get; set; }

        [JsonProperty("disclaimerText")]
        public string DisclaimerText { get; set; }

        [JsonProperty("payBtnText")]
        public string PayBtnText { get; set; }
        
        [JsonProperty("dealsOfferList")]
        public List<KeyValuePair<string, string>> OfferList { get; set; }

        [JsonProperty("deliveryTimeline")]
        public int DeliveryTimeline { get; set; }

        [JsonProperty("bookingReasons")]
        public BookingReasons Reasons { get; set; }
        
    }
}
