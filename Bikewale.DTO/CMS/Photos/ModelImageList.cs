using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Bikewale.DTO.BikeData;

namespace Bikewale.DTO.CMS.Photos
{
    [Serializable, DataContract]
    public class ModelImageList : BikeMakeModel
    {
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public int RecordCount { get; set; }
        [DataMember]
        public IEnumerable<CMSModelImageBase> ModelImage { get; set; }
        [DataMember]
        public string BikeName { get; set; }
    }
}
