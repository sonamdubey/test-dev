using Bikewale.Entities.Location;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Entities.NewBikeSearch
{
    public class BikeTopVersion
    {
        public ModelEntity BikeModel { get; set; }
        public IEnumerable<VersionEntity> VersionPrice { get; set; }
        public CityEntityBase City { set; get; }
    }
}
