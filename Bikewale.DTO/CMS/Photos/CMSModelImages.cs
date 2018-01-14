using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.CMS.Photos
{
    [Serializable, DataContract]
    public class CMSModelImages: CMSImageList
    {
        [DataMember]
        public MakeBase MakeBase { get; set; }
        [DataMember]
        public ModelBase ModelBase { get; set; }
    }
}
