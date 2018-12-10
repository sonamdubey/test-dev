using System;

namespace Carwale.Entity.Deals
{
    [Serializable]
    public class DiscountSummary
    {
        public int MaxDiscount { get; set; }
        public string MaskingName { get; set; }
        public int DealsCount { get; set; }
        public uint CityId { get; set; }
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public string ModelName { get; set; }
        public string Offers { get; set; }
    }
}
