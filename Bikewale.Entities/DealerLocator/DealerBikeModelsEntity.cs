using System.Collections.Generic;
using System.Runtime.Serialization;
using Bikewale.Entities.BikeData;

namespace Bikewale.Entities.DealerLocator
{
    public class DealerBikeModelsEntity
    {
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public IEnumerable<MostPopularBikesBase> Models { get; set; }
    }
}
