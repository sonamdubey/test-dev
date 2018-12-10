using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CentralizedCacheRefresh;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Common;
using Carwale.Entity.Geolocation;
using System.Collections.Generic;

namespace Carwale.Interfaces
{
    public interface ICarMakesCacheRepository
    {
        List<CarMakeEntityBase> GetCarMakesFromLocalCache();
        List<CarMakeEntityBase> GetCarMakesByType(string type, Modules? module = null, bool? isPopular = null,int filter = 0);
        CarMakeDescription GetCarMakeDescription(int makeId);
        List<ValuationMake> GetValuationMakes(int Year);
        PageMetaTags GetMakePageMetaTags(int makeId, int pageId);
        CarMakeEntityBase GetCarMakeDetails(int makeId);
        List<CarMakeDescription> GetSummary(int makeId);
        List<MakeLogoEntity> GetCarMakesWithLogo(string type);
        CarMakesEntity GetMakeDetailsByName(string carMake);
        List<CarMakeEntityBase> GetMakeListWithDealerAvailable();
        List<Cities> GetAllCitiesHavingDealerByMake(int makeId);
        bool GetDealerAvailabilityForMakeCity(int make, int city);
        IEnumerable<CarMakeEntityBase> GetMakes(string makeIds="", Modules module = Modules.Default);
        bool RefreshCarMakeCache(List<MakeAttribute> makeAttributes);
	}
}
