using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;


namespace Bikewale.DTO.CMS.Articles
{
    public class CMSVehicleTag
    {
        public MakeBase MakeBase { get; set; }
        public ModelBase ModelBase { get; set; }
        public VersionBase VersionBase { get; set; }
    }
}
