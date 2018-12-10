using Carwale.Entity.CMS.Articles;
using Carwale.Entity.Enum;

namespace Carwale.DTOs.CarData
{
    public class SubNavigationDTO
    {
        public int PageId { get; set;}
        public string MaskingName { get; set; }
        public int VideoCount { get; set; }
        public int ImageCount { get; set; }
        public bool OfferExists { get; set; }
        public bool IsMileageAvail { get; set; }
        public bool IsReviewsAvial { get; set; }
        public bool IsUsedCarAvail { get; set; }
        public Pages Page { get; set; }
        public bool Is360Avail { get; set; }
        public string ClassNameAP { get; set; }
        public string ClassNameInt { get; set; }
        public string ClassNameExt { get; set; }
        public int PQPageId { get; set; }
        public bool IsExpertReviewAvial { get; set; }       
        public bool IsUserReviewsAvailable { get; set; }
        public bool IsOverviewPage { get; set; }
        public bool IsVersionDetailPage { get; set; }
        public bool IsMileagePage { get; set; }
        public bool subNavOnCarCompare { get; set; }
        public string ModelName { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public string MakeName { get; set; }
        public bool ShowImagePopup { get; set; }
        public bool IsNew { get; set; }
        public bool IsFuturistic { get; set; }
        public bool IsModelPage { get; set; }
        public bool ShowColorsLink { get; set; }
        public ArticlePageDetails PageDetails { get; set; }
        public string Category { get; set; }
        public string Label { get; set; }
        public ThreeSixtyViewCategory Default360Category { get; set; }
    }
}
