using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Photos
{
    public class CMSImage
    {
        [DataMember]
        public uint RecordCount { get; set; }
        [DataMember]
        public List<ModelImage> Images { get; set; }
    }
}
