using Carwale.BL.GeoLocation;
using AEPLCore.Cache;
using Carwale.Cache.Geolocation;
using Carwale.DAL.Geolocation;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified.PopularUsedCars;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Interfaces.Classified.PopularUsedCarsDetails;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Microsoft.Practices.Unity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.DTOs.Classified.PopularUC;
using Carwale.Utility;

namespace Carwale.BL.Classified.PopularUsedCars
{
    public class PopularUCDetails : IPopularUCDetails
    {
        private readonly IPQGeoLocationBL _pqGeoLocation;

        public PopularUCDetails(IPQGeoLocationBL pqGeoLocation)
        {
            _pqGeoLocation = pqGeoLocation;
        }

        public List<T> GetPopularUsedCarDetails<T>(string cityId)
        {
            if (string.IsNullOrWhiteSpace(cityId)) cityId = "-1";
            List<T> t = default (List<T>);

            City city = FetchCityById(cityId);            

            List<PopularUsedCarModel> popularUCData = FillPopularUsedCarDetails(city);

            if (typeof(T) == typeof(PopularUCModelDesktop))
                t = (List<T>)Convert.ChangeType(GetPopularUCDataDesktop(popularUCData), typeof(List<T>));

            else if (typeof(T) == typeof(PopularUCModelApp))
                t = (List<T>)Convert.ChangeType(GetPopularUCDataApp(popularUCData), typeof(List<T>));
            
            return t;
        }

        public City FetchCityById(string cityId)
        {
            City city = new City() { CityName = "" };
            try
            {
                city = _pqGeoLocation.GetCityById(Convert.ToInt32(cityId));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Classified.PopularUsedCars.GetPopularUsedCarDetails(id)");
                objErr.LogException();
            }
            return city;
        }

        private List<PopularUCModelApp> GetPopularUCDataApp(List<PopularUsedCarModel> popularUCData)
        {
            Mapper.CreateMap<PopularUsedCarModel, PopularUCModelApp>();
            List<PopularUCModelApp> appModelData = Mapper.Map<List<PopularUsedCarModel>, List<PopularUCModelApp>>(popularUCData);
            return appModelData;
        }

        private List<PopularUCModelDesktop> GetPopularUCDataDesktop(List<PopularUsedCarModel> popularUCData)
        {
            Mapper.CreateMap<PopularUsedCarModel, PopularUCModelDesktop>();
            List<PopularUCModelDesktop> desktopModelData = Mapper.Map<List<PopularUsedCarModel>, List<PopularUCModelDesktop>>(popularUCData);
            return desktopModelData;
        }

        #region Function to Get Popular Used Car Details
        /// <summary>
        /// Created By : Sachin Shukla on 28 July 2015
        /// </summary>
        /// <returns></returns>
        public List<PopularUsedCarModel> FillPopularUsedCarDetails(City city)
        {
            List<PopularUsedCarModel> popularUCData = new List<PopularUsedCarModel>();
            string hosturl = ConfigurationManager.AppSettings["HostUrl"];

            if (city.CityName == "" || city.CityName == null)
            {
                popularUCData.Add(PopulateDetails("Maruti Suzuki Swift", "hatchback", "3 lakhs", "/0x0/design15/d/swift.jpg?q=85&v=2", "/used/marutisuzuki-swift-cars/", "/used/cars-for-sale/?bodytype=3&budget=0-6", "/webapi/classified/stock/?car=10.288", "/webapi/Classified/filters/?kms=0-&year=0-&bodytype=3"));
                popularUCData.Add(PopulateDetails("Hyundai Verna", "sedan", "8 lakhs", "/0x0/design15/d/verna.jpg?q=85", "/used/hyundai-verna-2011-2015-cars/", "/used/cars-for-sale/?bodytype=1&budget=6-12", "/webapi/classified/stock/?car=8.307", "/webapi/Classified/filters/?kms=0-&year=0-&bodytype=1"));
                popularUCData.Add(PopulateDetails("Mercedez-Benz C-Class", "luxury car", "12 lakhs", "/0x0/design15/d/c_class.jpg?q=85", "/used/mercedesbenz-c-class-2011-2014-cars/", "/used/cars-for-sale/?budget=12-", "/webapi/classified/stock/?car=11.65", "/webapi/Classified/filters/?kms=0-&year=0-&budget=12-"));
            }
            else
            {
                popularUCData.Add(PopulateDetails("Maruti Suzuki Swift", "hatchback", "3 lakhs", "/0x0/design15/d/swift.jpg?q=85&v=2", "/used/marutisuzuki-swift-cars-in-" + Format.RemoveSpecialCharacters(city.CityName.ToLower()) + "/", "/used/cars-in-" + Format.RemoveSpecialCharacters(city.CityName.ToLower()) + "/?bodytype=3&budget=0-6", "/webapi/classified/stock/?car=10.288&city=" + city.CityId, "/webapi/Classified/stock/?kms=0-&year=0-&city=" + city.CityId + "&bodytype=3"));
                popularUCData.Add(PopulateDetails("Hyundai Verna", "sedan", "8 lakhs", "/0x0/design15/d/verna.jpg?q=85", "/used/hyundai-verna-2011-2015-cars-in-" + Format.RemoveSpecialCharacters(city.CityName.ToLower()) + "/", "/used/cars-in-" + Format.RemoveSpecialCharacters(city.CityName.ToLower()) + "/?bodytype=1&budget=6-12", "/webapi/classified/stock/?car=8.307&city=" + city.CityId, "/webapi/Classified/stock/?kms=0-&year=0-&city=" + city.CityId + "&bodytype=1"));
                popularUCData.Add(PopulateDetails("Mercedez-Benz C-Class", "luxury car", "12 lakhs", "/0x0/design15/d/c_class.jpg?q=85", "/used/mercedesbenz-c-class-2011-2014-cars-in-" + Format.RemoveSpecialCharacters(city.CityName.ToLower()) + "/", "/used/cars-in-" + Format.RemoveSpecialCharacters(city.CityName.ToLower()) + "/?budget=12-", "/webapi/classified/stock/?car=11.65&city=" + city.CityId, "/webapi/Classified/stock/?kms=0-&year=0-&city=" + city.CityId + "&budget=12-"));
            }

            return popularUCData;
        }

        PopularUsedCarModel PopulateDetails(string carName, string bodyType, string price, string imagePath, string titleUrl, string viewSimilarUrl, string appTitleUrl, string appSimilarUrl)
        {
            PopularUsedCarModel objModel = new PopularUsedCarModel();
            try
            {
                objModel.carName = carName;
                objModel.bodyType = bodyType;
                objModel.price = price;
                objModel.Image = new CarImageBase()
                {
                    HostUrl = "https://imgd.aeplcdn.com",
                    ImagePath = imagePath
                };
                objModel.titleUrl = titleUrl;
                objModel.viewSimilarUrl = viewSimilarUrl;
                objModel.appTitleUrl = appTitleUrl;
                objModel.appSimilarUrl = appSimilarUrl;
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Carwale.BL.Classified.PopularUsedCars.GetPopularUsedCarDetails()");
                objErr.LogException();
            }
            return objModel;
        }
        #endregion

        public object appModelData { get; set; }
    }
}
