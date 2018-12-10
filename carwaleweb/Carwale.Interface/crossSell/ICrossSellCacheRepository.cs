using Carwale.Entity.CrossSell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CrossSell
{
    public interface ICrossSellCacheRepository
    {
        void StoreHouseCrossSellVersions(string cacheKey, List<int> featuredVersions);
        void StoreCrossSellCampaign(string cacheKey, List<FeaturedVersion> paidCrossSellCampaign);
    }
}
