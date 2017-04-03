
namespace Bikewale.Entities.GenericBikes
{
    public class BikeRankingPropertiesEntity : BikeRankingEntity
    {
        public uint ModelId { get; set; }
        public string StyleName { get; set; }
        public string BikeType { get; set; }
        public string RankText { get; set; }
    }
}
