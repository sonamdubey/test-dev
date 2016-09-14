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
        protected uint cityId, makeid, modelId, totalListing;
        protected ushort makeId;
        protected string makemasking = string.Empty, citymasking = string.Empty, modelmasking = string.Empty, pageno = string.Empty;
        protected string pageTitle = string.Empty, pageDescription = string.Empty, modelName = string.Empty, makeName = string.Empty, pageKeywords = string.Empty, cityName = "India", pageCanonical = string.Empty
            , heading = string.Empty, nextUrl = string.Empty, prevUrl = string.Empty;
        private const int _pageSize = 10;
        private int _pageNo = 1;
        private const int _pagerSlotSize = 5;
        #endregion

        #region events

        protected override void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ParseQueryString();
            BindSearchPageData();
            CreateMetas();
            CreatePager();
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
                objFilters.CityId = cityId;
                objFilters.Makes = makeId.ToString();
                objFilters.Models = modelId.ToString(); ;
                //objFilters.Budget = "40000+70000";
            objFilters.Age = "2";
            objFilters.Kms = "40000";
            objFilters.Owners = "1,2,3";
            objFilters.ST = "1,2";
            objFilters.PN = 1;
            objFilters.PS = 20;
            objFilters.SO = 0;
            

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
        /// Created by: Sangram Nandkhile on 13 Sep 2016
        /// Summary: Create title, metas and description for SEO
        /// </summary>
        public void CreateMetas()
        {
            try
            {
                string _bike = string.Format("{0} {1} ", makeName, modelName).Trim();
                if (_bike.Length > 0)
                    _bike = string.Format("{0} ", _bike);
                heading = string.Format("Used {0}Bikes in {1}", _bike, cityName);
                pageTitle = string.Format("Used {0}Bikes in {1} - Verified Bike Listing For Sale | BikeWale", _bike, cityName);
                pageDescription = string.Format("There are {1} used {0}bikes in {2} on BikeWale. Find largest stock of genuine, good condition, well maintained second-hand {0}bikes for sale in {2}", _bike, totalListing, cityName);
                pageKeywords = string.Format(@"Used {0}bikes in {1}, find {0}used {1} bikes in {0}, buy {0}used bikes in {0}, search {0}used bikes, find {0}used bikes, used bike listing, {0}bike used sale, {0}bike sale in {1}, {1} bike search, Bajaj, Aprilia, BMW, Ducati, Harley Davidson, Hero, Honda, Hyosung, KTM, Mahindra, Royal Enfield, Suzuki, Yamaha, Yo, TVS, Vespa, Kawasaki", _bike, cityName);
                pageCanonical = CreateCanonical(Request.RawUrl);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateMetas");
                objErr.SendMail();
            }
        }

        public string CreateCanonical(string rawUrl)
        {
            string returl = string.Empty;
            try
            {
                // Check if raw url already has page and if not, add page-1 for making cononical url
                if (rawUrl.Contains("/page-"))
                {
                    string[] urlArray = rawUrl.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string canon = string.Join("/", urlArray.Take(urlArray.Length - 1).ToArray());
                    returl = string.Format("http://www.bikewale.com/{0}/page-1/", canon.Replace("/m/", string.Empty));
                }
                else
                {
                    returl = string.Format("http://www.bikewale.com/{0}page-1/", Request.RawUrl.Replace("/m/", string.Empty));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, Request.ServerVariables["URL"] + " : CreateCanonical");
                objErr.SendMail();
            }
            return returl;
        }

        #endregion

        #region pagination methods
        private void CreatePager()
        {
            IPager objPager = GetPager();
            int _startIndex = 0, _endIndex = 0;
            objPager.GetStartEndIndex(_pageSize, _pageNo, out _startIndex, out _endIndex);
            BindLinkPager(objPager, 88, "XYZ", "vvv");
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

        private void BindLinkPager(IPager objPager, int recordCount, string makeName, string modelName)
        {
            PagerOutputEntity _pagerOutput = null;
            PagerEntity _pagerEntity = null;
            string _baseUrl = "/expert-reviews/";

            try
            {
                if (!String.IsNullOrEmpty(modelName))
                    _baseUrl = string.Format("/{0}-bikes/{1}/expert-reviews/", makeName, modelName);
                else if (!String.IsNullOrEmpty(makeName))
                    _baseUrl = string.Format("/{0}-bikes/expert-reviews/", makeName);

                _pagerEntity = new PagerEntity();
                _pagerEntity.BaseUrl = _baseUrl;
                _pagerEntity.PageNo = _pageNo; //Current page number
                _pagerEntity.PagerSlotSize = _pagerSlotSize; // 5 links on a page
                _pagerEntity.PageUrlType = "page/";
                _pagerEntity.TotalResults = recordCount; //total News count
                _pagerEntity.PageSize = _pageSize;  //No. of news to be displayed on a page

                _pagerOutput = objPager.GetPager<PagerOutputEntity>(_pagerEntity);

                // for RepeaterPager
                ctrlPager.PagerOutput = _pagerOutput;
                ctrlPager.CurrentPageNo = _pageNo;
                ctrlPager.TotalPages = objPager.GetTotalPages(recordCount, _pageSize);
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

        #endregion

    } // class
}   // namespace