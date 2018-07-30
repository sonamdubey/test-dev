
using Bikewale.Entities.GenericBikes;
namespace Bikewale.Models.Mobile.Videos
{
    public class GenericBikeInfoCard
    {
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public BikeInfoTabType PageId { get; set; }
        public bool IsSmallSlug { get; set; }
    }
}
