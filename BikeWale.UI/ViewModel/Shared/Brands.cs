
using Bikewale.Entities.BikeData;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models.Shared
{

    /// <summary>
    /// Created by : Sangram Nandkhile on 10 Mar 2017
    /// Summary: Model to holf scooter's brands- topBrands and remaining brands
    /// </summary>
    public class BrandWidget
    {
        public IEnumerable<BikeMakeEntityBase> TopBrands { get; set; }
        public IEnumerable<BikeMakeEntityBase> OtherBrands { get; set; }
        public bool HasOtherBrands { get { return (OtherBrands != null && OtherBrands.Any()); } }

    }

}
