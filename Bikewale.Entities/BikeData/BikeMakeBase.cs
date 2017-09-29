using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 28-Sep-2017
    /// Description :  BikeMakeBase
    /// </summary>
    [Serializable, DataContract]
    public class BikeMakeBase
    {
        [DataMember]
        public int MakeId { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
    }
}