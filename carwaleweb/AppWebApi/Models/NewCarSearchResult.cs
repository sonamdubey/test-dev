using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using NewCarSearch;
using System.Configuration;
using AppWebApi.Common;

namespace AppWebApi.Models
{
    public class NewCarSearchResult
    {
        private bool serverErrorOccurred = false;
        [ JsonIgnore ]  
        public bool ServerErrorOccurred
        {
            get { return serverErrorOccurred; }
            set { serverErrorOccurred = value; }
        }

        [JsonIgnore]
        public bool CarsFound
        {
            get { return carModels.Count > 0 ? true : false; }
        }

        private string nextPageUrl = "";
        [JsonProperty("nextPageUrl")]
        public string NextPageUrl 
        {
            get { return nextPageUrl; }
            set { nextPageUrl = value; }
        }

        [JsonProperty("exShowroomCity")]
        public string ExShowroomCity
        {
            get { return ConfigurationManager.AppSettings["DefaultCityName"].ToString(); }
        }

        [JsonProperty("exShowroomCityId")]
        private string ExShowroomCityId
        {
            get { return ConfigurationManager.AppSettings["DefaultCityId"].ToString(); }
        }

        public List<CarModel> carModels = new List<CarModel>();

        /* Author: Rakesh Yadav
         * Date Created: 14 June 2013
         * Discription: create list of carmodels*/
        public NewCarSearchResult(string makes, string budget, string fuelTypes, string bodyTypes, string transmission, string seatingCapacity, string enginePower, string importantFeatures, string pageNo, string pageSize, string sortCriteria, string sortOrder, string requestUri)
        {
            SearchCarModels sc = new SearchCarModels();
            //string re = HttpContext.Current.Request.Url.ParseQueryString;
            sc.Makes = makes;
            sc.Budget = budget;
            sc.FuelTypes = fuelTypes;
            sc.BodyTypes = bodyTypes;
            sc.Transmission = transmission;
            sc.SeatingCapacity = seatingCapacity;
            sc.EnginePower = enginePower;
            sc.ImportantFeatures = importantFeatures;
            sc.PageNo=pageNo;
            sc.PageSize = pageSize;
            sc.SortCriteria = sortCriteria;
            sc.SortOrder = sortOrder;
            sc.RequestUrl = requestUri;
            sc.ApiHostUrl = CommonOpn.ApiHostUrl;
            sc.ExShowroomCityId = ExShowroomCityId;
            sc.FilterCarModels();
            ServerErrorOccurred = sc.ServerErrorOccurred;
            NextPageUrl = sc.NextPageUrl;

            if (!ServerErrorOccurred) 
            {
                List<NewCarSearch.CarModel> cm = sc.CarModels;
                //Convert data from CarModels() of NewCarSearch library to CarModels() of AppWebApi Models
                carModels = cm.ConvertAll(x => new CarModel { SmallPic = x.SmallPic, LargePicUrl = x.LargePicUrl, MakeId = x.MakeId, ModelId = x.ModelId, MakeName = x.MakeName, ModelName = x.ModelName, MaxPrice = x.MaxPrice, MinPrice = x.MinPrice, CarRating = x.CarRating, CarModelUrl = x.CarModelUrl, CarPrice = x.CarPrice , HostUrl = x.HostUrl, OriginalImgPath = x.OriginalImgPath });
            }
        }
    }
}