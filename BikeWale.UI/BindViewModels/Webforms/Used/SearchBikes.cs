
using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.BAL.Used.Search;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.BikeData;
using Bikewale.DAL.Location;
using Bikewale.DAL.Used.Search;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.Used.Search;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.Used.Search;
using Bikewale.Mobile.Controls;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Bikewale.BindViewModels.Webforms.Used
{
    public class SearchUsedBikes
    {

        public string pageTitle = string.Empty, pageDescription = string.Empty, pageKeywords = string.Empty, pageCanonical = string.Empty
                  , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty, redirectUrl = string.Empty, alternateUrl = string.Empty;
        private const int _pageSize = 20;
        private int _pageNo = 1;
        public int startIndex = 0, endIndex = 0;
        private const int _pagerSlotSize = 5;



        private ICityMaskingCacheRepository objCityCache = null;
        private IBikeMakesCacheRepository objMakeCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> objModelsCache = null;
        private IBikeModels<BikeModelEntity, int> objModels = null;
        private ICityCacheRepository objCitiesCache = null;
        private ISearch objSearch = null;
        private IPager objPager = null;
        public ushort MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public uint TotalBikes { get; set; }
        public bool IsPageNotFound { get; set; }
        public bool IsPermanentRedirection { get; set; }
        public string RedirectionUrl { get; set; }
        public string City { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string BikeName { get; set; }
        public string CurrentQS { get; set; }
        public Bikewale.Entities.Used.Search.SearchResult UsedBikes = null;
        public IEnumerable<CityEntityBase> Cities = null;
        public IEnumerable<BikeMakeModelBase> MakeModels = null;
        public CityEntityBase SelectedCity = null;
        public BikeMakeEntityBase SelectedMake = null;

        public string modelMaskingName = string.Empty, cityMaskingName = string.Empty, makeMaskingName = string.Empty;



        /// <summary>
        /// Created By : Sushil Kumar on 23rd Sep 2016 
        /// Description : Parse Query String  and resolve containers on initialization
        /// </summary>
        public SearchUsedBikes()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                .RegisterType<ICacheManager, MemcacheManager>()
                .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                .RegisterType<ICityCacheRepository, CityCacheRepository>()
                .RegisterType<ICityMaskingCacheRepository, CityMaskingCache>()
                .RegisterType<ICity, CityRepository>()
                .RegisterType<IBikeMakesCacheRepository, BikeMakesCacheRepository>()
                .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                .RegisterType<ISearchFilters, ProcessSearchFilters>()
                .RegisterType<ISearchQuery, SearchQuery>()
                .RegisterType<ISearchRepository, SearchRepository>()
                .RegisterType<ISearch, SearchBikes>()
                .RegisterType<IPager, Pager>();

                objCityCache = container.Resolve<ICityMaskingCacheRepository>();
                objMakeCache = container.Resolve<IBikeMakesCacheRepository>();
                objCitiesCache = container.Resolve<ICityCacheRepository>();
                objModels = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                objModelsCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                objPager = container.Resolve<IPager>();
                objSearch = container.Resolve<ISearch>();
            }

            City = "India";
            ProcessQueryString();

        }

        /// <summary>
        /// Created By: Aditi Srivastava on 15 Sep 2016
        /// Description: To fetch all cities
        /// Modified By :Subodh Jain 2 jan 2017
        /// Description :- Made citymaksing name public
        /// <returns></returns>
        public void GetAllCities()
        {
            try
            {
                Cities = objCitiesCache.GetAllCities(EnumBikeType.Used);

                SelectedCity = Cities.FirstOrDefault(c => c.CityMaskingName == cityMaskingName);
                if (SelectedCity != null)
                {
                    City = SelectedCity.CityName;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : GetAllCities");

            }
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 15 Sep 2016
        /// Description: Gets all makes and models for filter binding
        /// Modified By :Subodh Jain 2 jan 2017
        /// Description :- Made masking name ,model masking name public
        /// </summary>
        /// <returns></returns>
        private void GetAllMakeModels()
        {
            try
            {
                MakeModels = objMakeCache.GetAllMakeModels();

                if (MakeModels != null)
                {
                    var _objMake = MakeModels.FirstOrDefault(m => m.Make.MaskingName == makeMaskingName);
                    if (_objMake != null && _objMake.Make != null)
                    {
                        Make = _objMake.Make.MakeName;
                        SelectedMake = _objMake.Make;
                        if (_objMake.Models != null)
                        {
                            var _objModel = _objMake.Models.FirstOrDefault(m => m.MaskingName == modelMaskingName);
                            if (_objModel != null)
                                Model = _objModel.ModelName;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : GetAllMakeModels");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 23rd Sep 2016
        /// Description : Bind search page related info for city and modela nd its filter
        /// Modified By : Sushil Kumar on 27th Jan 2016
        /// Description : Remove maintainance od city form the query string
        /// </summary>
        public bool BindSearchPageData()
        {
            try
            {
                GetAllCities();
                GetAllMakeModels();
                InputFilters objFilters = new InputFilters();
                CurrentQS = string.Empty;

                if (ModelId > 0)
                {
                    objFilters.Model = Convert.ToString(ModelId);
                    CurrentQS = string.Format("{0}&model={1}", CurrentQS, ModelId);
                }

                // Don't pass the make ids when modelid is fetched through Query string 
                if (ModelId == 0 && MakeId > 0)
                {
                    objFilters.Make = Convert.ToString(MakeId);
                    CurrentQS = string.Format("{0}&make={1}", CurrentQS, MakeId);
                }

                // If inputs are set by hash, hash overrides the query string parameters
                if (CityId > 0)
                {
                    objFilters.City = CityId;
                }

                if (!string.IsNullOrEmpty(CurrentQS)) CurrentQS = CurrentQS.Substring(1);

                objFilters.PN = _pageNo;
                objFilters.PS = _pageSize;


                UsedBikes = objSearch.GetUsedBikesList(objFilters);
                if (UsedBikes != null && UsedBikes.Result != null && UsedBikes.Result.Any())
                {
                    TotalBikes = Convert.ToUInt32(UsedBikes.TotalCount);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : BindSearchPageData");

            }
            return true;
        } // End of BindSearchPageData


        /// <summary>
        /// Created by: Sangram Nandkhile on 13 Sep 2016
        /// Summary: Create title, metas and description for SEO
        /// </summary>
        public void CreateMetas()
        {
            string _totalBikes = Format.FormatPrice(TotalBikes.ToString());
            try
            {
                // Common title, h1 and canonical
                BikeName = MakeId > 0 ? string.Format("{0} {1}", Make, Model).Trim() : string.Empty;

                var totalPagesCount = (uint)(TotalBikes / _pageSize);

                if ((TotalBikes % _pageSize) > 0)
                    totalPagesCount += 1;

                heading = string.Format("Used {0} Bikes in {1}", BikeName, City);

                pageTitle = string.Format("{0} Verified used {1} bikes in {2} - BikeWale", _totalBikes, BikeName, City);

                pageCanonical = string.Format("https://www.bikewale.com/{0}", HttpContext.Current.Request.RawUrl.ToLower().Substring(1));

                alternateUrl = string.Format("https://www.bikewale.com/m/{0}", HttpContext.Current.Request.RawUrl.ToLower().Substring(1));


                if (MakeId > 0)
                {
                    pageDescription = string.Format("There are {0} used {1} bikes in {2} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand {1} bikes for sale in {2}", _totalBikes, BikeName, City);
                    pageKeywords = string.Format("Used {0} bikes in {1}, used {0} bikes, find used {0} bikes in {1}, buy used {0} bikes in {1}, search used {0} bikes, find used {0} bikes, bike sale, {0} bike sale in {1}", BikeName, City);
                }
                else
                {
                    pageDescription = string.Format("There are {0} used bikes in {1} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand bikes for sale in {1}", _totalBikes, City);
                    pageKeywords = string.Format("Used bikes in {0}, find used bikes in {0}, buy used bikes in {0}, search used bikes, find used bikes, used bike listing, bike used sale, bike sale in {0}, {0} bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki", City);
                }

                if (_pageNo > 1)
                {
                    pageDescription = string.Format("Page {0} of {1} - {2}", _pageNo, totalPagesCount, pageDescription);
                    pageTitle = string.Format("Page {0} of {1} - {2}", _pageNo, totalPagesCount, pageTitle);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : CreateMetas");

            }
        }

        public string RemoveTrailingPage(string rawUrl)
        {
            string retUrl = rawUrl;
            if (rawUrl.Contains("/page-"))
            {
                string[] urlArray = rawUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                retUrl = string.Format("/{0}/", string.Join("/", urlArray.Take(urlArray.Length - 1).ToArray()));
            }
            return retUrl;
        }

        #region pagination methods

        /// <summary>
        /// Function to the start and end index for the current page number.
        /// </summary>
        /// <param name="pageSize">Total number of records per page.</param>
        /// <param name="currentPageNo">Current page number</param>
        /// <param name="startIndex">start index for records</param>
        /// <param name="endIndex">End index for records</param>
        public void GetStartEndIndex(int pageSize, int currentPageNo, out int startIndex, out int endIndex, int totalCount)
        {
            startIndex = 0;
            endIndex = 0;
            endIndex = currentPageNo * pageSize;
            startIndex = (endIndex - pageSize) + 1;
            if (totalCount < endIndex)
                endIndex = totalCount;
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 22nd Sep 2016
        /// Description : Bind pagination for the page according to common logic
        /// </summary>
        /// <param name="_ctrlPager"></param>
        public void BindLinkPager(LinkPagerControl _ctrlPager)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            int recordCount = Convert.ToInt32(TotalBikes);
            string _baseUrl = RemoveTrailingPage(HttpContext.Current.Request.RawUrl.ToLower());

            try
            {
                GetStartEndIndex(_pageSize, _pageNo, out startIndex, out endIndex, recordCount);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = string.Format("{0}page", _baseUrl);
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = objPager.GetUsedBikePager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                _ctrlPager.MakeId = Convert.ToString(MakeId);
                _ctrlPager.CityId = CityId;
                _ctrlPager.ModelId = Convert.ToString(ModelId);
                _ctrlPager.PagerOutput = _pagerOutput;
                _ctrlPager.CurrentPageNo = _pageNo;
                _ctrlPager.TotalPages = objPager.GetTotalPages((int)recordCount, _pageSize);
                _ctrlPager.BindPagerList();

                //For SEO

                prevUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? string.Empty : "https://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
        }
        #endregion

        /// <summary>
        /// Modified By : Sushil Kumar on 22nd Sep 2016
        /// Description : Removed unnecessary fetch logic for make,model and cities for their respective names.
        /// Modified by :   Sumit Kate on 28 Sep 2016
        /// Description :   Handle rename make masking with 301 redirect
        /// </summary>
        private void ProcessQueryString()
        {
            HttpContext page = HttpContext.Current;
            ModelMaskingResponse objModelResponse = null;
            CityMaskingResponse objCityResponse = null;
            MakeMaskingResponse objMakeResponse = null;
            string newMakeMasking = string.Empty;
            bool isMakeRedirection = false;
            try
            {
                cityMaskingName = page.Request.QueryString["city"];
                if (!string.IsNullOrEmpty(cityMaskingName))
                {
                    objCityResponse = objCityCache.GetCityMaskingResponse(cityMaskingName);
                }

                makeMaskingName = page.Request.QueryString["make"];
                if (!string.IsNullOrEmpty(makeMaskingName))
                {
                    objMakeResponse = objMakeCache.GetMakeMaskingResponse(makeMaskingName);
                }

                newMakeMasking = ProcessMakeMaskingName(makeMaskingName, out isMakeRedirection);

                modelMaskingName = page.Request.QueryString["model"];
                if (objMakeResponse != null && !string.IsNullOrEmpty(modelMaskingName))
                {
                    objModelResponse = objModelsCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeMaskingName, modelMaskingName));
                }

                if (!String.IsNullOrEmpty(page.Request.QueryString["pn"]))
                {
                    int result;
                    int.TryParse(page.Request.QueryString["pn"], out result);
                    _pageNo = result;
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, page.Request.ServerVariables["URL"] + "ParseQueryString");

            }
            finally
            {
                if (objMakeResponse != null)
                {
                    if (objMakeResponse.StatusCode == 200)
                    {
                        MakeId = Convert.ToUInt16(objMakeResponse.MakeId);
                    }
                    else if (objMakeResponse.StatusCode == 301)
                    {
                        RedirectionUrl = page.Request.RawUrl.ToLower().Replace(makeMaskingName, objMakeResponse.MaskingName);
                        IsPermanentRedirection = true;
                    }
                    else
                    {
                        IsPageNotFound = true;
                    }
                }
                if (objCityResponse != null)
                {
                    // Get cityId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objCityResponse.StatusCode == 200)
                    {
                        CityId = objCityResponse.CityId;
                    }
                    else if (objCityResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        RedirectionUrl = page.Request.RawUrl.ToLower().Replace(cityMaskingName, objCityResponse.MaskingName);
                        IsPermanentRedirection = true;
                    }
                    else
                    {
                        if (!cityMaskingName.ToLower().Equals("india"))
                            IsPageNotFound = true;
                    }
                }
                if (objModelResponse != null)
                {
                    // Get ModelId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objModelResponse.StatusCode == 200)
                    {
                        ModelId = objModelResponse.ModelId;
                    }
                    else if (objModelResponse.StatusCode == 301 || isMakeRedirection)
                    {
                        //redirect permanent to new page                         
                        RedirectionUrl = page.Request.RawUrl.ToLower().Replace(modelMaskingName, objModelResponse.MaskingName).Replace(makeMaskingName, newMakeMasking);
                        IsPermanentRedirection = true;
                    }
                    else
                    {
                        IsPageNotFound = true;
                    }
                }
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 11th Dec 2017
        /// Description : Process make masking name for redirection
        /// </summary>
        /// <param name="make"></param>
        /// <param name="isMakeRedirection"></param>
        /// <returns></returns>
        private string ProcessMakeMaskingName(string make, out bool isMakeRedirection)
        {
            MakeMaskingResponse makeResponse = null;
            Common.MakeHelper makeHelper = new Common.MakeHelper();
            isMakeRedirection = false;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    return makeResponse.MaskingName;
                }
                else if (makeResponse.StatusCode == 301)
                {
                    isMakeRedirection = true;
                    return makeResponse.MaskingName;
                }
                else
                {
                    return "";
                }
            }

            return "";
        }
    }
}