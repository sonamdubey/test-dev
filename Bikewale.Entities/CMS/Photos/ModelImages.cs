using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Photos
{
    [Serializable, DataContract]
    public class ModelImages
    {
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public int RecordCount { get; set; }
        [DataMember]
        public IEnumerable<ModelImage> ModelImage { get; set; }
        [DataMember]
        public string BikeName { get; set; }
        [DataMember]
        public BikeMakeEntityBase MakeBase { get; set; }
        [DataMember]
        public BikeModelEntityBase ModelBase { get; set; }
    }

    [Serializable, DataContract]
    public class ModelImageWrapper
    {
        [DataMember]
        public int RecordCount { get; set; }

        [DataMember]
        public string PrevPageUrl { get; set; }

        [DataMember]
        public string NextPageUrl { get; set; }

        [DataMember]
        public IEnumerable<ModelImages> Models { get; set; }
    }

}
