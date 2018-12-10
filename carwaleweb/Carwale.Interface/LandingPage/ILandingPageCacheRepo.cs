using Carwale.Entity;
using Carwale.Entity.Geolocation;
using Carwale.Entity.LandingPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.LandingPage
{
    public interface ILandingPageCacheRepo
    {
        Tuple<LandingPageDetails, IEnumerable<MakeModelIdsEntity>, IEnumerable<Cities>> GetLandingPageDetails(int CampaignId);
    }
}
