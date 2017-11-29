﻿
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.Used
{    /// <summary>
     /// Created By : subodh jain 06 oct 2016
     /// Description : Bike Count and city details
     /// </summary>
    [Serializable, DataContract]
    public class UsedBikeCities : Location.CityEntityBase
    {
        [DataMember]
        public uint BikesCount { get; set; }
        [DataMember]
        public uint Priority { get; set; }
    }
}
