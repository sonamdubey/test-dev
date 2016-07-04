using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 5th Aug 2014
    /// </summary>
    [Serializable, DataContract]
    public class SimilarBikeEntity : MinSpecsEntity
    {
        [DataMember]
        public int MinPrice { get; set; }
        [DataMember]
        public int MaxPrice { get; set; }
        [DataMember]
        public int VersionPrice { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string SmallPicUrl { get; set; }
        [DataMember]
        public string LargePicUrl { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public Double ReviewRate { get; set; }
        [DataMember]
        public UInt16 ReviewCount { get; set; }
        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        [DataMember]
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }
        private BikeModelEntityBase objModelBase = new BikeModelEntityBase();
        [DataMember]
        public BikeModelEntityBase ModelBase { get { return objModelBase; } set { objModelBase = value; } }

        private BikeVersionEntityBase objDesc = new BikeVersionEntityBase();
        [DataMember]
        public BikeVersionEntityBase VersionBase { get { return objDesc; } set { objDesc = value; } }
    }
}
