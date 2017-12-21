using Bikewale.Entities.GenericBikes;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Modified by : Vivek Singh Tomar on 27th Sep 2017
    /// Summary : modified the type of series id from int to uint 
    /// </summary>
    [Serializable, DataContract]
    public class BikeSeriesEntityBase
    {
        public uint SeriesId { get; set; }
        public string SeriesName { get; set; }
        public string MaskingName { get; set; }
        public bool IsSeriesPageUrl { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
    }
}
