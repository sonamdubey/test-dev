using System.Collections.Generic;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity;
using Carwale.Entity.Geolocation;

namespace Carwale.Interfaces
{
    public interface ICarMakesRepository
    {
        List<CarMakeEntityBase> GetCarMakesByType(string type, Modules? module = null, bool? isPopular = null,int filter = 0,bool isCriticalRead = false);
        CarMakeDescription GetCarMakeDescription(int makeId,bool isCriticalRead = false);
        List<ValuationMake> GetValuationMakes(int Year,bool isCriticalRead = false);
        PageMetaTags GetMakePageMetaTags(int makeId, int pageId, bool isCriticalRead = false);
        CarMakeEntityBase GetCarMakeDetails(int makeId,bool isCriticalRead = false);
        List<CarMakeDescription> GetSummary(int makeId,bool isCriticalRead = false);
        List<MakeLogoEntity> GetCarMakesWithLogo(string type,bool isCriticalRead = false);
        CarMakesEntity GetMakeDetailsByName(string carMake,bool isCriticalRead = false);
        List<CarMakeEntityBase> GetMakeListWithAvailableDealer(bool isCriticalRead = false);
        List<Cities> GetAllCitiesHavingDealerByMake(int makeId,bool isCriticalRead = false);
        bool GetDealerAvailabilityForMakeCity(int make, int city,bool isCriticalRead = false);
        IEnumerable<CarMakeEntityBase> GetMakes(Modules module, string makeIds,bool isCriticalRead = false);
    }
}

