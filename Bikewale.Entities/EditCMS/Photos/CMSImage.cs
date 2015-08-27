using Bikewale.Entity.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entity.CMS.Photos
{
    [Serializable]
    public class CMSImage
    {
        public uint RecordCount { get; set; }
        public List<ModelImage> Images { get; set; }
    }
}
