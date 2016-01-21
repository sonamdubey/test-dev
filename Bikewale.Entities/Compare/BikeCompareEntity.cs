using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    public class BikeCompareEntity
    {
        //IEnumerable<BikeDetails> Bikes { get; set; }
        public IEnumerable<BikeEntityBase> BasicInfo { get; set; }
        public IEnumerable<BikeSpecification> Specifications { get; set; }
        public IEnumerable<BikeFeature> Features { get; set; }        
        public List<BikeColor> Color { get; set; }
        public CompareMainCategory ComapareSpecifications { get; set; }
        public CompareMainCategory CompareFeatures { get; set; }
    }
}
