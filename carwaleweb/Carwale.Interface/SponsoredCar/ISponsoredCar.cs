using Carwale.Entity.CarData;
using Carwale.Entity.Sponsored;
using System;
using System.Collections.Generic;

namespace Carwale.Interfaces.SponsoredCar
{
    public interface ISponsoredCar
    {
        int GetFeaturedCar(string versionIds, int categotyId, int platformId);
        List<Sponsored_Car> GetSponsoredCampaigns(int categoryId, int platformId, int categorySection, out DateTime nextCampaignStartDate, string param, int applicationId);
        List<GlobalSearchSponsoredModelEntity> GetAllSponsoredTrendingCars(int platformId);
        List<SponsoredHistoryModels> GetAllSponsoredHistoryModels(int platformId);
    }
}
