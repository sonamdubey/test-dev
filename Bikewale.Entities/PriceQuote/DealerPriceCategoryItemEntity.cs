﻿namespace Bikewale.Entities.PriceQuote
{
    public class DealerPriceCategoryItemEntity
    {
        public string ItemName { get; set; }
        public int Price { get; set; }
        public uint DealerId { get; set; }
        public uint ItemId { get; set; }
        public uint VersionId { get; set; }
    }
}
