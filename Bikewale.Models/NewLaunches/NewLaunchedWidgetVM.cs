using Bikewale.Entities.BikeData.NewLaunched;
using System.Collections.Generic;

namespace Bikewale.Models
{
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
