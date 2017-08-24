using Bikewale.Entities.GenericBikes;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By :Sushil Kumar
    /// Modified On : 21st Jan 2016
    /// Description : Added provision to get version colors
    /// Modified by: Vivek Singh Tomar on 23 Aug 2017
    /// Summary: Added body style
    /// </summary>
    [Serializable, DataContract]
    public class BikeVersionMinSpecs : BikeVersionsListEntity
    {
        [DataMember]
        public string BrakeType { get; set; }
        [DataMember]
        public bool AlloyWheels { get; set; }
        [DataMember]
        public bool ElectricStart { get; set; }
        [DataMember]
        public bool AntilockBrakingSystem { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
    }
}
