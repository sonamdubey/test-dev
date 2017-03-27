
using Bikewale.Entities.BikeData.NewLaunched;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Sangram Nandkhile on 23 Mar 2017
    /// Summary : View Model for Homepage
    /// </summary>
    public class HomePageVM : ModelBase
    {
        public BrandWidgetVM Brands { get; set; }
        public NewLaunchedWidgetVM NewLaunchedBikes { get; set; }
        public BestBikeWidgetVM BestBikes { get; set; }
    }


    /// <summary>
    /// </summary>
    /// <author>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary:  View Model for New launches
    /// </author>
    public class NewLaunchedWidgetVM
    {
        public IEnumerable<NewLaunchedBikeEntityBase> Bikes { get; set; }
        public uint PQSourceId { get; set; }
        public uint PageCatId { get; set; }
    }
}
