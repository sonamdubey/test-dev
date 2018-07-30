using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Entities.Location;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <author>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary:  View Model for New launches
    /// Modified by : Sanskar Gupta on 12th Feb 2018
    /// Description : Added City object
    /// </author>
    public class NewLaunchedWidgetVM
    {
        public IEnumerable<NewLaunchedBikeEntityBase> Bikes { get; set; }
        public uint PQSourceId { get; set; }
        public uint PageCatId { get; set; }

        public CityEntityBase City { get; set; }
    }
}
