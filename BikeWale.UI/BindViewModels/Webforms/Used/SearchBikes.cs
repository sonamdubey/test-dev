
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
namespace Bikewale.BindViewModels.Webforms.Used
{
    public class SearchUsedBikes
    {

        protected string makemasking = string.Empty, citymasking = string.Empty, strTotal = string.Empty;
        public string pageTitle = string.Empty, pageDescription = string.Empty, pageKeywords = string.Empty, pageCanonical = string.Empty
                  , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty, redirectUrl = string.Empty, alternateUrl = string.Empty;
        private const int _pageSize = 20;
        private int _pageNo = 1;
        protected int _startIndex = 0, _endIndex = 0;
        private const int _pagerSlotSize = 5;
        public LinkPagerControl ctrlPager;



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
        public uint TotalBikes { get; set; }
        public bool IsPageNotFound { get; set; }
        public bool IsPermanentRedirection { get; set; }
        public string RedirectionUrl { get; set; }
        public string City { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string BikeName { get; set; }
        public Bikewale.Entities.Used.Search.SearchResult UsedBikes = null;
        public IEnumerable<CityEntityBase> Cities = null;
        public IEnumerable<BikeMakeModelBase> MakeModels = null;

        private string _modelMaskingName = string.Empty, _cityMaskingName = string.Empty, _makeMaskingName = string.Empty;




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
        /// </summary>
        /// <returns></returns>
        public void GetAllCities()
        {
            try
            {
                Cities = objCitiesCache.GetAllCities(EnumBikeType.Used);

                var cityBase = (from c in Cities
                                where c.CityMaskingName == _cityMaskingName
                                select c).FirstOrDefault();
                if (cityBase != null)
                {
                    City = cityBase.CityName;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : GetAllCities");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By: Aditi Srivastava on 15 Sep 2016
        /// Description: Gets all makes and models for filter binding
        /// </summary>
        /// <returns></returns>
        private void GetAllMakeModels()
        {
            try
            {
                MakeModels = objMakeCache.GetAllMakeModels();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " : GetAllMakeModels");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Function to bind the search result to the repeater
        /// </summary>
        public bool BindSearchPageData()
        {
            try
            {
                GetAllCities();
                GetAllMakeModels();
                InputFilters objFilters = new InputFilters();

                // If inputs are set by hash, hash overrides the query string parameters
                if (CityId > 0)
                    objFilters.City = CityId;

                // Don't pass the make ids when modelid is fetched through Query string 
                if (ModelId == 0 && MakeId > 0)
                {
                    objFilters.Make = Convert.ToString(MakeId);
                }

                if (ModelId > 0)
                    objFilters.Model = Convert.ToString(ModelId);

                objFilters.PN = _pageNo;
                objFilters.PS = _pageSize;


                UsedBikes = objSearch.GetUsedBikesList(objFilters);
                if (UsedBikes != null && UsedBikes.Result != null && UsedBikes.Result.Count() > 0)
                {
                    TotalBikes = Convert.ToUInt32(UsedBikes.TotalCount);
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
        public void CreateMetas()
        {
            try
            {
                // Common title, h1 and canonical
                BikeName = string.Format("{0} {1}", Make, Model);

                heading = string.Format("Used {0} Bikes in {1}", BikeName, City);

                pageTitle = string.Format("Used {0} Bikes in {1} - {2} Verified Bike Listing For Sale | BikeWale", BikeName, City, TotalBikes);

                pageCanonical = string.Format("http://www.bikewale.com/{0}", HttpContext.Current.Request.RawUrl.ToLower());

                alternateUrl = string.Format("http://www.bikewale.com/m/{0}", HttpContext.Current.Request.RawUrl.ToLower());

                // Make models specific
                if (!string.IsNullOrEmpty(Model))
                {
                    pageDescription = string.Format("There are {0} used {1} {2} bikes in {3} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand {2} bikes for sale in {3}", TotalBikes, Make, Model, City);
                    pageKeywords = string.Format("Used {1} bikes in {2}, used {1} bikes, find used {1} bikes in {2}, buy used {1} bikes in {2}, search used {0} {1} bikes, find used {0} {1} bikes, {1} bike sale in {2}, used {0} {1} bikes in {2}, used {0} {1} bikes", Make, Model, City);
                }
                else if (!string.IsNullOrEmpty(Make))
                {
                    pageDescription = string.Format("There are {0} used {1} bikes in {2} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand {1} bikes for sale in {2}", TotalBikes, Make, City);
                    pageKeywords = string.Format("Used {0} bikes in {1}, used {0} bikes, find used {0} bikes in {1}, buy used {0} bikes in {1}, search used {0} bikes, find used {0} bikes, bike sale, {0} bike sale in {1}", Make, City);
                }
                else
                {
                    pageDescription = string.Format("There are {0} used bikes in {1} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand bikes for sale in {1}", TotalBikes, City);
                    pageKeywords = string.Format("Used bikes in {0}, find used bikes in {0}, buy used bikes in {0}, search used bikes, find used bikes, used bike listing, bike used sale, bike sale in {0}, {0} bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki", City);
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

        public void BindLinkPager()
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            int recordCount = Convert.ToInt32(TotalBikes);
            string _baseUrl = RemoveTrailingPage(HttpContext.Current.Request.RawUrl.ToLower());

            try
            {
                GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex, recordCount);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = string.Format("{0}page", _baseUrl);
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                // _pagerEntity.PageUrlType = "page-{0}/";
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = objPager.GetUsedBikePager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                ctrlPager.MakeId = Convert.ToString(MakeId);
                ctrlPager.CityId = CityId;
                ctrlPager.ModelId = Convert.ToString(ModelId);
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
            //, _make = string.Empty;
            try
            {
                _cityMaskingName = page.Request.QueryString["city"];
                if (!string.IsNullOrEmpty(_cityMaskingName))
                {
                    objCityResponse = objCityCache.GetCityMaskingResponse(_cityMaskingName);
                }

                _makeMaskingName = page.Request.QueryString["make"];
                if (!string.IsNullOrEmpty(_makeMaskingName))
                {
                    string _strMakeId = MakeMapping.GetMakeId(_makeMaskingName);
                    ushort _makeId = default(ushort);
                    //verify the id as passed in the url
                    if (ushort.TryParse(_strMakeId, out _makeId))
                    {
                        MakeId = _makeId;
                        BikeMakeEntityBase makeDetails = objMakeCache.GetMakeDetails(MakeId);
                        Make = makeDetails != null ? makeDetails.MakeName : string.Empty;
                    }
                    else
                    {
                        IsPageNotFound = true;
                    }
                }

                _modelMaskingName = page.Request.QueryString["model"];
                if (!string.IsNullOrEmpty(_modelMaskingName))
                {
                    objModelResponse = objModelsCache.GetModelMaskingResponse(_modelMaskingName);
                    if (objModelResponse != null && objModelResponse.ModelId > 0)
                    {
                        BikeModelEntity modelEntity = objModels.GetById(Convert.ToInt32(objModelResponse.ModelId));
                        Model = modelEntity != null ? modelEntity.ModelName : string.Empty;
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
                    // Get cityId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objCityResponse.StatusCode == 200)
                    {
                        CityId = objCityResponse.CityId;
                    }
                    else if (objCityResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        RedirectionUrl = page.Request.RawUrl.ToLower().Replace(_cityMaskingName, objCityResponse.MaskingName);
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
                        ModelId = objModelResponse.ModelId;
                    }
                    else if (objModelResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        RedirectionUrl = page.Request.RawUrl.ToLower().Replace(_modelMaskingName, objModelResponse.MaskingName);
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