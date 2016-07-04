using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Compare Entity
    /// </summary>
    [Serializable, DataContract]
    public class BikeCompareEntity
    {
        [DataMember]
        public IEnumerable<BikeEntityBase> BasicInfo { get; set; }
        [DataMember]
        public IEnumerable<BikeSpecification> Specifications { get; set; }
        [DataMember]
        public IEnumerable<BikeFeature> Features { get; set; }
        [DataMember]
        public List<BikeColor> Color { get; set; }
        [DataMember]
        public CompareMainCategory CompareSpecifications { get; set; }
        [DataMember]
        public CompareMainCategory CompareFeatures { get; set; }
        [DataMember]
        public CompareBikeColorCategory CompareColors { get; set; }
    }
}
