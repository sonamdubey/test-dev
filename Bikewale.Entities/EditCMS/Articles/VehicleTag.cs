using Bikewale.Entities.BikeData;

namespace Bikewale.Entity.CMS.Articles
{
    /// <summary>
    /// Created By : Ashwini Todkar on 17 Sept 2014
    /// </summary>

    public class VehicleTag
    {
        public BikeMakeEntityBase MakeBase { get; set; }
        public BikeModelEntityBase ModelBase { get; set; }
        public BikeVersionEntity VersionBase { get; set; }
    }
}
