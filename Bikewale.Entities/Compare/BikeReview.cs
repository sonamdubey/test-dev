using Bikewale.Entities.UserReviews.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By:Snehal Dange on 11 Sep 2017
    /// Description :Bike Compare Reviews Entity
    /// </summary>
   public class BikeReview
    {
        [DataMember]
        public uint VersionId { get; set; }
      
      
        [DataMember]
        public UserReviewSummary MostHelpfullReview { get; set; }


    }
}
