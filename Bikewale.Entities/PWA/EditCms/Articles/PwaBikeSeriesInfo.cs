using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By : Monika Korrapati on 02 Nov 2018
    /// </summary>
    [Serializable, DataContract]
    public class PwaBikeSeriesInfo
    {
        [DataMember]
        public string DescriptionLabel { get; set; }
        [DataMember]
        public string PricePrefix { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string SeriesLinkUrl { get; set; }
        [DataMember]
        public string SeriesLinkTitle { get; set; }
    }
}
