using System.Collections.Generic;

namespace Bikewale.Entity.CMS.Photos
{

    public class CMSImage
    {
        public uint RecordCount { get; set; }
        public List<ModelImage> Images { get; set; }
    }
}
