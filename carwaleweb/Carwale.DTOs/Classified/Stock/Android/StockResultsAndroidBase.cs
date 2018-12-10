using Carwale.Entity.Enum;
using Carwale.Utility;
using Newtonsoft.Json;
using System;

namespace Carwale.DTOs.Classified.Stock
{
    public class StockResultsAndroidBase
    {
        /*
         *Author: Jugal Singh
         *Date Created: 26/08/2014
         *DESC: Contains Car properties for Android
         */
        string _frontImgPath = string.Empty;
        string _largeImgPath = string.Empty;//Added By Sachin Bharti(11th Aug 2015)

        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("carName")]
        public string CarName { get; set; }

        [JsonProperty("city")]
        public string CityName { get; set; }

        [JsonProperty("usedCarDetail")]
        public string Url { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("formattedPrice")]
        public string FormattedPrice { get; set; }

        [JsonProperty("kms")]
        public string Km { get; set; }

        [JsonProperty("year")]
        public string MakeYear { get; set; }

        [JsonProperty("isPremium")]
        public string IsPremium { get; set; }

        [JsonProperty("updated")]
        public string LastUpdatedOn { get; set; }

        [JsonProperty("AreaName")]
        public string AreaName { get; set; }

        [JsonProperty("MaskingNumber")]
        public string MaskingNumber { get; set; }

        [JsonProperty("AbsureScore")]
        public string AbsureScore { get; set; }

        [JsonProperty("CertifiedLogoUrl")]
        public string CertifiedLogoUrl { get; set; }

        [JsonProperty("InspectionText")]
        public string InspectionText { get; set; }

        [JsonProperty("HasWarranty")]
        public string HasWarranty { get; set; }

        [JsonProperty("OriginalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("HostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("deliveryText")]
        public string DeliveryText { get; set; }

        [JsonProperty("deliveryCityId")]
        public int DeliveryCity { get; set; }

        [JsonProperty("smallPicUrl")]
        public string FrontImagePath
        {
            get
            {
                return _frontImgPath;
            }
            set
            {
                if (!String.IsNullOrEmpty(OriginalImgPath) && !String.IsNullOrEmpty(HostUrl))
                    _frontImgPath = HostUrl + ImageSizes._310X174 + OriginalImgPath;
            }
        }

        /// <summary>
        /// Added By Sachin Bharti(11th Aug 2015)
        /// </summary>
        [JsonProperty("largePicUrl")]
        public string LargePicUrl
        {
            get
            {
                return _largeImgPath;
            }
            set
            {
                if (!String.IsNullOrEmpty(OriginalImgPath) && !String.IsNullOrEmpty(HostUrl))
                    _largeImgPath = HostUrl + ImageSizes._762X429 + OriginalImgPath;
            }
        }

        /// <summary>
        /// Added By Purohith Guguloth on 30th September, 2015
        /// </summary>
        [JsonProperty("Fuel")]
        public string Fuel { get; set; }

        [JsonProperty("AdditionalFuel")]
        public string AdditionalFuel { get; set; }

        [JsonProperty("GearBox")]
        public string GearBox { get; set; }

        [JsonProperty("CertificationScore")]
        public string CertificationScore { get; set; }

        [JsonProperty("financeEmi")]
        public string FinanceEmi { get; set; }

        [JsonProperty("financeLinkText")]
        public string FinanceLinkText { get; set; }

        [JsonProperty("makeId")]
        public uint MakeId { get; set; }

        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("usedCarCityId")]
        public uint CityId { get; set; }

        [JsonProperty("valuationUrl")]
        public string ValuationUrl { get; set; }

        [JsonProperty("valuationText")]
        public string ValuationText { get; set; }

        [JsonProperty("dealerRatingText")]
        public string DealerRatingText { get; set; }

        [JsonProperty("certProgLogoUrl")]
        public string CertProgLogoUrl { get; set; }

        [JsonProperty("isChatAvailable")]
        public bool IsChatAvailable { get; set; }

        [JsonProperty("stockRecommendationsUrl")]
        public string StockRecommendationsUrl { get; set; }

        [JsonProperty("cwBasePackageId")]
        public CwBasePackageId CwBasePackageId { get; set; }

        [JsonProperty("dealerCarsUrl")]
        public string DealerCarsUrl { get; set; }

        [JsonProperty("photoCount")]
        public string PhotoCount { get; set; }
    }
}
