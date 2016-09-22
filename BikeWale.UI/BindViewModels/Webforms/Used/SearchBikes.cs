
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
using Bikewale.Memcache;
using Bikewale.Mobile.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
namespace Bikewale.BindViewModels.Webforms.Used
{
    public class SearchUsedBikes
    {
        protected Repeater rptUsedListings;
        protected uint cityId;
        bool redirectPermanent, redirectToPageNotFound;
        protected string makeId, modelId = string.Empty;
        protected string makemasking = string.Empty, citymasking = string.Empty, strTotal = string.Empty;// modelmasking = string.Empty, pageno = string.Empty;
        protected string pageTitle = string.Empty, pageDescription = string.Empty, modelName = string.Empty, makeName = string.Empty, pageKeywords = string.Empty, cityName = "India", pageCanonical = string.Empty
                  , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty, redirectUrl = string.Empty;
        private const int _pageSize = 20;
        private int _pageNo = 1;
        protected int _startIndex = 0, _endIndex = 0, totalListing;
        private const int _pagerSlotSize = 5;

        protected IEnumerable<CityEntityBase> cities = null;
        protected IEnumerable<BikeMakeModelBase> makeModels = null;


        private ICityMaskingCacheRepository objCityCache = null;
        private IBikeMakesCacheRepository<int> objMakeCache = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> objModelsCache = null;
        private IBikeModels<BikeModelEntity, int> objModels = null;
        private ICityCacheRepository objCitiesCache = null;
        private ISearch objSearch = null;
        private IPager objPager = null;
        public ushort MakeId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public bool IsPageNotFound { get; set; }
        public bool IsPermanentRedirection { get; set; }
        public string RedirectionUrl { get; set; }
        public Bikewale.Entities.Used.Search.SearchResult objUsedBikes = null;




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
                .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>()
                .RegisterType<ISearchFilters, ProcessSearchFilters>()
                .RegisterType<ISearchQuery, SearchQuery>()
                .RegisterType<ISearchRepository, SearchRepository>()
                .RegisterType<ISearch, SearchBikes>()
                .RegisterType<IPager, Pager>();

                objCityCache = container.Resolve<ICityMaskingCacheRepository>();
                objMakeCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                objCitiesCache = container.Resolve<ICityCacheRepository>();
                objPager = container.Resolve<IPager>();
                objSearch = container.Resolve<ISearch>();
            }
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 15 Sep 2016
        /// Description: To fetch all cities
        /// </summary>
        /// <returns></returns>
        private IEnumerable<CityEntityBase> GetAllCities()
        {
            cities = new List<CityEntityBase>();
            try
            {
                cities = objCitiesCache.GetAllCities(EnumBikeType.Used);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : GetAllCities");
                objErr.SendMail();
            }
            return cities;
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 15 Sep 2016
        /// Description: Gets all makes and models for filter binding
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BikeMakeModelBase> GetAllMakeModels()
        {
            makeModels = new List<BikeMakeModelBase>();
            try
            {

                makeModels = objMakeCache.GetAllMakeModels();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : GetAllMakeModels");
                objErr.SendMail();
            }
            return makeModels;
        }

        /// <summary>
        /// Function to bind the search result to the repeater
        /// </summary>
        private bool BindSearchPageData()
        {
            try
            {
                InputFilters objFilters = new InputFilters();

                // If inputs are set by hash, hash overrides the query string parameters
                objFilters.City = cityId;

                // Don't pass the make ids when modelid is fetched through Query string 
                if (modelId == string.Empty)
                {
                    objFilters.Make = makeId;
                }
                objFilters.Model = modelId;
                objFilters.PN = _pageNo;
                objFilters.PS = _pageSize;


                objUsedBikes = objSearch.GetUsedBikesList(objFilters);
                if (objUsedBikes != null && objUsedBikes.Result != null && objUsedBikes.Result.Count() > 0)
                {
                    //pageno = objResult.CurrentPageNo.ToString();
                    totalListing = objUsedBikes.TotalCount;
                    strTotal = totalListing.ToString();
                    rptUsedListings.DataSource = objUsedBikes.Result;
                    rptUsedListings.DataBind();
                    //return true;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
            }
            return true;
        } // End of BindSearchPageData


        /// <summary>
        /// Created by: Sangram Nandkhile on 13 Sep 2016
        /// Summary: Create title, metas and description for SEO
        /// </summary>
        public void CreateMetas(string strMake, string strModel, string strCity, int count)
        {
            try
            {
                // Common title, h1 and canonical
                string bikeName = string.Format("{0} {1} ", strMake, strModel).Trim();

                if (bikeName.Length > 0)
                    bikeName = string.Format("{0} ", bikeName);

                heading = string.Format("Used {0}Bikes in {1}", bikeName, strCity);

                pageTitle = string.Format("Used {0}Bikes in {1} - {2} Verified Bike Listing For Sale | BikeWale", bikeName, strCity, totalListing);

                pageCanonical = string.Format("http://www.bikewale.com/{0}", HttpContext.Current.Request.RawUrl.ToLower().Replace("/m/", string.Empty));

                // Make models specific
                if (strModel.Length > 0)
                {
                    pageDescription = string.Format("There are {0} used {1} {2} bikes in {3} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand {2} bikes for sale in {3}", count, strMake, strModel, strCity);
                    pageKeywords = string.Format("Used {1} bikes in {2}, used {1} bikes, find used {1} bikes in {2}, buy used {1} bikes in {2}, search used {0} {1} bikes, find used {0} {1} bikes, {1} bike sale in {2}, used {0} {1} bikes in {2}, used {0} {1} bikes", strMake, strModel, strCity);
                }
                else if (strMake.Length > 0)
                {
                    pageDescription = string.Format("There are {0} used {1} bikes in {2} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand {1} bikes for sale in {2}", count, strMake, strCity);
                    pageKeywords = string.Format("Used {0} bikes in {1}, used {0} bikes, find used {0} bikes in {1}, buy used {0} bikes in {1}, search used {0} bikes, find used {0} bikes, bike sale, {0} bike sale in {1}", strMake, strCity);
                }
                else
                {
                    pageDescription = string.Format("There are {0} used bikes in {1} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand bikes for sale in {1}", count, strCity);
                    pageKeywords = string.Format("Used bikes in {0}, find used bikes in {0}, buy used bikes in {0}, search used bikes, find used bikes, used bike listing, bike used sale, bike sale in {0}, {0} bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki", strCity);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
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

        private void BindLinkPager(LinkPagerControl ctrlPager, IPager objPager, int recordCount)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;

            string _baseUrl = RemoveTrailingPage(HttpContext.Current.Request.RawUrl.ToLower());

            try
            {
                GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex, totalListing);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = string.Format("{0}page", _baseUrl);
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                // _pagerEntity.PageUrlType = "page-{0}/";
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = objPager.GetUsedBikePager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                ctrlPager.MakeId = makeId;
                ctrlPager.CityId = cityId;
                ctrlPager.ModelId = modelId;
                ctrlPager.PagerOutput = _pagerOutput;
                ctrlPager.CurrentPageNo = _pageNo;
                ctrlPager.TotalPages = objPager.GetTotalPages((int)recordCount, _pageSize);
                ctrlPager.BindPagerList();

                //For SEO
                //CreatePrevNextUrl(ctrlPager.TotalPages,_baseUrl);
                prevUrl = String.IsNullOrEmpty(_pagerOutput.PreviousPageUrl) ? string.Empty : "http://www.bikewale.com" + _pagerOutput.PreviousPageUrl;
                nextUrl = String.IsNullOrEmpty(_pagerOutput.NextPageUrl) ? string.Empty : "http://www.bikewale.com" + _pagerOutput.NextPageUrl;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion


        private void ProcessQueryString()
        {
            HttpContext page = HttpContext.Current;
            ModelMaskingResponse objModelResponse = null;
            CityMaskingResponse objCityResponse = null;
            string model = string.Empty, city = string.Empty, _make = string.Empty;
            try
            {
                city = page.Request.QueryString["city"];
                if (!string.IsNullOrEmpty(city))
                {
                    objCityResponse = objCityCache.GetCityMaskingResponse(city);
                }

                if (!string.IsNullOrEmpty(page.Request.QueryString["make"]))
                {
                    string makeMaskingName = page.Request.QueryString["make"];
                    makeId = MakeMapping.GetMakeId(makeMaskingName);
                    ushort _makeId = default(ushort);
                    //verify the id as passed in the url
                    if (ushort.TryParse(makeId, out _makeId))
                    {

                        BikeMakeEntityBase makeDetails = objMakeCache.GetMakeDetails(_makeId);
                        if (makeDetails != null)
                        {
                            makeName = makeDetails.MakeName;
                        }
                    }
                    else
                    {
                        IsPageNotFound = true;
                    }
                }

                model = page.Request.QueryString["model"];
                if (!string.IsNullOrEmpty(model))
                {
                    objModelResponse = objModelsCache.GetModelMaskingResponse(model);
                    if (objModelResponse != null && objModelResponse.ModelId > 0)
                    {
                        BikeModelEntity modelEntity = objModels.GetById(Convert.ToInt32(objModelResponse.ModelId));
                        modelName = modelEntity.ModelName;
                    }
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, page.Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();
            }
            finally
            {
                if (objCityResponse != null)
                {
                    IEnumerable<CityEntityBase> GetCityDetails = GetAllCities();
                    CityEntityBase cityBase = (from c in GetCityDetails
                                               where c.CityMaskingName == city
                                               select c).FirstOrDefault();
                    if (cityBase != null)
                    {
                        cityName = cityBase.CityName;
                    }
                    // Get cityId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objCityResponse.StatusCode == 200)
                    {
                        cityId = objCityResponse.CityId;
                    }
                    else if (objCityResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        RedirectionUrl = page.Request.RawUrl.ToLower().Replace(city, objCityResponse.MaskingName);
                        IsPermanentRedirection = true;
                    }
                    else
                    {
                        IsPageNotFound = true;
                    }
                }
                if (objModelResponse != null)
                {
                    // Get ModelId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objModelResponse.StatusCode == 200)
                    {
                        modelId = objModelResponse.ModelId.ToString();
                    }
                    else if (objModelResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        RedirectionUrl = page.Request.RawUrl.ToLower().Replace(model, objModelResponse.MaskingName);
                        IsPermanentRedirection = true;
                    }
                    else
                    {
                        IsPageNotFound = true;
                    }
                }
            }
        }
    }
}