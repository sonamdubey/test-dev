using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    [Serializable, DataContract]
    public class ReviewObject
    {
        [DataMember]
        public uint RatingValue { get; set; }
        [DataMember]
        public string ReviewTitle { get; set; }
        [DataMember]
        public string ReviewDescription { get; set; }
        [DataMember]
        public string ReviewListUrl { get; set; }
        [DataMember]
        public string ReviewDetailUrl { get; set; }
    }
}
