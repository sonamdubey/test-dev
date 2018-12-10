using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CompareCars
{
    [Serializable]
    public class CompareCarsDetails
    {
        public List<CompareCarOverview> CompareCars { get; set; }
        public string NextPageUrl { get; set; }
    }

    [Serializable]
    public class CompareCarOverview
    {
        public CompareCarVersionInfo Car1 { get; set; }
        public CompareCarVersionInfo Car2 { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImgPath { get; set; }
        public string DetailsUrl { get; set; }
        public bool IsSponsored { get; set; }
    }

    [Serializable]
    public class CompareCarVersionInfo
    {
        public int VersionId { get; set; }
        public string CarName { get; set; }
        public int ModelId { get; set; }
    }
}
