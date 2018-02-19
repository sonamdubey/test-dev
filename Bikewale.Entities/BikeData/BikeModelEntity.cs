using Bikewale.Entities.SEO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{

    /// <summary>
    /// Modified by : Ashutosh Sharma on 30 Aug 2017 
    /// Description : Removed IsGstPrice property
    /// Modified by : Vivek Singh Tomar on 10th Nov 2017
    /// Description : Added ReviewRateStar to hold review rate
    /// Modified by : Rajan Chauhan on 06 Feb 2018
    /// Description : Added NewsCount property
    /// </summary>
    [Serializable, DataContract]
    public class BikeModelEntity : BikeModelEntityBase
    {
        [DataMember]
        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        [DataMember]
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }
        [DataMember]
        private BikeSeriesEntityBase objEntityBase = new BikeSeriesEntityBase();
        [DataMember]
        public BikeSeriesEntityBase ModelSeries { get { return objEntityBase; } set { objEntityBase = value; } }
        [DataMember]
        public bool New { get; set; }   //Added by suresh prajapati
        [DataMember]
        public bool Used { get; set; }  //Added by suresh prajapati
        [DataMember]
        public bool Futuristic { get; set; }
        [DataMember]
        public string SmallPicUrl { get; set; }
        [DataMember]
        public string LargePicUrl { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public Int64 MinPrice { get; set; }
        [DataMember]
        public Int64 MaxPrice { get; set; }
        [DataMember]
        public double ReviewRate { get; set; }
        [DataMember]
        public string ReviewUIRating { get; set; }
        [DataMember]
        public int ReviewCount { get; set; }
        [DataMember]
        public int RatingCount { get; set; } //Added by Sajal Gupta on 20-05-2017
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public int PhotosCount { get; set; }  //Added by Aditi Srivastava
        [DataMember]
        public int VideosCount { get; set; }  //Added by Aditi Srivastava
        [DataMember]
        public uint UsedListingsCnt { get; set; }
        [DataMember]
        public IEnumerable<CustomPageMetas> Metas { get; set; }
        [DataMember]
        public byte ReviewRateStar { get; set; }
        [DataMember]
        public uint ExpertReviewsCount { get; set; }
        [DataMember]
        public uint NewsCount { get; set; }
    }
}
