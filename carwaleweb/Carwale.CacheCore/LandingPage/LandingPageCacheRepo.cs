using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Entity;
using Carwale.Entity.Geolocation;
using Carwale.Entity.LandingPage;
using Carwale.Interfaces;
using Carwale.Interfaces.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Cache.LandingPage
{
    public class LandingPageCacheRepo : ILandingPageCacheRepo
    {
        private readonly ICacheManager _cacheProvider;
        private readonly ILandingPageRepository _landingPageRepo;

        public LandingPageCacheRepo(ICacheManager cacheProvider, ILandingPageRepository landingPageRepo)
        {
            _cacheProvider = cacheProvider;
            _landingPageRepo = landingPageRepo;
        }

        public Tuple<LandingPageDetails, IEnumerable<MakeModelIdsEntity>, IEnumerable<Cities>> GetLandingPageDetails(int campaignId)
        {
            return _cacheProvider.GetFromCache<Tuple<LandingPageDetails, IEnumerable<MakeModelIdsEntity>, IEnumerable<Cities>>>(String.Format("test-drive-details-on-campaignId-{0}", campaignId), CacheRefreshTime.NeverExpire(),
                    () => _landingPageRepo.GetLandingPageDetails(campaignId));
        }
    }
}
