using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.DTOs.Classified.CarDetails
{
    public class UsedCarDetails
    {
        public List<Data> general { get; set; }
        public List<Data> features { get; set; }
        public List<ConditionData> carCondition { get; set; }
        public List<CarPhoto> carPhoto { get; set; }
        public List<UsedCar> alternativeCars = new List<UsedCar>();
        public List<CarAdditionalInfo> carAdditionalInfo { get; set; }
        public List<object> absureInfo { get; set; }
        public List<object> carFinanceQuoteInfo { get; set; }
        public List<object> sellerOfferData { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("carName")]
        public string CarName { get; set; }

        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("kms")]
        public string Kms { get; set; }

        [JsonProperty("sellerNote")]
        public string SellerNote { get; set; }

        [JsonProperty("reasonForSelling")]
        public string ReasonForSelling { get; set; }

        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("certifiedLogoUrl")]
        public string CertifiedLogoUrl { get; set; }

        [JsonProperty("isDealerCar")]
        public string IsDealerCar { get; set; }

        [JsonProperty("deliveryText")]
        public string DeliveryText { get; set; }

        [JsonProperty("deliveryCityId")]
        public int DeliveryCity { get; set; }

        [JsonProperty("shareUrl")]
        public string ShareUrl { get; set; }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        [JsonProperty("statusId")]
        public int StatusId { get; set; }

        [JsonProperty("dealerQuickBloxId")]
        public int dealerQuickBloxId { get; set; }

        [JsonProperty("dealerRatingText")]
        public string DealerRatingText { get; set; }
    }
}