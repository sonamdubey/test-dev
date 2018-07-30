
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Videos;
using Bikewale.Models.Images;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Mar 2017
    /// Description :   MakeVideosPage View model
    /// Modified by :   Pratibha Verma on 8 Feb 2018
	/// Description :   Added Property ImageWidgetVM to bind MakeImage Page from Videos
    /// </summary>
    public class MakeVideosPageVM : ModelBase
    {
        public uint CityId { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public IEnumerable<BikeVideoModelEntity> Videos { get; set; }
        public ImageWidgetVM PopularSportsBikesWidget { get; set; }
    }
}
