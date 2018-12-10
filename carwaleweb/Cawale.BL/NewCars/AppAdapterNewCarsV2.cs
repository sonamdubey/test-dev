using Carwale.Entity;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using Carwale.DTOs.NewCars;
using Carwale.Interfaces.NewCars;
using System;
using System.Collections.Generic;
using Carwale.Cache.Classification;
using Carwale.Entity.Classification;
using Carwale.Utility;
using System.Configuration;
using AutoMapper;
using Carwale.DTOs.Classification;
using Carwale.Interfaces.Classification;

namespace Carwale.BL.NewCars
{
    //Created By : Ashwini Todkar on 5 Aug 2015 
    //logic to get app new car landing data
    public class AppAdapterNewCarsV2 : Carwale.Interfaces.Home.IServiceAdapter
    {
        private readonly IUnityContainer _container;
        private readonly ICarMakesCacheRepository _carMakesRepo;
        private readonly ICarModels _carModels;
        private readonly ICompareCarsBL _compareCars;
        private readonly IBodyTypeCache _bodyTypeCache;

        public AppAdapterNewCarsV2( IUnityContainer container, 
                                    ICarMakesCacheRepository carMakesCache,
                                    ICarModels carModelBL,
                                    ICompareCarsBL compareCarBL,                     
                                    IBodyTypeCache bodyTypeCache)
        {
            _container = container;
            _carMakesRepo = carMakesCache;
            _carModels = carModelBL;
            _compareCars = compareCarBL;
            _bodyTypeCache = bodyTypeCache;
        }

        public T Get<T>(string cityId)
        {
            return (T)Convert.ChangeType(GetNewCarsData(int.Parse(cityId)), typeof(T));
        }

        private NewCarHomeV2 GetNewCarsData(int cityId)
        {
            NewCarHomeV2 _data = null;
            try
            {
                ushort CarModelsPageSize = ConfigurationManager.AppSettings["NewCarApiModelCount"] != null ? Convert.ToUInt16(ConfigurationManager.AppSettings["NewCarApiModelCount"]) : (ushort)3/*Default*/;

                var _bodyType= new List<BodyTypeDTO>();
                var _upcomingList = new List<UpcomingModelV2>();
                var _topSellingList = new List<TopSellingCarModelV2>();
                var _launchList = new List<LaunchedCarModelV2>();
                var _makeLogo = new List<CarMakeLogoV2>();
                var _comparisonList = new List<HotCarComparisonV2>();
               
                var bodyType = _bodyTypeCache.GetBodyType();                
                _bodyType = Mapper.Map<List<Carwale.Entity.Classification.BodyType>, List<BodyTypeDTO>>(bodyType);               
                foreach(var bodytype in _bodyType)
                     bodytype.LandingUrl = "/api/v2/NewCarSearchResult/"+cityId+"/?Makes=-1&Budget=-1,-1&FuelTypes=-1&BodyTypes="+bodytype .Id+"&Transmission=-1&SeatingCapacity=-1&EnginePower=-1&ImportantFeatures=-1&PageNo=1&PageSize=10&SortCriteria=-1&SortOrder=-1";

                Pagination _page = new Pagination { PageNo = 1, PageSize = CarModelsPageSize, IsFromApp = true };

                //upcoming models
                var upcoming = _carModels.GetUpcomingCarModels(_page);
                _upcomingList = Mapper.Map<List<Entity.CarData.UpcomingCarModel>, List<UpcomingModelV2>>(upcoming);

                //top selling models
                var topSelling = _carModels.GetTopSellingCarModels(_page, cityId);
                _topSellingList = Mapper.Map<List<Entity.CarData.TopSellingCarModel>, List<TopSellingCarModelV2>>(topSelling);

                //new launches
                var newLaunch = _carModels.GetLaunchedCarModels(_page, cityId);
                _launchList = Mapper.Map<List<Entity.CarData.LaunchedCarModel>, List<LaunchedCarModelV2>>(newLaunch);
               

                var makeLogo=_carMakesRepo.GetCarMakesWithLogo("New");
                _makeLogo = Mapper.Map<List<MakeLogoEntity>, List<CarMakeLogoV2>>(makeLogo);

                Pagination _hotcompare = new Pagination { PageNo = 1, PageSize = 4 };

                var hotComparisons = _compareCars.GetHotComaprisons(_hotcompare);
                _comparisonList = Mapper.Map<List<Carwale.Entity.CompareCars.HotCarComparison>, List<Carwale.DTOs.NewCars.HotCarComparisonV2>>(hotComparisons);

                _data = new NewCarHomeV2()
                {
                    BodyType = _bodyType,
                    Makes = _makeLogo,
                    TopCarCompare = _comparisonList,
                    RecentLaunches = _launchList,
                    TopSellingModels = _topSellingList,
                    UpcomingModels = _upcomingList
                };
            }
           catch(Exception ex)
           {
               ExceptionHandler err = new ExceptionHandler(ex, "AppAdapterNewCars");
               err.LogException();
           }

           return _data;
        }

       //private List<ComparisonCarModel> GetHotCars(List<Carwale.Entity.CompareCars.ComparisonCarModel> hotCars)
       //{
       //    var _compareModel = new List<ComparisonCarModel>();
       //    hotCars.ForEach(x => _compareModel.Add(new ComparisonCarModel() { Make = new MakeEntity() { MakeId = x.MakeId, MakeName = x.MakeName }, Model = new ModelEntity() { MaskingName = x.MaskingName, ModelId = x.ModelId, ModelName = x.ModelName }, VersionId=x.VersionId, Price = Format.PriceLacCr(x.Price.ToString()), Review = x.Review }));
       //    return _compareModel;
       //}


       public T Get<T>()
       {
           throw new NotImplementedException();
       }
    }
}
