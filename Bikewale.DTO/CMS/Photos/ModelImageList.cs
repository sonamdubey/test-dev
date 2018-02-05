using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Bikewale.DTO.BikeData;

namespace Bikewale.DTO.CMS.Photos
{
    [Serializable, DataContract]
    public class ModelImageList
    {
        [DataMember]
        public int RecordCount { get; set; }

        [DataMember]
        public string PrevPageUrl { get; set; }

        [DataMember]
        public string NextPageUrl { get; set; }

        [DataMember]
        public IEnumerable<CMSModelImages> Models { get; set; }

    }


}
