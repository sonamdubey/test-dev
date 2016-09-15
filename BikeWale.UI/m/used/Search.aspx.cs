﻿using Bikewale.BAL.BikeData;
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
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Bikewale.Mobile.Used
{
    /// <summary>
    /// Created by: Ashish Kamble on 10 Sep 2016
    /// </summary>
    public class Search : System.Web.UI.Page
    {
        #region variables

        protected LinkPagerControl ctrlPager;
        protected Repeater rptUsedListings;
        protected uint cityId, modelId;
        protected ushort makeId;
        protected string makemasking = string.Empty, citymasking = string.Empty, modelmasking = string.Empty, pageno = string.Empty;
        protected string pageTitle = string.Empty, pageDescription = string.Empty, modelName = string.Empty, makeName = string.Empty, pageKeywords = string.Empty, cityName = "India", pageCanonical = string.Empty
                  , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty;
        private const int _pageSize = 20;
        private int _pageNo = 1;
        protected int _startIndex = 0, _endIndex = 0, totalListing;
        private const int _pagerSlotSize = 5;
        protected HiddenField hdnHash;
        protected IEnumerable<CityEntityBase> cities = null;
        protected IEnumerable<BikeMakeModelBase> makeModels = null;
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            GetAllCities();
            GetAllMakeModels();
            BindSearchPageData();
            CreateMetas(makeName, modelName, cityName, totalListing);
            CreatePager();
        }
        /// <summary>
        /// Creted by: Sangram Nandkhile on 15 Sep 2016
        /// Summary: Check if hidden field has parameters and set the input query parameters
        ///          Create a keyvalue pair to retrive hash values and process ahead
        /// </summary>
        /// <param name="input"></param>
        private void CheckHashUrlParams(InputFilters input)
        {
            try
            {

                //***************************** REMOVE THIS LINE *******************************************
                hdnHash.Value = "city=2&make=1&model=59+349&budget=40000+50000&so=0&age=7";
                //***************************** REMOVE THIS LINE *******************************************

                if (!string.IsNullOrEmpty(hdnHash.Value))
                {
                    string hash = hdnHash.Value;
                    string[] arrHash = hash.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    foreach (var val in arrHash)
                    {
                        if (val.Contains('='))
                        {
                            string[] arr = val.Split('=');
                            dictionary.Add(arr[0], arr[1]);
                        }
                    }

                    if (dictionary.ContainsKey("city"))
                    {
                        input.CityId = Convert.ToUInt16(dictionary["city"]);
                    }
                    // Check budgets
                    if (dictionary.ContainsKey("budget"))
                    {
                        input.Budget = dictionary["budget"];
                    }
                    // Check make
                    if (dictionary.ContainsKey("make"))
                    {
                        input.Makes = dictionary["make"].Replace("+", ",");
                    }
                    // check model
                    if (dictionary.ContainsKey("model"))
                    {
                        input.Models = dictionary["model"].Replace("+", ",");
                    }
                    // Age
                    if (dictionary.ContainsKey("age"))
                    {
                        input.Age = dictionary["age"];
                    }
                    // SO sort order
                    if (dictionary.ContainsKey("so"))
                    {
                        input.SO = Convert.ToUInt16(dictionary["so"]);
                    }

                    if (dictionary.ContainsKey("st"))
                    {
                        input.ST = dictionary["st"].Replace("+", ",");
                    }
                    dictionary = null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Function to bind the search result to the repeater
        /// </summary>
        private void BindSearchPageData()
        {
            try
            {
                InputFilters objFilters = new InputFilters();
                CheckHashUrlParams(objFilters);

                // If inputs are set by hash, hash overrides the query string parameters
                if (objFilters != null && objFilters.CityId == 0 && cityId > 0)
                    objFilters.CityId = cityId;
                if (objFilters != null && objFilters.Makes.Length == 0 && makeId > 0)
                    objFilters.Makes = makeId.ToString();
                if (objFilters != null && objFilters.Models.Length == 0 && modelId > 0)
                    objFilters.Models = modelId.ToString();
                //objFilters.Budget = "40000+70000";
                //objFilters.Age = "2";
                // objFilters.Kms = "40000";
                //objFilters.Owners = "1,2,3";
                // objFilters.ST = "1,2";
                //objFilters.SO = 0;

                objFilters.PN = _pageNo;
                objFilters.PS = _pageSize;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ISearch, SearchBikes>();
                    container.RegisterType<ISearchFilters, ProcessSearchFilters>();
                    container.RegisterType<ISearchQuery, SearchQuery>();
                    container.RegisterType<ISearchRepository, SearchRepository>();

                    ISearch searchRepo = container.Resolve<ISearch>();
                    SearchResult objResult = searchRepo.GetUsedBikesList(objFilters);
                    if (objResult != null && objResult.Result != null && objResult.Result.Count() > 0)
                    {
                        pageno = objResult.CurrentPageNo.ToString();
                        totalListing = objResult.TotalCount;
                        rptUsedListings.DataSource = objResult.Result;
                        rptUsedListings.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
            }
        } // End of BindSearchPageData

        /// <summary>
        /// Parse query string and set variables
        /// </summary>
        public void ParseQueryString()
        {
            try
            {
                ModelMaskingResponse objResponse = null;
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                             .RegisterType<ICacheManager, MemcacheManager>()
                             .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                             .RegisterType<ICityCacheRepository, CityCacheRepository>()
                             .RegisterType<ICity, CityRepository>()
                             .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                             .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                             .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();

                    if (!string.IsNullOrEmpty(Request.QueryString["city"]))
                    {
                        citymasking = Request.QueryString["city"];
                        var objCache = container.Resolve<ICityCacheRepository>();
                        IEnumerable<CityEntityBase> GetCityDetails = objCache.GetAllCities(EnumBikeType.All);
                        CityEntityBase cityBase = (from c in GetCityDetails
                                                   where c.CityMaskingName == citymasking
                                                   select c).FirstOrDefault();
                        if (cityBase != null)
                        {
                            cityName = cityBase.CityName;
                            cityId = cityBase.CityId;
                        }
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["make"]))
                    {
                        makemasking = Request.QueryString["make"];
                        var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                        makeId = Convert.ToUInt16(MakeMapping.GetMakeId(Request.QueryString["make"]));
                        var makeDetails = objCache.GetMakeDetails(makeId);
                        if (makeDetails != null)
                        {
                            makeName = makeDetails.MakeName;
                        }
                    }

                    if (!string.IsNullOrEmpty(Request.QueryString["model"]))
                    {
                        var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                        objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);
                        if (objResponse != null)
                        {
                            modelId = objResponse.ModelId;
                            var objCachenew = container.Resolve<IBikeModels<BikeModelEntity, int>>();
                            BikeModelEntity modelEntity = objCachenew.GetById(Convert.ToInt32(modelId));
                            modelName = modelEntity.ModelName;
                        }
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                    {
                        int result;
                        int.TryParse(Request.QueryString["pn"], out result);
                        _pageNo = result;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : ParseQueryString");
                objErr.SendMail();
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
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<ICityCacheRepository, CityCacheRepository>()
                                .RegisterType<ICity, CityRepository>()
                                .RegisterType<ICacheManager, MemcacheManager>();

                    var _objCitiesCache = container.Resolve<ICityCacheRepository>();
                    cities = _objCitiesCache.GetAllCities(EnumBikeType.Used);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : GetAllCities");
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
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                .RegisterType<IBikeModelsCacheRepository<int>, BikeModelsCacheRepository<BikeModelEntity, int>>()
                                .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                .RegisterType<ICacheManager, MemcacheManager>();

                    var _objMakeCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                    makeModels = _objMakeCache.GetAllMakeModels();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : GetAllMakeModels");
                objErr.SendMail();
            }
            return makeModels;
        }

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
                pageTitle = string.Format("Used {0}Bikes in {1} - Verified Bike Listing For Sale | BikeWale", bikeName, strCity);
                pageCanonical = CreateCanonical(Request.RawUrl);

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
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
            }
        }

        public string CreateCanonical(string rawUrl)
        {
            string returl = rawUrl;
            try
            {
                // Check if raw url already has page and if not, add page-1 for making cononical url
                if (rawUrl.Contains("/page-"))
                {
                    returl = RemoveTrailingPage(rawUrl);
                }
                returl = string.Format("http://www.bikewale.com/{0}page-1/", returl.Replace("/m/", string.Empty));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateCanonical");
                objErr.SendMail();
            }
            return returl;
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
        #endregion

        #region pagination methods
        private void CreatePager()
        {
            IPager objPager = GetPager();
            GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex, totalListing);
            BindLinkPager(objPager, totalListing);
        }

        private IPager GetPager()
        {
            IPager _objPager = null;
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IPager, Pager>();
                _objPager = container.Resolve<IPager>();
            }
            return _objPager;
        }

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

        private void BindLinkPager(IPager objPager, int recordCount)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;

            string _baseUrl = RemoveTrailingPage(Request.RawUrl);

            try
            {
                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = string.Format("{0}page", _baseUrl);
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                // _pagerEntity.PageUrlType = "page-{0}/";
                _pagerEntity.TotalResults = (int)recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page
                _pagerOutput = objPager.GetUsedBikePager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
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
                Trace.Warn(ex.Message);
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }

        //public T GetUsedBikePager<T>(PagerEntity pagerDetails) where T : PagerOutputEntity, new()
        //{
        //    var results = new List<PagerUrlList>();
        //    T t = new T();

        //    try
        //    {
        //        bool firstPage = false, lastPage = false;
        //        string pageUrl;
        //        int startIndex, endIndex;
        //        int pageCount = (int)Math.Ceiling((double)pagerDetails.TotalResults / (double)pagerDetails.PageSize);// Total number of pages in the pager.

        //        if (pageCount > 1)
        //        {
        //            int totalSlots = (int)Math.Ceiling((double)pageCount / (double)pagerDetails.PagerSlotSize); // The total number of slots for the pager.
        //            int curSlot = ((int)Math.Floor((double)(pagerDetails.PageNo - 1) / (double)pagerDetails.PagerSlotSize)) + 1; // Current page slot.

        //            if (pagerDetails.PageNo == 1)// check for if the current page is the first page or the last page.
        //                firstPage = true;
        //            else if (pagerDetails.PageNo == pageCount)
        //                lastPage = true;

        //            // Get Start and End Index.
        //            GetStartEndIndex(pagerDetails, pageCount, curSlot, out startIndex, out endIndex);

        //            // Set the first and last page Urls.
        //            if (firstPage == false) t.FirstPageUrl = string.Format("{0}-1/", pagerDetails.BaseUrl);
        //            if (lastPage == false) t.LastPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, pageCount);

        //            //Get the list of page Urls.
        //            for (int i = startIndex; i <= endIndex; i++)
        //            {
        //                PagerUrlList pagerUrlList = new PagerUrlList();
        //                pageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, i);
        //                pagerUrlList.PageNo = i;
        //                pagerUrlList.PageUrl = pageUrl;
        //                results.Add(pagerUrlList);
        //            }

        //            //Set previous and next page Url.
        //            if (pagerDetails.PageNo == 1)
        //            {
        //                t.PreviousPageUrl = "";
        //                t.NextPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo + 1));
        //            }
        //            else if (endIndex == pagerDetails.PageNo)
        //            {
        //                t.NextPageUrl = "";
        //                t.PreviousPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo - 1));
        //            }
        //            else
        //            {
        //                t.NextPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo + 1));
        //                t.PreviousPageUrl = string.Format("{0}-{1}/", pagerDetails.BaseUrl, (pagerDetails.PageNo - 1));
        //            }
        //        }
        //        else
        //        {
        //            PagerUrlList pagerUrlList = new PagerUrlList();
        //            pagerUrlList.PageNo = -1;
        //            results.Add(pagerUrlList); //No pager created if the number of pages is equal to 1.
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorClass objErr = new ErrorClass(ex, "Pager Class Error. GetPager");
        //        objErr.SendMail();
        //    }

        //    t.PagesDetail = results;
        //    return t;
        //}

        //private void GetStartEndIndex(PagerEntity pagerDetails, int pageCount, int curSlot, out int startIndex, out int endIndex)
        //{
        //    bool isPagerSlotEven = false;

        //    startIndex = (curSlot - 1) * pagerDetails.PagerSlotSize + 1;//Calculate the start index.
        //    endIndex = curSlot * pagerDetails.PagerSlotSize;//Calculate the end Index.

        //    if (pagerDetails.PagerSlotSize != pageCount)
        //    {
        //        if (pagerDetails.PagerSlotSize % 2 == 0)// Check if pager slot size is even or odd to calculate the start index and the end index.
        //        {                                       // This is required to keep the selected page in the centre of the pager whilst the pager size is maintained.
        //            isPagerSlotEven = true;
        //        }

        //        int pagerSlotHalf = (int)Math.Floor((double)pagerDetails.PagerSlotSize / 2);

        //        //This sets the start index and end index such that the current page is in always in the centre, whilst the pager slot size is maintained.
        //        if (pagerSlotHalf < pagerDetails.PageNo)
        //        {
        //            if (isPagerSlotEven)
        //            {
        //                startIndex = pagerDetails.PageNo - (pagerSlotHalf - 1);
        //                endIndex = pagerDetails.PageNo + (pagerSlotHalf);
        //            }
        //            else
        //            {
        //                startIndex = pagerDetails.PageNo - pagerSlotHalf;
        //                endIndex = pagerDetails.PageNo + pagerSlotHalf;
        //            }
        //        }
        //        endIndex = endIndex <= pageCount ? endIndex : pageCount;
        //    }
        //}

        #endregion

    } // class
}   // namespace