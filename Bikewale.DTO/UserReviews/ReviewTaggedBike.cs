using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;


namespace Bikewale.Entities.DTO
{
    public class ReviewTaggedBike
    {
        public MakeBase MakeEntity { get; set; }
        public ModelBase ModelEntity { get; set; }
        public VersionBase VersionEntity { get; set; }

        public uint ReviewsCount { get; set; }
        public uint Price { get; set; }
    }
}
