using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CentralizedCacheRefresh
{
    public class CacheRefreshWrapper
    {
        public List<MakeAttribute> MakeAttribute { get; set; }
        public List<ModelAttribute> ModelAttribute { get; set; }
        public List<VersionAttribute> VersionAttribute { get; set; }
        public List<ModelRootAttribute> ModelRootAttribute { get; set; }
        public NewCarFinderAttribute NewCarFinderAttribute { get; set; }
    }
}
