using Bikewale.Entities.BikeData;
using Bikewale.Entities.Used;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Compare
{
    /// <summary>
    /// Created by  :   Sumit Kate on 22 Jan 2016
    /// Modified By :   Sushil Kumar on 2nd Feb 2017
    /// Description :   Added EstimatedPriceMin,EstimatedPriceMax,ExpectedLaunch,UsedBikeCount,ModelId and Versiosn for other details related to comparisions
    /// Modified by :   Vivek Singh Tomar on 1st Nov 2017
    /// Description :   Added IsNew, IsUpcoming and IsDiscontinued properties to handle discontinued on bikecomparisions
    /// </summary>
    [Serializable, DataContract]
    public class BikeEntityBase
    {
        [DataMember]
        public uint VersionId { get; set; }
        [DataMember]
        public string Make { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MakeMaskingName { get; set; }
        [DataMember]
        public string ModelMaskingName { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public int Price { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public UInt16 VersionRating { get; set; }
        [DataMember]
        public UInt16 ModelRating { get; set; }
        [DataMember]
        public UInt32 EstimatedPriceMin { get; set; }
        [DataMember]
        public UInt32 EstimatedPriceMax { get; set; }
        [DataMember]
        public DateTime? ExpectedLaunch { get; set; }
        [DataMember]
        public uint ModelId { get; set; }
        [DataMember]
        public IEnumerable<BikeVersionEntityBase> Versions { get; set; }
        [DataMember]
        public UsedBikesCountInCity UsedBikeCount { get; set; }
        [DataMember]
        public bool IsNew { get; set; }
        [DataMember]
        public bool IsUpcoming { get; set; }
        [DataMember]
        public bool IsDiscontinued { get; set; }
        [DataMember]
        public ushort Mileage { get; set; }


    }
}
