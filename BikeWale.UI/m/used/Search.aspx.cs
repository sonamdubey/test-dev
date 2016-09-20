using Bikewale.BAL.BikeData;
using Bikewale.BAL.Pager;
using Bikewale.BAL.Used.Search;
using Bikewale.Cache.BikeData;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.Common;
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
            ProcessQueryString();
            GetAllCities();
            GetAllMakeModels();
            if (BindSearchPageData())
            {
                CreateMetas(makeName, modelName, cityName, totalListing);
                CreatePager();
            }

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
                        cityId = Convert.ToUInt16(dictionary["city"]);
                    }
                    // Check budgets
                    if (dictionary.ContainsKey("budget"))
                    {
                        input.Budget = dictionary["budget"];
                    }
                    // Check make
                    if (dictionary.ContainsKey("make"))
                    {
                        makeId = dictionary["make"].Replace("+", ",");
                    }
                    // check model
                    if (dictionary.ContainsKey("model"))
                    {
                        modelId = dictionary["model"].Replace("+", ",");
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

                    if (dictionary.ContainsKey("pn"))
                    {
                        _pageNo = Convert.ToUInt16(dictionary["pn"]);
                    }

                    if (dictionary.ContainsKey("owner"))
                    {
                        input.Owner = dictionary["owner"].Replace("+", ",");
                    }

                    //if (dictionary.ContainsKey("ps"))
                    //{
                    //    _pageSize = Convert.ToUInt16(dictionary["ps"]);
                    //}
                    dictionary = null;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Function to bind the search result to the repeater
        /// </summary>
        private bool BindSearchPageData()
        {
            try
            {
                InputFilters objFilters = new InputFilters();
                //CheckHashUrlParams(objFilters);

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
                        //pageno = objResult.CurrentPageNo.ToString();
                        totalListing = objResult.TotalCount;
                        strTotal = totalListing.ToString();
                        rptUsedListings.DataSource = objResult.Result;
                        rptUsedListings.DataBind();
                        //return true;
                    }
                    //else
                    //{
                    //    Response.Redirect("/pagenotfound.aspx", false);
                    //    return false;
                    //}
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
            }
            return true;
        } // End of BindSearchPageData

        /// <summary>
        /// Parse query string and set variables
        /// </summary>
        //public void ParseQueryString()
        //{
        //    try
        //    {
        //        ModelMaskingResponse objResponse = null;
        //        CityMaskingResponse objCityResponse = null;
        //        using (IUnityContainer container = new UnityContainer())
        //        {
        //            container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
        //                     .RegisterType<ICacheManager, MemcacheManager>()
        //                     .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
        //                     .RegisterType<ICityCacheRepository, CityCacheRepository>()
        //                     .RegisterType<ICityMaskingCacheRepository, CityMaskingCache>()
        //                     .RegisterType<ICity, CityRepository>()
        //                     .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
        //                     .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
        //                     .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();

        //            if (!string.IsNullOrEmpty(Request.QueryString["city"]))
        //            {
        //                citymasking = Request.QueryString["city"];
        //                var objCache = container.Resolve<ICityMaskingCacheRepository>();
        //                objCityResponse = objCache.GetCityMaskingResponse(citymasking);

        //                //IEnumerable<CityEntityBase> GetCityDetails = objCache.GetAllCities(EnumBikeType.All);
        //                //CityEntityBase cityBase = (from c in GetCityDetails
        //                //                           where c.CityMaskingName == citymasking
        //                //                           select c).FirstOrDefault();
        //                //if (cityBase != null)
        //                //{
        //                //    cityName = cityBase.CityName;
        //                //    cityId = cityBase.CityId;
        //                //}
        //            }

        //            if (!string.IsNullOrEmpty(Request.QueryString["make"]))
        //            {
        //                makemasking = Request.QueryString["make"];
        //                var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
        //                uint mkId = Convert.ToUInt16(MakeMapping.GetMakeId(Request.QueryString["make"]));
        //                makeId = mkId.ToString();
        //                BikeMakeEntityBase makeDetails = objCache.GetMakeDetails(mkId);
        //                if (makeDetails != null)
        //                {
        //                    makeName = makeDetails.MakeName;
        //                }

        //            }

        //            if (!string.IsNullOrEmpty(Request.QueryString["model"]))
        //            {
        //                var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
        //                objResponse = objCache.GetModelMaskingResponse(Request.QueryString["model"]);
        //                if (objResponse != null)
        //                {
        //                    modelId = objResponse.ModelId.ToString();
        //                    var objCachenew = container.Resolve<IBikeModels<BikeModelEntity, int>>();
        //                    BikeModelEntity modelEntity = objCachenew.GetById(Convert.ToInt32(modelId));
        //                    modelName = modelEntity.ModelName;
        //                }
        //            }
        //            if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
        //            {
        //                int result;
        //                int.TryParse(Request.QueryString["pn"], out result);
        //                _pageNo = result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : ParseQueryString");
        //        objErr.SendMail();
        //    }
        //    finally
        //    {

        //    }
        //}

        private void ProcessQueryString()
        {
            ModelMaskingResponse objModelResponse = null;
            CityMaskingResponse objCityResponse = null;
            string model = string.Empty, city = string.Empty, _make = string.Empty;
            IUnityContainer container = null;
            using (container = new UnityContainer())
            {
                container.RegisterType<IBikeMaskingCacheRepository<BikeModelEntity, int>, BikeModelMaskingCache<BikeModelEntity, int>>()
                                          .RegisterType<ICacheManager, MemcacheManager>()
                                          .RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>()
                                          .RegisterType<ICityCacheRepository, CityCacheRepository>()
                                          .RegisterType<ICityMaskingCacheRepository, CityMaskingCache>()
                                          .RegisterType<ICity, CityRepository>()
                                          .RegisterType<IBikeMakesCacheRepository<int>, BikeMakesCacheRepository<BikeMakeEntity, int>>()
                                          .RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>()
                                          .RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
            }
            try
            {
                city = Request.QueryString["city"];
                if (!string.IsNullOrEmpty(city))
                {
                    var objCache = container.Resolve<ICityMaskingCacheRepository>();
                    objCityResponse = objCache.GetCityMaskingResponse(city);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["make"]))
                {
                    string makeMaskingName = Request.QueryString["make"];
                    makeId = MakeMapping.GetMakeId(makeMaskingName);
                    //verify the id as passed in the url
                    if (CommonOpn.CheckId(makeId) == false)
                    {
                        redirectToPageNotFound = true;
                    }
                    else
                    {
                        var objCache = container.Resolve<IBikeMakesCacheRepository<int>>();
                        BikeMakeEntityBase makeDetails = objCache.GetMakeDetails(Convert.ToUInt16(makeId));
                        if (makeDetails != null)
                        {
                            makeName = makeDetails.MakeName;
                        }
                    }
                }

                model = Request.QueryString["model"];
                if (!string.IsNullOrEmpty(model))
                {
                    var objCache = container.Resolve<IBikeMaskingCacheRepository<BikeModelEntity, int>>();
                    objModelResponse = objCache.GetModelMaskingResponse(model);
                }

                if (!String.IsNullOrEmpty(Request.QueryString["pn"]))
                {
                    int result;
                    int.TryParse(Request.QueryString["pn"], out result);
                    _pageNo = result;
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + "ParseQueryString");
                objErr.SendMail();

                Response.Redirect("/customerror.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                this.Page.Visible = false;
            }
            finally
            {
                if (objCityResponse != null)
                {
                    var objCache = container.Resolve<ICityMaskingCacheRepository>();
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
                        redirectUrl = Request.RawUrl.ToLower().Replace(city, objCityResponse.MaskingName);
                        redirectPermanent = true;
                    }
                    else
                    {
                        redirectToPageNotFound = true;
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
                        redirectUrl = Request.RawUrl.ToLower().Replace(model, objModelResponse.MaskingName);
                        redirectPermanent = true;
                    }
                    else
                    {
                        redirectToPageNotFound = true;
                    }
                }
                //else
                //{
                //    redirectToPageNotFound = true;
                //}
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : GetAllCities");
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : GetAllMakeModels");
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

                pageCanonical = string.Format("http://www.bikewale.com/{0}", Request.RawUrl.ToLower().Replace("/m/", string.Empty));

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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateMetas");
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
        #endregion

        #region pagination methods
        public void CreatePager()
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

            string _baseUrl = RemoveTrailingPage(Request.RawUrl.ToLower());

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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

    } // class
}   // namespace