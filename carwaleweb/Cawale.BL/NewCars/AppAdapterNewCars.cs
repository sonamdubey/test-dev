using AEPLCore.Cache.Interfaces;
using Carwale.Cache.Classification;
using Carwale.DTOs.NewCars;
using Carwale.Entity;
using Carwale.Entity.Classification;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.Classification;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.NewCars;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carwale.BL.NewCars
{
    //Created By : Ashwini Todkar on 5 Aug 2015 
    //logic to get app new car landing data
    public class AppAdapterNewCars : IServiceAdapter
    {
        protected IUnityContainer container;
        public AppAdapterNewCars(IUnityContainer _container)
        {
            container = _container;     
        }
        public T Get<T>()
        {
            return (T)Convert.ChangeType(GetNewCarsData(), typeof(T));
        }

       private NewCarHome GetNewCarsData()
        {
            NewCarHome _data = null;
            try
            {
                ushort CarModelsPageSize = System.Configuration.ConfigurationManager.AppSettings["NewCarApiModelCount"] != null ? Convert.ToUInt16(System.Configuration.ConfigurationManager.AppSettings["NewCarApiModelCount"]) : (ushort)3/*Default*/;

                ICarMakesCacheRepository carMakesCache = container.Resolve<ICarMakesCacheRepository>();
                ICarModels carModelBL = container.Resolve<ICarModels>();
                ICacheManager _memcache = container.Resolve<ICacheManager>();
                ICompareCarsBL compareCarBL = container.Resolve<ICompareCarsBL>();
                IBodyTypeCache bodyTypeCache = container.Resolve<IBodyTypeCache>();

                List<BodyType> _bodyTypes = bodyTypeCache.GetBodyType();
                int appver = 0;
                int.TryParse(System.Web.HttpContext.Current.Request.Headers["appVersion"], out appver);

                _bodyTypes.ForEach(x => x.LandingUrl = "/api/NewCarSearchResult?Makes=-1&Budget=-1,-1&FuelTypes=-1&BodyTypes=" + x.Id + "&Transmission=-1&SeatingCapacity=-1&EnginePower=-1&ImportantFeatures=-1&PageNo=1&PageSize=10&SortCriteria=-1&SortOrder=-1");
                if (appver <= 91) _bodyTypes = _bodyTypes.Where(x => x.Id < 9).ToList();

                Pagination _page = new Pagination { PageNo = 1, PageSize = CarModelsPageSize, IsFromApp = true };
                
                var _upcomingList = new List<UpcomingModel>();
                var _topSellingList = new List<TopSellingCarModel>();
                var _launchList = new List<LaunchedCarModel>();
                var _makeList = new List<CarMakeLogo>();
                var _comparisonList = new List<HotCarComparison>();

                carMakesCache.GetCarMakesWithLogo("New").ForEach(item => _makeList.Add(new CarMakeLogo() { Image = new Entity.CarData.CarImageBase() { HostUrl = item.HostURL, ImagePath = item.OriginalImgPath }, Make = new MakeEntity() { MakeId = item.MakeId , MakeName = item.MakeName} }));
                carModelBL.GetLaunchedCarModels(_page).ForEach(item => _launchList.Add(new LaunchedCarModel() { Make = item.Make, Model = item.Model, Image = item.Image, Review = item.Review, Version = item.Version, City = new DTOs.City() { CityId = item.City.CityId, CityName = item.City.CityName } , Price = Format.PriceLacCr(item.Price.MinPrice.ToString()) + " - " + Format.PriceLacCr(item.Price.MaxPrice.ToString()), LaunchedDate = item.LaunchedDate }));       
                carModelBL.GetTopSellingCarModels(_page).ForEach(item => _topSellingList.Add(new TopSellingCarModel() { Make = item.Make, Model = item.Model, Image = item.Image, Review = item.Review, City = new DTOs.City() { CityId = item.City.CityId, CityName = item.City.CityName }, Price = Format.PriceLacCr(item.Price.MinPrice.ToString()) + " - " + Format.PriceLacCr(item.Price.MaxPrice.ToString()) }));
                carModelBL.GetUpcomingCarModels(_page).ForEach(item => _upcomingList.Add(new UpcomingModel() { Price = Format.PriceLacCr(item.Price.MinPrice.ToString("0.")) + " - " + Format.PriceLacCr(item.Price.MaxPrice.ToString("0.")), Image = item.Image, ExpectedLaunch = item.ExpectedLaunch, ExpectedLaunchId = item.ExpectedLaunchId, Make = new MakeEntity() { MakeName = item.MakeName }, Model = new ModelEntity() { MaskingName = item.MaskingName, ModelName = item.ModelName, ModelId = item.ModelId } }));

                Pagination _hotcompare = new Pagination { PageNo = 1, PageSize = 4 };

                compareCarBL.GetHotComaprisons(_hotcompare).ForEach(item => _comparisonList.Add(new HotCarComparison()
                {
                    Image = item.Image,
                    HotCars = GetHotCars(item.HotCars)
                }));

                _data = new NewCarHome()
                {
                    BodyType = _bodyTypes,
                    Makes = _makeList,
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

       private List<ComparisonCarModel> GetHotCars(List<Carwale.Entity.CompareCars.ComparisonCarModel> hotCars)
       {
           var _compareModel = new List<ComparisonCarModel>();
           hotCars.ForEach(x => _compareModel.Add(new ComparisonCarModel() { Make = new MakeEntity() { MakeId = x.MakeId, MakeName = x.MakeName }, Model = new ModelEntity() { MaskingName = x.MaskingName, ModelId = x.ModelId, ModelName = x.ModelName }, VersionId=x.VersionId, Price = Format.PriceLacCr(x.Price.ToString()), Review = x.Review }));
           return _compareModel;
       }


       public T Get<T>(string param)
       {
           throw new NotImplementedException();
       }
    }
}
