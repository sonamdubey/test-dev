using System;
using Carwale.Notifications;
using Carwale.Interfaces;
using Carwale.Entity.DeepLinking;
using Carwale.Entity;
using System.Text.RegularExpressions;
using Carwale.Entity.CarData;
using System.Configuration;
using Carwale.Interfaces.CarData;
using System.Web;
using Carwale.Entity.Enum;
using System.Collections.Specialized;
using Carwale.DTO.DeepLinking;
using Carwale.Interfaces.Geolocation;

namespace Carwale.BL
{
    public class GetApiForCwApp : IDeepLinking
    {

        ICarModelCacheRepository _cache;
        protected readonly ICarMakesCacheRepository _carmakecache;
        private readonly ICarModels _carModelBl;
        private readonly IGeoCitiesCacheRepository _cityRepo;
        public GetApiForCwApp(ICarModelCacheRepository cache, ICarMakesCacheRepository carmakecache, ICarModels carModelBl, IGeoCitiesCacheRepository cityRepo)
        {
            _cache = cache;
            _carmakecache = carmakecache;
            _carModelBl = carModelBl;
            _cityRepo = cityRepo;
        }
        ///95-98
        /// <summary>
        /// Gets the screen Id of android app and also the api to hit in order to retrieve the data.
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public DeepLinkingEntity GetLinkToApp(string siteUrl)
        {
            var requestHeaders = HttpContext.Current.Request.Headers;
            var strSourceId = requestHeaders["sourceId"];
            var strAppVersion = requestHeaders["appVersion"];
            int sourceId, appVersion;
            Int32.TryParse(strSourceId, out sourceId);
            Int32.TryParse(strAppVersion, out appVersion);
            int defaultScreenId = -1;
            
            // append / at the end of each request
            int qsStartIndex = siteUrl.IndexOf('?');
            if (qsStartIndex > 0)
                siteUrl = siteUrl.Substring(0, qsStartIndex).EndsWith("/") ? siteUrl : siteUrl.Insert(qsStartIndex, "/");
            else
                siteUrl = siteUrl.EndsWith("/") || siteUrl.EndsWith(".html") ? siteUrl : siteUrl + "/";


            ///added by rakesh yadav on 5 dec 2016 to handle issue in android app having versions 95 to 98
            if (sourceId == (int)Platform.CarwaleAndroid && appVersion >= 95 && appVersion <= 98)
                defaultScreenId = 0;

            if (!string.IsNullOrWhiteSpace(siteUrl)) siteUrl = siteUrl.ToLower();
            int questionIndex = siteUrl.IndexOf("?");
            if (questionIndex >= 0) siteUrl = siteUrl.Substring(0, questionIndex);
            var linkDetails = new DeepLinkingEntity()
            {
                CwApi = String.Empty,
                CwAppScreenId = defaultScreenId,
                cwAppExtraParams = String.Empty

            };

            try
            {
               string apiHostUrl = ConfigurationManager.AppSettings["WebApiHostUrl"];
                if (Regex.IsMatch(siteUrl, "/(.*)-cars/(.*)/userreviews/([\\d]+).html"))
                {
                    char[] separators = { '/', '.' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    string reviewId = urlSplitList[urlSplitList.Length - 2];
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.ReviewDetailsPage;
                    linkDetails.CwApi = apiHostUrl + "UserReviewDetail?reviewId=" + reviewId;
                }
                else if (Regex.IsMatch(siteUrl, "/(.*)-cars/(.*)/userreviews/"))
                {
                    string modelMaskingName = "";
                    char[] separators = { '/', '-', '.' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    modelMaskingName = urlSplitList[urlSplitList.Length - 3];
                    ModelMaskingValidationEntity cmr = null;
                    try
                    {
                        cmr = _carModelBl.FetchModelIdFromMaskingName(modelMaskingName,string.Empty);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler objErr = new ExceptionHandler(ex, "GetApiForCwApp-Get Masking Name Details|modelMaskingName=" + modelMaskingName + "|siteUrl=" + siteUrl);
                        objErr.LogException();
                    }
                    int modelId = cmr.ModelId;
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.ReviewsList;
                    linkDetails.CwApi = apiHostUrl + "UserReviews/?modelId=" + modelId + "&versionId=-1&pageNo=1&pageSize=10&sortCriteria=1";
                }
                else if (Regex.IsMatch(siteUrl, "/(.*)/advantage/$")) //advantage landing page
                { 
                    // do mapping once deeplinking is required
                    // corrently it is not required 
                }
                else if (Regex.IsMatch(siteUrl, "/(.*)/advantage/")) //advantage details page
                {
                    // do mapping once deeplinking is required
                    // corrently it is not required 
                }
               // Version details page.   
                else if (Regex.IsMatch(siteUrl, "/(.*)-cars/(.*)/(.*)-([\\d]+)/"))
                {
                    char[] separators = { '/', '-', '.' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    linkDetails.CwApi = apiHostUrl + "Versiondetails?versionId=" + urlSplitList[urlSplitList.Length - 2];
                    string modelName = urlSplitList[3];
                    ModelMaskingValidationEntity cmr;
                    cmr = _carModelBl.FetchModelIdFromMaskingName(modelName,string.Empty);
                    linkDetails.cwAppExtraParams = cmr.ModelId.ToString();
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.VersionDetails;
                }

                else if (Regex.IsMatch(siteUrl, "/(.*)-cars/upcoming/"))
                {
                    char[] separators = { '/', '-' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    string makeMaskingName = urlSplitList[1];
                    var makeDetails = new CarMakesEntity();
                    try
                    {
                        makeDetails = GetMakeDetailsByName(makeMaskingName);
                        int makeId = makeDetails.MakeId;

                        linkDetails.CwAppScreenId = (int)CwAppScreenEnum.UpcomingList;
                        linkDetails.CwApi = apiHostUrl + "UpComingCars/?pageNo=1&pageSize=10&makeId=" + makeId;
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler objErr = new ExceptionHandler(ex, "GetApiForCwApp-Get Masking Name Details-Make Details");
                        objErr.LogException();
                    }



                }


               // Model Details Page.
                else if (Regex.IsMatch(siteUrl, "/(.*)-cars/(.*)/$"))
                {
                    char[] separators = { '/' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    string modelMaskingName = urlSplitList[urlSplitList.Length - 2].StartsWith("price-in") ? urlSplitList[urlSplitList.Length - 3] : urlSplitList[urlSplitList.Length - 2];
                    ModelMaskingValidationEntity cmr;
                    try
                    {
                        cmr = _carModelBl.FetchModelIdFromMaskingName(modelMaskingName,string.Empty);
                        int modelId = cmr.ModelId;
                        var model = _cache.GetModelDetailsById(modelId);
                        linkDetails.cwAppExtraParams = modelId.ToString();
                        if (cmr.ModelId > 0)
                        {
                            if (model.Futuristic)
                            {
                                linkDetails.CwAppScreenId = (int)CwAppScreenEnum.UpcomingCarDetail;
                                linkDetails.CwApi = apiHostUrl + "UpComingCarDetail?id=" + modelId;
                            }
                            else
                            {
                                linkDetails.CwAppScreenId = (int)CwAppScreenEnum.ModelDetail;
                                linkDetails.CwApi = apiHostUrl + "modeldetails/?ModelId=" + modelId;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler objErr = new ExceptionHandler(ex, "GetApiForCwApp-Get Masking Name Details - Model Details");
                        objErr.LogException();
                    }

                }                            
                        // This is for news details page.
                else if (Regex.IsMatch(siteUrl, "/news/([\\d]+)-(.*).html"))
                {
                    char[] separators = { '/', '-' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    linkDetails.CwApi = apiHostUrl + "newsdetail?id=" + urlSplitList[2];
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.NewsDetail;
                }


                // This is for news list page 2 onwards. 
                else if (Regex.IsMatch(siteUrl, "/news/page/([\\d]+)/"))
                {
                    char[] separators = { '/' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    linkDetails.CwApi = apiHostUrl + "Newslisting/?pageNo=" + urlSplitList[2] + "&pageSize=10";
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.NewsList;
                }

                // This is for news list page.
                else if (Regex.IsMatch(siteUrl, "/news/"))
                {
                    linkDetails.CwApi = apiHostUrl + "Newslisting/?pageNo=1&pageSize=10";
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.NewsList;
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/car-care/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=29&pageNo=1&pageSize=10";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/new-car-purchase/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=27&pageNo=1&pageSize=10";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/used-car-purchase/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=28&pageNo=1&pageSize=10";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/car-insurance/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=26&pageNo=1&pageSize=10";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/driving-a-car/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=30&pageNo=1&pageSize=10";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/tyres-and-wheels/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=39&pageNo=1&pageSize=10";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/safety-and-security/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=31&pageNo=1&pageSize=10";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/car-loan/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=32&pageNo=1&pageSize=10";
                }
                else if (Regex.IsMatch(siteUrl, "/tipsadvice/(.*)-([\\d]+)/p([\\d]+)/"))
                {
                    char[] separators = { '/', '-' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    string pageNo = urlSplitList[urlSplitList.Length - 2];
                    pageNo = pageNo.Substring(1);
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipDetails;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvicesDetail/?subCatid=-1&basicId=" + urlSplitList[urlSplitList.Length - 3] + "&priority=" + pageNo;
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/(.*)-([\\d]+)/"))
                {
                    char[] separators = { '/', '-' };
                    string[] urlSplitList = siteUrl.Split(separators, StringSplitOptions.None);
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipDetails;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvicesDetail/?subCatid=-1&basicId=" + urlSplitList[urlSplitList.Length - 2] + "&priority=1";
                }

                else if (Regex.IsMatch(siteUrl, "/tipsadvice/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.TipsAndAdvicesList;
                    linkDetails.CwApi = apiHostUrl + "TipsAndAdvices/?subCatid=-1&pageNo=1&pageSize=10";
                }



                else if (Regex.IsMatch(siteUrl, "/upcoming-cars/"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.UpcomingList;
                    linkDetails.CwApi = apiHostUrl + "UpComingCars?pageNo=1&pageSize=10";
                }
                else if (Regex.IsMatch(siteUrl, "/new/$"))
                {
                    if (sourceId == (int)Platform.CarwaleAndroid && appVersion <= 98)
                    {
                        linkDetails.CwApi = string.Empty;
                        linkDetails.CwAppScreenId = defaultScreenId;
                    }
                    else
                    {
                        linkDetails.CwAppScreenId = (int)CwAppScreenEnum.NewCarLanding;
                        linkDetails.CwApi = apiHostUrl + "newcars/";
                    }
                }
                else if (Regex.IsMatch(siteUrl, @"^((/)|(/m)|(/m/))$"))
                {
                    linkDetails.CwAppScreenId = (int)CwAppScreenEnum.HomePage;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "Get Link To App-DeepLinking");
                objErr.LogException();
            }

            return linkDetails;
        }

        public DeepLinkingDTO GetLinkToAppV2(string queryString) {
            DeepLinkingDTO DTO = new DeepLinkingDTO();
            DeepLinkingEnum screenId = DeepLinkingEnum.HomePage;
            NameValueCollection QS = HttpUtility.ParseQueryString(queryString);
            Enum.TryParse<DeepLinkingEnum>(QS["screenId"],out screenId);
            DTO.ScreenId = (int)screenId;
            int _versionId = -1;
            int makeId = 0;
            string cityMaskingName = string.Empty;
            Entity.Geolocation.City cityDetails = null;
            try
            {
                switch (screenId)
                {
                    case DeepLinkingEnum.ModelDetail:
                        var cardetails = _cache.GetModelDetailsById(_carModelBl.FetchModelIdFromMaskingName(QS["model"],string.Empty).ModelId);
                        if (QS.Count == 3)
                        {
                            cityMaskingName = QS["city"];
                            cityDetails = _cityRepo.GetCityDetailsByMaskingName(cityMaskingName);
                            DTO.Params = new ModelDetailsDTO { ModelId = cardetails.ModelId, CityId = (cityDetails == null ? 0 :  cityDetails.CityId) };
                        }
                        else
                        {
                            if (cardetails.Futuristic)
                            {
                                DTO.ScreenId = (int)DeepLinkingEnum.UpcomingCarDetail;
                            }
                            DTO.Params = new ModelDetailsDTO { ModelId = cardetails.ModelId };
                        }
                        break;
                    case DeepLinkingEnum.SearchResult:
                        int.TryParse(QS["makeId"], out makeId);
                        DTO.Params = new SearchResultDTO
                        {
                            MakeId = makeId
                        };
                        break;
                    case DeepLinkingEnum.VersionDetail:
                        int.TryParse(QS["versionId"], out _versionId);
                        if (QS.Count == 3)
                        {
                            cityMaskingName = QS["city"];
                            cityDetails = _cityRepo.GetCityDetailsByMaskingName(cityMaskingName);
                            DTO.Params = new ModelDetailsDTO { VersionId = _versionId, CityId = (cityDetails == null ? 0 : cityDetails.CityId) };
                        }
                        else
                        {
                            DTO.Params = new ModelDetailsDTO { VersionId = _versionId };
                        }
                        break;
                    case DeepLinkingEnum.CompareCars:
                        int verId1 = 0;
                        int verId2 = 0;
                        if (QS.Count == 3)
                        {
                            int modelId1 = _carModelBl.FetchModelIdFromMaskingName(QS["model1"],string.Empty).ModelId;
                            int modelId2 = _carModelBl.FetchModelIdFromMaskingName(QS["model2"],string.Empty).ModelId;
                            verId1 = getDefaultVersionId(modelId1);
                            verId2 = getDefaultVersionId(modelId2);
                            DTO.Params = new CarCompareDetailsDTO
                            {
                                ModelId1 = modelId1,
                                ModelId2 = modelId2,
                                VersionId1 = verId1,
                                VersionId2 = verId2
                            };
                        }
                        else if(QS.Count >3)
                        {
                            int.TryParse(QS["car1"], out verId1);
                            int.TryParse(QS["car2"], out verId2);
                            DTO.Params = new CarCompareDetailsDTO
                            {
                                ModelId1 = _carModelBl.FetchModelIdFromMaskingName(QS["model1"],string.Empty).ModelId,
                                ModelId2 = _carModelBl.FetchModelIdFromMaskingName(QS["model2"],string.Empty).ModelId,
                                VersionId1 = verId1,
                                VersionId2 = verId2
                            };
                        } 
                                              
                        break;                    
                    case DeepLinkingEnum.UsedCarList:
                        if (QS.Count == 2)
                        {
                            cityMaskingName = QS["city"];
                            cityDetails = _cityRepo.GetCityDetailsByMaskingName(cityMaskingName);
                            DTO.Params = new ModelDetailsDTO { CityId = (cityDetails == null ? -1 : cityDetails.CityId) };
                        }
                        break;
                    case DeepLinkingEnum.UsedCarDetails:
                        DTO.Params = new UsedCarProfileDTO { ProfileId = QS["profileId"].ToUpper() };
                        break;
                    case DeepLinkingEnum.NewsList:
                        int categoryId = 0;
                        int.TryParse(QS["categoryId"], out categoryId);
                        if (QS.Count == 4)
                        {
                            int.TryParse(QS["categoryId"], out categoryId);
                            int.TryParse(QS["makeId"], out makeId);
                            DTO.Params = new NewsDetailDTO
                            {
                                CategoryId = categoryId,
                                MakeId = makeId,
                                MakeName = QS["make"]
                            };
                        }
                        else if (QS.Count == 5)
                        {
                            int.TryParse(QS["categoryId"], out categoryId);
                            int.TryParse(QS["makeId"], out makeId);
                            DTO.Params = new NewsDetailDTO
                            {
                                CategoryId = categoryId,
                                MakeId = makeId,
                                ModelId = _carModelBl.FetchModelIdFromMaskingName(QS["model"], string.Empty).ModelId,
                                MakeName = QS["make"],
                                ModelName = QS["model"]
                            };
                        }
                        else
                        {
                            DTO.Params = new NewsDetailDTO { CategoryId = categoryId };
                        }
                        break;
                    case DeepLinkingEnum.NewsDetail:                        
                        if(QS.Count == 4)
                        {
                            int.TryParse(QS["categoryId"], out categoryId);
                            int.TryParse(QS["makeId"], out makeId);
                            DTO.Params = new NewsDetailDTO
                            {
                                CategoryId = categoryId,
                                MakeId = makeId,
                                MakeName = QS["make"]
                            };
                        }
                        else if(QS.Count == 3)
                        {
                            int.TryParse(QS["categoryId"], out categoryId);
                            DTO.Params = new NewsDetailDTO
                            {
                                CategoryId = categoryId,
                                ModelId = _carModelBl.FetchModelIdFromMaskingName(QS["model"],string.Empty).ModelId
                            };
                        }
                        else
                        {
                            int basicId = 0;
                            int.TryParse(QS["basicId"], out basicId);
                            DTO.Params = new NewsDetailDTO { BasicId = basicId };
                        }
                        break;
                    case DeepLinkingEnum.UpcomingCarDetail:                       
                        DTO.Params = new ModelDetailsDTO { ModelId = _carModelBl.FetchModelIdFromMaskingName(QS["model"],string.Empty).ModelId, CityId = null };
                        break;
                    case DeepLinkingEnum.LocateNewCarDealerSelectCity:
                        int.TryParse(QS["makeId"], out makeId);
                        DTO.Params = new ModelDetailsDTO { MakeId = makeId};
                        break;
                    case DeepLinkingEnum.LocateNewCarDealerListing:
                        int.TryParse(QS["makeId"], out makeId);
                        cityMaskingName = QS["city"];
                        cityDetails = _cityRepo.GetCityDetailsByMaskingName(cityMaskingName);
                        DTO.Params = new ModelDetailsDTO { MakeId = makeId, CityId = (cityDetails == null ? -1 : cityDetails.CityId) };
                        break;
                    case DeepLinkingEnum.AdvantageDetailPage:
                        cityMaskingName = QS["city"];
                            cityDetails = _cityRepo.GetCityDetailsByMaskingName(cityMaskingName);
                        int model = _carModelBl.FetchModelIdFromMaskingName(QS["model"],string.Empty).ModelId;
                        int.TryParse(QS["versionid"], out _versionId);
                        int? versionId = null;
                        if (_versionId != 0)
                        {
                            versionId = _versionId;
                        }
                        DTO.Params = new ModelDetailsDTO
                        {
                            ModelId = model,
                            VersionId = versionId,
                            CityId = (cityDetails == null ? -1 : cityDetails.CityId)
                        };
                        break;
                    case DeepLinkingEnum.AdvantageListPage:
                        if(QS.Count > 1)
                        {
                            int.TryParse(QS["makes"], out makeId);
                            int? _makeId = null;
                            if (makeId != 0)
                            {
                                _makeId = makeId;
                            }
                            cityMaskingName = QS["city"];
                            cityDetails = _cityRepo.GetCityDetailsByMaskingName(cityMaskingName);
                            DTO.Params = new ModelDetailsDTO
                            {
                                MakeId = _makeId,
                                CityId = (cityDetails == null ? -1 : cityDetails.CityId)
                            };
                        }
                        break;
                    case DeepLinkingEnum.UpcomingCarsList:
                        if(QS.Count == 2)
                        {
                            int.TryParse(QS["makeId"], out makeId);
                            DTO.Params = new SearchResultDTO { MakeId = makeId };
                        }                        
                        break;
                    case DeepLinkingEnum.UserReviewList:
                        if(QS.Count > 1)
                        {
                            int.TryParse(QS["makeId"], out makeId);
                            DTO.Params = new ModelDetailsDTO
                            {
                                MakeId = makeId,
                                ModelId = _carModelBl.FetchModelIdFromMaskingName(QS["model"],string.Empty).ModelId
                            };
                        }
                        break;
                    case DeepLinkingEnum.LocateNewCarDealerDetails:
                        int.TryParse(QS["makeId"], out makeId);
                        DTO.Params = new SearchResultDTO
                        {
                            MakeId = makeId,
                            Type = "new"
                        };
                        break;
                    case DeepLinkingEnum.UserReviewDetailsPage:
                        DTO.Params = new SearchResultDTO
                        {
                            QueryString = queryString
                        };
                        break;
                    case DeepLinkingEnum.PQLanding:
                        break;
                    case DeepLinkingEnum.LocateNewCarDealerLanding:
                        break;
                    case DeepLinkingEnum.HomePage:
                        break;
                    case DeepLinkingEnum.NewCarLanding:
                        break;
                    case DeepLinkingEnum.Insurance:
                        break;                    
                    default:
                        DTO.ScreenId = -1;
                        break;

                }
            }
            catch(Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "Error in Handling"+ queryString);
                objErr.LogException();
            }
            return DTO;
        }

        private CarMakesEntity GetMakeDetailsByName(string makeName)
        {
            return _carmakecache.GetMakeDetailsByName(makeName);
        }
        /// <summary>
        /// Get Default Vesion ID if no version of model is selected in Compare Cars URL
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns>Default Version ID</returns>
        public int getDefaultVersionId(int modelID)
        {
            var modelDetails = _cache.GetModelDetailsById(modelID);
            return modelDetails.PopularVersion;
        }
}
}