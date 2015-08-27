using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using System.Runtime.Serialization;

namespace Bikewale.DTO.CMS.Photos
{
    public class CMSModelImageBase
    {
        public uint ImageId { get; set; }  
        public string HostUrl { get; set; }
        public string ImagePathThumbnail { get; set; }
        public string ImagePathLarge { get; set; }
        public short MainImgCategoryId { get; set; }
        public string ImageCategory { get; set; }
        public string Caption { get; set; }
        public string ImageName { get; set; }
        public string AltImageName { get; set; }
        public string ImageTitle { get; set; }
        public string ImageDescription { get; set; }
        public MakeBase MakeBase { get; set; }
        public ModelBase ModelBase { get; set; }
        public string OriginalImgPath{ get; set; }
    }

}
