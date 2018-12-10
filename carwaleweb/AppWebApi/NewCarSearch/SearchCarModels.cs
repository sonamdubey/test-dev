using AppWebApi.Common;
using Carwale.BL.PriceQuote;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.PriceQuote;
using Carwale.Service;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NewCarSearch
{
    public class SearchCarModels
    {

        public const string MIN_PRICE = "0", MAX_PRICE = "900000000";

        /* Author: Rakesh Yadav
         * Date: Created: 14 June 2013
         * Discription: Search query properties */
        public string SelectClause { get; set; }
        public string FromClause { get; set; }
        public string OrderByClause { get; set; }
        public string WhereClause { get; set; }

        /* Author: Rakesh Yadav
         * Date: Created: 14 June 2013
         * Discription: search criteria properties */
        private string budget = "-1";
        public string Budget
        {
            get { return budget; }
            set { budget = value; }
        }
        private string makes = "-1";
        public string Makes
        {
            get { return makes; }
            set { makes = value; }
        }
        private string fuelTypes = "-1";
        public string FuelTypes
        {
            get { return fuelTypes; }
            set { fuelTypes = value; }
        }
        private string bodyTypes = "-1";
        public string BodyTypes
        {
            get { return bodyTypes; }
            set { bodyTypes = value; }
        }
        private string transmission = "-1";
        public string Transmission
        {
            get { return transmission; }
            set { transmission = value; }
        }
        private string pageNo = "1";
        public string PageNo
        {
            get { return pageNo; }
            set { pageNo = value; }
        }
        public string pageSize = "10";
        public string PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        public string StartIndex
        {
            get { return Convert.ToString(((Convert.ToInt32(PageNo) - 1) * Convert.ToInt32(PageSize)) + 1); }
        }
        public string LastIndex
        {
            get { return Convert.ToString(Convert.ToInt32(StartIndex) + Convert.ToInt32(PageSize) - 1); }
        }
        private bool serverErrorOccurred = false;
        public bool ServerErrorOccurred
        {
            get { return serverErrorOccurred; }
            set { serverErrorOccurred = value; }
        }
        private string seatingCapacity = "-1";
        public string SeatingCapacity
        {
            get { return seatingCapacity; }
            set { seatingCapacity = value; }
        }
        private string enginePower = "-1";
        public string EnginePower
        {
            get { return enginePower; }
            set { enginePower = value; }
        }
        private string importantFeatures = "-1";
        public string ImportantFeatures
        {
            get { return importantFeatures; }
            set { importantFeatures = value; }
        }
        private string sortCriteria = "2";
        public string SortCriteria
        {
            get { return sortCriteria; }
            set { sortCriteria = value; }
        }
        private string sortOrder = "1";
        public string SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }
        private string requestUrl = "";
        public string RequestUrl
        {
            get { return requestUrl; }
            set { requestUrl = value; }
        }
        private string nextPageUrl = "";
        public string NextPageUrl
        {
            get { return nextPageUrl; }
            set { nextPageUrl = value; }
        }

        private string apiHostUrl = "";
        public string ApiHostUrl
        {
            get { return apiHostUrl; }
            set { apiHostUrl = value; }
        }

        private string exShowroomCityId = "";
        public string ExShowroomCityId
        {
            get { return exShowroomCityId; }
            set { exShowroomCityId = value; }
        }

        private int CarCount { get; set; }
        protected string minBudget, maxBudget;
        public List<CarModel> CarModels = new List<CarModel>();
        public List<CarModelV2> CarModelsV2 = new List<CarModelV2>();

        public void FilterCarModels()
        {
            GetMinPriceMaxBudget();
            CarModelURI uri = new CarModelURI();
            if (!string.IsNullOrEmpty(Makes) && RegExValidations.ValidateCommaSeperatedNumbers(Makes))
            {
                uri.CarMakeIds = Makes;
            }
            if (!string.IsNullOrEmpty(FuelTypes) && RegExValidations.IsNumberInRange(FuelTypes, 1, 5))
            {
                uri.FuelTypeIds = FuelTypes;
            }
            if (!string.IsNullOrEmpty(BodyTypes) && RegExValidations.ValidateCommaSeperatedNumbers(BodyTypes))
            {
                uri.BodyStyleIds = BodyTypes;
            }
            if (!string.IsNullOrEmpty(Transmission) && RegExValidations.IsNumberInRange(Transmission, 1, 2))
            {
                uri.TransmissionTypeIds = Transmission;
            }
            if (!string.IsNullOrEmpty(SortCriteria) && SortCriteria != "-1")
            {
                uri.SortCriteria = SortCriteria;
            }
            if (!string.IsNullOrEmpty(SortOrder) && SortOrder != "-1")
            {
                uri.SortOrder = SortOrder;
            }
            if (!string.IsNullOrEmpty(minBudget) && !string.IsNullOrEmpty(maxBudget))
            {
                if (RegExValidations.ValidateCommaSeperatedNumbers(minBudget) && RegExValidations.ValidateCommaSeperatedNumbers(maxBudget))
                {
                    uri.MinPrice = minBudget;
                    uri.MaxPrice = maxBudget;
                }
            }
            uri.StartIndex = StartIndex;
            uri.LastIndex = LastIndex;

            IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();
            try
            {
                ICarModelRepository model = container.Resolve<ICarModelRepository>();
                List<CarModelDetails> modelDetails = model.GetNewCarSearchResult(uri);

                CarModel c;
                for (int i = 0; i < modelDetails.Count; i++)
                {
                    c = new CarModel();
                    c.SmallPic = modelDetails[i].ModelImageSmall;
                    c.LargePicUrl = modelDetails[i].ModelImageLarge;
                    c.MakeId = modelDetails[i].MakeId.ToString();
                    c.ModelId = modelDetails[i].ModelId.ToString();
                    c.MakeName = modelDetails[i].MakeName;
                    c.ModelName = modelDetails[i].ModelName;
                    c.MaxPrice = CommonOpn.GetPrice(modelDetails[i].MaxPrice.ToString());
                    c.MinPrice = CommonOpn.GetPrice(modelDetails[i].MinPrice.ToString());
                    c.CarRating = CommonOpn.GetAbsReviewRate(modelDetails[i].ModelRating);
                    c.CarModelUrl = RequestUrl + "&modelId=" + modelDetails[i].ModelId.ToString();
                    c.CarPrice = CommonOpn.GetPriceInLakhs(modelDetails[i].MinPrice.ToString()).Replace(" Lakh", "") + " - " + CommonOpn.GetPriceInLakhs(modelDetails[i].MaxPrice.ToString()).Replace("₹ ", "");
                    c.HostUrl = modelDetails[i].HostUrl;
                    c.OriginalImgPath = modelDetails[i].OriginalImage;
                    CarModels.Add(c);
                }
                if (modelDetails.Count > 0)
                {
                    CarCount = modelDetails[0].CarCount;
                }
                if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= CarCount)
                    NextPageUrl = "";
                else
                    NextPageUrl = ApiHostUrl + "NewCarSearchResult?Makes=" + Makes + "&Budget=" + Budget + "&FuelTypes=" + FuelTypes + "&BodyTypes=" + BodyTypes + "&Transmission=" + Transmission + "&SeatingCapacity=" + SeatingCapacity + "&EnginePower=" + EnginePower + "&ImportantFeatures=" + ImportantFeatures + "&PageNo=" + (Convert.ToInt32(PageNo) + 1) + "&PageSize=" + PageSize + "&SortCriteria=" + SortCriteria + "&SortOrder=" + SortOrder;
            }

            catch (Exception err)
            {
                ServerErrorOccurred = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        public void FilterCarModelsV2(int cityId)
        {
            GetMinPriceMaxBudget();
            CarModelURI uri = new CarModelURI();
            if (!string.IsNullOrEmpty(Makes) && RegExValidations.ValidateCommaSeperatedNumbers(Makes))
            {
                uri.CarMakeIds = Makes;
            }
            if (!string.IsNullOrEmpty(FuelTypes) && RegExValidations.IsNumberInRange(FuelTypes, 1, 5))
            {
                uri.FuelTypeIds = FuelTypes;
            }
            if (!string.IsNullOrEmpty(BodyTypes) && RegExValidations.ValidateCommaSeperatedNumbers(BodyTypes))
            {
                uri.BodyStyleIds = BodyTypes;
            }
            if (!string.IsNullOrEmpty(Transmission) && RegExValidations.IsNumberInRange(Transmission, 1, 2))
            {
                uri.TransmissionTypeIds = Transmission;
            }
            if (!string.IsNullOrEmpty(SortCriteria) && SortCriteria != "-1")
            {
                uri.SortCriteria = SortCriteria;
            }
            if (!string.IsNullOrEmpty(SortOrder) && SortOrder != "-1")
            {
                uri.SortOrder = SortOrder;
            }
            if (!string.IsNullOrEmpty(minBudget) && !string.IsNullOrEmpty(maxBudget))
            {
                if (RegExValidations.ValidateCommaSeperatedNumbers(minBudget) && RegExValidations.ValidateCommaSeperatedNumbers(maxBudget))
                {
                    uri.MinPrice = minBudget;
                    uri.MaxPrice = maxBudget;
                }
            }
            uri.StartIndex = StartIndex;
            uri.LastIndex = LastIndex;

            IUnityContainer container = UnityBootstrapper.Resolver.GetContainer();
            try
            {
                ICarModelRepository model = container.Resolve<ICarModelRepository>();
                ICarPriceQuoteAdapter _iPrice = container.Resolve<CarPriceQuoteAdapter>();
                List<CarModelDetails> modelDetails = model.GetNewCarSearchResult(uri);

                var modelList = modelDetails.Select(x => x.ModelId).ToList();
                var modelPrices = _iPrice.GetModelsCarPriceOverview(modelList, cityId);

                CarModelV2 carModelV2;
                for (int i = 0; i < modelDetails.Count; i++)
                {
                    carModelV2 = new CarModelV2();
                    carModelV2.MakeId = modelDetails[i].MakeId.ToString();
                    carModelV2.ModelId = modelDetails[i].ModelId.ToString();
                    carModelV2.MakeName = modelDetails[i].MakeName;
                    carModelV2.ModelName = modelDetails[i].ModelName;
                    carModelV2.CarRating = CommonOpn.GetAbsReviewRate(modelDetails[i].ModelRating);
                    carModelV2.CarModelUrl = ApiHostUrl + "v1/model/" + modelDetails[i].ModelId + "/?cityid=" + cityId;
                    carModelV2.HostUrl = modelDetails[i].HostUrl;
                    carModelV2.OriginalImgPath = modelDetails[i].OriginalImage;
                    if (modelPrices[modelDetails[i].ModelId] != null)
                    {
                        if (modelPrices[modelDetails[i].ModelId].PriceStatus == (int)PriceBucket.HaveUserCity)
                        {

                            carModelV2.PriceOverview = new PriceOverviewDTO
                            {
                                Price = ConfigurationManager.AppSettings["rupeeSymbol"] + Carwale.Utility.Format.PriceLacCr(modelPrices[modelDetails[i].ModelId].Price.ToString()),
                                PriceLabel = modelPrices[modelDetails[i].ModelId].PriceLabel,
                                PricePrefix = modelPrices[modelDetails[i].ModelId].PriceVersionCount > 1 ? ConfigurationManager.AppSettings["pricePrefix"] : string.Empty,
                                PriceSuffix = modelPrices[modelDetails[i].ModelId].PriceVersionCount > 1 ? ConfigurationManager.AppSettings["priceSuffix"] : string.Empty,
                                City = new Carwale.DTOs.Geolocation.CityDTO
                               {
                                   Id = modelPrices[modelDetails[i].ModelId].City != null ? modelPrices[modelDetails[i].ModelId].City.CityId : 0,
                                   Name = modelPrices[modelDetails[i].ModelId].City != null ? modelPrices[modelDetails[i].ModelId].City.CityName : ""

                               },
                                LabelColor = modelPrices[modelDetails[i].ModelId].PriceStatus == (int)PriceBucket.PriceNotAvailable ? "#c1a611 " : modelPrices[modelDetails[i].ModelId].PriceStatus == (int)PriceBucket.CarNotSold ? "#ef3f30 " : "#82888b",
                                CityColor = ConfigurationManager.AppSettings["cityColor"] != null ? ConfigurationManager.AppSettings["cityColor"] : string.Empty,
                                ReasonText = modelPrices[modelDetails[i].ModelId].ReasonText,
                                PriceStatus = modelPrices[modelDetails[i].ModelId].PriceStatus,
                                PriceForSorting=modelPrices[modelDetails[i].ModelId].Price
                            };
                        }
                        else
                        {
                            carModelV2.PriceOverview = new PriceOverviewDTO
                            {
                                Price = ConfigurationManager.AppSettings["rupeeSymbol"] + Carwale.Utility.Format.PriceLacCr(modelPrices[modelDetails[i].ModelId].Price.ToString()),
                                PriceLabel = modelPrices[modelDetails[i].ModelId].PriceLabel,
                                PricePrefix = modelPrices[modelDetails[i].ModelId].PriceVersionCount > 1 ? ConfigurationManager.AppSettings["pricePrefix"] : string.Empty,
                                PriceSuffix = modelPrices[modelDetails[i].ModelId].PriceVersionCount > 1 ? ConfigurationManager.AppSettings["priceSuffix"] : string.Empty,
                                City = new Carwale.DTOs.Geolocation.CityDTO(),
                                LabelColor = modelPrices[modelDetails[i].ModelId].PriceStatus == (int)PriceBucket.PriceNotAvailable ? "#c1a611 " : modelPrices[modelDetails[i].ModelId].PriceStatus == (int)PriceBucket.CarNotSold ? "#ef3f30 " : "#82888b",
                                CityColor = ConfigurationManager.AppSettings["cityColor"] != null ? ConfigurationManager.AppSettings["cityColor"] : string.Empty,
                                ReasonText = modelPrices[modelDetails[i].ModelId].ReasonText,
                                PriceStatus=modelPrices[modelDetails[i].ModelId].PriceStatus,
                                PriceForSorting=modelPrices[modelDetails[i].ModelId].Price
                            };
                        }
                    }
                    else
                    {
                        carModelV2.PriceOverview = new PriceOverviewDTO
                        {
                            Price = ConfigurationManager.AppSettings["rupeeSymbol"] + Carwale.Utility.Format.PriceLacCr("0"),
                            PriceLabel  = string.Empty,
                            PricePrefix = string.Empty,
                            PriceSuffix = string.Empty,
                            City = new Carwale.DTOs.Geolocation.CityDTO
                           {
                               Id =  0,
                               Name = string.Empty

                           },
                            LabelColor =string.Empty,
                            CityColor = string.Empty,
                            ReasonText = string.Empty,
                            PriceStatus=0,
                            PriceForSorting=0
                        };
                    }
                    CarModelsV2.Add(carModelV2);
                }
                CarModelsV2 = CarModelsV2.OrderBy(x => x.PriceOverview.PriceForSorting).ToList();
                if (modelDetails.Count > 0)
                {
                    CarCount = modelDetails[0].CarCount;
                }
                if ((Convert.ToInt32(PageNo) * Convert.ToInt32(PageSize)) >= CarCount)
                    NextPageUrl = "";
                else
                    NextPageUrl = ApiHostUrl + "v2/NewCarSearchResult/" + cityId + "/" + "?Makes=" + Makes + "&Budget=" + Budget + "&FuelTypes=" + FuelTypes + "&BodyTypes=" + BodyTypes + "&Transmission=" + Transmission + "&SeatingCapacity=" + SeatingCapacity + "&EnginePower=" + EnginePower + "&ImportantFeatures=" + ImportantFeatures + "&PageNo=" + (Convert.ToInt32(PageNo) + 1) + "&PageSize=" + PageSize + "&SortCriteria=" + SortCriteria + "&SortOrder=" + SortOrder;              
            }

            catch (Exception err)
            {
                ServerErrorOccurred = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Returns the Minimum budget and Maximum budget
        /// </summary>
        private void GetMinPriceMaxBudget()
        {
            if (!String.IsNullOrEmpty(Budget))
            {
                string[] _arr = Budget.Split(',');

                if (_arr.Length == 2)
                {
                    minBudget = _arr[0];
                    maxBudget = _arr[1];

                    if (minBudget == "-1")
                        minBudget = MIN_PRICE.ToString();

                    if (maxBudget == "-1")
                        maxBudget = MAX_PRICE.ToString();
                }
            }
        }
    }
}
