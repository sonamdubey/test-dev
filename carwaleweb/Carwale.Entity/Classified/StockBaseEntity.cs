using Carwale.Entity.Enum;
using Carwale.Entity.Classified.Enum;
using Nest;
using System;

namespace Carwale.Entity.Classified
{
    public class StockBaseEntity
    {
        private string deliveryText = string.Empty;
        private int deliveryCity = 0;
        // Edited By Jugal || Using Elastic Property Name.

        public string ProfileId { get; set; }
        public int DealerId { get; set; }
        public string CarName { get; set; }
        public string MakeId { get; set; }
        public string RootId { get; set; }
        public string ModelId { get; set; }
        public int VersionId { get; set; }
        public string Url { get; set; }
        public string FrontImagePath { get; set; }
        public int InquiryId { get; set; }
        public string Seller { get; set; } //Individual
        public string SellerName { get; set; }
        public int PhotoCount { get; set; }
        public string Price { get; set; }
        public string PriceNumeric { get; set; }
        [Keyword(Name = "Kilometers")]
        public string Km { get; set; }
        public string KmNumeric { get; set; }
        public string MakeYear { get; set; }
        public int MakeMonth { get; set; }
        public string MakeName { get; set; }
        public string RootName { get; set; }
        public string ModelName { get; set; }
        public string VersionName { get; set; }
        public string MaskingName { get; set; }
        public string Color { get; set; }

        [Keyword(Name = "fuelType")]
        public string Fuel { get; set; }
        public string AdditionalFuel { get; set; }//Petrol

        //Added By : Sadhana Upadhyay on 3 Mar 2015
        [Keyword(Name = "transmission")]
        public string GearBox { get; set; }

        [Keyword(Name = "Comments")]
        public string SellerNote { get; set; }
        public int VideoCount { get; set; }
        public string OfferStartDate { get; set; }
        public string OfferEndDate { get; set; }
        [Keyword(Name = "LastUpdated")]
        public string LastUpdatedOn { get; set; }
        public string AreaName { get; set; }
        public string CityName { get; set; }
        public string CityId { get; set; }
        public string StateName { get; set; }
        public string CertifiedLogoUrl { get; set; }
        [Keyword(Name = "owners")]
        public string NoOfOwners { get; set; }
        public int OwnerTypeId { get; set; }
        public bool IsPremium { get; set; }
        public bool IsHotDeal { get; set; }
        public decimal Emi { get; set; }
        public string EmiFormatted { get; set; }
        public string FinanceUrlText { get; set; }
        public string AbsureWarranty { get; set; }
        public string AbsureScore { get; set; }
        public string FinanceUrl { get; set; }
        public string MakeMapping { get; set; }
        public string RootMapping { get; set; }
        public string ApiFlag { get; set; }
        public string SortScore { get; set; }
        public string MaskingNumber { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public int CertificationId { get; set; }
        public string InspectionText { get; set; }
        public bool HasWarranty { get; set; }
        public int DealerQuickBloxId { get; set; }
        public string SubSegmentID { get; set; }
        public string VersionSubSegmentID { get; set; }
        public string BodyStyleId { get; set; }
        public string NBCityStripId { get; set; }
        public string DeliveryText
        {
            get
            {
                return deliveryText;
            }
            set
            {
                deliveryText = value;
            }
        }
        public int DeliveryCity
        {
            get
            {
                return deliveryCity;
            }
            set
            {
                deliveryCity = value;
            }
        }
        public string NearbyCityText { get; set; }
        public bool IsPremiumPackage { get; set; }
        public bool IsNearbyCityListing { get; set; }
        public decimal? CertificationScore { get; set; }
        public int Responses { get; set; }
        public bool IsEligibleForFinance { get; set; }
        public string ResponsesText { get; set; }
        public DateTime MfgDate { get; set; }
        public string ValuationUrl { get; set; }
        public string DealerRatingText { get; set; }
        public int CertProgId { get; set; }
        public string CertProgLogoUrl { get; set; }
        public string Rank { get; set; }
        public string StockRecommendationsUrl { get; set; }
        public bool IsChatAvailable { get; set; }
        public NearbyCarsBucket NearbyCarsBucket { get; set; }
        public int CtePackageId { get; set; }
        public CwBasePackageId CwBasePackageId { get; set; }
        public string DealerCarsUrl { get; set; }
        public int SlotId { get; set; }
    }
}