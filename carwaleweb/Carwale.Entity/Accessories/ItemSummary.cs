using System;

namespace Carwale.Entity.Accessories.Tyres
{
    [Serializable]
    public class ItemSummary
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string Title { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public string Dimension_width { get; set; }
        public string Dimension_Length { get; set; }
        public string Dimension_Height { get; set; }
        public string Weight { get; set; }
        public int Price { get; set; }
        public string ProductFeature { get; set; }
        public string ProductDescription { get; set; }
    }
}
