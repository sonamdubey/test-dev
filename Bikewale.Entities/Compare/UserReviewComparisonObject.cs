﻿using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created by sajal Gupta on 15-09-2017
    /// Description :  USer review object to save comparison data
    /// </summary>
    [Serializable, DataContract]
    public class UserReviewComparisonObject
    {
        [DataMember]
        public string ReviewRate { get; set; }
        [DataMember]
        public string RatingCount { get; set; }
        [DataMember]
        public string ReviewCount { get; set; }
        [DataMember]
        public string ReviewListUrl { get; set; }
    }
}
