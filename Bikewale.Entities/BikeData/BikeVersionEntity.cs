using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    /// <summary>
    /// Updated By : Sangram Nandkhile on 20 Jun 2016
    /// Desc: Added ModelName, ModelMasking and Href
    /// </summary>

    [Serializable, DataContract]
    public class BikeVersionEntity : BikeVersionEntityBase
    {
        [DataMember]
        public bool New { get; set; }
        [DataMember]
        public bool Used { get; set; }
        [DataMember]
        public bool Futuristic { get; set; }
        [DataMember]
        public string BikeName { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string SmallPicUrl { get; set; }
        [DataMember]
        public string LargePicUrl { get; set; }
        [DataMember]
        public Int64 Price { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string ModelMasking { get; set; }
        [DataMember]
        public string Href { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }

        private BikeMakeEntityBase objmakeBase = new BikeMakeEntityBase();
        [DataMember]
        public BikeMakeEntityBase MakeBase { get { return objmakeBase; } set { objmakeBase = value; } }

        private BikeModelEntityBase objmodelBase = new BikeModelEntityBase();
        [DataMember]
        public BikeModelEntityBase ModelBase { get { return objmodelBase; } set { objmodelBase = value; } }
    }
}