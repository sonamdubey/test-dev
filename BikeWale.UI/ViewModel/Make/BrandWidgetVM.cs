using Bikewale.Entities.BikeData;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 24-Mar-2017
    /// Summary :  View model for Brand widget
    /// </summary>
    public class BrandWidgetVM
    {
        public IEnumerable<BikeMakeEntityBase> TopBrands { get; set; }
        public IEnumerable<BikeMakeEntityBase> OtherBrands { get; set; }
        public bool HasOtherBrands { get { return (OtherBrands != null && OtherBrands.Any()); } }
    }
}
