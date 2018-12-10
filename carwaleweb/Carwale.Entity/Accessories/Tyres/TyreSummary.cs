using System;

namespace Carwale.Entity.Accessories.Tyres
{
    [Serializable]
    public class TyreSummary
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string ModelName { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public string Size { get; set; }
        public string Warranty { get; set; }
        public bool Tubeless { get; set; }
        public int Price { get; set; }
        public string TyreDetailPageUrl { get; set; }
        public bool IsSponsored { get; set; }
    }
}