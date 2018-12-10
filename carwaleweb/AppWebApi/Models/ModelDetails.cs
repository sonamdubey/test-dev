using AppWebApi.Common;
using Carwale.DTOs.NewCars;
using Carwale.Interfaces.NewCars;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
namespace AppWebApi.Models
{
    public class ModelDetails : IDisposable
    {
        /* Author: Rakesh Yadav
           Date Created: 14 June 2013 
         */
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
            get { return modelVersions.Count > 0 ? true : false; }
        }

        public List<Color> modelColors = new List<Color>();
        // public List<ModelVersion> modelVersions = new List<ModelVersion>();
        /* Author: Rakesh Yadav
           Date Created: 24 June 2013
           Desc: create properties of model details
        */

        [JsonProperty("offerExists")]
        private bool offerExists = false;
        private string largPic = "";
        [JsonProperty("largePicUrl")]
        public string LargePic
        {
            get { return largPic; }
            set { largPic = value; }
        }
        private string smallPic = "";
        [JsonProperty("smallPicUrl")]
        public string SmallPic
        {
            get { return smallPic; }
            set { smallPic = value; }
        }
        private string makeName = "";
        [JsonProperty("makeName")]
        public string MakeName
        {
            get { return makeName; }
            set { makeName = value; }
        }
        private string modelName = "";
        [JsonProperty("modelName")]
        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }
        private string reviewRate = "";
        [JsonProperty("reviewRate")]
        public string ReviewRate
        {
            get { return reviewRate; }
            set { reviewRate = value; }
        }
        private string reviewCount = "";
        [JsonProperty("reviewCount")]
        public string ReviewCount
        {
            get { return reviewCount; }
            set { reviewCount = value; }
        }
        private string minPrice = "";
        [JsonProperty("minPrice")]
        public string MinPrice
        {
            get { return minPrice; }
            set { minPrice = value; }
        }
        private string maxPrice = "";
        [JsonProperty("maxPrice")]
        public string MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }

        private string reviewUrl = "";
        [JsonProperty("reviewUrl")]
        public string ReviewUrl
        {
            get { return reviewUrl; }
            set { reviewUrl = value; }
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

        private string onRoadPriceVersionCityUrl = "";
        [JsonProperty("onRoadPriceVersionCityUrl")]
        public string OnRoadPriceVersionCityUrl
        {
            get { return onRoadPriceVersionCityUrl; }
            set { onRoadPriceVersionCityUrl = value; }
        }

        private string newCarPhotoUrl = "";
        [JsonProperty("newCarPhotoUrl")]
        public string NewCarPhotoUrl
        {
            get { return newCarPhotoUrl; }
            set { newCarPhotoUrl = value; }
        }

        private string newCarGalleryUrl = "";
        [JsonProperty("newCarGalleryUrl")]
        public string NewCarGalleryUrl
        {
            get { return newCarGalleryUrl; }
            set { newCarGalleryUrl = value; }
        }

        //Added by supriya on 11/6/2014
        private string shareUrl = "";
        [JsonProperty("shareUrl")]
        public string ShareUrl
        {
            get { return shareUrl; }
            set { shareUrl = value; }
        }

        [JsonProperty("tinyShareUrl")]
        public string TinyShareUrl { get; set; }

        private string callSlugNumber = "";
        [JsonProperty("callSlugNumber")]
        public string CallSlugNumber
        {
            get { return callSlugNumber; }
            set { callSlugNumber = value; }
        }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        public List<ModelVersion> modelVersions = new List<ModelVersion>();
        public List<string> videoUrl = new List<string>();
        public List<ModelVideoList> modelVideoList = new List<ModelVideoList>();
        public List<CarDetail> alternativeCars = new List<CarDetail>();

        ModelVersion cv;
        private string SimilarModels { get; set; }
        private DataSet dsVideo = new DataSet();
        protected string Budget;
        public const string MIN_PRICE = "0", MAX_PRICE = "900000000";
       

        ModelPageDTO_Android modelDTO;
        public ModelDetails(IUnityContainer container,string budget, string fuelTypes, string bodyTypes, string transmission, string seatingCapacity, string enginePower, string importantFeatures, string modelId)
        {

            //IUnityContainer container = new UnityContainer();

           // container.RegisterType<IServiceAdapter, ModelPageAdapterAndroid>();
            container.RegisterInstance<int>(Convert.ToInt16(modelId));

            IServiceAdapter modelPageAdapter = container.Resolve<IServiceAdapter>("ModelPageAndroid");
            modelDTO = modelPageAdapter.Get<ModelPageDTO_Android>();
            try
            {
                /*********************Model details***************************/
                offerExists = modelDTO.ModelDetails.OfferExists;
                MakeName = modelDTO.ModelDetails.MakeName;
                ModelName = modelDTO.ModelDetails.ModelName;
                LargePic = modelDTO.ModelDetails.ModelImageLarge;
                SmallPic = modelDTO.ModelDetails.ModelImageSmall;
                ReviewRate = CommonOpn.GetAbsReviewRate(Convert.ToDouble(modelDTO.ModelDetails.ModelRating.ToString()));
                ReviewCount = modelDTO.ModelDetails.ReviewCount.ToString();
                MinPrice = modelDTO.ModelDetails.MinPrice.ToString() == "0" ? "" : CommonOpn.GetPrice(modelDTO.ModelDetails.MinPrice.ToString());
                MaxPrice = modelDTO.ModelDetails.MaxPrice.ToString() == "0" ? "" : CommonOpn.GetPrice(modelDTO.ModelDetails.MaxPrice.ToString());
                CallSlugNumber = modelDTO.CallSlugNumber;
                HostUrl = modelDTO.ModelDetails.HostUrl;
                OriginalImgPath = modelDTO.ModelDetails.OriginalImage;
 
                if (!string.IsNullOrEmpty(MakeName))
                    ShareUrl = "https://www.carwale.com/" + CommonOpn.FormatSpecial(MakeName) + "-cars/" + modelDTO.ModelDetails.MaskingName + "/";

                /**********************Model Colors******************************/
                Color cmc;
                for (int i = 0; i < modelDTO.ModelColors.Count; i++)
                {
                    cmc = new Color();
                    cmc.Code = modelDTO.ModelColors[i].HexCode;
                    cmc.Name = modelDTO.ModelColors[i].Color;
                    modelColors.Add(cmc);
                }

                /******************Model VideosList******************************/
                ModelVideoList mvl;
                for (int i = 0; i < modelDTO.ModelVideos.Count; i++)
                {
                    mvl = new ModelVideoList();
                    mvl.VideoUrl = modelDTO.ModelVideos[i].VideoUrl;
                    mvl.VideoTitle = modelDTO.ModelVideos[i].VideoTitle;
                    modelVideoList.Add(mvl);
                }

                /******************Model Videos URL*******************************/
                for (int i = 0; i < modelDTO.ModelVideos.Count; i++)
                {
                    videoUrl.Add(modelDTO.ModelVideos[i].VideoUrl.ToString());
                }

                /*****************Similar Cars(Alternate Cars)**********************/
                CarDetail cd;
                for (int i = 0; i < modelDTO.SimilarCars.Count; i++)
                {
                    cd = new CarDetail();
                    cd.Make = modelDTO.SimilarCars[i].MakeName;
                    cd.Model = modelDTO.SimilarCars[i].ModelName;
                    cd.LargePicUrl = modelDTO.SimilarCars[i].LargePic;
                    cd.SmallPicUrl = modelDTO.SimilarCars[i].SmallPic;
                    cd.MinPrice = modelDTO.SimilarCars[i].MinPrice.ToString() == "0" ? "" : CommonOpn.GetPrice(modelDTO.SimilarCars[i].MinPrice.ToString());
                    cd.ReviewRate = CommonOpn.GetAbsReviewRate(Convert.ToDouble(modelDTO.SimilarCars[i].ReviewRate));
                    cd.ReviewCount = modelDTO.SimilarCars[i].ReviewCount.ToString();
                    cd.VersionId = modelDTO.SimilarCars[i].PopularVersionId.ToString();
                    cd.CarModelUrl = CommonOpn.ApiHostUrl + "modeldetails/?budget=-1&fuelTypes=-1&bodyTypes=-1&transmission=-1&seatingCapacity=-1&enginePower=-1&importantFeatures=-1&modelId=" + modelDTO.SimilarCars[i].ModelId;
                    cd.HostUrl = modelDTO.SimilarCars[i].HostUrl;
                    cd.OriginalImgPath = modelDTO.SimilarCars[i].ModelImageOriginal;
                    cd.ExShowRoomCityId = modelDTO.SimilarCars[i].ExShowRoomCityId;
                    cd.ExShowRoomCityName = modelDTO.SimilarCars[i].ExShowRoomCityName;
                    alternativeCars.Add(cd);
                }

                Budget = budget;

                /*************************Model VersionsList***********************/
                for (int i = 0; i < modelDTO.NewCarVersions.Count; i++)
                {
                    cv = new ModelVersion();
                    cv.Name = modelDTO.NewCarVersions[i].Version;

                    if (!string.IsNullOrEmpty(modelDTO.NewCarVersions[i].MinPriceNew.ToString() == "0" ? "" : modelDTO.NewCarVersions[i].MinPriceNew.ToString()))
                    {
                        cv.ExShowroomPrice = (modelDTO.NewCarVersions[i].MinPriceNew.ToString() == "0") ? "N/A" : modelDTO.NewCarVersions[i].MinPriceNew.ToString();
                    }
                    cv.Features = modelDTO.NewCarVersions[i].SpecsSummary;
                    cv.FuelType = modelDTO.NewCarVersions[i].CarFuelType;
                    cv.VersionUrl = CommonOpn.ApiHostUrl + "VersionDetails?versionId=" + modelDTO.NewCarVersions[i].Id.ToString();
                    cv.VersionId = modelDTO.NewCarVersions[i].Id.ToString();
                    cv.ExShowRoomCityId = modelDTO.NewCarVersions[i].ExShowRoomCityId;
                    cv.ExShowRoomCityName = modelDTO.NewCarVersions[i].ExShowRoomCityName.ToString();
                    if (MatchingCriteria(fuelTypes, modelDTO.NewCarVersions[i].FuelTypeId.ToString()) && MatchingCriteria(transmission, modelDTO.NewCarVersions[i].TransmissionTypeId.ToString()) && MatchingCriteria(bodyTypes, modelDTO.NewCarVersions[i].BodyStyleId.ToString()) && MatchBudget(modelDTO.NewCarVersions[i].MinPrice.ToString() == "0" ? "" : modelDTO.NewCarVersions[i].MinPrice.ToString()))
                        cv.IsMatchingCriteria = true;
                    else
                        cv.IsMatchingCriteria = false;
                    modelVersions.Add(cv);
                }

                if (modelId != null && modelId != "" && modelId != "-1")
                {
                    OnRoadPriceVersionCityUrl = CommonOpn.ApiHostUrl + "PqVersionsAndCities/?modelId=" + modelId;
                    ReviewUrl = CommonOpn.ApiHostUrl + "UserReviews/?modelId=" + modelId + "&versionId=-1&pageNo=1&pageSize=10&sortCriteria=1";
                    NewCarPhotoUrl = CommonOpn.ApiHostUrl + "NewCarPhotos/?modelId=" + modelId + "&categoryId=-1";
                    NewCarGalleryUrl = CommonOpn.ApiHostUrl.Replace("/api/","/webapi/") + "CarModeldata/Gallery/?applicationid=1&modelid=" + modelId + "&categoryidlist=8,10&totalrecords=500";
                }
            }
            catch (Exception err)
            {
                ServerErrorOccurred = true;
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        /* Author: Rakesh Yadav
           * Date Created: 19 June 2013
           * Discription: Check if FuelType, Transmission and BodyTypes are matching user criteria */
        public bool MatchingCriteria(string SearchCriteriaSelected, string VersionProperty)
        {
            if (SearchCriteriaSelected == "-1")
                return true;

            if (SearchCriteriaSelected.Split(',').ToList<string>().Contains(VersionProperty))
                return true;
            else
                return false;
        }

        /* Author: Rakesh Yadav
      * Date Created: 19 June 2013
      * Discription: Check if budget is matching user criteria */
        private bool MatchBudget(string price)
        {
            if (Budget == "-1")
                return true;

            string minBudget = "", maxBudget = "";
            string[] _arr = Budget.Split(',');

            if (_arr.Length == 2)
            {
                minBudget = _arr[0];
                maxBudget = _arr[1];

                if (minBudget == "-1")
                    minBudget = MIN_PRICE.ToString();

                if (maxBudget == "-1")
                    maxBudget = MAX_PRICE.ToString();

                if (!string.IsNullOrEmpty(price))
                {
                    if (Convert.ToInt32(price) >= Convert.ToInt32(minBudget) && Convert.ToInt32(price) <= Convert.ToInt32(maxBudget))
                        return true;
                }
            }
            return false;
        }

        public void Dispose() {
            this.dsVideo.Dispose();
        }
    }
}