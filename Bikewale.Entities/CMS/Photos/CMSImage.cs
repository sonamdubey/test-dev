using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS.Photos
{
    [Serializable]
    public class CMSImage
    {
        [DataMember]
        public uint RecordCount { get; set; }
        [DataMember]
        public List<ModelImage> Images { get; set; }
    }
}
