using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Photos
{
    /// <summary>
    /// written by Natesh Kumar on 25/9/14
    /// </summary>
    [Serializable]
    public class CMSImage
    {
        public uint RecordCount { get; set; }
        public List<ModelImage> Images { get; set; }
    }
}
