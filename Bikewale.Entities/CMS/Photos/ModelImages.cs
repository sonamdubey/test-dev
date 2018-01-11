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
    }
}
