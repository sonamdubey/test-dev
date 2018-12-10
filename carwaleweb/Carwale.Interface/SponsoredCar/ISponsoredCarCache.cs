using Carwale.Entity.CarData;
using Carwale.Entity.Sponsored;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.SponsoredCar
{
    public interface ISponsoredCarCache
    {
        List<Sponsored_Car> GetSponsoredCampaigns(int categoryId, int platformId, int categorySection = -1, string param = "", int applicationId = 1);
        int GetFeaturedCar(string versionIds, int categoryId, int platformId);
        List<GlobalSearchSponsoredModelEntity> GetAllSponsoredTrendingCars(int platformId);
        List<SponsoredHistoryModels> GetAllSponsoredHistoryModels(int platformId);
    }
}
