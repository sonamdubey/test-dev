using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UserReviews
{
    [Serializable,DataContract]
    public class ReviewEntityBase
    {
        [DataMember]
        public int ReviewId { get; set; }
        [DataMember]
        public string ReviewTitle { get; set; }
        [DataMember]
        public DateTime ReviewDate { get; set; }
        [DataMember]
        public string WrittenBy { get; set; }
    }
}
