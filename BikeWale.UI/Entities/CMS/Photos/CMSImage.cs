using Bikewale.Entities.CMS.Photos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS.Photos
{
    public class CMSImage
    {
        public uint RecordCount { get; set; }
        public List<ModelImage> Images { get; set; }
    }
}
