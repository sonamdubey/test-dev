using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
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
        public int ReviewCount { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public int PhotosCount { get; set; }  //Added by Aditi Srivastava
        [DataMember]
        public int VideosCount { get; set; }  //Added by Aditi Srivastava
        
    }
}
