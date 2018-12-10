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
    /// <summary>
    /// Created by :  Sachin Bharti on 11th Aug 2016
    /// Purpose : Send price based on userId
    /// </summary>
    public class NewCarSearchResultV2
    {
        private bool serverErrorOccurred = false;
        [JsonIgnore]
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

       


        [JsonProperty("carModels")]
        public List<CarModelV2> carModels = new List<CarModelV2>();

        /// <summary>
        /// Created By  :   Sachin Bharti(9th Aug 2016)
        /// Purpose :   Created new version of new car searsh result for user city price
        /// </summary>
        /// <param name="makes"></param>
        /// <param name="budget"></param>
        /// <param name="fuelTypes"></param>
        /// <param name="bodyTypes"></param>
        /// <param name="transmission"></param>
        /// <param name="seatingCapacity"></param>
        /// <param name="enginePower"></param>
        /// <param name="importantFeatures"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortCriteria"></param>
        /// <param name="sortOrder"></param>
        public NewCarSearchResultV2(int cityId, string makes, string budget, string fuelTypes, string bodyTypes, string transmission, string seatingCapacity, string enginePower, string importantFeatures, string pageNo, string pageSize, string sortCriteria, string sortOrder)
        {
            SearchCarModels sc = new SearchCarModels();
            sc.Makes = makes;
            sc.Budget = budget;
            sc.FuelTypes = fuelTypes;
            sc.BodyTypes = bodyTypes;
            sc.Transmission = transmission;
            sc.SeatingCapacity = seatingCapacity;
            sc.EnginePower = enginePower;
            sc.ImportantFeatures = importantFeatures;
            sc.PageNo = pageNo;
            sc.PageSize = pageSize;
            sc.SortCriteria = sortCriteria;
            sc.SortOrder = sortOrder;
            sc.ApiHostUrl = CommonOpn.ApiHostUrl;
            sc.FilterCarModelsV2(cityId);
            ServerErrorOccurred = sc.ServerErrorOccurred;
            NextPageUrl = sc.NextPageUrl;

            if (!ServerErrorOccurred)
            {
                List<NewCarSearch.CarModelV2> cm = sc.CarModelsV2;
                carModels = cm.ConvertAll(x => new CarModelV2 { MakeId = x.MakeId, ModelId = x.ModelId, MakeName = x.MakeName, ModelName = x.ModelName, CarRating = x.CarRating, CarModelUrl = x.CarModelUrl, HostUrl = x.HostUrl, OriginalImgPath = x.OriginalImgPath, PriceOverview = x.PriceOverview });
            }
        }
    }
}