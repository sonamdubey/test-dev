using System;
using System.Collections.Generic;
using System.Linq;
using Carwale.Entity.CarData;
using Carwale.Entity.Sponsored;
using Carwale.Entity.CompareCars;

namespace Carwale.Interfaces.SponsoredCar
{
    public interface ISponsoredCarBL
    {
        int GetFeaturedCar(string versionIds, int categoryId, int platformId);
        GlobalSearchSponsoredModelEntity GetSponsoredTrendingCar(int platformId);
        FeaturedCarDataEntity GetFeaturedCarData(string versionIds, int categoryId, int cityId = -1);
        SponsoredHistoryModels GetAllSponsoredHistoryModels(string modelIds, int platformId);
        Sponsored_Car GetDoodle(int platformId);
    }
}
