using FluentValidation.Attributes;

namespace Carwale.Entity.Stock
{
    [Validator(typeof(StockRecoParamsValidator))]
    public class StockRecoParams
    {
        public string ProfileId { get; set; }
        public int RootId { get; set; }
        public int CityId { get; set; }
        public int Price { get; set; }
        public int RecommendationsCount { get; set; }
        public int VersionSubSegmentId { get; set; }
    }
}
