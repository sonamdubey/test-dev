using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.DTO.Make;


namespace Bikewale.DTO.CMS.Articles
{
    public class CMSVehicleTag
    {
        public MakeBase MakeBase { get; set; }
        public ModelBase ModelBase { get; set; }
        public VersionBase VersionBase { get; set; }
    }
}
