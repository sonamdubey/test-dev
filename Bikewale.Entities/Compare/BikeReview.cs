using Bikewale.Entities.UserReviews.V2;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created By:Snehal Dange on 11 Sep 2017
    /// Description :Bike Compare Reviews Entity
    /// </summary>
    [Serializable]
    public class BikeReview
    {
        [DataMember]
        public uint VersionId { get; set; }


        [DataMember]
        public ModelWiseUserReview ModelReview { get; set; }


    }
}
