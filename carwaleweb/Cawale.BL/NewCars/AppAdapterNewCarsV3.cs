using AEPLCore.Cache.Interfaces;
using AutoMapper;
using Carwale.Cache.Classification;
using Carwale.DTOs.Classification;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classification;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Collections.Generic;

namespace Carwale.BL.NewCars
{
    //Created By : Vishvaraj on 5 Jan 2018 
    //logic to get app new car landing data
    public class AppAdapterNewCarsV3 : IServiceAdapterV2
    {
        private readonly ICarMakesCacheRepository _carMakesRepo;
        private readonly ICarModels _carModels;
        private readonly ICompareCarsBL _compareCars; 
        private readonly IBodyTypeCache _bodyTypeCache;
        private static Pagination hotcompare = new Pagination { PageNo = 1, PageSize = 4 };
        private readonly Pagination page = new Pagination { PageNo = 1, PageSize = CWConfiguration.newCarApiModelCount };

        public AppAdapterNewCarsV3(ICarMakesCacheRepository carMakesCache, ICarModels carModelBL, ICompareCarsBL compareCarBL, ICacheManager memcache, IBodyTypeCache bodyTypeCache)
        {
            _carMakesRepo = carMakesCache;
            _carModels = carModelBL;
            _compareCars = compareCarBL;
            _bodyTypeCache = bodyTypeCache;
        }

        public T Get<T,U>(U input)
        {
            return (T)Convert.ChangeType(GetNewCarsData((int)Convert.ChangeType(input, typeof(U))), typeof(T));
        }

        private NewCarHomeV3 GetNewCarsData(int cityId)
        {
            NewCarHomeV3 data = null;
            try
            {
                var bodyType = _bodyTypeCache.GetBodyType();
                List<BodyTypeDTO> bodyTypeList = Mapper.Map<List<Entity.Classification.BodyType>, List<BodyTypeDTO>>(bodyType);
                foreach (var bodytype in bodyTypeList)
                {                  
                    bodytype.LandingUrl = string.Format("/api/newcars/search/models/v1/?bodyStyleIds={1}&pageNo=1&pageSize=10&countOnly=false&cityId={0}", cityId, bodytype.Id);
                }
           
                var upcoming = _carModels.GetUpcomingCarModels(page);
                List<UpcomingModelV2> upcomingList = Mapper.Map<List<Entity.CarData.UpcomingCarModel>, List<UpcomingModelV2>>(upcoming);
          
                var topSelling = _carModels.GetTopSellingCarModels(page, cityId,true);
                List<TopSellingCarModelV2> topSellingList = Mapper.Map<List<Entity.CarData.TopSellingCarModel>, List<TopSellingCarModelV2>>(topSelling);
            
                var newLaunch = _carModels.GetLaunchedCarModels(page, cityId,true);
                List<LaunchedCarModelV2> launchList = Mapper.Map<List<Entity.CarData.LaunchedCarModel>, List<LaunchedCarModelV2>>(newLaunch);              

                var makeLogo=_carMakesRepo.GetCarMakesWithLogo("New");
                List<CarMakeLogoV2> makeLogoList = Mapper.Map<List<MakeLogoEntity>, List<CarMakeLogoV2>>(makeLogo);

                var hotComparisons = _compareCars.GetHotComaprisons(hotcompare, cityId, true);
                List<HotCarComparisonV2> comparisonList = Mapper.Map<List<Entity.CompareCars.HotCarComparison>, List<HotCarComparisonV2>>(hotComparisons);

                data = new NewCarHomeV3
                {
                    BodyType = bodyTypeList,
                    Makes = makeLogoList,
                    TopCarCompare = comparisonList,
                    RecentLaunches = launchList,
                    TopSellingModels = topSellingList,
                    UpcomingModels = upcomingList,
                    OrpText = cityId <1 ? CWConfiguration.orpText : string.Empty
                };
            }
           catch(Exception ex)
           {
                Logger.LogException(ex);
           }
           return data;
        }
    }
}
