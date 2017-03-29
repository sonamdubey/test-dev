
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Mar 2017
    /// Description :   MakeVideosPage View model
    /// </summary>
    public class MakeVideosPageVM : ModelBase
    {
        public uint CityId { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public IEnumerable<BikeVideoModelEntity> Videos { get; set; }
    }
}
