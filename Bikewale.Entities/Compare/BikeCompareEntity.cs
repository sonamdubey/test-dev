using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Compare Entity
    /// </summary>
    [Serializable]
    public class BikeCompareEntity
    {        
        public IEnumerable<BikeEntityBase> BasicInfo { get; set; }
        public IEnumerable<BikeSpecification> Specifications { get; set; }
        public IEnumerable<BikeFeature> Features { get; set; }        
        public List<BikeColor> Color { get; set; }
        public CompareMainCategory ComapareSpecifications { get; set; }
        public CompareMainCategory CompareFeatures { get; set; }
        public CompareBikeColorCategory CompareColors { get; set; }
    }
}
