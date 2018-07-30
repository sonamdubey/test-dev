using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.DTO.CMS.Photos
{
    [Serializable, DataContract]
    public class CMSModelImages : CMSImageList
    {
        [DataMember]
        public MakeBase MakeBase { get; set; }
        [DataMember]
        public ModelBase ModelBase { get; set; }
    }

}
