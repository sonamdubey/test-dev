using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Models
{

    /// <author>
    /// Create by: Sangram Nandkhile on 13-Apr-2017
    /// Summary:  View Model for other makes
    /// </author>
    public class OtherMakesVM
    {
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }
    }
}
