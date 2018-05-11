using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Apr 2014
    /// Summary : Class to hold the properties for bike versions list on the models page.
    /// Modified by : Ashutosh Sharma on 30 Aug 2017 
    /// Description : Removed IsGstPrice property
    /// /// Modified by : Ashutosh Sharma on 29 Sep 2017 
    /// Description : Added AverageExShowroom property
    /// </summary>
    [Serializable, DataContract]
    public class BikeVersionsListEntity
    {
        [DataMember]
        public int VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public UInt64 Price { get; set; }
        [DataMember]
        public UInt32 AverageExShowroom { get; set; }
        [DataMember]
        public bool IsDealerPriceQuote { get; set; }

        public string HostUrl { get; set; }

        public string OriginalImagePath { get; set; }
        public string FuelType { get; set; }
        //  public string MaskingName { get; set; }

    }
}
