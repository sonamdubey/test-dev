﻿using Bikewale.Entities.BikeData;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Videos
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Feb 2017
    /// Summary    : Entity for modelwise video count on make/video page
    /// </summary>
    [DataContract, Serializable]
    public class BikeVideoModelEntity : BikeModelEntityBase
    {
        [DataMember]
        public BikeMakeEntityBase objMake { get; set; }
        [DataMember]
        public int VideoCount { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        
    }
}
