using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.ViewModels.CarData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Interfaces.CarData
{
   public interface ICarDataLogic
    {
       List<Entity.CarData.CarDataPresentation> GetCombinedCarData(List<int> versionIds);
       Entity.CarData.CarDataPresentation GetCombinedCarDataOldApp(List<int> versionIds);
       
       List<Carwale.Entity.CarData.CarModelSpecs> GetCarModelSpecs(IEnumerable<int> versionIds, int modelId);

       FeaturedCarDataEntity GetFeaturedCar(CompareCarInputParam inputVal);

       List<CarWithImageEntity> GetVersionDetails(List<int> versionList, int featuredVersionId, Location custLocation, Platform type);

       bool CheckForCampaign(List<CarWithImageEntity> carDetails, Location location);

       ModelDataSummary GetCarModelDataSummary(List<int> versionIds, int modelId);
    }
}
