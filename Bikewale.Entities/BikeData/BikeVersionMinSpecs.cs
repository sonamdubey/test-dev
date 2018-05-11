using Bikewale.Entities.GenericBikes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified By :Sushil Kumar
    /// Modified On : 21st Jan 2016
    /// Description : Added provision to get version colors
    /// Modified by: Vivek Singh Tomar on 23 Aug 2017
    /// Summary: Added body style
    /// Modified by : Rajan Chauhan on 23 Mar 2018
    /// Description : Replaced hardcoded minspecs with MinSpecsList
    /// </summary>
    [Serializable, DataContract]
    public class BikeVersionMinSpecs : BikeVersionsListEntity
    {
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
        [DataMember]
        public EnumBikeBodyStyles BodyStyle { get; set; }
    }
}
